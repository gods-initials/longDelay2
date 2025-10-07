using longDelayTests.TestStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace longDelayTests.Tests
{
    public class Test3 : Test
    {
        private CancellationTokenSource cts;
        public Test3() : base()
        {
            testName = "Test 3";
            testStages = new List<TestStage>
            {
                new TestStageString(tmpPath) {stageName = "stageString1"},
                new TestStageString(tmpPath) {stageName = "stageString2"},
            };
        }
    }
}
