namespace CarDealer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Models;
    using CarDealer.Models.BindingModels;
    using CarDealer.Models.ViewModels;
    using CarDealer.Models.ViewModels.Supplier;

    public class SuppliersService : Service
    {
        public IEnumerable<SupplierViewModel> GetSuppliersByType(string type)
        {
            IEnumerable<Supplier> suppliersByType = GetSupplierModelsByType(type);
            IEnumerable<SupplierViewModel> viewModels = suppliersByType.Select(s => new SupplierViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                NumberOfParts = s.Parts.Count
            });
            return viewModels;
        }

        public IEnumerable<AllSupplierViewModel> GetSuppliersByTypeForUsers(string type)
        {
            IEnumerable<Supplier> suppliersByType = GetSupplierModelsByType(type);
            IEnumerable<AllSupplierViewModel> viewModels = suppliersByType.Select(s => new AllSupplierViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                IsImporter = s.IsImporter
            });
            return viewModels;
        }

        private IEnumerable<Supplier> GetSupplierModelsByType(string type)
        {
            IEnumerable<Supplier> suppliersWanted;
            if (type == null)
            {
                suppliersWanted = this.context.Suppliers;
            }
            else if (type.ToLower() == "local")
            {
                suppliersWanted = this.context.Suppliers.Where(supplier => !supplier.IsImporter);
            }
            else if (type.ToLower() == "importers")
            {
                suppliersWanted = this.context.Suppliers.Where(supplier => supplier.IsImporter);
            }
            else
            {
                throw new ArgumentException("Invalid argument for the type of the supplier!");
            }

            return suppliersWanted;
        }

        public void AddSupplier(AddSupplierBindingModel bind, int userId)
        {
            Supplier supplier = new Supplier()
            {
                Name = bind.Name,
                IsImporter = bind.IsImporter
            };
            this.context.Suppliers.Add(supplier);
            this.context.SaveChanges();
            this.AddLog(userId, OperationLog.Add, "suppliers");
        }

        private void AddLog(int userId, OperationLog operation, string modifiedTable)
        {
            User loggedUser = this.context.Users.Find(userId);
            Log log = new Log()
            {
                User = loggedUser,
                ModifiedAt = DateTime.Now,
                ModifiedTableName = modifiedTable,
                Operation = operation
            };

            this.context.Logs.Add(log);
            this.context.SaveChanges();
        }

        public EditSupplierViewModel GetEditSupplierViewModel(int id)
        {
            Supplier supplier = this.context.Suppliers.Find(id);
            EditSupplierViewModel vm = new EditSupplierViewModel()
            {
                Id = supplier.Id,
                Name = supplier.Name,
                IsImporter = supplier.IsImporter
            };
            return vm;
        }

        public void EditSupplier(EditSupplierBindingModel bind, int userId)
        {
            Supplier model = this.context.Suppliers.Find(bind.Id);
            model.IsImporter = bind.IsImporter == "on";
            model.Name = bind.Name;
            this.context.SaveChanges();

            this.AddLog(userId, OperationLog.Edit, "suppliers");
        }

        public DeleteSupplierViewModel GetDeleteSupplierViewModel(int id)
        {
            Supplier supplier = this.context.Suppliers.Find(id);
            DeleteSupplierViewModel vm = new DeleteSupplierViewModel()
            {
                Id = supplier.Id,
                Name = supplier.Name
            };
            return vm;
        }

        public void DeleteSupplier(DeleteSupplierBindingModel bind, int userId)
        {
            Supplier supplier = this.context.Suppliers.Find(bind.Id);
            this.context.Suppliers.Remove(supplier);
            this.context.SaveChanges();

            this.AddLog(userId, OperationLog.Delete, "suppliers");
        }
    }
}
