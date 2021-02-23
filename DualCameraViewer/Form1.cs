using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DualCameraViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Height = 434;
            this.Width = 960;
            textBox1.Text = Properties.Settings.Default.Number1;
            textBox2.Text = Properties.Settings.Default.Number2;
        }

        bool isCameraRunning = false;

        private void CaptureCamera()
        {
            try
            {
                int index = Int32.Parse(textBox1.Text);
                Properties.Settings.Default.Number1 = textBox1.Text;
                Task.Run(() => CaptureCameraCallback(index, pictureBox1));
            }
            catch { }
            try
            {
                int index = Int32.Parse(textBox2.Text);
                Properties.Settings.Default.Number2 = textBox2.Text;
                Task.Run(() => CaptureCameraCallback(index, pictureBox2));
            }
            catch { }

            Properties.Settings.Default.Save();
        }

        private void CaptureCameraCallback(int index, PictureBox pictureBox)
        {
            Mat frame = new Mat();
            VideoCapture capture = new VideoCapture(index);

            if (capture.IsOpened())
            {
                while (isCameraRunning)
                {
                    try
                    {
                        capture.Read(frame);
                        Image image;
                        image = pictureBox.Image;
                        pictureBox.Image = BitmapConverter.ToBitmap(frame);
                        if (image != null)
                        {
                            image.Dispose();
                        }
                    }
                    catch { }
                }
                capture.Release();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("Start"))
            {
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                CaptureCamera();
                button1.Text = "Stop";
                isCameraRunning = true;
            }
            else
            {
                button1.Text = "Start";
                isCameraRunning = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }
    }
}
