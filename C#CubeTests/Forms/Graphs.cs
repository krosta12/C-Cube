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
            maxGraphs();
        }

        void maxGraphs()
        {
            string[] lines = File.ReadAllLines("output.txt");
            foreach (string s in lines)
            {
                int oldTemp = 0;
                string[] temp = s.Split(new string[] { " -> " }, StringSplitOptions.None);
                if (oldTemp < Convert.ToInt32(temp[1])) 
                {
                    oldTemp = Convert.ToInt32(temp[1]);
                    chart1.Series[temp[0].Split(' ').Length - 2 ].Points.AddXY(Convert.ToDouble(1), Convert.ToDouble(oldTemp));
                    chart1.Series[temp[0].Split(' ').Length - 2 ].Name = $"слой номер {temp[0].Split(' ').Length} - {oldTemp}";
                }
            }
        }
    }
}
