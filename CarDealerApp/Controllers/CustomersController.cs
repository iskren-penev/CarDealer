namespace CarDealerApp.Controllers
{
    using System.Web.Mvc;
    using CarDealer.Models.BindingModels;
    using CarDealer.Models.ViewModels;
    using CarDealer.Services;

    [RoutePrefix("customers")]
    public class CustomersController : Controller
    {
        private CustormerService service;

        public CustomersController()
        {
            this.service = new CustormerService();
        }

        [Route("all/{order:regex(ascending|descending)}")]
        public ActionResult All(string order)
        {

            var viewModels = this.service.GetAllCustomers(order);

            return View(viewModels);
        }

        [Route("{id:int:min(1)}")]
        public ActionResult About(int id)
        {
            var viewModel = this.service.GetCustomerInfoById(id);

            return this.View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

        [Route("add")]
        [HttpPost]
        public ActionResult Add([Bind(Include = "Name, Birthdate")] AddCustomerBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.service.AddCustomer(model);

                return this.RedirectToAction("All", new { order = "ascending" });
            }
            return this.View();
        }

        [Route("edit/{id:int:min(1)}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EditCustomerViewModel model = this.service.GetEditCustomerViewModel(id);
            return this.View(model);
        }

        [Route("edit/{id:int:min(1)}")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,BirthDate")] EditCustomerBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.service.EditCustomer(model);
                return this.RedirectToAction("All", new {order = "ascending"});
            }
            return this.View(this.service.GetEditCustomerViewModel(model.Id));
        }
    }
}