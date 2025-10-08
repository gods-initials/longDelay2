using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using longDelayTests.TestQueue;
using longDelayTests.Tests;
using longDelayTests.TestStages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace longDelayUI
{
    public partial class MainForm : Form
    {
        private string selectedProductID;
        private BindingList<TestOption> testOptions;
        private BindingList<TestOption> testsSelected;
        private TestQueue testQueue;
        private BindingList<TestOutput> testsCompleted;
        private SystemState currentSystemState;

        private enum SystemState
        {
            Idle,
            QueueCreated,
            Running,
            Paused,
        }
        public class TestOption
        {
            public string TestName { get; set; }
            public Func<Test> CreateTest { get; set; }
        }
        public struct TestOutput
        {
            public string ProductID { get; set; }
            public string TestName { get; set; }
            public string TestStageName { get; set; }
            public string TestStageResult { get; set; }
        }
        private string GetStateTitle(SystemState state)
        {
            string title = "";
            if (state == SystemState.Idle) title = "Статус: ожидается";
            if (state == SystemState.QueueCreated) title = "Статус: очередь создана";
            if (state == SystemState.Running) title = "Статус: выполняется тест";
            if (state == SystemState.Paused) title = "Статус: приостановлено";
            return title;
        }
        private void SetSystemState(SystemState state)
        {
            currentSystemState = state;

            lblStatus.Text = GetStateTitle(state);

            buttonCreateQueue.Enabled = currentSystemState == SystemState.Idle;
            buttonContinue.Enabled = currentSystemState == SystemState.QueueCreated || currentSystemState == SystemState.Paused;
            buttonPause.Enabled = currentSystemState == SystemState.Running;
            buttonCancel.Enabled = currentSystemState == SystemState.Paused || currentSystemState == SystemState.QueueCreated;

            addTestButton.Enabled = currentSystemState == SystemState.Idle;
            removeTestButton.Enabled = currentSystemState == SystemState.Idle;
            moveDownButton.Enabled = currentSystemState == SystemState.Idle;
            moveUpButton.Enabled = currentSystemState == SystemState.Idle;
        }
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
        public MainForm()
        {
            InitializeComponent();
        }
        public void AddEntry(Test test, TestStage stageObj)
        {
            var json = JObject.FromObject(stageObj);
            var newEntry = new TestOutput
            {
                ProductID = selectedProductID,
                TestStageName = json["stageName"].ToString(),
                TestName = test.testName,
            };
            if (Convert.ToBoolean(json["stageSuccessful"]) == true)
            {
                newEntry.TestStageResult = json["StageOutput"].ToString();
                testsCompleted.Add(newEntry);
            }
            else
            {
                newEntry.TestStageResult = json["stageError"].ToString();
                testsCompleted.Add(newEntry);
            }
        }
        public void StageFailedResponse(Test test, TestStage stageObj)
        {
            AddEntry(test, stageObj);
            DialogResult result = MessageBox.Show(
                "Этап теста завершился с ошибкой. Повторить?",
                "Ошибка",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error
            );
            if ( result == DialogResult.No )
            {
                testQueue.Pop();
            }
            SetSystemState(SystemState.Running);
            _ = testQueue.RunQueue();
        }
        public void StageCompletedResponse(Test test, TestStage stageObj)
        {
            AddEntry(test, stageObj);
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
            SetSystemState(SystemState.QueueCreated);
            testQueue = new TestQueue(selectedProductID);
            foreach (var testOption in testsSelected)
            {
                Test newTest = testOption.CreateTest();
                testQueue.Add(newTest);
            }
            testQueue.StageCompleted += (t, s) => StageCompletedResponse(t, s);
            testQueue.StageFailed += (t, s) => StageFailedResponse(t, s);
            testQueue.QueuePaused += () => SetSystemState(SystemState.Paused);
            testQueue.QueueCancelled += () =>
            {
                SetSystemState(SystemState.Idle);
                RegenerateTestOptions();
            };
        }
        private async void continueButton_Click(object sender, EventArgs e)
        {
            SetSystemState(SystemState.Running);
            await testQueue.RunQueue();
        }        
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            testQueue.Cancel();
        }
        private void buttonPause_Click(object sender, EventArgs e)
        {
            SetSystemState(SystemState.Paused);
            testQueue.Pause();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetSystemState(SystemState.Idle);
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
        private void addTestButton_Click(object sender, EventArgs e)
        {
            if (testOptions.Count > 0)
            {
                int transferId = availableTestsGridView.CurrentCell.RowIndex;
                testsSelected.Add(testOptions[transferId]);
                testOptions.RemoveAt(transferId);
            }
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
        private void availableTestsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void selectedTestsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}