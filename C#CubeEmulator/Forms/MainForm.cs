using RubiksCubeSimulator.Rubiks;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
            int count = 0;
            if (e.KeyCode != Keys.Enter) return;
            string lower = textBoxCommand.Text;
            string[] splitted = textBoxCommand.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var moveList = new List<CubeMove>();
            Thread thread = new Thread(new ThreadStart(() =>
            {
                foreach (string str in splitted)
                {
                    try
                    {
                        moveList.Add(new CubeMove(str));
                    }
                    catch (ArgumentException)
                    {
                        continue; //skip problem
                    }
                }
                while (!rubiksCube.Solved | count == 0)
                {
                    foreach (var move in moveList)
                    {
                        count++;
                        lblStatus.Text = "Last Move: " + move;
                        rubiksCube.MakeMove(move);
                    }
                }
                lblStatus.Text = count.ToString();

                e.SuppressKeyPress = true;
            })); thread.Start();
        }
    }
}
