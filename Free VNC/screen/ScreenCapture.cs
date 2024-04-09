using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Threading;

namespace Free_VNC.screen
{
    public class ScreenCapture
    {
        public event EventHandler<Bitmap> screenCapture;

        //스레드 우아하게 종료
        private volatile bool isCaptureRunning = false;

        #region 싱글톤 설정
        private static ScreenCapture instance;

        private Thread captureThread;

        //싱글톤 직접 참조금지
        private ScreenCapture() { }

        public static ScreenCapture Instance
        {
            get
            {
                //동시접근 금지
                lock (instance)
                {
                    if (instance == null)
                    {
                        instance = new ScreenCapture();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public void Start()
        {
            if(!isCaptureRunning)
            {
                isCaptureRunning = true;
                captureThread = new Thread(new ThreadStart(() =>
                {
                    CaptureScreen();
                }));
                captureThread.IsBackground = true;
                captureThread.Name = "Screen Capture";
                captureThread.Start();
            }
            else
            {
                throw new Exception("already capture thread running...");
            }
        }

        public void Stop()
        {
            if(isCaptureRunning)
            {
                if(captureThread != null && captureThread.IsAlive) {
                    captureThread.Join();
                }
                else
                {
                    isCaptureRunning = false;
                }
            }
            else
            {
                throw new Exception("already capture thread stoped");
            }
        }

        private void CaptureScreen()
        {
            while (isCaptureRunning)
            {
                OnImageCapture(CaptureScreenAsBitmap());
                Thread.Sleep(500);
            }
        }

        private void OnImageCapture(Bitmap screenshot)
        {
            screenCapture?.Invoke(this, screenshot);
        }

        private Bitmap CaptureScreenAsBitmap()
        {
            Bitmap bitmap = null;

            double screenLeft = SystemParameters.PrimaryScreenWidth;
            double screenTop = SystemParameters.PrimaryScreenHeight;

            bitmap = new Bitmap((int)screenLeft, (int)screenTop);
            // UI 스레드에서 실행되도록 Dispatcher 사용
            Application.Current.Dispatcher.Invoke(() =>
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                }
            });

            return bitmap;
        }

    }
}
