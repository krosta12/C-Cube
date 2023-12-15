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
using System.Windows.Forms.DataVisualization.Charting;

namespace RubiksCubeSimulator.Forms
{
    public partial class Graphs : Form
    {
        public Graphs()
        {
            InitializeComponent();
        }

        void maxGraphs()
        {
            string[] lines = File.ReadAllLines("output.txt");
            int[] oldTemp = { 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (string s in lines)
            {
                string[] temp = s.Split(new string[] { " -> " }, StringSplitOptions.None);
                if (oldTemp[temp[0].Split(' ').Length - 1] < Convert.ToInt32(temp[1]))
                {
                    oldTemp[temp[0].Split(' ').Length - 1] = Convert.ToInt32(temp[1]);
                    chart1.Series[temp[0].Split(' ').Length - 1].Points.AddXY(Convert.ToDouble(1), Convert.ToDouble(oldTemp[temp[0].Split(' ').Length - 1]));
                    chart1.Series[temp[0].Split(' ').Length - 1].Name = $"слой номер {temp[0].Split(' ').Length} - {oldTemp[temp[0].Split(' ').Length - 1]}";
                }
            }
        }

        void minGraphs()
        {
            string[] lines = File.ReadAllLines("output.txt");
            int[] oldTemp = { 90000, 90000, 90000, 90000, 90000, 90000, 90000, 90000 };

            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (string s in lines)
            {
                string[] temp = s.Split(new string[] { " -> " }, StringSplitOptions.None);
                if (oldTemp[temp[0].Split(' ').Length - 1] > Convert.ToInt32(temp[1]))
                {
                    oldTemp[temp[0].Split(' ').Length - 1] = Convert.ToInt32(temp[1]);
                    chart1.Series[temp[0].Split(' ').Length - 1].Points.Clear();
                    chart1.Series[temp[0].Split(' ').Length - 1].Points.AddXY(Convert.ToDouble(1), Convert.ToDouble(oldTemp[temp[0].Split(' ').Length - 1]));
                    chart1.Series[temp[0].Split(' ').Length - 1].Name = $"слой номер {temp[0].Split(' ').Length} - {oldTemp[temp[0].Split(' ').Length - 1]}";
                }
            }
        }

        void averageGraphs()
        {
            string[] lines = File.ReadAllLines("output.txt");
            int[] answer = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] counter = { 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (string s in lines)
            {
                string[] temp = s.Split(new string[] { " -> " }, StringSplitOptions.None);

                answer[temp[0].Split(' ').Length - 1] += Convert.ToInt32(temp[1]);
                counter[temp[0].Split(' ').Length - 1] += 1;
            }
            for (int i = 0; i < answer.Length; i++)
            {
                if (counter[i] > 0)
                {
                    answer[i] = answer[i] / counter[i];
                }
            }

            foreach (int a in answer)
            {
                if (a > 0)
                {
                    chart1.Series[Array.IndexOf(answer, a)].Points.AddXY(Convert.ToDouble(1), Convert.ToDouble(a));
                    chart1.Series[Array.IndexOf(answer, a)].Name = $"слой номер {Array.IndexOf(answer, a) + 1} - {answer[Array.IndexOf(answer, a)]}";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            maxGraphs();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            minGraphs();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            averageGraphs();
        }
    }
}
