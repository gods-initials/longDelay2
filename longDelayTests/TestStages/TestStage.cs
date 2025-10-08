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

        }
        public abstract void DoStageSpecific();
        protected void OnStageCompleted()
        {
            StageCompleted.Invoke(this);
        }
        protected void OnStageFailed()
        {
            StageFailed.Invoke(this);
        }
        public async Task RunStage(CancellationTokenSource cts)
        {
            rand = new Random();
            stageError = "";
            if (!stageSuccessful)
            {
                await Task.Delay(stageDuration, cts.Token);
                stageSuccessful = Convert.ToBoolean(rand.Next(1));
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
            else
            {
                return;
            }
        }
    }
}