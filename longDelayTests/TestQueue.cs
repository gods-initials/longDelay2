using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using longDelayTests.Tests;
using longDelayTests.TestStages;

namespace longDelayTests.TestQueue
{
    public class TestQueue
    {
        private Queue<Test> queue;
        private Queue<Test> exportQueue;
        private CancellationTokenSource cts;
        private readonly string selectedProductID;

        public event Action<Test, TestStage> StageCompleted;
        public event Action<Test, TestStage> StageFailed;
        public event Action<Test> TestFailed;
        // IF MULTIPLE QUEUES ARE SUPPORTED THEN REWRITE ACCORDINGLY
        public event Action QueuePaused;
        public event Action QueueCancelled;

        public bool IsRunning { get; private set; }
        public bool IsPaused => cts?.IsCancellationRequested ?? false;

        public TestQueue(string id, IEnumerable<Test> tests) : this(id)
        {
            foreach (var test in tests)
            {
                Add(test);
            }
        }
        public TestQueue(string id)
        {
            //StageFailed += (t, s) => Pause();
            selectedProductID = id;
            IsRunning = false;
            queue = new Queue<Test>();
            exportQueue = new Queue<Test>();
        }
        public void Add(Test test)
        {
            queue.Enqueue(test);
            test.StageCompleted += (t, s) => StageCompleted(t, s);
            test.StageFailed += (t, s) =>
            {
                if (test.testReruns > 0)
                {
                    OnTestFailed(t);
                }
                else
                {
                    OnStageFailed(t, s);
                }
            };
            
        }
        public void Pop() => queue.Dequeue();
        public async Task RunQueue()
        {
            IsRunning = true;
            cts = new CancellationTokenSource();
            while (queue.Count > 0)
            {
                var test = queue.Peek();
                try
                {
                    await test.Run(cts);
                    exportQueue.Enqueue(queue.Peek());
                    queue.Dequeue();
                }
                catch (OperationCanceledException)
                {
                    IsRunning = false;
                    if (test.testReruns > 0)
                    {
                        OnTestFailed(test);
                    }
                    else
                    {
                        return;
                    }                        
                }
            }
            ExportResults();
            QueueCancelled?.Invoke();
        }
        public void ExportResults()
        {
            string path = Directory.GetCurrentDirectory();
            if (!Directory.Exists(path + "\\results"))
            {
                Directory.CreateDirectory(path + "\\results");
            }
            using (StreamWriter outputFile = new StreamWriter(path + $"\\results\\{selectedProductID}.txt"))
            {
                while (exportQueue.Count > 0)
                {
                    var test = exportQueue.Dequeue();
                    outputFile.WriteLine($"{test.testName}:");
                    foreach (var stage in test.testStages)
                    {
                        if (stage.stageSuccessful) outputFile.WriteLine($"{stage.stageName}: {stage.StageOutput}");
                        else outputFile.WriteLine($"{stage.stageName}: {stage.stageError}");
                    }
                    outputFile.WriteLine();
                }
            }
        }
        public void Pause()
        {
            cts.Cancel();
            QueuePaused.Invoke();
        }
        public void Cancel()
        {
            queue.Clear();
            QueueCancelled.Invoke();
        }
        public void OnTestFailed(Test test)
        {
            exportQueue.Enqueue(queue.Peek());
            queue.Dequeue();
            TestFailed?.Invoke(test);
        }
        public void OnStageFailed(Test test, TestStage stage)
        {
            StageFailed?.Invoke(test, stage);
        }
    }
}