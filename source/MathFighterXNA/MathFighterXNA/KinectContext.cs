using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA
{
    public class KinectContext
    {
        public KinectSensor Sensor { get; private set; }
        
        public Texture2D CurrentBitmap { get; private set; }      
        public List<Skeleton> Skeletons { get; private set; }

        private GraphicsDevice graphicsDevice { get; set; }

        public KinectContext(GraphicsDevice device)
        {
            graphicsDevice = device;
            Skeletons = new List<Skeleton>();
        }

        public void Initialize()
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.Sensor = potentialSensor;
                    break;
                }
            }

            if (this.Sensor != null)
            {
                var parameters = new TransformSmoothParameters
                {
                    Smoothing = 0.1f,
                    Correction = 0.0f,
                    Prediction = 0.0f,
                    JitterRadius = 1.0f,
                    MaxDeviationRadius = 0.5f
                };
                
                this.Sensor.SkeletonStream.Enable(parameters);
                this.Sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                try
                {
                    this.Sensor.Start();
                }
                catch (System.IO.IOException ex)
                {
                    this.Sensor = null;
                }
            }

            if (this.Sensor == null)
            {
                throw new Exception("Keine Kinect verfügbar!");
            }
        }

        void ProcessColorFrame()
        {
            using (var cif = this.Sensor.ColorStream.OpenNextFrame(0))
            {
                if (cif != null )
                {
                    Byte[] pixels = new Byte[cif.PixelDataLength];
                    cif.CopyPixelDataTo(pixels);

                    CurrentBitmap = new Texture2D(this.graphicsDevice, cif.Width, cif.Height, false, SurfaceFormat.Color);                   

                    Color[] color = new Color[cif.Height * cif.Width];

                    int index = 0;

                    for (int y = 0; y < cif.Height; y++)
                    {
                        for (int x = 0; x < cif.Width; x++, index += 4)
                        {
                            color[y * cif.Width + x] = new Color(pixels[index + 2], pixels[index + 1], pixels[index + 0]);
                        }
                    }

                    CurrentBitmap.SetData(color);
                }
            }
        }

        void ProcessSkeletonFrame()
        {
            using (var skeletonFrame = this.Sensor.SkeletonStream.OpenNextFrame(0))
            {
                if (skeletonFrame != null)
                {
                    Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);

                    Skeletons.Clear();
                    foreach (Skeleton skel in skeletons)
                    {
                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            Skeletons.Add(skel);             
                        }
                    }
                }
            }
        }

        public Vector2 SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            ColorImagePoint depthPoint = Sensor.MapSkeletonPointToColor(skelpoint, ColorImageFormat.RgbResolution640x480Fps30);
            return new Vector2(depthPoint.X, depthPoint.Y);
        }

        public Skeleton GetSkeletonById(int id)
        {
            foreach (var skel in Skeletons)
            {
                if (skel.TrackingId == id)
                    return skel;
            }

            return null;
        }

        public void Update(GameTime gameTime)
        {
            ProcessColorFrame();
            ProcessSkeletonFrame();
        }
    }
}
