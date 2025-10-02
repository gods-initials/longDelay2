using longDelayTests;
using System.Threading;
using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace longDelayUI
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource cts = null;
        private Test currentTest;
        private BindingList<TestOption> testOptions;
        private BindingList<TestOption> testsSelected;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            string productId = txtProductId.Text.Trim();
            if (string.IsNullOrEmpty(productId))
            {
                MessageBox.Show("Введите идентификатор изделия.");
                return;
            }
            if (testsSelected.Count == 0)
            {
                MessageBox.Show("Выберите один или несколько тестов.");
                return;
            }
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lblStatus.Text = "Статус: выполняется тест";
            cts = new CancellationTokenSource();
            foreach (TestOption testOption in testsSelected)
            {
                currentTest = testOption.Test;
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
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    cts = null;
                    lblStatus.Text = "Статус: ожидается";
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
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            if (cts != null)
            {
                cts.Cancel();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            testOptions = new BindingList<TestOption>
            {
                new TestOption { TestName = "Test 1", Test = new Test1() },
                //new TestOption { TestName = "Test 2", Test = new Test2() },
                //new TestOption { TestName = "Test 3", Test = new Test3() },
            };
            availableTestsGridView.AutoGenerateColumns = false;
            availableTestsGridView.DataSource = testOptions;

            testsSelected = new BindingList<TestOption> { };
            selectedTestsGridView.AutoGenerateColumns = false;
            selectedTestsGridView.DataSource = testsSelected;
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
    }
    public class TestOption
    {
        public string TestName { get; set; }
        public Test Test { get; set; }
    }
}