using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace longDelayTests.TestStages
{
    internal class TestStageInt : TestStage
    {
        private Random rand;
        private int _stageOutput;
        public TestStageInt(string path) : base(path)
        {
            stageName = "stageInt";
            stageSuccessful = false;
            stageDuration = 500;
        }
        public override object StageOutput
        {
            get => _stageOutput;
            set => _stageOutput = (int)value;
        }
        public override void DoStageSpecific()
        {
            rand = new Random();
            StageOutput = rand.Next(0, 100);
        }
    }
}