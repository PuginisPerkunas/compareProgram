using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pirmaUzduotis
{
    public partial class Form1 : Form
    {
        private string folderPath = "";
        private int timerValue;
        delegate void SetTextCallback(string text);
        public Form1()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                pathLabel.Text = folderPath;
            }
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            Thread worker = new Thread(ComparerFunction);
            timer1.Start();
            worker.Start();
        }

        public void ComparerFunction()
        {
            Compare compare = new Compare(folderPath);
            compare.Start();
            int i = 1;
            foreach(string k in Compare.filesList)
            {
                if(i%2 == 1)
                {
                    SetText("Failas esantis " + k + "\r\n");
                }
                else
                {
                    SetText("Yra dublikatas: " + k + "\r\n\r\n");
                }
                i++;
            }
            timer1.Stop();
        }
        private void SetText(string text)
        {
            if (this.listView.InvokeRequired)
            {
                SetTextCallback setTextCallback = new SetTextCallback(SetText);
                this.Invoke(setTextCallback, new object[] { text });
            }
            else
            {
                this.listView.Items.Add(text);
            }
        }

        private void tick(object sender, EventArgs e)
        {
            timerValue++;
            timeLabel.Text = "Time spended: " + timerValue.ToString();
        }
    }//class
}//namespace
