namespace CarDealer.Models.ViewModels.Sale
{
    using CarDealer.Models.ViewModels.Car;

    public class SaleViewModel
    {
        public CarViewModel Car { get; set; }

        public string Customer { get; set; }

        public double? Price { get; set; }

        public double Discount { get; set; }


    }
}
