
namespace CarDealerApp.Controllers
{
    using System.Web.Mvc;
    using CarDealer.Models.BindingModels;
    using CarDealer.Models.ViewModels;
    using CarDealer.Models.ViewModels.Sale;
    using CarDealer.Services;
    using CarDealerApp.Security;

    [RoutePrefix("sales")]
    public class SalesController : Controller
    {
        private SalesService service;

        public SalesController()
        {
            this.service=new SalesService();
        }

        [Route]
        public ActionResult All()
        {
            var models = this.service.GetAllSales();
            return View(models);
        }

        [Route("{id:int:min(1)}")]
        public ActionResult About(int id)
        {
            var model = this.service.GetSaleInfoById(id);
            return this.View(model);
        }

        [Route("discounted/{percent:double?}/")]
        public ActionResult DiscountedSales(double? percent)
        {
            var models = this.service.GetDiscountedSales(percent);
            return this.View(models);
        }

        [HttpGet]
        [Route("add/")]
        public ActionResult Add()
        {
            var cookie = this.Request.Cookies.Get("sessionId");
            if (cookie == null || !AuthenticationManager.IsAuthenticated(cookie.Value))
            {
                return this.RedirectToAction("Login", "Users");
            }

            AddSaleViewModel viewModel = this.service.GetSalesViewModel();
            return this.View(viewModel);
        }

        [HttpPost]
        [Route("add/")]
        public ActionResult Add([Bind(Include = "CustomerId, CarId, Discount")] AddSaleBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                AddSaleConfirmationViewModel confirmationViewModel = this.service.GetSaleCofirmationViewModel(model);
                return this.RedirectToAction("AddConfirmation", confirmationViewModel);
            }

            AddSaleViewModel viewModel = this.service.GetSalesViewModel();
            return this.View(viewModel);
        }

        [HttpGet]
        [Route("AddConfirmation")]
        public ActionResult AddConfirmation(AddSaleConfirmationViewModel viewModel)
        {
            var cookie = this.Request.Cookies.Get("sessionId");
            if (cookie == null || !AuthenticationManager.IsAuthenticated(cookie.Value))
            {
                return this.RedirectToAction("Login", "Users");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [Route("AddConfirmation")]
        public ActionResult AddConfirmation(AddSaleBindingModel model)
        {
            var cookie = this.Request.Cookies.Get("sessionId");
            if (cookie == null || !AuthenticationManager.IsAuthenticated(cookie.Value))
            {
                return this.RedirectToAction("Login", "Users");
            }

            this.service.AddSale(model);
            return this.RedirectToAction("All");
        }
    }
}