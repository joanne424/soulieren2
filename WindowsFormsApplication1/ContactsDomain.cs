
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SendMSM
{
    public class ContactsDomain
    {
        private static ContactsDomain instance;

        public static ContactsDomain Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContactsDomain();
                }

                return instance;
            }

        }

        private ContactsDomain()
        {
            this.ContractEntities = new List<ContractEntity>();
        }

        public List<ContractEntity> ContractEntities { get; set; }

        public void ReadContractsByFile(string filePath)
        {
            try
            {
                if (ContractEntities.Any())
                {
                    ContractEntities.Clear();
                }

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var read = new StreamReader(fs, Encoding.UTF8);
                    string strLine;
                    string[] aryLine;
                    while ((strLine = read.ReadLine()) != null)
                    {
                        aryLine = strLine.Split(',');
                        var contact = new ContractEntity();
                        if (aryLine.Length >= 2)
                        {
                            contact.Name = aryLine[0];
                            contact.Phone = aryLine[1];
                        }

                        if (aryLine.Length >= 3)
                        {
                            contact.Email = aryLine[2];
                        }

                        if (aryLine.Length >= 4)
                        {
                            contact.Title = aryLine[3];
                        }

                        ContractEntities.Add(contact);
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取收件人异常：" + ex);
            }
        }
    }
}