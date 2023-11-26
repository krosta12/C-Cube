using System.Windows.Forms;
using RubiksCubeSimulator.Rubiks;
using System.Threading.Tasks;

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

        private async void textBoxCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

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
}