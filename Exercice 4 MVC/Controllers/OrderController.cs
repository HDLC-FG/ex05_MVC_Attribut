using Exercice_5_MVC.Service;
using Exercice_5_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Exercice_5_MVC.Controllers
{
    public class OrderController : Controller
    {
        private OrderService orderService;

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
            return View(new OrderViewModel());
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["OrderDetails"] = orderService.GetOrderDetails().Select(x => x.ToViewModel()).ToList();
                ViewData["Articles"] = orderService.GetArticles().Select(x => x.ToViewModel()).ToList();
                return View(new OrderViewModel());
            }

            var articles = orderService.GetArticles();
            
            var listarticlesSelected = JsonConvert.DeserializeObject<List<ArticleView>>("[" + viewModel.ListArticlesSelected.Substring(1) + "]");

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

    internal class ArticleView
    {
        public int Id { get; set; }
        public int Qte { get; set; }
    }
}
