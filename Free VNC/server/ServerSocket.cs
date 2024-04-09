using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Free_VNC.server
{
    public class ServerSocket
    {
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryWriter writer;
        private Thread captureThread;

        public ServerSocket()
        {
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();

            client = listener.AcceptTcpClient();
            stream = client.GetStream();
            writer = new BinaryWriter(stream);
            captureThread = new Thread(new ThreadStart(CaptureScreen));
            captureThread.Start();
        }

        private void CaptureScreen()
        {
            while (true)
            {
                Bitmap screenshot = CaptureScreenAsBitmap();
                SendImage(screenshot);
                Thread.Sleep(100); // Adjust the delay as needed
            }
        }

        private Bitmap CaptureScreenAsBitmap()
        {
            Rect bounds = new Rect(SystemParameters.VirtualScreenLeft, SystemParameters.VirtualScreenTop,
                                    SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);

            // BitmapSource를 Bitmap으로 변환
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return new Bitmap(ms);
            }
        }

        private void SendImage(Bitmap image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            byte[] imageData = ms.ToArray();
            writer.Write(imageData.Length);
            writer.Write(imageData);
        }
    }
}
