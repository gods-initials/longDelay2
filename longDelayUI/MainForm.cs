using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using longDelayTests;
using longDelayTests.TestStages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace longDelayUI
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource cts = null;
        private Test currentTest;
        string selectedProductID;
        private BindingList<TestOption> testOptions;
        private BindingList<TestOption> testsSelected;
        private List<Test> testsExecuting;
        private BindingList<TestOutput> testsCompleted;
        private bool eventsLinked = false;

        public struct TestOutput
        {
            public string ProductID { get; set; }
            public string TestName { get; set; }
            public string TestStageName { get; set; }
            public string TestStageResult { get; set; }
        }
        public class TestOption
        {
            public string TestName { get; set; }
            public Func<Test> CreateTest { get; set; }
        }

        public MainForm()
        {
            InitializeComponent();
        }
        public void StageCompletedResponse(TestStage stageObj)
        {
            var json = JObject.FromObject(stageObj);
            var newEntry = new TestOutput
            {
                ProductID = selectedProductID,
                TestStageName = json["stageName"].ToString(),
                TestName = currentTest.testName,
            };
            if (Convert.ToBoolean(json["stageSuccessful"]) == true)
            {
                newEntry.TestStageResult = json["StageOutput"].ToString();
            }
            else
            {
                newEntry.TestStageResult = json["stageError"].ToString();
                MessageBox.Show("фейл");
                buttonPause_Click(null, null);
            }
            testsCompleted.Add(newEntry);
        }

        private void buttonCreateQueue_Click(object sender, EventArgs e)
        {
            selectedProductID = txtProductId.Text.Trim();
            if (string.IsNullOrEmpty(selectedProductID))
            {
                MessageBox.Show("Введите идентификатор изделия.");
                return;
            }
            if (testsSelected.Count == 0)
            {
                MessageBox.Show("Выберите один или несколько тестов.");
                return;
            }
            testsExecuting = new List<Test> { };
            foreach (var testOption in testsSelected)
            {
                Test newTest = testOption.CreateTest();
                foreach (TestStage ts in newTest.testStages)
                {
                    ts.StageCompleted += StageCompletedResponse;
                }
                testsExecuting.Add(newTest);
            }
            buttonCreateQueue.Enabled = false;
            buttonCancel.Enabled = true;
            buttonContinue.Enabled = true;
        }
        private async void continueButton_Click(object sender, EventArgs e)
        {
            buttonContinue.Enabled = false;
            buttonCancel.Enabled = false;
            buttonPause.Enabled = true;
            foreach (Test test in testsExecuting)
            {
                buttonCancel.Enabled = true;
                lblStatus.Text = "Статус: выполняется тест";
                cts = new CancellationTokenSource();
                currentTest = test;
                try
                {
                    await currentTest.Run(cts);
                }
                catch (OperationCanceledException)
                {
                    lblStatus.Text = "Статус: ожидается";
                }
                finally
                {
                    buttonCreateQueue.Enabled = true;
                    buttonCancel.Enabled = false;
                    cts = null;
                    lblStatus.Text = "Статус: ожидается";
                }
            }
        }
        /*
        try
        {

            Dictionary<string, string> results = currentTest.ReturnResults();
            string path = Directory.GetCurrentDirectory();
            if (!Directory.Exists(path + "\\results"))
            {
                Directory.CreateDirectory(path + "\\results");
            }
            using (StreamWriter outputFile = new StreamWriter(path + $"\\results\\{productId}.txt"))
            {
                if (results["testSuccessful"] == "True")
                {
                    foreach (string key in results.Keys.Skip(2))
                    {
                        outputFile.WriteLine($"{key}: {results[key]}");
                    }
                }
                else if (results["testSuccessful"] == "False")
                {
                    outputFile.WriteLine($"error: {results["error"]}");
                }
                lblStatus.Text = "Статус: тест завершён. Результаты сохранены.";
            }
        }
        catch (OperationCanceledException)
        {
            lblStatus.Text = "Статус: ожидается";
        }
        finally
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            cts = null;
        }
        */

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            buttonCreateQueue.Enabled = true;
            buttonCancel.Enabled = false;
            buttonContinue.Enabled = false;
            buttonPause.Enabled = false;
            if (cts != null)
            {
                cts.Cancel();
            }
        }
        private void buttonPause_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            testOptions = new BindingList<TestOption>
            {
                new TestOption { TestName = "Test 1", CreateTest = () => new Test1(), },
                new TestOption { TestName = "Test 2", CreateTest = () => new Test2(), },
                //new TestOption { TestName = "Test 3", Test = new Test3() },
            };
            availableTestsGridView.AutoGenerateColumns = false;
            availableTestsGridView.DataSource = testOptions;

            testsSelected = new BindingList<TestOption> { };
            selectedTestsGridView.AutoGenerateColumns = false;
            selectedTestsGridView.DataSource = testsSelected;

            testsCompleted = new BindingList<TestOutput> { };
            testsCompletedGridView.AutoGenerateColumns = false;
            testsCompletedGridView.DataSource = testsCompleted;

            string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(baseFolder, "longDelay2", "temp");
            string[] filePaths = Directory.GetFiles(appFolder);
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
        }

        private void availableTestsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void addTestButton_Click(object sender, EventArgs e)
        {
            if (testOptions.Count > 0)
            {
                int transferId = availableTestsGridView.CurrentCell.RowIndex;
                testsSelected.Add(testOptions[transferId]);
                testOptions.RemoveAt(transferId);
            }
        }

        private void selectedTestsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void removeTestButton_Click(object sender, EventArgs e)
        {
            if (testsSelected.Count > 0)
            {
                int transferId = selectedTestsGridView.CurrentCell.RowIndex;
                testOptions.Add(testsSelected[transferId]);
                testsSelected.RemoveAt(transferId);
            }
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            if (testsSelected.Count > 1 && selectedTestsGridView.CurrentCell.RowIndex > 0)
            {
                int transferId = selectedTestsGridView.CurrentCell.RowIndex;
                var temp = testsSelected[transferId - 1];
                testsSelected[transferId - 1] = testsSelected[transferId];
                testsSelected[transferId] = temp;
                selectedTestsGridView.Rows[transferId].Selected = false;
                selectedTestsGridView.Rows[transferId - 1].Selected = true;
                selectedTestsGridView.CurrentCell = selectedTestsGridView.Rows[transferId - 1].Cells[0];
            }
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            if (testsSelected.Count > 1 && selectedTestsGridView.CurrentCell.RowIndex < testsSelected.Count-1)
            {
                int transferId = selectedTestsGridView.CurrentCell.RowIndex;
                var temp = testsSelected[transferId + 1];
                testsSelected[transferId + 1] = testsSelected[transferId];
                testsSelected[transferId] = temp;
                selectedTestsGridView.Rows[transferId].Selected = false;
                selectedTestsGridView.Rows[transferId + 1].Selected = true;
                selectedTestsGridView.CurrentCell = selectedTestsGridView.Rows[transferId + 1].Cells[0];
            }
        }

        private void resultsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}