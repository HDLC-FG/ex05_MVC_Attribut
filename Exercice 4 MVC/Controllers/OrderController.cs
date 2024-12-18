using Exercice_5_MVC.Service;
using Exercice_5_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            if (!ModelState.IsValid)
            {
                ViewData["OrderDetails"] = orderService.GetOrderDetails().Select(x => x.ToViewModel()).ToList();
                ViewData["Articles"] = orderService.GetArticles().Select(x => x.ToViewModel()).ToList();
                ViewData["ArticlesSelected"] = articleSelectedViewModels;
                return View(viewModel);
            }

            viewModel.Id = orderService.GetOrders().Max(x => x.Id) + 1;

            var articles = orderService.GetArticles();

            var listarticlesSelected = articleSelectedViewModels;
            
            var i = 1;
            foreach (var articleSelected in listarticlesSelected)
            {
                var article = articles.Find(x => x.Id == articleSelected.Id)!;
                if(article.StockQuantity > articleSelected.Qte)
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
        [ActionName("CreateArticle")]
        public IActionResult Create(ArticleSelectedViewModel articleSelected)
        {
            articleSelectedViewModels.Add(articleSelected);
            ViewData["ArticlesSelected"] = articleSelectedViewModels;
            return PartialView("_ListeArticles", articleSelectedViewModels);
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
    }
}
