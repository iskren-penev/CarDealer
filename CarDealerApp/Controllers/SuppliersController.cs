namespace CarDealerApp.Controllers
{
    using System.Web.Mvc;
    using CarDealer.Models;
    using CarDealer.Models.BindingModels;
    using CarDealer.Models.ViewModels.Supplier;
    using CarDealer.Services;
    using CarDealerApp.Security;


    public class SuppliersController : Controller
    {
        private SuppliersService service;

        public SuppliersController()
        {
            this.service = new SuppliersService();
        }

       
        [Route("suppliers/{type:regex(local|importers)}")]
        public ActionResult All(string type)
        {
            var viewModels = this.service.GetSuppliersByType(type);
            return View(viewModels);
        }

        [HttpGet]
        [Route("add/")]
        public ActionResult Add()
        {
            var httpCookie = this.Request.Cookies.Get("sessionId");
            if (httpCookie == null || !AuthenticationManager.IsAuthenticated(httpCookie.Value))
            {
                return this.RedirectToAction("All");
            }

            return this.View();
        }

        [HttpPost]
        [Route("add")]
        public ActionResult Add([Bind(Include = "Name, IsImporter")] AddSupplierBindingModel model)
        {
            var httpCookie = this.Request.Cookies.Get("sessionId");
            if (httpCookie == null || !AuthenticationManager.IsAuthenticated(httpCookie.Value))
            {
                return this.RedirectToAction("All");
            }
            User loggedInUser = AuthenticationManager.GetAuthenticatedUser(httpCookie.Value);

            this.service.AddSupplier(model, loggedInUser.Id);
            return this.RedirectToAction("All");
        }

        [HttpGet]
        [Route("edit/{id:int}")]
        public ActionResult Edit(int id)
        {
            var httpCookie = this.Request.Cookies.Get("sessionId");
            if (httpCookie == null || !AuthenticationManager.IsAuthenticated(httpCookie.Value))
            {
                return this.RedirectToAction("All");
            }

            EditSupplierViewModel viewModel = this.service.GetEditSupplierViewModel(id);
            return this.View(viewModel);
        }

        [HttpPost]
        [Route("edit/{id:int}")]
        public ActionResult Edit([Bind(Include = "Id, Name, IsImporter")] EditSupplierBindingModel model)
        {
            var httpCookie = this.Request.Cookies.Get("sessionId");
            if (httpCookie == null || !AuthenticationManager.IsAuthenticated(httpCookie.Value))
            {
                return this.RedirectToAction("All");
            }

            if (!this.ModelState.IsValid)
            {
                EditSupplierViewModel vm = this.service.GetEditSupplierViewModel(model.Id);
                return this.View(vm);
            }

            User loggedInUser = AuthenticationManager.GetAuthenticatedUser(httpCookie.Value);

            this.service.EditSupplier(model, loggedInUser.Id);
            return this.RedirectToAction("All");
        }

        [HttpGet]
        [Route("delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            var httpCookie = this.Request.Cookies.Get("sessionId");
            if (httpCookie == null || !AuthenticationManager.IsAuthenticated(httpCookie.Value))
            {
                return this.RedirectToAction("All");
            }

            DeleteSupplierViewModel vm = this.service.GetDeleteSupplierViewModel(id);
            return this.View(vm);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public ActionResult Delete([Bind(Include = "Id")]DeleteSupplierBindingModel model)
        {
            var httpCookie = this.Request.Cookies.Get("sessionId");
            if (httpCookie == null || !AuthenticationManager.IsAuthenticated(httpCookie.Value))
            {
                return this.RedirectToAction("All");
            }

            if (!this.ModelState.IsValid)
            {
                DeleteSupplierViewModel vm = this.service.GetDeleteSupplierViewModel(model.Id);
                return this.View(vm);
            }

            User loggedInUser = AuthenticationManager.GetAuthenticatedUser(httpCookie.Value);

            this.service.DeleteSupplier(model, loggedInUser.Id);
            return this.RedirectToAction("All");
        }
    }
}