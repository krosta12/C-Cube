using System.Windows.Forms;
using RubiksCubeSimulator.Rubiks;
using System.Threading;

namespace RubiksCubeSimulator.Forms
{
    public partial class MainForm : Form
    {
        private RubiksCube rubiksCube;
        public MainForm()
        {
            InitializeComponent();
            SetRubiksCube();
        }

        private void SetRubiksCube()
        {
            rubiksCube = new RubiksCube(Settings.Instance.CubeColors);
        }

        private void textBoxCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                rubiksCube.StartMath();

                this.Invoke((MethodInvoker)delegate
                {
                    labellErrorStatus.Text = "All was calculated";
                });
            }));

            thread.Start();

            labellErrorStatus.Text = "in Process...";
        }
    }
}
