using System.Drawing;

namespace RubiksCubeSimulator.ColorGrid
{
    public class ColorGridStyle
    {
        public Color[,] Colors { get; set; }
        public float CellSpacingScale { get; set; }

        public int RoundedRadius { get; set; }


        public ColorGridStyle(Color[,] colors, float cellSpacing, int roundedRadius)
        {
            Colors = colors;
            CellSpacingScale = cellSpacing;
            RoundedRadius = roundedRadius;
        }
    }
}
