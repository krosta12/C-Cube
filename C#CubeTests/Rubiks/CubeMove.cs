using System;

namespace RubiksCubeSimulator.Rubiks
{
    internal class CubeMove
    {
        public CubeSide Side { get; }
        public Rotation Rotation { get; }

        public CubeMove(CubeSide side, Rotation rotation)
        {
            Side = side;
            Rotation = rotation;
        }
        public CubeMove(string notation)
        {
            switch (notation.Trim().ToLower())
            {
                case "f":
                    Rotation = Rotation.Cw;
                    Side = CubeSide.Front;
                    break;

                case "fb":
                    Rotation = Rotation.Ccw;
                    Side = CubeSide.Front;
                    break;

                case "b":
                    Rotation = Rotation.Cw;
                    Side = CubeSide.Back;
                    break;

                case "bb":
                    Rotation = Rotation.Ccw;
                    Side = CubeSide.Back;
                    break;

                case "r":
                    Rotation = Rotation.Cw;
                    Side = CubeSide.Right;
                    break;

                case "rb":
                    Rotation = Rotation.Ccw;
                    Side = CubeSide.Right;
                    break;

                case "l":
                    Rotation = Rotation.Cw;
                    Side = CubeSide.Left;
                    break;

                case "lb":
                    Rotation = Rotation.Ccw;
                    Side = CubeSide.Left;
                    break;

                case "u":
                    Rotation = Rotation.Cw;
                    Side = CubeSide.Up;
                    break;

                case "ub":
                    Rotation = Rotation.Ccw;
                    Side = CubeSide.Up;
                    break;

                case "d":
                    Rotation = Rotation.Cw;
                    Side = CubeSide.Down;
                    break;

                case "db":
                    Rotation = Rotation.Ccw;
                    Side = CubeSide.Down;
                    break;

                default:
                    throw new ArgumentException("Value is not valid notation.", nameof(notation));
            }
        }
    }
}
