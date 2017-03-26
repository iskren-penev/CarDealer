namespace CarDealer.Models.ViewModels.Car
{
    using System.Collections.Generic;

    public class DetailedCarViewModel
    {
        public CarViewModel Car { get; set; }

        public ICollection<CarPartsViewModel> Parts { get; set; }
    }
}
