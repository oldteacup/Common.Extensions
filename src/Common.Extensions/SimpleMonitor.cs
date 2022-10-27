using System;
using System.Threading;

namespace Common.Extensions
{
    public class SimpleMonitor : IDisposable
    {
        private int _busyCount;

        public bool Busy
        {
            get
            {
                return _busyCount > 0;
            }
        }

        public void Enter()
        {
            Interlocked.Increment(ref _busyCount);
        }

        public void Dispose()
        {
            Interlocked.Decrement(ref _busyCount);
        }
    }
}
