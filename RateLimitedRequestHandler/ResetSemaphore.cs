using System.Threading;

namespace RateLimitedRequestHandler {

    class ResetSemaphore {

        private readonly ushort maxThreads;
        private ushort availThreads;

        private readonly Semaphore localSemaphore;

        /// <summary>
        /// Constructs a semaphore with an automatic reset thread count. This is used to control Rate Limiting.
        /// </summary>
        /// <param name="avail">Number of requests that are initially available</param>
        /// <param name="max">Maximum number of requests available per timeout period</param>
        /// <param name="timeout">Number of milliseconds before thread counts reset</param>
        public ResetSemaphore(ushort avail, ushort max, int timeout) {
            this.availThreads = avail;
            this.maxThreads = max;
            this.localSemaphore = new Semaphore(avail, max);

            Thread t = new Thread(new ThreadStart(() => {
                while (true) {
                    Thread.Sleep(timeout);
                    try {
                        lock (this) {
                            if (availThreads < maxThreads) {
                                // Should only be releasing if items have been allocated
                                localSemaphore.Release(maxThreads - availThreads);
                                availThreads = maxThreads;
                            }
                        }
                    } catch (SemaphoreFullException) { }
                }
            })) {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
            t.Start();
        }

        /// <summary>
        /// Call to request entry to the Semaphore
        /// </summary>
        public void Wait() {
            localSemaphore.WaitOne();
            lock (this) {
                availThreads--;
            }
        }
    }
}
