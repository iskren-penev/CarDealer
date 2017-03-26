namespace CarDealerApp.Controllers
{
    using System.Web.Mvc;
    using CarDealer.Models.BindingModels;
    using CarDealer.Models.ViewModels;
    using CarDealer.Models.ViewModels.Parts;
    using CarDealer.Services;

    [RoutePrefix("parts")]
    public class PartsController : Controller
    {
        private PartsService service;

        public PartsController()
        {
            this.service = new PartsService();
        }

        [Route]
        [Route("all")]
        [HttpGet]
        public ActionResult All()
        {
            var viewModels = this.service.GetAllParts();
            return View(viewModels);
        }

        [Route("add")]
        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

        [Route("add")]
        [HttpPost]
        public ActionResult Add([Bind(Include = "Name,Price,Quantity")] AddPartBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.service.AddNewPart(model);
                return this.RedirectToAction("All");
            }
            return this.View();
        }

        [Route("edit/{id:int:min(1)}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EditPartViewModel model = this.service.GetEditPartViewModel(id);
            return this.View(model);
        }

        [Route("edit/{id:int:min(1)}")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Price,Quantity")] EditPartBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.service.EditPart(model);
                return this.RedirectToAction("All");
            }
            return this.View(this.service.GetEditPartViewModel(model.Id));
        }

        [Route("delete/{id:int:min(1)}")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            EditPartViewModel model = this.service.GetEditPartViewModel(id);
            return this.View(model);
        }

        [Route("delete/{id:int:min(1)}")]
        [HttpPost]
        public ActionResult Delete([Bind(Include = "Id")] DeleteBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.service.DeletePart(model);
                return RedirectToAction("All");
            }
            return this.View(this.service.GetEditPartViewModel(model.Id));
        }
    }


}