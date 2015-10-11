using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Wall.UserControls
{
    public sealed partial class Cell : UserControl
    {
        public static SolidColorBrush SELECTED_COLOR = new SolidColorBrush(Windows.UI.Color.FromArgb(150, 255, 10, 10));
        public static SolidColorBrush UNSELECTED_COLOR = new SolidColorBrush(Windows.UI.Color.FromArgb(50, 255, 255, 255));

        private bool isSelected = false;
        /// <summary>
        /// Seçildi mi?
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;

                if (value)
                    this.Hexagon.Fill = SELECTED_COLOR;
                else
                    this.Hexagon.Fill = UNSELECTED_COLOR;
            }
        }

        private Point coordinate;
        public Point Coordinate
        {
            set
            {
                this.coordinate = value;
                this.CoordinateTB.Text = value.X + "" + value.Y;
            }
            get { return this.coordinate; }
        }

        public Cell()
        {
            this.InitializeComponent();

            this.SizeChanged += (s, e) => { this.DrawHexagon(); };
        }

        private void DrawHexagon()
        {
            this.Hexagon.Stroke = new SolidColorBrush(Windows.UI.Color.FromArgb(50, 255, 255, 255));
            this.Hexagon.StrokeThickness = 1;
            this.Hexagon.Points = new PointCollection()
            {
                new Point(this.ActualWidth / 2, 0), 
                new Point(this.ActualWidth / 2 + this.ActualWidth / 4 * Math.Sqrt(3), this.ActualHeight / 4), 
                new Point(this.ActualWidth / 2 + this.ActualWidth / 4 * Math.Sqrt(3), this.ActualHeight / 4 + this.ActualHeight / 2), 
                new Point(this.ActualWidth / 2, this.ActualHeight),
                new Point(this.ActualWidth / 2 - this.ActualWidth / 4 * Math.Sqrt(3), this.ActualHeight / 4 + this.ActualHeight / 2), 
                new Point(this.ActualWidth / 2 - this.ActualWidth / 4 * Math.Sqrt(3), this.ActualHeight / 4), 
            };

            this.IsSelected = this.isSelected;
        }
    }
}
