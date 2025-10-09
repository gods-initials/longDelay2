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
        public int testFails;

        public async Task Run(CancellationTokenSource cts)
        {
            if (!eventsLinked)
            {
                foreach (var stage in testStages)
                {
                    stage.StageCompleted += s => OnStageCompleted(s);
                    stage.StageFailed += s => OnStageFailed(s);
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
                if (!stage.stageSuccessful)
                {
                    break;
                }
            }
        }
        private void OnStageCompleted(TestStage stage)
        {
            StageCompleted.Invoke(this, stage);
        }
        private void OnStageFailed(TestStage stage)
        {
            testFails++;
            StageFailed.Invoke(this, stage);
        }
    }
}