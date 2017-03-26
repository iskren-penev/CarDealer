namespace CarDealer.Services
{
    using System;
    using System.Collections.Generic;
    using CarDealer.Models.ViewModels;
    using System.Linq;
    using CarDealer.Models;
    using CarDealer.Models.BindingModels;

    public class CustormerService : Service
    {
        public IEnumerable<CustomerViewModel> GetAllCustomers(string order)
        {
            List<CustomerViewModel> viewModels = new List<CustomerViewModel>();
            if (order == "ascending")
            {
                viewModels = this.context.Customers
                    .OrderBy(c => c.BirthDate).ThenBy(c => c.IsYoungDriver)
                    .Select(c => new CustomerViewModel()
                    {
                        Name = c.Name,
                        BirthDate = c.BirthDate,
                        IsYoungDriver = c.IsYoungDriver
                    }).ToList();
            }
            else if (order == "descending")
            {
                viewModels = this.context.Customers
                     .OrderByDescending(c => c.BirthDate).ThenBy(c => c.IsYoungDriver)
                     .Select(c => new CustomerViewModel()
                     {
                         Name = c.Name,
                         BirthDate = c.BirthDate,
                         IsYoungDriver = c.IsYoungDriver
                     }).ToList();
            }
            else
            {
                throw new ArgumentException("Unknown order type for customers");
            }
            return viewModels;
        }

        public DetailedCustomerViewModel GetCustomerInfoById(int id)
        {
            Customer customer = this.context.Customers.Find(id);
            DetailedCustomerViewModel viewModel = new DetailedCustomerViewModel()
            {
                Name = customer.Name,
                CarsBought = customer.Sales.Count,
                TotalMoneySpent = customer.Sales.Sum(s => s.Car.Parts.Sum(p => p.Price))
            };
            return viewModel;
        }

        public void AddCustomer(AddCustomerBindingModel model)
        {
            Customer customer = new Customer()
            {
                Name = model.Name,
                BirthDate = model.BirthDate
            };
            if (DateTime.Now.Year - model.BirthDate.Year < 21)
            {
                customer.IsYoungDriver = true;
            }

            this.context.Customers.Add(customer);
            this.context.SaveChanges();
        }

        public EditCustomerViewModel GetEditCustomerViewModel(int id)
        {
            Customer customer = this.context.Customers.Find(id);
            EditCustomerViewModel model = new EditCustomerViewModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                BirthDate = customer.BirthDate
            };
            return model;

        }

        public void EditCustomer(EditCustomerBindingModel model)
        {
            Customer customer = this.context.Customers.Find(model.Id);

            if (customer == null)
            {
                throw new ArgumentException("Cannot find customer with such id!");
            }
            customer.Name = model.Name;
            customer.BirthDate = model.BirthDate;

            this.context.SaveChanges();

        }
    }
}
