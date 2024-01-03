using System;

using System.IO;

using System.Windows.Forms;


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
            using (StreamReader reader = new StreamReader("output.txt"))
            {
                int [] oldTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] temp = line.Split(new string[] { " -> " }, StringSplitOptions.None);

                    if (oldTemp[temp[0].Split(' ').Length - 1] < Convert.ToInt64(temp[1]))
                    {
                        oldTemp[temp[0].Split(' ').Length - 1] = (int)Convert.ToInt64(temp[1]);
                        chart1.Series[temp[0].Split(' ').Length - 1].Points.Clear();
                        chart1.Series[temp[0].Split(' ').Length - 1].Points.AddXY(Convert.ToDouble(1), Convert.ToDouble(oldTemp[temp[0].Split(' ').Length - 1]));
                        chart1.Series[temp[0].Split(' ').Length - 1].Name = $"слой номер {temp[0].Split(' ').Length} - {oldTemp[temp[0].Split(' ').Length - 1]}";
                    }
                }
            }
        }

        void minGraphs()
        {
            using (StreamReader reader = new StreamReader("output.txt"))
            {
                int[] oldTemp = { 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000 };
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] temp = line.Split(new string[] { " -> " }, StringSplitOptions.None);

                    if (oldTemp[temp[0].Split(' ').Length - 1] > Convert.ToInt64(temp[1]))
                    {
                        oldTemp[temp[0].Split(' ').Length - 1] = (int)Convert.ToInt64(temp[1]);
                        chart1.Series[temp[0].Split(' ').Length - 1].Points.Clear();
                        chart1.Series[temp[0].Split(' ').Length - 1].Points.AddXY(Convert.ToDouble(1), Convert.ToDouble(oldTemp[temp[0].Split(' ').Length - 1]));
                        chart1.Series[temp[0].Split(' ').Length - 1].Name = $"слой номер {temp[0].Split(' ').Length} - {oldTemp[temp[0].Split(' ').Length - 1]}";
                    }
                }
            }
        }

        void averageGraphs()
        {
            using (StreamReader reader = new StreamReader("output.txt"))
            {
                long[] answerSum = new long[12];
                int[] counter = new int[12];

                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] temp = line.Split(new string[] { " -> " }, StringSplitOptions.None);

                    int index = temp[0].Split(' ').Length - 1;

                    answerSum[index] += Convert.ToInt64(temp[1]);
                    counter[index]++;
                }

                for (int i = 0; i < answerSum.Length; i++)
                {
                    if (counter[i] > 0)
                    {
                        long average = answerSum[i] / counter[i];
                        chart1.Series[i].Points.AddXY(1.0, average);
                        chart1.Series[i].Name = $"слой номер {i + 1} - {average}";
                    }
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
