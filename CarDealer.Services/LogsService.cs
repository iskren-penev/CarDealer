namespace CarDealer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Models;
    using CarDealer.Models.ViewModels;

    public class LogsService : Service
    {
        public AllLogsViewModel GetAllLogsPageVm(string username, int? page)
        {
            var currentPage = 1;
            if (page != null)
            {
                currentPage = page.Value;
            }

            IEnumerable<Log> logs;
            if (username != null)
            {
                logs = this.context.Logs.Where(log => log.User.Username == username);
            }
            else
            {
                logs = this.context.Logs;
            }

            int allLogPagesCount = logs.Count() / 20 + (logs.Count() % 20 == 0 ? 0 : 1);
            int logsTotake = 20;
            if (allLogPagesCount == currentPage)
            {
                logsTotake = logs.Count() % 20 == 0 ? 20 : logs.Count() % 20;
            }

            logs = logs.Skip((currentPage - 1) * 20).Take(logsTotake);

            List<LogViewModel> logVms = new List<LogViewModel>();
            foreach (Log log in logs)
            {
                logVms.Add(new LogViewModel()
                {
                    Operation = log.Operation,
                    ModfiedTable = log.ModifiedTableName,
                    UserName = log.User.Username,
                    ModifiedAt = log.ModifiedAt
                });
            }


            AllLogsViewModel pageVm = new AllLogsViewModel()
            {
                WantedUserName = username,
                CurrentPage = currentPage,
                TotalNumberOfPages = allLogPagesCount,
                Logs = logVms
            };

            return pageVm;
        }

        public void DeleteAllLogs()
        {
            this.context.Logs.RemoveRange(this.context.Logs);
            this.context.SaveChanges();
        }
    }
}
