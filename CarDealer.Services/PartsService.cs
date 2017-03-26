namespace CarDealer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Models;
    using CarDealer.Models.BindingModels;
    using CarDealer.Models.ViewModels;
    using CarDealer.Models.ViewModels.Parts;

    public class PartsService : Service
    {
        public IEnumerable<PartViewModel> GetAllParts()
        {
            IEnumerable<PartViewModel> parts = this.context.Parts
                .Select(p => new PartViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                });

            return parts;
        }

        public void AddNewPart(AddPartBindingModel model)
        {
            Part part = new Part()
            {
                Name = model.Name,
                Price = model.Price,
                Quantity = model.Quantity
            };

            this.context.Parts.Add(part);
            this.context.SaveChanges();
        }

        public EditPartViewModel GetEditPartViewModel(int id)
        {
            Part part = this.context.Parts.Find(id);
            EditPartViewModel model = new EditPartViewModel()
            {
                Id = part.Id,
                Name = part.Name,
                Price = part.Price,
                Quantity = part.Quantity
            };

            return model;
        }

        public void EditPart(EditPartBindingModel model)
        {
            Part part = this.context.Parts.Find(model.Id);
            part.Price = model.Price;
            part.Quantity = model.Quantity;

            this.context.SaveChanges();
        }

        public void DeletePart(DeleteBindingModel model)
        {
            Part part = this.context.Parts.Find(model.Id);
            this.context.Parts.Remove(part);
            this.context.SaveChanges();
        }
    }
}
