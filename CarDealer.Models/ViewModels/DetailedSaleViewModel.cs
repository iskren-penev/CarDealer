namespace CarDealer.Models.ViewModels
{
    using CarDealer.Models.ViewModels.Car;

    public class DetailedSaleViewModel
    {
        public CarViewModel Car { get; set; }

        public string Customer { get; set; }
    }
}
