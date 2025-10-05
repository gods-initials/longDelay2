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
        protected string tmpPath;
        public event Action<TestStage> StageCompleted;
        public event Action<TestStage> StageFailed;
        public virtual object StageOutput { get; set; }
        public TestStage(string path)
        {
            tmpPath = path;
        }
        protected void RecordStage()
        {
            var existing = JArray.Parse(File.ReadAllText(tmpPath));
            var newJson = JObject.FromObject(this);
            newJson.Remove("rand");
            existing.Add(newJson);
            File.WriteAllText(tmpPath, existing.ToString(Formatting.Indented));
        }
        protected bool IsStageFinished()
        {
            var existing = JArray.Parse(File.ReadAllText(tmpPath));
            foreach (var item in existing)
            {
                if (item["stageName"].ToString() == stageName && Convert.ToBoolean(item["stageSuccessful"])==true)
                {
                    return true;
                }
            }
            return false;
        }
        public abstract void DoStageSpecific();
        protected void RemoveFailedStageEntry()
        {
            var existing = JArray.Parse(File.ReadAllText(tmpPath));
        }
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
            if (IsStageFinished())
            {
                Console.WriteLine($"{stageName} done");
            }
            else
            {
                await Task.Delay(stageDuration, cts.Token);
                stageSuccessful = Convert.ToBoolean(rand.Next(200));
                if (stageSuccessful)
                {
                    DoStageSpecific();
                    RecordStage();
                }
                else
                {
                    cts.Cancel();
                    stageError = "Произошла ошибка";
                }
                OnStageCompleted();
            }
        }
    }
}