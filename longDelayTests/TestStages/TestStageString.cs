using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace longDelayTests.TestStages
{
    internal class TestStageString : TestStage
    {
        private Random rand;
        private string _stageOutput;
        public TestStageString(string path) : base(path)
        {
            stageName = "stageString";
            stageSuccessful = false;
            stageDuration = 500;
        }
        public override object StageOutput
        {
            get => _stageOutput;
            set => _stageOutput = (string)value;
        }
        private string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, rand.Next(1, 11)).Select(s => s[rand.Next(s.Length)]).ToArray());
        }
        public override void DoStageSpecific()
        {
            rand = new Random();
            StageOutput = RandomString();
        }
    }
}
