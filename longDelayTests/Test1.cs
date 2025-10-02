using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using longDelayTests.TestStages;

namespace longDelayTests
{
    public class Test1 : Test
    {
        private int testDuration;
        private bool testSuccessful;
        private CancellationTokenSource cts;
        private string error;
        private float param1;
        public Test1() : base()
        {
            param1 = 0;
            error = "";
            testDuration = 10000;
            testStages = new List<TestStage>
            {
                new TestStageInt(tmpPath) {stageName = "stageInt1"},
                new TestStageString(tmpPath) {stageName = "stageString1"},
                /*
                new TestStageInt(tmpPath) {stageName = "stageInt2"},
                new TestStageInt(tmpPath) {stageName = "stageInt3"},
                new TestStageInt(tmpPath) {stageName = "stageInt4"},
                */
            };
        }
        public override async Task Run(CancellationTokenSource cts)
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
    }
}