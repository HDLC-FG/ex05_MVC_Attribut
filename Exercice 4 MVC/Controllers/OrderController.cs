using Exercice_5_MVC.Service;
using Exercice_5_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Exercice_5_MVC.Controllers
{
    public class OrderController : Controller
    {
        private OrderService orderService;
        private static List<ArticleSelectedViewModel> articleSelectedViewModels = new List<ArticleSelectedViewModel>();

        public OrderController()
        {
            orderService = new OrderService();
        }

        // GET: OrderController
        public ActionResult Index()
        {
            return View(orderService.GetOrders().Select(x => x.ToViewModel()));
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View(orderService.GetOrders().FirstOrDefault(x => x.Id == id));
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            ViewData["OrderDetails"] = orderService.GetOrderDetails().Select(x => x.ToViewModel()).ToList();
            ViewData["Articles"] = orderService.GetArticles().Select(x => x.ToViewModel()).ToList();
            ViewData["ArticlesSelected"] = articleSelectedViewModels;
            return View(new OrderViewModel());
        }

        // POST: OrderController/Create
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel viewModel)
        {
            if(articleSelectedViewModels.Count > 0)
            {
                ModelState["ListArticlesSelected"].ValidationState = ModelValidationState.Valid;
            }
            if (!ModelState.IsValid)
            {
                ViewData["OrderDetails"] = orderService.GetOrderDetails().Select(x => x.ToViewModel()).ToList();
                ViewData["Articles"] = orderService.GetArticles().Select(x => x.ToViewModel()).ToList();
                ViewData["ArticlesSelected"] = articleSelectedViewModels;
                return View(viewModel);
            }

            viewModel.Id = orderService.GetOrders().Max(x => x.Id) + 1;

            var articles = orderService.GetArticles();
            
            var i = 1;
            foreach (var articleSelected in articleSelectedViewModels)
            {
                var article = articles.Find(x => x.Id == articleSelected.Id)!;
                if(article.StockQuantity >= articleSelected.Qte)
                {
                    viewModel.OrderDetails.Add(new OrderDetailViewModel
                    {
                        Id = i,
                        OrderId = (int)viewModel.Id,
                        Order = viewModel,
                        ArticleId = articleSelected.Id,
                        Article = article.ToViewModel(),
                        Quantity = articleSelected.Qte,
                        UnitPrice = article.Price
                    });
                    i++;
                    orderService.AddArticleQuantity(articleSelected.Id, -articleSelected.Qte);
                }
            }

            try
            {
                orderService.Add(viewModel.ToModel());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult CreateArticle(ArticleSelectedViewModel articleSelected)
        {
            var article = orderService.GetArticles().Find(x => x.Id == articleSelected.Id);
            var success = false;
            if (article?.StockQuantity >= articleSelected.Qte)
            {
                articleSelected.Price = article.Price;
                articleSelectedViewModels.Add(articleSelected);
                success = true;

                orderService.AddArticleQuantity(articleSelected.Id, -articleSelected.Qte);
            }

            ViewData["ArticlesSelected"] = articleSelectedViewModels;
            return Json(new
            {
                Success = success,
                TotalAmount = articleSelectedViewModels.Sum(x => x.Price * x.Qte),
                PartialView = RenderViewToString("_ListeArticles.cshtml", articleSelectedViewModels)
            });
        }

        [HttpPost]
        public IActionResult DeleteArticle(int Id, int Qte)
        {
            orderService.AddArticleQuantity(Id, Qte);
            var articleToRemove = articleSelectedViewModels.Find(x => x.Id == Id);
            articleSelectedViewModels.Remove(articleToRemove);
            return Json(new
            {
                TotalAmount = articleSelectedViewModels.Sum(x => x.Price * x.Qte),
                PartialView = RenderViewToString("_ListeArticles.cshtml", articleSelectedViewModels)
            });
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            var order = orderService.Get(id);
            return View(order?.ToViewModel());
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderViewModel viewModel)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                orderService.Update(viewModel.ToModel());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            var order = orderService.Get(id);
            return View(order?.ToViewModel());
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrderViewModel viewModel)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                orderService.Delete(viewModel.ToModel());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private string RenderViewToString(string viewName, object model)
        {
            var controllerContext = ControllerContext;
            var viewEngine = controllerContext.HttpContext.RequestServices.GetRequiredService<IRazorViewEngine>();
            var tempDataProvider = controllerContext.HttpContext.RequestServices.GetRequiredService<ITempDataProvider>();

            // Obtient l'action et la vue à partir du moteur de vues Razor
            var viewResult = viewEngine.GetView("Views/Shared/", viewName, false);
            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"View {viewName} not found.");
            }

            // Crée un StringWriter pour capter le contenu HTML rendu
            var stringWriter = new StringWriter();

            var viewContext = new ViewContext(
                controllerContext,
                viewResult.View,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), controllerContext.ModelState) { Model = model },
                new TempDataDictionary(controllerContext.HttpContext, tempDataProvider),
                stringWriter,
                new HtmlHelperOptions()
            );
            
            // Rendre la vue en HTML dans le StringWriter
            viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();

            // Retourner le contenu HTML généré sous forme de chaîne
            return stringWriter.ToString();
        }
    }
}
