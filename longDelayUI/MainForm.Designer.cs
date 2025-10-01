namespace longDelayUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtProductId;
        private System.Windows.Forms.ComboBox cmbTests;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIdLabel;
        private System.Windows.Forms.Label lblTestLabel;

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
            this.txtProductId = new System.Windows.Forms.TextBox();
            this.cmbTests = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblIdLabel = new System.Windows.Forms.Label();
            this.lblTestLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtProductId
            // 
            this.txtProductId.Location = new System.Drawing.Point(150, 20);
            this.txtProductId.Name = "txtProductId";
            this.txtProductId.Size = new System.Drawing.Size(250, 20);
            // 
            // cmbTests
            // 
            this.cmbTests.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTests.FormattingEnabled = true;
            this.cmbTests.Items.AddRange(new string[] {
                "Тест 1",
                "Тест 2",
                "Тест 3" });
            this.cmbTests.Location = new System.Drawing.Point(150, 60);
            this.cmbTests.Name = "cmbTests";
            this.cmbTests.Size = new System.Drawing.Size(250, 21);
            this.cmbTests.SelectedIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(420, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 30);
            this.btnStart.Text = "Запустить";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(420, 60);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 30);
            this.btnStop.Text = "Остановить";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 110);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 13);
            this.lblStatus.Text = "Статус: ожидается";
            // 
            // lblIdLabel
            // 
            this.lblIdLabel.AutoSize = true;
            this.lblIdLabel.Location = new System.Drawing.Point(20, 23);
            this.lblIdLabel.Text = "Идентификатор:";
            // 
            // lblTestLabel
            // 
            this.lblTestLabel.AutoSize = true;
            this.lblTestLabel.Location = new System.Drawing.Point(20, 63);
            this.lblTestLabel.Text = "Выберите тест:";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(550, 160);
            this.Controls.Add(this.lblTestLabel);
            this.Controls.Add(this.lblIdLabel);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.cmbTests);
            this.Controls.Add(this.txtProductId);
            this.Name = "MainForm";
            this.Text = "Тесты";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}