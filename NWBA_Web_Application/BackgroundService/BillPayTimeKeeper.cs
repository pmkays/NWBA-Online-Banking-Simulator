using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Data;
using NWBA_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NWBA_Web_Application.BackgroundService
{
    public abstract class BillPayTimeKeeper : HostedService
    {

        public BillPayTimeKeeper()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Process();
                await Task.Delay(1000, cancellationToken); //5 seconds delay

            }
        }

        public Boolean AreEqual(DateTime d1, DateTime d2)
        {
            return d1.ToString("yyyyMMddHHmmss") == d2.ToString("yyyyMMddHHmmss");
        }

        protected abstract Task Process();
    }
}

