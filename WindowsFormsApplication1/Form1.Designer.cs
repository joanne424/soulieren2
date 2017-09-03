namespace SendMSM
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.BtBrowser = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbTemplate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtParas = new System.Windows.Forms.TextBox();
            this.btSend = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.GVContracts = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.LbMsgs = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GVContracts)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(100, 17);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(298, 21);
            this.txtFilePath.TabIndex = 0;
            // 
            // BtBrowser
            // 
            this.BtBrowser.Location = new System.Drawing.Point(404, 16);
            this.BtBrowser.Name = "BtBrowser";
            this.BtBrowser.Size = new System.Drawing.Size(75, 23);
            this.BtBrowser.TabIndex = 1;
            this.BtBrowser.Text = "打开文件";
            this.BtBrowser.UseVisualStyleBackColor = true;
            this.BtBrowser.Click += new System.EventHandler(this.BtBrowser_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "文件路径：";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(99, 253);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(380, 79);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "内容预览：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "短信模板：";
            // 
            // cbTemplate
            // 
            this.cbTemplate.FormattingEnabled = true;
            this.cbTemplate.Location = new System.Drawing.Point(101, 175);
            this.cbTemplate.Name = "cbTemplate";
            this.cbTemplate.Size = new System.Drawing.Size(378, 20);
            this.cbTemplate.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "短信参数：";
            // 
            // txtParas
            // 
            this.txtParas.Location = new System.Drawing.Point(101, 213);
            this.txtParas.Name = "txtParas";
            this.txtParas.Size = new System.Drawing.Size(378, 21);
            this.txtParas.TabIndex = 6;
            this.txtParas.Text = "{\"order_id\":\"211\"}";
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(227, 353);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(75, 23);
            this.btSend.TabIndex = 7;
            this.btSend.Text = "发送短信";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // GVContracts
            // 
            this.GVContracts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GVContracts.Location = new System.Drawing.Point(100, 45);
            this.GVContracts.Name = "GVContracts";
            this.GVContracts.RowHeadersVisible = false;
            this.GVContracts.RowTemplate.Height = 23;
            this.GVContracts.Size = new System.Drawing.Size(379, 124);
            this.GVContracts.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "短信模板：";
            // 
            // LbMsgs
            // 
            this.LbMsgs.FormattingEnabled = true;
            this.LbMsgs.ItemHeight = 12;
            this.LbMsgs.Location = new System.Drawing.Point(100, 391);
            this.LbMsgs.Name = "LbMsgs";
            this.LbMsgs.Size = new System.Drawing.Size(379, 148);
            this.LbMsgs.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 404);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "发送结果";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 549);
            this.Controls.Add(this.LbMsgs);
            this.Controls.Add(this.GVContracts);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.txtParas);
            this.Controls.Add(this.cbTemplate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtBrowser);
            this.Controls.Add(this.txtFilePath);
            this.Name = "Form1";
            this.Text = "短信发送器";
            ((System.ComponentModel.ISupportInitialize)(this.GVContracts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button BtBrowser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbTemplate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtParas;
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView GVContracts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox LbMsgs;
        private System.Windows.Forms.Label label6;
    }
}

