namespace CarDealer.Services
{
    using CarDealer.Data;

    public abstract class Service
    {
        protected CarDealerContext context;

        public Service()
        {
            this.context = new CarDealerContext();
        }
    }
}
