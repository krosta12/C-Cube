using System.Windows.Forms;
using RubiksCubeSimulator.Rubiks;
using System.Threading.Tasks;
using System;

namespace RubiksCubeSimulator.Forms
{
    public partial class MainForm : Form
    {
        private RubiksCube rubiksCube;
        private static Graphs graphs;

        public MainForm()
        {
            InitializeComponent();
            SetRubiksCube();
        }

        private void SetRubiksCube()
        {
            rubiksCube = new RubiksCube(Settings.Instance.CubeColors);
        }

        private async void textBoxCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (labellErrorStatus.Text != "Sorting")
            {
                labellErrorStatus.Text = "In Process...";

                await Task.Run(() =>
                {
                    rubiksCube.StartMath();

                    this.Invoke((MethodInvoker)delegate
                    {
                        labellErrorStatus.Text = "All was calculated";
                    });
                });
            }
        }

        private async void button1_Click(object sender, System.EventArgs e)
        {
            Control button = (Control)sender;
            if (labellErrorStatus.Text == "In Process...")
            {
                button.Text = "Try Later";
            }
            else
            {
                button.Text = "sort";
                labellErrorStatus.Text = "Sorting";
                await Task.Run(() =>
                {
                    FileSorter.SortFileByDigits("output.txt", "sortedOutput.txt");

                    this.Invoke((MethodInvoker)delegate
                    {
                        labellErrorStatus.Text = "List sorted!";
                    });
                });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            graphs = new Graphs();
            graphs.Show();
        }
    }
}