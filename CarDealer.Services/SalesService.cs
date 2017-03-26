namespace CarDealer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Models;
    using CarDealer.Models.BindingModels;
    using CarDealer.Models.ViewModels;
    using CarDealer.Models.ViewModels.Car;
    using CarDealer.Models.ViewModels.Sale;

    public class SalesService : Service
    {
        public IEnumerable<SaleViewModel> GetAllSales()
        {
            List<SaleViewModel> models = this.context.Sales
                .Select(s => new SaleViewModel()
                {
                    Car = new CarViewModel()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Customer = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Price),
                    Discount = s.Discount
                }).ToList();
            return models;
        }

        public DetailedSaleViewModel GetSaleInfoById(int id)
        {
            Sale sale = this.context.Sales.Find(id);
            DetailedSaleViewModel model = new DetailedSaleViewModel()
            {
                Car = new CarViewModel()
                {
                    Make = sale.Car.Make,
                    Model = sale.Car.Model
                },
                Customer = sale.Customer.Name
            };
            return model;
        }

        public IEnumerable<SaleViewModel> GetDiscountedSales(double? percent)
        {
            var models = this.GetAllSales().Where(s => s.Discount > 0);
            if (percent != null)
            {
                percent /= 100;
                models = models.Where(m => m.Discount == percent);
            }
            return models;
        }

        public AddSaleViewModel GetSalesViewModel()
        {
            AddSaleViewModel vm = new AddSaleViewModel();
            IEnumerable<Car> carModels = this.context.Cars;
            IEnumerable<Customer> customerModels = this.context.Customers;

            IEnumerable<AddSaleCarViewModel> carVms = carModels.Select(c => new AddSaleCarViewModel()
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model
            });
            IEnumerable<AddSaleCustomerViewModel> customerVms =
                customerModels.Select(c => new AddSaleCustomerViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                });

            List<int> discounts = new List<int>();
            for (int i = 0; i <= 50; i += 5)
            {
                discounts.Add(i);
            }

            vm.Cars = carVms;
            vm.Customers = customerVms;
            vm.Discounts = discounts;

            return vm;
        }

        public AddSaleConfirmationViewModel GetSaleCofirmationViewModel(AddSaleBindingModel bind)
        {
            Car carModel = this.context.Cars.Find(bind.CarId);
            Customer customerModel = this.context.Customers.Find(bind.CustomerId);
            AddSaleConfirmationViewModel vm = new AddSaleConfirmationViewModel()
            {
                Discount = bind.Discount,
                CarPrice = (decimal)carModel.Parts.Sum(part => part.Price).Value,
                CarId = carModel.Id,
                CarRepresentation = $"{carModel.Make} {carModel.Model}",
                CustomerId = customerModel.Id,
                CustomerName = customerModel.Name
            };

            vm.Discount += customerModel.IsYoungDriver ? 5 : 0;
            vm.FinalCarPrice = vm.CarPrice - vm.CarPrice * vm.Discount / 100;
            return vm;
        }

        public void AddSale(AddSaleBindingModel vm)
        {
            Car carModel = this.context.Cars.Find(vm.CarId);
            Customer customerModel = this.context.Customers.Find(vm.CustomerId);
            Sale sale = new Sale()
            {
                Customer = customerModel,
                Car = carModel,
                Discount = vm.Discount / 100.0
            };

            this.context.Sales.Add(sale);
            this.context.SaveChanges();
        }
    }
}
