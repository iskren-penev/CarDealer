namespace CarDealer.Models.ViewModels
{
    using System.Collections.Generic;

    public class AllLogsViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalNumberOfPages { get; set; }

        public IEnumerable<LogViewModel> Logs { get; set; }

        public string WantedUserName { get; set; }
    }
}
