using Free_VNC.screen;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Free_VNC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScreenCapture screenCapture;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            screenCapture.Stop();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            screenCapture = ScreenCapture.Instance;
            screenCapture.screenCapture += received_Screen;
            screenCapture.Start();
        }

        private void received_Screen(object? sender, Bitmap e)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    ImageSource imageSource = ConvertBitmapToImageSource(e, 50);
                    imageView.Width = imageSource.Width;
                    imageView.Height = imageSource.Height;
                    mainForm.Width = imageSource.Width; mainForm.Height = imageSource.Height;
                    imageView.Source = imageSource;
                }
                finally
                {
                    // 이미지 리소스 해제
                    e.Dispose();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="quality"> 0(min) ~ 100(max)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private ImageSource ConvertBitmapToImageSource(Bitmap bitmap, long quality)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            MemoryStream ms = new MemoryStream();
            ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);

            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            bitmap.Save(ms, jpegCodec, encoderParameters);
            ms.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }

    }
}