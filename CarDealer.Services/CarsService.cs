namespace CarDealer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Models;
    using CarDealer.Models.BindingModels;
    using CarDealer.Models.ViewModels;
    using CarDealer.Models.ViewModels.Car;

    public class CarsService : Service
    {
        public IEnumerable<CarViewModel> GetCarsByMake(string make)
        {
            List<CarViewModel> viewModels = this.context.Cars
                                                    .Where(c => c.Make == make)
                                                    .Select(c => new CarViewModel()
                                                    {
                                                        Make = c.Make,
                                                        Model = c.Model,
                                                        TravelledDistance = c.TravelledDistance
                                                    })
                                                    .OrderBy(c => c.Model)
                                                    .ThenByDescending(c => c.TravelledDistance)
                                                    .ToList();
            return viewModels;
        }

        public IEnumerable<CarViewModel> GetAllCars()
        {
            List<CarViewModel> viewModels = this.context.Cars
                                                    .Select(c => new CarViewModel()
                                                    {
                                                        Make = c.Make,
                                                        Model = c.Model,
                                                        TravelledDistance = c.TravelledDistance
                                                    })
                                                    .OrderBy(c => c.Make)
                                                    .ThenBy(c => c.Model)
                                                    .ThenByDescending(c => c.TravelledDistance)
                                                    .ToList();

            return viewModels;
        }

        public DetailedCarViewModel GetCarInfoById(int id)
        {
            Car car = this.context.Cars.Find(id);
            DetailedCarViewModel viewModel = new DetailedCarViewModel()
            {
                Car = new CarViewModel() { Make = car.Make, Model = car.Model, TravelledDistance = car.TravelledDistance},
                Parts = car.Parts.Select(p=> new CarPartsViewModel()
                {
                    Name = p.Name,
                    Price = p.Price
                }).ToList()
            };

            return viewModel;
        }

        public void AddCar(AddCarBindingModel model)
        {
            Car car = new Car()
            {
                Make = model.Make,
                Model = model.Model,
                TravelledDistance = model.TravelledDistance
            };
            int[] partIds = model.Parts.Split(' ').Select(int.Parse).ToArray();
            foreach (var partId in partIds)
            {
                Part part = this.context.Parts.Find(partId);
                if (part != null)
                {
                    car.Parts.Add(part);
                }
            }
            this.context.Cars.Add(car);
            this.context.SaveChanges();
        }
    }
}
