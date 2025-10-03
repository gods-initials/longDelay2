using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using longDelayTests.TestStages;

namespace longDelayTests
{
    public class Test2 : Test
    {
        private CancellationTokenSource cts;
        public Test2() : base()
        {
            testName = "Test 2";
            testStages = new List<TestStage>
            {
                new TestStageString(tmpPath) {stageName = "stageString1"},
                new TestStageString(tmpPath) {stageName = "stageString2"},
                new TestStageString(tmpPath) {stageName = "stageString3"},
                new TestStageInt(tmpPath) { stageName = "stageInt1" },
                new TestStageInt(tmpPath) { stageName = "stageInt2" },
                /*
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