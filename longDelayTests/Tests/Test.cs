using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using longDelayTests.TestStages;

namespace longDelayTests.Tests
{
    public abstract class Test
    {
        public List<TestStage> testStages;
        public bool testCompleted;

        private CancellationTokenSource cts;
        protected string tmpPath;
        public string testName;
        public async Task Run(CancellationTokenSource cts)
        {
            if (!File.Exists(tmpPath))
            {
                File.WriteAllText(tmpPath, "[]");
            }
            foreach (var stage in testStages)
            {
                await stage.RunStage(cts);
            }
        }
        public Test()
        {
            string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(baseFolder, "longDelay2", "temp");
            Directory.CreateDirectory(appFolder);
            tmpPath = Path.Combine(appFolder, $"{Guid.NewGuid().ToString()}.tmp");
        }
    }
}