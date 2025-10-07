using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using longDelayTests.Tests;

namespace longDelayTests.TestQueue
{
    public class TestQueue
    {
        private readonly Queue<Test> queue;
        private CancellationTokenSource cts;

        public bool IsRunning { get; private set; }
        public bool IsPaused => cts?.IsCancellationRequested ?? false;

        public TestQueue(IEnumerable<Test> tests)
        {
            queue = new Queue<Test>(tests);
            cts = new CancellationTokenSource();
        }

        public async Task RunQueue()
        {
            IsRunning = true;
            cts = new CancellationTokenSource();
            while (queue.Count > 0)
            {
                var test = queue.Peek();
                await test.Run(cts);
            }
        }
        
    }
}