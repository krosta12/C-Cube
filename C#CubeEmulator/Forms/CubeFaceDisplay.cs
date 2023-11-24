using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RubiksCubeSimulator.ColorGrid;
using RubiksCubeSimulator.Properties;
using RubiksCubeSimulator.Rubiks;

namespace RubiksCubeSimulator.Forms
{
    /// <summary>
    /// Specifies a mode for adjusting the cube colors.
    /// </summary>
    public enum ClickMode
    {
        /// <summary>
        /// No effect will happen.
        /// </summary>
        None,
        /// <summary>
        /// The user will be able to rotate with the left and right mouse buttons.
        /// </summary>
        Rotation,
        /// <summary>
        /// The user will be able to set colors with the right mouse buttons.
        /// </summary>
        ColorSet
    }

    /// <summary>
    /// Represents a grid point mapper, to build point arrays on a grid.
    /// </summary>
    internal class CubeFaceDisplay : Control
    {
        private RectangleF hoveredRect;

        #region Properties
        private ColorGridStyle Style =>
            new ColorGridStyle(GetDisplayColors(), 0.05f, RoundedRadius);

        private ClickMode clickMode;
        [Category("Appearance")]
        [DefaultValue(ClickMode.None)]
        [Description("The operations to perform when clicking")]
        public ClickMode ClickMode
        {
            get { return clickMode; }
            set
            {
                if (clickMode == value) return;
                clickMode = value;

  
                Invalidate();
            }
        }

        private CubeSide face = CubeSide.None;
        [Category("Appearance"),DefaultValue(CubeSide.None)]
        [Description("The face or side of the cube to handle and display")]
        public CubeSide Face
        {
            get { return face; }
            set
            {
                face = value;
                Invalidate();
            }
        }

        private Color newColor;
        [Description("Determines the color to be set when right-clicking a cell")]
        [Category("Behavior")]
        public Color NewColor
        {
            get { return newColor; }
            set
            {
                if (newColor != value)
                {
                    newColor = value;
                    Invalidate();
                }
            }
        }

        private RubiksCube rubiksCube;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RubiksCube RubiksCube
        {
            get { return rubiksCube; }
            set
            {
                rubiksCube = value;
                Invalidate();
            }
        }

        private Color[,] FaceColors
        {
            get
            {
                if (rubiksCube == null) return null;

                switch (Face)
                {
                    case CubeSide.Front: return RubiksCube.FrontColors;
                    case CubeSide.Back: return RubiksCube.BackColors;
                    case CubeSide.Right: return RubiksCube.RightColors;
                    case CubeSide.Left: return RubiksCube.LeftColors;
                    case CubeSide.Up: return RubiksCube.UpColors;
                    case CubeSide.Down: return RubiksCube.DownColors;
                    default: return null;
                }
            }
        }

        private int roundedRadius = 5;
        [Category("Appearance"),DefaultValue(5)]
        [Description("The corner radius of the rounded rectangles used with this control")]
        public int RoundedRadius
        {
            get { return roundedRadius; }
            set
            {
                roundedRadius = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        #endregion

        

        #region Overrides

        [Description("Occurs when a new cell has been hovered over by the mouse")]
        public event EventHandler<RectangleF> HoveredCellChanged;
        /// <summary>
        /// Raises the <see cref="HoveredCellChanged"/> event.
        /// </summary>
        protected virtual void OnHoverCellChanged(RectangleF cellPos)
        {
            HoveredCellChanged?.Invoke(this, cellPos);
        }

        [Description("Occurs when a cell has been clicked by the mouse")]
  

        
        #endregion

        private Color[,] GetDisplayColors()
        {
            var faceColors = FaceColors;
            // If face == null then the CubeSide enum has not been set.
            if (rubiksCube == null || faceColors == null)
            {
                return RubiksCube.CreateFace(Color.White);
            }

            return faceColors;
        }
        
    }

    /// <summary>
    /// Represents event arguments for the <see cref="CubeFaceDisplay.CellMouseClicked"/> event.
    /// </summary>
    internal class CellMouseClickedEventArgs : EventArgs
    {
    }
}
