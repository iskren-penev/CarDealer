namespace CarDealerApp.Controllers
{
    using System.Web.Mvc;
    using CarDealer.Models.BindingModels;
    using CarDealer.Services;

    [RoutePrefix("cars")]
    public class CarsController : Controller
    {
        private CarsService service;

        public CarsController()
        {
            this.service = new CarsService();
        }

        [Route("{make}")]
        public ActionResult ByMake(string make)
        {
            var viewModels = this.service.GetCarsByMake(make);

            return View(viewModels);
        }

        [Route("all")]
        public ActionResult All()
        {
            var viewModels = this.service.GetAllCars();
            return View(viewModels);
        }

        [Route("{id:int:min(1)}/parts")]
        public ActionResult About(int id)
        {
            var viewModel = this.service.GetCarInfoById(id);
            return this.View(viewModel);
        }

        [HttpGet]
        [Route("add")]
        public ActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Route("add")]
        public ActionResult Add([Bind(Include = "Make, Model, TravelledDistance, Parts")] AddCarBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.service.AddCar(model);

                return this.RedirectToAction("All");
            }

            return this.View();
        }
    }
}