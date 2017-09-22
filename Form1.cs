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
            SetText("Failas esantis: " + compare.getFirstString());
            SetText("Yra identiskas: " + compare.getSecondString());
            timer1.Stop();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerValue++;
            timeLabel.Text = "Time spended: " + timerValue.ToString();
        }

        delegate void SetTextCallback(string text);
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
        
    }//class
}//namespace
