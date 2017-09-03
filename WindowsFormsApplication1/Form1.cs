using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Infrastructure.Log;

namespace SendMSM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Binding();
        }

        private void Binding()
        {
            cbTemplate.DataSource = SmsMessage.GetTemplate();
            cbTemplate.DisplayMember = "Name";
            cbTemplate.ValueMember = "Id";
        }

        private void BtBrowser_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "表格文件|*.xlsx|表格文件|*.xls|表格文件|*.csv";
            openFile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\contracts";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFile.FileName;
            }

            this.ReadContracts();
        }

        private void ReadContracts()
        {
            string[] aryLine = null;

            var filePath = txtFilePath.Text;
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            ContactsDomain.Instance.ReadContractsByFile(filePath);
            GVContracts.DataSource = null;
            GVContracts.DataSource = ContactsDomain.Instance.ContractEntities;
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            SmsOpt.Instance.Send(ContactsDomain.Instance.ContractEntities, this.cbTemplate.SelectedValue.ToString(), this.txtParas.Text, this.LbMsgs);
        }
    }
}