namespace longDelayUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button buttonCreateQueue;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIdLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.buttonCreateQueue = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblIdLabel = new System.Windows.Forms.Label();
            this.testsCompletedGridView = new System.Windows.Forms.DataGridView();
            this.productID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testStageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testStageResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtProductId = new System.Windows.Forms.TextBox();
            this.availableTestsGridView = new System.Windows.Forms.DataGridView();
            this.availableTestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectedTestsGridView = new System.Windows.Forms.DataGridView();
            this.selectedTestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addTestButton = new System.Windows.Forms.Button();
            this.removeTestButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.testsCompletedGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availableTestsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectedTestsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCreateQueue
            // 
            this.buttonCreateQueue.Location = new System.Drawing.Point(23, 213);
            this.buttonCreateQueue.Name = "buttonCreateQueue";
            this.buttonCreateQueue.Size = new System.Drawing.Size(99, 30);
            this.buttonCreateQueue.TabIndex = 4;
            this.buttonCreateQueue.Text = "Выбрать тесты";
            this.buttonCreateQueue.Click += new System.EventHandler(this.buttonCreateQueue_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(163, 213);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 30);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Отменить";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 246);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(102, 13);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Статус: ожидается";
            // 
            // lblIdLabel
            // 
            this.lblIdLabel.AutoSize = true;
            this.lblIdLabel.Location = new System.Drawing.Point(20, 23);
            this.lblIdLabel.Name = "lblIdLabel";
            this.lblIdLabel.Size = new System.Drawing.Size(90, 13);
            this.lblIdLabel.TabIndex = 1;
            this.lblIdLabel.Text = "Идентификатор:";
            // 
            // testsCompletedGridView
            // 
            this.testsCompletedGridView.AllowUserToAddRows = false;
            this.testsCompletedGridView.AllowUserToDeleteRows = false;
            this.testsCompletedGridView.AllowUserToResizeColumns = false;
            this.testsCompletedGridView.AllowUserToResizeRows = false;
            this.testsCompletedGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.testsCompletedGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testsCompletedGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productID,
            this.TestName,
            this.testStageName,
            this.testStageResult});
            this.testsCompletedGridView.Location = new System.Drawing.Point(23, 274);
            this.testsCompletedGridView.Name = "testsCompletedGridView";
            this.testsCompletedGridView.ReadOnly = true;
            this.testsCompletedGridView.RowHeadersVisible = false;
            this.testsCompletedGridView.Size = new System.Drawing.Size(561, 150);
            this.testsCompletedGridView.TabIndex = 7;
            this.testsCompletedGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultsGridView_CellContentClick);
            // 
            // productID
            // 
            this.productID.DataPropertyName = "ProductID";
            this.productID.HeaderText = "№ изделия";
            this.productID.Name = "productID";
            this.productID.ReadOnly = true;
            // 
            // TestName
            // 
            this.TestName.DataPropertyName = "TestName";
            this.TestName.HeaderText = "Тест";
            this.TestName.Name = "TestName";
            this.TestName.ReadOnly = true;
            // 
            // testStageName
            // 
            this.testStageName.DataPropertyName = "TestStageName";
            this.testStageName.HeaderText = "Этап";
            this.testStageName.Name = "testStageName";
            this.testStageName.ReadOnly = true;
            // 
            // testStageResult
            // 
            this.testStageResult.DataPropertyName = "TestStageResult";
            this.testStageResult.HeaderText = "Результат";
            this.testStageResult.Name = "testStageResult";
            this.testStageResult.ReadOnly = true;
            // 
            // txtProductId
            // 
            this.txtProductId.Location = new System.Drawing.Point(116, 20);
            this.txtProductId.Name = "txtProductId";
            this.txtProductId.Size = new System.Drawing.Size(250, 20);
            this.txtProductId.TabIndex = 6;
            // 
            // availableTestsGridView
            // 
            this.availableTestsGridView.AllowUserToAddRows = false;
            this.availableTestsGridView.AllowUserToDeleteRows = false;
            this.availableTestsGridView.AllowUserToResizeColumns = false;
            this.availableTestsGridView.AllowUserToResizeRows = false;
            this.availableTestsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.availableTestsGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.availableTestsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.availableTestsGridView.ColumnHeadersVisible = false;
            this.availableTestsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.availableTestName});
            this.availableTestsGridView.Location = new System.Drawing.Point(23, 57);
            this.availableTestsGridView.MultiSelect = false;
            this.availableTestsGridView.Name = "availableTestsGridView";
            this.availableTestsGridView.ReadOnly = true;
            this.availableTestsGridView.RowHeadersVisible = false;
            this.availableTestsGridView.Size = new System.Drawing.Size(240, 150);
            this.availableTestsGridView.TabIndex = 8;
            this.availableTestsGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.availableTestsGridView_CellContentClick);
            // 
            // availableTestName
            // 
            this.availableTestName.DataPropertyName = "TestName";
            this.availableTestName.HeaderText = "TestName";
            this.availableTestName.Name = "availableTestName";
            this.availableTestName.ReadOnly = true;
            // 
            // selectedTestsGridView
            // 
            this.selectedTestsGridView.AllowUserToAddRows = false;
            this.selectedTestsGridView.AllowUserToDeleteRows = false;
            this.selectedTestsGridView.AllowUserToResizeColumns = false;
            this.selectedTestsGridView.AllowUserToResizeRows = false;
            this.selectedTestsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.selectedTestsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.selectedTestsGridView.ColumnHeadersVisible = false;
            this.selectedTestsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectedTestName});
            this.selectedTestsGridView.Location = new System.Drawing.Point(344, 57);
            this.selectedTestsGridView.MultiSelect = false;
            this.selectedTestsGridView.Name = "selectedTestsGridView";
            this.selectedTestsGridView.ReadOnly = true;
            this.selectedTestsGridView.RowHeadersVisible = false;
            this.selectedTestsGridView.Size = new System.Drawing.Size(240, 150);
            this.selectedTestsGridView.TabIndex = 9;
            this.selectedTestsGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.selectedTestsGridView_CellContentClick);
            // 
            // selectedTestName
            // 
            this.selectedTestName.DataPropertyName = "TestName";
            this.selectedTestName.HeaderText = "TestName";
            this.selectedTestName.Name = "selectedTestName";
            this.selectedTestName.ReadOnly = true;
            // 
            // addTestButton
            // 
            this.addTestButton.Location = new System.Drawing.Point(270, 70);
            this.addTestButton.Name = "addTestButton";
            this.addTestButton.Size = new System.Drawing.Size(68, 23);
            this.addTestButton.TabIndex = 10;
            this.addTestButton.Text = ">>>";
            this.addTestButton.UseVisualStyleBackColor = true;
            this.addTestButton.Click += new System.EventHandler(this.addTestButton_Click);
            // 
            // removeTestButton
            // 
            this.removeTestButton.Location = new System.Drawing.Point(270, 100);
            this.removeTestButton.Name = "removeTestButton";
            this.removeTestButton.Size = new System.Drawing.Size(68, 23);
            this.removeTestButton.TabIndex = 11;
            this.removeTestButton.Text = "<<<";
            this.removeTestButton.UseVisualStyleBackColor = true;
            this.removeTestButton.Click += new System.EventHandler(this.removeTestButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.moveUpButton.Location = new System.Drawing.Point(270, 130);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(26, 23);
            this.moveUpButton.TabIndex = 12;
            this.moveUpButton.Text = "↑";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.moveDownButton.Location = new System.Drawing.Point(311, 130);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(27, 23);
            this.moveDownButton.TabIndex = 13;
            this.moveDownButton.Text = "↓";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // buttonContinue
            // 
            this.buttonContinue.Enabled = false;
            this.buttonContinue.Location = new System.Drawing.Point(344, 213);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(88, 30);
            this.buttonContinue.TabIndex = 14;
            this.buttonContinue.Text = "Продолжить";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Enabled = false;
            this.buttonPause.Location = new System.Drawing.Point(509, 213);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 30);
            this.buttonPause.TabIndex = 15;
            this.buttonPause.Text = "Пауза";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(635, 596);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.removeTestButton);
            this.Controls.Add(this.addTestButton);
            this.Controls.Add(this.selectedTestsGridView);
            this.Controls.Add(this.availableTestsGridView);
            this.Controls.Add(this.testsCompletedGridView);
            this.Controls.Add(this.lblIdLabel);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCreateQueue);
            this.Controls.Add(this.txtProductId);
            this.Name = "MainForm";
            this.Text = " ";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.testsCompletedGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availableTestsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectedTestsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DataGridView testsCompletedGridView;
        private System.Windows.Forms.TextBox txtProductId;
        private System.Windows.Forms.DataGridView availableTestsGridView;
        private System.Windows.Forms.DataGridView selectedTestsGridView;
        private System.Windows.Forms.Button addTestButton;
        private System.Windows.Forms.Button removeTestButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn availableTestName;
        private System.Windows.Forms.DataGridViewTextBoxColumn selectedTestName;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.DataGridViewTextBoxColumn productID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestName;
        private System.Windows.Forms.DataGridViewTextBoxColumn testStageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn testStageResult;
    }
}