namespace RubiksCubeSimulator.Rubiks
{

    public enum Rotation
    {
        /// <summary>
        /// Clockwise.
        /// </summary>
        Cw,
        /// <summary>
        /// Counter-clockwise.
        /// </summary>
        Ccw
    }

    /// <summary>
    /// Specifies the sides of a cube.
    /// </summary>
    public enum CubeSide
    {
        None,
        /// <summary>
        /// The front of the cube.
        /// </summary>
        Front,
        /// <summary>
        /// The back of the cube.
        /// </summary>
        Back,
        /// <summary>
        /// The right of the cube.
        /// </summary>
        Right,
        /// <summary>
        /// The left of the cube.
        /// </summary>
        Left,
        /// <summary>
        /// The top of the cube.
        /// </summary>
        Up,
        /// <summary>
        /// The bottom of the cube.
        /// </summary>
        Down
    }
}
