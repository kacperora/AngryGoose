using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace MouseEater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double speed = 10;
        public MainWindow()
        {

            InitializeComponent();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();



        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var x = Canvas.GetLeft(Goose) + Goose.Width/2;
            var y = Canvas.GetTop(Goose) + Goose.Height/2; 
            Point position = Goose.PointToScreen(new Point(0d, 0d));
            var mouse = GetMousePosition();
            double xDiff = position.X - mouse.X;
            double yDiff = position.Y - mouse.Y;
            var angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
            x = Canvas.GetLeft(Goose)+ Math.Cos(angle) * speed;
            y = Canvas.GetTop(Goose) + Math.Sin(angle) * speed;
            Canvas.SetTop(Goose, y);
            Canvas.SetLeft(Goose, x);


        }
        public Point MovePointTowards(Point a, Point b, double distance)
        {
            var vector = new Point(b.X - a.X, b.Y - a.Y);
            var length = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            var unitVector = new Point(vector.X / length, vector.Y / length);
            return new Point(a.X + unitVector.X * distance, a.Y + unitVector.Y * distance);
        }
    }
}
