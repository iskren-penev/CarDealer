namespace CarDealerApp.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

   public class HomeController : Controller
    {
        [Route]
        [Route("home/index")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("home/about")]
        public ActionResult About()
        {
            var ctx = new CarDealer.Data.CarDealerContext();
            ViewBag.Message = "Your application description page." + ctx.Cars.Count();

            return View();
        }

        [Route("home/contacts")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}