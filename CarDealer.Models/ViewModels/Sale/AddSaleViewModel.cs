namespace CarDealer.Models.ViewModels.Sale
{
    using System.Collections.Generic;

    public class AddSaleViewModel
    {
        public IEnumerable<AddSaleCustomerViewModel> Customers { get; set; }

        public IEnumerable<AddSaleCarViewModel> Cars { get; set; }

        public IEnumerable<int> Discounts { get; set; }
    }
}
