using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entities;
using BLL_VirtualTrader;

namespace Share_Brokering___David_Hart
{
    public partial class Trader : Form
    {
        public Trader()
        {
            InitializeComponent();
        }

        private void btnUploadData_Click(object sender, EventArgs e)
        {
            string fileStr = OpenFileUploader();

            if (fileStr.Length > 0)
            {
                ConvertAndProcessFile(fileStr);
            }
        }

        private void ConvertAndProcessFile(string fileStr)
        {
            Trading trade = new Trading(fileStr);
            trade.Start();
            lblOutput.Text = trade.Result();
        }

        private string OpenFileUploader()
        {
            Stream fileStream = null;
            string fileString = string.Empty;

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open CSV File";

            // can add actual csv's later; allow unknown extensions option for now. 
            openDialog.Filter = "Text files|*.txt|All Types|*.*";
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((fileStream = openDialog.OpenFile()) != null)
                    {
                        byte[] buffer = new byte[fileStream.Length];

                        // for scope for reading share values for a month, unlikely to exceed a few Kb but if it were several shares, maybe consider stream chunks, buffering for performance?
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileString = reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                     MessageBox.Show("Error: Could not read file " + ex.Message);
                }
            }
            return fileString;
        }
    }
}
