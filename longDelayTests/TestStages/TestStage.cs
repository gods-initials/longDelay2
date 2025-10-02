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
        public string stageName;
        protected int stageDuration;
        public bool stageSuccessful;
        public string stageError;
        protected string tmpPath;
        public virtual object StageOutput { get; set; }
        public TestStage(string path)
        {
            tmpPath = path;
        }
        protected void RecordStage()
        {
            var existing = JArray.Parse(File.ReadAllText(tmpPath));
            existing.Add(JObject.FromObject(this));
            File.WriteAllText(tmpPath, existing.ToString(Formatting.Indented));
        }
        protected bool IsStageFinished()
        {
            var existing = JArray.Parse(File.ReadAllText(tmpPath));
            foreach (var item in existing)
            {
                if (item["stageName"].ToString() == stageName && Convert.ToBoolean(item["stageSuccessful"])==true)
                {
                    return false;
                }
            }
            return false;
        }
        public abstract Task RunStage(CancellationTokenSource tokenSource);
    }
}