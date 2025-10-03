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
            stageError = "";
            if (IsStageFinished())
            {
                Console.WriteLine($"{stageName} done");
            }
            else
            {
                await Task.Delay(stageDuration, cts.Token);
                stageSuccessful = Convert.ToBoolean(rand.Next(10));
                if (stageSuccessful)
                {
                    StageOutput = rand.Next(0, 100);
                    RecordStage();
                }
                else
                {
                    stageError = "Произошла ошибка";
                }                
                OnStageCompleted();
            }          
        }
    }
}
