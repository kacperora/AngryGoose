using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;



namespace AngryGoose
{
    public partial class Form1 : Form
    {
        bool rickrolled = false;
        SoundPlayer? simpleSound;
        private const double speed = 10;
        private readonly double speed2 = 10;
        private readonly double difficulty = 100;
        //private int lastx = 0;
        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackgroundImage = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\Microsoft\Windows\Themes\TranscodedWallpaper");
            Goose.BringToFront();
        }
        
        private void GooseGotYou()
        {

            timer1.Stop();
            Goose.Image = new Bitmap(Properties.Resources.rsz5mlpatqp31);


            simpleSound = new SoundPlayer(Properties.Resources.Honk);
            try
            {
                simpleSound.PlayLooping();
            } catch (Exception)
            {

            }
            timer2.Start();

        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            var x = Goose.PointToScreen(Point.Empty).X+Goose.Width/2;
            var y = Goose.PointToScreen(Point.Empty).Y+Goose.Height/2;
            Point cursor;
            cursor = Cursor.Position;
            double angle = Math.Atan2(cursor.Y - y, cursor.X - x);
            var dx = (int)(Math.Ceiling((Math.Cos(angle) * speed)));
           // if ((lastx*cursor.X < 0)) 
            //{
               // Goose.Image = Properties.Resources.image_processing20200917_21316_1ovdnp0;
                //Goose.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //} 
            //lastx = dx;
            var dy = (int)(Math.Ceiling((Math.Sin(angle) * speed)));
            Goose.Location = new Point(x+dx-Goose.Width/2, y+dy-Goose.Height/2);
            if(x-Goose.Width/2 <= cursor.X && cursor.X <= x+Goose.Width/2 && y-Goose.Height/2 <= cursor.Y && cursor.Y <= y +Goose.Height/2)
            {
                GooseGotYou();
            }
            
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            Point pos = Cursor.Position;
            var x = Goose.PointToScreen(Point.Empty).X + Goose.Width / 2;
            var y = Goose.PointToScreen(Point.Empty).Y + Goose.Height / 2;
            double distance = Math.Sqrt(Math.Pow(pos.X - x, 2) + Math.Pow(pos.Y - y, 2) * 1.0);
            if (distance > difficulty)
            {
                timer2.Stop();
                var gif = Properties.Resources.image_processing20200917_21316_1ovdnp0;
                Goose.Image = gif;
                timer1.Start();
                rickrolled = false;
                if (simpleSound is not null)
                {
                    simpleSound.Stop();
                }
            }
            else
            {
                Point target;
                target = Location;
                if (Math.Sqrt(Math.Pow(target.X - x, 2) + Math.Pow(target.Y - y, 2) * 1.0) > speed2)
                {
                    double angle = Math.Atan2(target.Y - y, target.X - x);
                    var dx = (int)(Math.Ceiling((Math.Cos(angle) * speed2)));
                    var dy = (int)(Math.Ceiling((Math.Sin(angle) * speed2)));
                    Goose.Location = new Point(x + dx - Goose.Width / 2, y + dy - Goose.Height / 2);
                } else
                {
                    if (!rickrolled)
                    {
                        rickrolled = true;
                        OpenUrl("https://youtu.be/dQw4w9WgXcQ");
                    }
                }

                Cursor.Position = Goose.Location;
            }
        }
    }
}