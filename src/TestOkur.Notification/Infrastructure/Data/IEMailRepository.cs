namespace TestOkur.Notification.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface IEMailRepository
    {
        Task AddAsync(EMail email);

        Task<List<EMail>> GetEmailsAsync(DateTime from, DateTime to);
    }
}