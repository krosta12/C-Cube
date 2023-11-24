using System;
using System.Drawing;
using System.Linq;
using RubiksCubeSimulator.Rubiks;

namespace RubiksCubeSimulator
{
    [Serializable]
    class Settings : SettingsBase<Settings>
    {
        public Color[] Palette { get; set; }
        public Color[][,] CubeColors { get; set; }
        public override void Reset()
        {
            var tempCube = RubiksCube.Create(CubeColorScheme.DevsScheme);
            CubeColors = tempCube.AllColors;
            Palette = tempCube.GetColorsFlattened().Distinct().ToArray();
        }
    }
}
