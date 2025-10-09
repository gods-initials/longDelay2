using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace longDelayTests.TestStages
{
    public abstract class TestStage
    {
        private Random rand;
        public string stageName;
        protected int stageDuration;
        public bool stageSuccessful;
        public string stageError;

        public event Action<TestStage> StageCompleted;
        public event Action<TestStage> StageFailed;
        public virtual object StageOutput { get; set; }
        public TestStage(string path)
        {
            stageSuccessful = false;
        }
        public abstract void DoStageSpecific();
        protected void OnStageCompleted()
        {
            StageCompleted.Invoke(this);
        }
        protected void OnStageFailed()
        {
            StageFailed.Invoke(this);
            throw new OperationCanceledException();
        }
        public async Task RunStage(CancellationTokenSource cts)
        {
            rand = new Random();
            stageError = "";
            if (!stageSuccessful)
            {
                try
                {
                    await Task.Delay(stageDuration, cts.Token);
                    stageSuccessful = Convert.ToBoolean(rand.Next(2));
                    if (stageSuccessful)
                    {
                        DoStageSpecific();
                        OnStageCompleted();
                    }
                    else
                    {
                        stageError = "Произошла ошибка";
                        OnStageFailed();
                    }
                }
                catch (TaskCanceledException)
                {
                    throw new OperationCanceledException(cts.Token);
                }
            }
            else
            {
                return;
            }
        }
    }
}
