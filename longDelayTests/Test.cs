using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using longDelayTests.TestStages;

namespace longDelayTests
{
    public abstract class Test
    {
        private bool testSuccessful;
        private string error = "";
        private int testDuration;

        public List<TestStage> testStages;

        private CancellationTokenSource cts;
        protected string tmpPath;
        public abstract Task Run(CancellationTokenSource tokenSource);
        public Test()
        {
            string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(baseFolder, "longDelay2", "temp");
            Directory.CreateDirectory(appFolder);
            tmpPath = Path.Combine(appFolder, $"{Guid.NewGuid().ToString()}.tmp");
        }
    }
}