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
        private int testReruns;

        private void RegenerateTestOptions()
        {
            testOptions = new BindingList<TestOption>
            {
                new TestOption { TestName = "Test 1", CreateTest = () => new Test1(), },
                new TestOption { TestName = "Test 2", CreateTest = () => new Test2(), },
                new TestOption { TestName = "Test 3", CreateTest = () => new Test3(), },
                new TestOption { TestName = "Test 4", CreateTest = () => new Test4(), },
                new TestOption { TestName = "Test 5", CreateTest = () => new Test5(), },
            };
            testsSelected = new BindingList<TestOption> { };
            testsCompleted = new BindingList<TestOutput> { };
            availableTestsGridView.DataSource = testOptions;
            selectedTestsGridView.DataSource = testsSelected;
            testsCompletedGridView.DataSource = testsCompleted;
            txtProductId.Clear();
        }
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

        public void ExportResults()
        {
            string path = Directory.GetCurrentDirectory();
            if (!Directory.Exists(path + "\\results"))
            {
                Directory.CreateDirectory(path + "\\results");
            }
            using (StreamWriter outputFile = new StreamWriter(path + $"\\results\\{selectedProductID}.txt"))
            {
                foreach (var test in testsExecuting)
                {
                    outputFile.WriteLine($"Результаты {test.testName}:");
                    foreach (var stage in test.testStages)
                    {
                        if (stage.stageSuccessful) outputFile.WriteLine($"Результаты {stage.stageName}: {stage.StageOutput}");
                        else outputFile.WriteLine($"Результаты {stage.stageName}: {stage.stageError}");
                    }
                }
                lblStatus.Text = "Статус: тест завершён. Результаты сохранены.";
            }
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
                buttonPause_Click(null, null);
                if (testReruns == 0) 
                {
                    DialogResult result = MessageBox.Show(
                        "Этап теста завершился с ошибкой. Повторить?",
                        "Ошибка",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error
                    );
                    if (result == DialogResult.Yes)
                    {
                        continueButton_Click(null, null);
                        testReruns++;
                    }
                    else
                    {
                        testsExecuting.RemoveAt(0);
                    }
                }
                else
                {
                    testsExecuting.RemoveAt(0);
                }
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
            addTestButton.Enabled = false;
            removeTestButton.Enabled = false;
            moveDownButton.Enabled = false;
            moveUpButton.Enabled = false;
        }
        private async void continueButton_Click(object sender, EventArgs e)
        {
            buttonContinue.Enabled = false;
            buttonCancel.Enabled = false;
            buttonPause.Enabled = true;
            try
            {
                cts = new CancellationTokenSource();
                foreach (Test test in testsExecuting)
                {
                    testReruns = 0;
                    lblStatus.Text = "Статус: выполняется тест";
                    currentTest = test;
                    await currentTest.Run(cts);
                }
                buttonCancel_Click(null, null);
                ExportResults();
                lblStatus.Text = "Статус: ожидается";
                cts = null;
            }
            catch (OperationCanceledException)
            {
                lblStatus.Text = "Статус: ожидается";
                if (testsExecuting.Count == 0)
                {
                    buttonCancel_Click(null, null);
                }
            }
            
        }
        

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            buttonCreateQueue.Enabled = true;
            buttonCancel.Enabled = false;
            buttonContinue.Enabled = false;
            buttonPause.Enabled = false;
            RegenerateTestOptions();
            addTestButton.Enabled = true;
            removeTestButton.Enabled = true;
            moveDownButton.Enabled = true;
            moveUpButton.Enabled = true;
        }
        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
            }
            buttonPause.Enabled = false;
            buttonContinue.Enabled = true;
            buttonCancel.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            availableTestsGridView.AutoGenerateColumns = false;
            selectedTestsGridView.AutoGenerateColumns = false;
            testsCompletedGridView.AutoGenerateColumns = false;
            RegenerateTestOptions();

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

        private void txtProductId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}