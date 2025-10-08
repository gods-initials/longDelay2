using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using longDelayTests.TestStages;

namespace longDelayTests.Tests
{
    public abstract class Test
    {
        public List<TestStage> testStages;
        public bool testCompleted;
        private bool eventsLinked;

        public event Action<Test, TestStage> StageCompleted;
        public event Action<Test, TestStage> StageFailed;

        private CancellationTokenSource cts;
        protected string tmpPath;
        public string testName;
        public int testReruns;
        public async Task Run(CancellationTokenSource cts)
        {
            if (!eventsLinked)
            {
                foreach (var stage in testStages)
                {
                    stage.StageCompleted += s =>
                    {
                        StageCompleted(this, s);
                    };
                    stage.StageFailed += s =>
                    {
                        StageFailed(this, s);
                        testReruns++;
                    };
                }
                eventsLinked = true;
            }
            /*
            if (!File.Exists(tmpPath))
            {
                File.WriteAllText(tmpPath, "[]");
            }
            */
            foreach (var stage in testStages)
            {
                await stage.RunStage(cts);
            }
        }
        public Test()
        {
            eventsLinked = false;
            testReruns = 0;
            /*
            string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(baseFolder, "longDelay2", "temp");
            Directory.CreateDirectory(appFolder);
            tmpPath = Path.Combine(appFolder, $"{Guid.NewGuid().ToString()}.tmp");
            */
        }
    }
}