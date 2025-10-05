using longDelayTests.TestStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace longDelayTests
{
    public class Test5 : Test
    {
        private CancellationTokenSource cts;
        public Test5() : base()
        {
            testName = "Test 5";
            testStages = new List<TestStage>
            {
                new TestStageInt(tmpPath) {stageName = "stageInt1"},
                new TestStageString(tmpPath) {stageName = "stageString1"},
                new TestStageInt(tmpPath) { stageName = "stageInt2" },
            };
        }
    }
}
