using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NWBA_Web_Application.BackgroundService
{
    public abstract class HostedService : IHostedService
    {
        //Reference to task being done as well as the cancellation token
        private Task _taskBeingDone;
        private CancellationTokenSource _cancelToken;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Creates a cancel token and store it, we trigger cancellation outside of this token's cancellation
            _cancelToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Stores a reference to task being done
            _taskBeingDone = ExecuteAsync(_cancelToken.Token);

            // If task is done return it
            return _taskBeingDone.IsCompleted ? _taskBeingDone : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping");
            // Cancel if the task hasn't actually been started
            if (_taskBeingDone == null)
            {
                return;
            }

            // Use the cancel token to cancel the task
            _cancelToken.Cancel();

            // wait until the task completes or the stop token triggers
            await Task.WhenAny(_taskBeingDone, Task.Delay(-1, cancellationToken));

            // throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        }

        // Method that contains logic to be ran in background until cancellation is requested
        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
