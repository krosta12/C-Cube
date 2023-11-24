using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace RubiksCubeSimulator.Rubiks
{
    internal class RubiksCube : ICloneable
    {
        private readonly Color[][,] origColors;

        #region Properties
        public bool RaiseEvents { get; set; } = true;
        public Color[][,] AllColors { get; private set; } = new Color[6][,];

        public Color[,] FrontColors
        {
            get { return AllColors[0]; }
            private set { AllColors[0] = value; }
        }

        public Color[,] BackColors
        {
            get { return AllColors[1]; }
            private set { AllColors[1] = value; }
        }

        public Color[,] RightColors
        {
            get { return AllColors[2]; }
            private set { AllColors[2] = value; }
        }

        public Color[,] LeftColors
        {
            get { return AllColors[3]; }
            private set { AllColors[3] = value; }
        }

        public Color[,] UpColors
        {
            get { return AllColors[4]; }
            private set { AllColors[4] = value; }
        }

        public Color[,] DownColors
        {
            get { return AllColors[5]; }
            private set { AllColors[5] = value; }
        }
        public bool Solved
        {
            get
            {
                foreach (var array in AllColors)
                {
                    Color lastColor = array[0, 0];

                    foreach (var color in array)
                    {
                        if (lastColor != color)
                            return false;
                    }
                }

                return true;
            }
        }
        #endregion

        public RubiksCube(Color[][,] colors)
        {
            origColors = CloneColors(colors);
            AllColors = colors;
        }

        public event EventHandler<CubeMove> MoveMade;
        protected virtual void OnMoveMade(CubeMove move)
        {
            MoveMade?.Invoke(this, move);
        }
        public static RubiksCube Create(CubeColorScheme scheme)
        {
            var colors = new Color[6][,];
            colors[0] = CreateFace(scheme.FrontColor);
            colors[1] = CreateFace(scheme.BackColor);
            colors[2] = CreateFace(scheme.RightColor);
            colors[3] = CreateFace(scheme.LeftColor);
            colors[4] = CreateFace(scheme.UpColor);
            colors[5] = CreateFace(scheme.DownColor);
            return new RubiksCube(colors);
        }


        public static Color[,] CreateFace(Color faceColor)
        {
            return new[,]
            {
               {faceColor, faceColor, faceColor},
               {faceColor, faceColor, faceColor},
               {faceColor, faceColor, faceColor}
            };
        }
        private Color[,] GetFaceColors(CubeSide side)
        {
            switch (side)
            {
                case CubeSide.Back: return BackColors;
                case CubeSide.Front: return FrontColors;
                case CubeSide.Left: return LeftColors;
                case CubeSide.Right: return RightColors;
                case CubeSide.Up: return UpColors;
                case CubeSide.Down: return DownColors;
                default: return null;
            }
        }
        private void SetSide(CubeSide side, Color[,] value)
        {
            switch (side)
            {
                case CubeSide.Back: BackColors = value; break;
                case CubeSide.Front: FrontColors = value; break;
                case CubeSide.Left: LeftColors = value; break;
                case CubeSide.Right: RightColors = value; break;
                case CubeSide.Up: UpColors = value; break;
                case CubeSide.Down: DownColors = value; break;
            }
        }
        private void RotateFace(CubeSide side, Rotation rotation)
        {
            var faceToRotate = GetFaceColors(side);
            var newFace = new Color[3, 3];

            if (rotation == Rotation.Cw)
            {
                for (int i = 2; i >= 0; i--)
                    for (int i2 = 0; i2 < 3; i2++)
                        newFace[i2, 2 - i] = faceToRotate[i, i2];
            }
            else
            {
                for (int i = 2; i >= 0; i--)
                    for (int i2 = 0; i2 < 3; i2++)
                        newFace[2 - i, i2] = faceToRotate[i2, i];
            }

            SetSide(side, newFace);
        }
        public IEnumerable<Color> GetColorsFlattened()
        {
            var colorStack = new Stack<Color>();

            foreach (Color[,] array in AllColors)
            {
                for (int row = 0; row < array.GetLength(0); row++)
                {
                    for (int clm = 0; clm < array.GetLength(1); clm++)
                        colorStack.Push(array[row, clm]);
                }
            }

            return colorStack;
        }

        private static Color[][,] CloneColors(Color[][,] source)
        {
            var cloned = new Color[source.Length][,];

            for (int i = 0; i < source.Length; i++)
            {
                cloned[i] = new Color[3, 3];

                for (int row = 0; row < source[i].GetLength(0); row++)
                {
                    for (int clm = 0; clm < source[i].GetLength(1); clm++)
                    {
                        cloned[i][row, clm] = source[i][row, clm];
                    }
                }
            }

            return cloned;
        }

        public void StartMath()
        {
            var outputLines = new List<string>(); // list for str-s
            int count = 0;
            var moveList = new List<CubeMove>();
            string[] allChars = { "r", "rb", "u", "ub", "l", "lb", "d", "db", "f", "fb", "b", "bb" };
            int[] indices = new int[8];
            int length = 1;
            int iteration = 0;

            if (File.Exists("state.txt"))
            {
                var stateLines = File.ReadAllLines("state.txt");
                length = int.Parse(stateLines[0]);
                indices = stateLines[1].Split().Select(int.Parse).ToArray();
                outputLines = File.ReadAllLines("output.txt").ToList();
            }

            for (; length <= 8; length++)
            {
                for (int i = indices[0]; i < allChars.Length; i++)
                {
                    for (int i2 = indices[1]; i2 < (length > 1 ? allChars.Length : 1); i2++)
                    {
                        for (int i3 = indices[2]; i3 < (length > 2 ? allChars.Length : 1); i3++)
                        {
                            for (int i4 = indices[3]; i4 < (length > 3 ? allChars.Length : 1); i4++)
                            {
                                for (int i5 = indices[4]; i5 < (length > 4 ? allChars.Length : 1); i5++)
                                {
                                    for (int i6 = indices[5]; i6 < (length > 5 ? allChars.Length : 1); i6++)
                                    {
                                        for (int i7 = indices[6]; i7 < (length > 6 ? allChars.Length : 1); i7++)
                                        {
                                            for (int i8 = indices[7]; i8 < (length > 7 ? allChars.Length : 1); i8++)
                                            {
                                                List<string> moves = new List<string>();
                                                moves.Add(allChars[i]);
                                                if (length > 1) moves.Add(allChars[i2]);
                                                if (length > 2) moves.Add(allChars[i3]);
                                                if (length > 3) moves.Add(allChars[i4]);
                                                if (length > 4) moves.Add(allChars[i5]);
                                                if (length > 5) moves.Add(allChars[i6]);
                                                if (length > 6) moves.Add(allChars[i7]);
                                                if (length > 7) moves.Add(allChars[i8]);

                                                foreach (string str in moves)
                                                {
                                                    moveList.Add(new CubeMove(string.Join(" ", str)));
                                                }
                                                while (!Solved | count == 0)
                                                {
                                                    foreach (var move in moveList)
                                                    {
                                                        count++;
                                                        MakeMove(move);
                                                    }
                                                }
                                                moveList.Clear();
                                                outputLines.Add($"{string.Join(" ", moves)} -> {count}");
                                                count = 0;

                                                indices = new int[] { i, i2, i3, i4, i5, i6, i7, i8 };
                                                iteration++;
                                                if (iteration % 100000 == 0)
                                                {
                                                    File.WriteAllLines("state.txt", new string[] { length.ToString(), string.Join(" ", indices) });
                                                    File.WriteAllLines("output.txt", outputLines);
                                                }
                                            }
                                            indices[7] = 0;
                                        }
                                        indices[6] = 0;
                                    }
                                    indices[5] = 0;
                                }
                                indices[4] = 0;
                            }
                            indices[3] = 0;
                        }
                        indices[2] = 0;
                    }
                    indices[1] = 0;
                }
                indices[0] = 0;
            }
        }


        public void MakeMove(CubeMove move)
        {
            MakeMove(move.Side, move.Rotation);
        }

        public void MakeMove(CubeSide side, Rotation rotation)
        {
            if (side == CubeSide.None) return;
            RotateFace(side, rotation);
            var newColors = CloneColors(AllColors);

            switch (side)
            {
                #region Front Shift
                case CubeSide.Front:

                    if (rotation == Rotation.Cw)
                    {
                        // move Left to up
                        newColors[4][2, 0] = LeftColors[2, 2];
                        newColors[4][2, 1] = LeftColors[1, 2];
                        newColors[4][2, 2] = LeftColors[0, 2];
                        // move up to right
                        newColors[2][0, 0] = UpColors[2, 0];
                        newColors[2][1, 0] = UpColors[2, 1];
                        newColors[2][2, 0] = UpColors[2, 2];
                        // move right to down
                        newColors[5][0, 2] = RightColors[0, 0];
                        newColors[5][0, 1] = RightColors[1, 0];
                        newColors[5][0, 0] = RightColors[2, 0];
                        // move down to left
                        newColors[3][0, 2] = DownColors[0, 0];
                        newColors[3][1, 2] = DownColors[0, 1];
                        newColors[3][2, 2] = DownColors[0, 2];
                    }
                    else
                    {
                        // 0 Front, 1 back, 2 right, 3 left, 4 up, 5 down
                        // move up to left
                        newColors[3][2, 2] = UpColors[2, 0];
                        newColors[3][1, 2] = UpColors[2, 1];
                        newColors[3][0, 2] = UpColors[2, 2];
                        // move right to up
                        newColors[4][2, 0] = RightColors[0, 0];
                        newColors[4][2, 1] = RightColors[1, 0];
                        newColors[4][2, 2] = RightColors[2, 0];
                        // move down to right
                        newColors[2][0, 0] = DownColors[0, 2];
                        newColors[2][1, 0] = DownColors[0, 1];
                        newColors[2][2, 0] = DownColors[0, 0];
                        // move left to down
                        newColors[5][0, 0] = LeftColors[0, 2];
                        newColors[5][0, 1] = LeftColors[1, 2];
                        newColors[5][0, 2] = LeftColors[2, 2];
                    }

                    break;
                #endregion

                #region Back Shift
                case CubeSide.Back:

                    if (rotation == Rotation.Ccw)
                    {
                        // move Left to up
                        newColors[4][0, 0] = LeftColors[2, 0];
                        newColors[4][0, 1] = LeftColors[1, 0];
                        newColors[4][0, 2] = LeftColors[0, 0];
                        // move up to right
                        newColors[2][0, 2] = UpColors[0, 0];
                        newColors[2][1, 2] = UpColors[0, 1];
                        newColors[2][2, 2] = UpColors[0, 2];
                        // move right to down
                        newColors[5][2, 2] = RightColors[0, 2];
                        newColors[5][2, 1] = RightColors[1, 2];
                        newColors[5][2, 0] = RightColors[2, 2];
                        // move down to left
                        newColors[3][0, 0] = DownColors[2, 0];
                        newColors[3][1, 0] = DownColors[2, 1];
                        newColors[3][2, 0] = DownColors[2, 2];
                    }
                    else
                    {
                        // 0 Front, 1 back, 2 right, 3 left, 4 up, 5 down
                        // move up to left
                        newColors[3][2, 0] = UpColors[0, 0];
                        newColors[3][1, 0] = UpColors[0, 1];
                        newColors[3][0, 0] = UpColors[0, 2];
                        // move right to up
                        newColors[4][0, 0] = RightColors[0, 2];
                        newColors[4][0, 1] = RightColors[1, 2];
                        newColors[4][0, 2] = RightColors[2, 2];
                        // move down to right
                        newColors[2][0, 2] = DownColors[2, 2];
                        newColors[2][1, 2] = DownColors[2, 1];
                        newColors[2][2, 2] = DownColors[2, 0];
                        // move left to down
                        newColors[5][2, 0] = LeftColors[0, 0];
                        newColors[5][2, 1] = LeftColors[1, 0];
                        newColors[5][2, 2] = LeftColors[2, 0];
                    }

                    break;
                #endregion

                #region Right Shift
                case CubeSide.Right:

                    if (rotation == Rotation.Cw)
                    {
                        // 0 Front, 1 back, 2 right, 3 left, 4 up, 5 down
                        // move front to up
                        newColors[4][0, 2] = FrontColors[0, 2];
                        newColors[4][1, 2] = FrontColors[1, 2];
                        newColors[4][2, 2] = FrontColors[2, 2];
                        // move up to back
                        newColors[1][2, 0] = UpColors[0, 2];
                        newColors[1][1, 0] = UpColors[1, 2];
                        newColors[1][0, 0] = UpColors[2, 2];
                        // move back to down
                        newColors[5][0, 2] = BackColors[2, 0];
                        newColors[5][1, 2] = BackColors[1, 0];
                        newColors[5][2, 2] = BackColors[0, 0];
                        // move down to front
                        newColors[0][0, 2] = DownColors[0, 2];
                        newColors[0][1, 2] = DownColors[1, 2];
                        newColors[0][2, 2] = DownColors[2, 2];
                    }
                    else
                    {
                        // 0 Front, 1 back, 2 right, 3 left, 4 up, 5 down
                        // move up to front
                        newColors[0][0, 2] = UpColors[0, 2];
                        newColors[0][1, 2] = UpColors[1, 2];
                        newColors[0][2, 2] = UpColors[2, 2];
                        // move back to up
                        newColors[4][0, 2] = BackColors[2, 0];
                        newColors[4][1, 2] = BackColors[1, 0];
                        newColors[4][2, 2] = BackColors[0, 0];
                        // move down to back
                        newColors[1][0, 0] = DownColors[2, 2];
                        newColors[1][1, 0] = DownColors[1, 2];
                        newColors[1][2, 0] = DownColors[0, 2];
                        // move front to down
                        newColors[5][0, 2] = FrontColors[0, 2];
                        newColors[5][1, 2] = FrontColors[1, 2];
                        newColors[5][2, 2] = FrontColors[2, 2];
                    }

                    break;
                #endregion

                #region Left Shift
                case CubeSide.Left:

                    if (rotation == Rotation.Cw)
                    {
                        // move up to front
                        newColors[0][0, 0] = UpColors[0, 0];
                        newColors[0][1, 0] = UpColors[1, 0];
                        newColors[0][2, 0] = UpColors[2, 0];
                        // move front to down
                        newColors[5][0, 0] = FrontColors[0, 0];
                        newColors[5][1, 0] = FrontColors[1, 0];
                        newColors[5][2, 0] = FrontColors[2, 0];
                        // move down to back
                        newColors[1][2, 2] = DownColors[0, 0];
                        newColors[1][1, 2] = DownColors[1, 0];
                        newColors[1][0, 2] = DownColors[2, 0];
                        // move back to up
                        newColors[4][2, 0] = BackColors[0, 2];
                        newColors[4][1, 0] = BackColors[1, 2];
                        newColors[4][0, 0] = BackColors[2, 2];
                    }
                    else
                    {
                        // 0 Front, 1 back, 2 right, 3 left, 4 up, 5 down
                        // move front to up
                        newColors[4][0, 0] = FrontColors[0, 0];
                        newColors[4][1, 0] = FrontColors[1, 0];
                        newColors[4][2, 0] = FrontColors[2, 0];
                        // move down to front
                        newColors[0][0, 0] = DownColors[0, 0];
                        newColors[0][1, 0] = DownColors[1, 0];
                        newColors[0][2, 0] = DownColors[2, 0];
                        // move back to down
                        newColors[5][0, 0] = BackColors[2, 2];
                        newColors[5][1, 0] = BackColors[1, 2];
                        newColors[5][2, 0] = BackColors[0, 2];
                        // move up to back
                        newColors[1][0, 2] = UpColors[2, 0];
                        newColors[1][1, 2] = UpColors[1, 0];
                        newColors[1][2, 2] = UpColors[0, 0];
                    }

                    break;
                #endregion

                #region Up Shift
                case CubeSide.Up:

                    if (rotation == Rotation.Cw)
                    {
                        // move right to front
                        newColors[0][0, 0] = RightColors[0, 0];
                        newColors[0][0, 1] = RightColors[0, 1];
                        newColors[0][0, 2] = RightColors[0, 2];
                        // move front to left
                        newColors[3][0, 0] = FrontColors[0, 0];
                        newColors[3][0, 1] = FrontColors[0, 1];
                        newColors[3][0, 2] = FrontColors[0, 2];
                        // move left to back
                        newColors[1][0, 0] = LeftColors[0, 0];
                        newColors[1][0, 1] = LeftColors[0, 1];
                        newColors[1][0, 2] = LeftColors[0, 2];
                        // move back to right
                        newColors[2][0, 0] = BackColors[0, 0];
                        newColors[2][0, 1] = BackColors[0, 1];
                        newColors[2][0, 2] = BackColors[0, 2];
                    }
                    else
                    {
                        //  0 Front, 1 back, 2 right, 3 left, 4 up, 5 down
                        // move front to right
                        newColors[2][0, 0] = FrontColors[0, 0];
                        newColors[2][0, 1] = FrontColors[0, 1];
                        newColors[2][0, 2] = FrontColors[0, 2];
                        // move left to front
                        newColors[0][0, 0] = LeftColors[0, 0];
                        newColors[0][0, 1] = LeftColors[0, 1];
                        newColors[0][0, 2] = LeftColors[0, 2];
                        // move back to left
                        newColors[3][0, 0] = BackColors[0, 0];
                        newColors[3][0, 1] = BackColors[0, 1];
                        newColors[3][0, 2] = BackColors[0, 2];
                        // move right to back
                        newColors[1][0, 0] = RightColors[0, 0];
                        newColors[1][0, 1] = RightColors[0, 1];
                        newColors[1][0, 2] = RightColors[0, 2];
                    }
                    break;
                #endregion

                #region Down Shift
                case CubeSide.Down:

                    if (rotation == Rotation.Ccw)
                    {
                        //  0 Front, 1 back, 2 right, 3 left, 4 up, 5 down
                        // move right to front
                        newColors[0][2, 0] = RightColors[2, 0];
                        newColors[0][2, 1] = RightColors[2, 1];
                        newColors[0][2, 2] = RightColors[2, 2];
                        // move front to left
                        newColors[3][2, 0] = FrontColors[2, 0];
                        newColors[3][2, 1] = FrontColors[2, 1];
                        newColors[3][2, 2] = FrontColors[2, 2];
                        // move left to back
                        newColors[1][2, 0] = LeftColors[2, 0];
                        newColors[1][2, 1] = LeftColors[2, 1];
                        newColors[1][2, 2] = LeftColors[2, 2];
                        // move back to right
                        newColors[2][2, 0] = BackColors[2, 0];
                        newColors[2][2, 1] = BackColors[2, 1];
                        newColors[2][2, 2] = BackColors[2, 2];
                    }
                    else
                    {
                        // move front to right
                        newColors[2][2, 0] = FrontColors[2, 0];
                        newColors[2][2, 1] = FrontColors[2, 1];
                        newColors[2][2, 2] = FrontColors[2, 2];
                        // move left to front
                        newColors[0][2, 0] = LeftColors[2, 0];
                        newColors[0][2, 1] = LeftColors[2, 1];
                        newColors[0][2, 2] = LeftColors[2, 2];
                        // move back to left
                        newColors[3][2, 0] = BackColors[2, 0];
                        newColors[3][2, 1] = BackColors[2, 1];
                        newColors[3][2, 2] = BackColors[2, 2];
                        // move right to back
                        newColors[1][2, 0] = RightColors[2, 0];
                        newColors[1][2, 1] = RightColors[2, 1];
                        newColors[1][2, 2] = RightColors[2, 2];
                    }
                    break;
                    #endregion
            }

            AllColors = newColors;

            if (RaiseEvents)
                OnMoveMade(new CubeMove(side, rotation));
        }

        public object Clone()
        {
            var colors = CloneColors(AllColors);
            return new RubiksCube(colors);
        }
    }
}
