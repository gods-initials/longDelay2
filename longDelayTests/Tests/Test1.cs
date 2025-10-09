using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using longDelayTests.TestStages;

namespace longDelayTests.Tests
{
    public class Test1 : Test
    {
        private CancellationTokenSource cts;
        public Test1() : base()
        {
            testName = "Test 1";
            testStages = new List<TestStage>
            {
                new TestStageInt(tmpPath) {stageName = "stageInt1"},
                //new TestStageString(tmpPath) {stageName = "stageString1"},
                //new TestStageInt(tmpPath) {stageName = "stageInt2"},
            };
        }
    }
}