using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace FotoFaultFixerUI.Controls
{
    /// <summary>
    /// Interaction logic for SubSelectionCanvas.xaml
    /// </summary>
    public partial class SubSelectionCanvas : UserControl
    {
        public bool IsActive = false;
        private Ellipse _selectedCornerPoint = null;

        public SubSelectionCanvas()
        {
            InitializeComponent();
        }

        public void Setup(int x, int y, int width, int height)
        {
            _selectedCornerPoint = null;
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
            this.Width = width;
            this.Height = height;
        }

        public Point[] RequestPointsAndClose()
        {
            // Convert to % Points
            var points = new List<Point>()
            {
                new Point((Canvas.GetLeft(CornerPoint3) + 5) / this.Width, (Canvas.GetTop(CornerPoint3) + 5) / this.Height),
                new Point((Canvas.GetLeft(CornerPoint1) + 5) / this.Width, (Canvas.GetTop(CornerPoint1) + 5) / this.Height),
                new Point((Canvas.GetLeft(CornerPoint2) + 5) / this.Width, (Canvas.GetTop(CornerPoint2) + 5) / this.Height),
                new Point((Canvas.GetLeft(CornerPoint4) + 5) / this.Width, (Canvas.GetTop(CornerPoint4) + 5) / this.Height)
            };            

            DisactivateSubSelection();

            return points.ToArray();
        }

        private void SelectionMask_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (e.OriginalSource is Ellipse)
            {
                _selectedCornerPoint = (Ellipse)e.OriginalSource;

                SelectionMask.MouseMove += SelectionMask_MouseMove;
                SelectionMask.MouseUp += SelectionMask_MouseUp;

            }                        
        }
        
        private void SelectionMask_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            var mousePos = e.GetPosition(this);
            
            Canvas.SetLeft(_selectedCornerPoint, mousePos.X);
            Canvas.SetTop(_selectedCornerPoint, mousePos.Y);

            SetPolygonPointsBasedOnCornerPoints();
        }

        private void SelectionMask_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            _selectedCornerPoint = null;            

            SelectionMask.MouseMove -= SelectionMask_MouseMove;
            SelectionMask.MouseUp -= SelectionMask_MouseUp;
        }

        public void ActivateSubSelection()
        {
            if (!IsActive)
            {
                IsActive = true;

                selectionPolygon.Width = this.Width;
                selectionPolygon.Height = this.Height;

                ResetPointsAndPolygon();

                this.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void DisactivateSubSelection()
        {
            if (IsActive)
            {
                IsActive = false;

                ResetPointsAndPolygon();

                this.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void ResetPointsAndPolygon()
        {
            selectionPolygon.Width = this.Width;
            selectionPolygon.Height = this.Height;

            Canvas.SetTop(CornerPoint1, 0);
            Canvas.SetLeft(CornerPoint1, 0);

            Canvas.SetTop(CornerPoint2, 0);
            Canvas.SetLeft(CornerPoint2, this.Width - 5);

            Canvas.SetTop(CornerPoint3, this.Height - 5);
            Canvas.SetLeft(CornerPoint3, 0);

            Canvas.SetTop(CornerPoint4, this.Height - 5);
            Canvas.SetLeft(CornerPoint4, this.Width - 5);

            SetPolygonPointsBasedOnCornerPoints();
        }

        private void SetPolygonPointsBasedOnCornerPoints()
        {
            selectionPolygon.Points.Clear();
            selectionPolygon.Points = new PointCollection(new List<Point>() {
                    new Point(Canvas.GetLeft(CornerPoint1) + 5, Canvas.GetTop(CornerPoint1) + 5),
                    new Point(Canvas.GetLeft(CornerPoint2) + 5, Canvas.GetTop(CornerPoint2) + 5),
                    new Point(Canvas.GetLeft(CornerPoint4) + 5, Canvas.GetTop(CornerPoint4) + 5),
                    new Point(Canvas.GetLeft(CornerPoint3) + 5, Canvas.GetTop(CornerPoint3) + 5),
                });
        }
    }
}
