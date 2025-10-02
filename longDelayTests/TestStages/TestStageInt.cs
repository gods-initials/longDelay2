using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace longDelayTests.TestStages
{
    internal class TestStageInt : TestStage
    {
        public Random rand;
        private int _stageOutput;
        public TestStageInt(string path) : base(path)
        {
            stageName = "stageInt";
            stageSuccessful = false;
            stageDuration = 3000;
        }
        public override object StageOutput
        {
            get => _stageOutput;
            set => _stageOutput = (int)value;
        }
        public override async Task RunStage(CancellationTokenSource cts)
        {
            rand = new Random();
            if (!IsStageFinished())
            {
                await Task.Delay(stageDuration, cts.Token);
                stageSuccessful = Convert.ToBoolean(rand.Next(10));
                if (stageSuccessful)
                {
                    StageOutput = rand.Next(0, 100);
                }
                else
                {
                    stageError = "Произошла ошибка";
                }
                RecordStage();
                OnStageCompleted();
            }            
        }
    }
}
