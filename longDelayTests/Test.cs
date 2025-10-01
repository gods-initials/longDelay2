using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace longDelayTests
{
    public abstract class Test
    {
        private bool testSuccessful;
        private string error = "";
        private int testDuration;
        private CancellationTokenSource cts;
        public abstract Task Run();
        public abstract Dictionary<string, string> ReturnResults();
    }
}