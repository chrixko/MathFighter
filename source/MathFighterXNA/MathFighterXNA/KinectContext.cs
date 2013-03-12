using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA {

    public class KinectContext {

        public KinectSensor Sensor { get; private set; }
        
        public Texture2D CurrentBitmap { get; private set; }      
        public List<Skeleton> Skeletons { get; private set; }

        private GraphicsDevice graphicsDevice { get; set; }

        public KinectContext(GraphicsDevice device) {
            graphicsDevice = device;
            Skeletons = new List<Skeleton>();
        }

        public void Initialize() {
            foreach (var potentialSensor in KinectSensor.KinectSensors) {
                if (potentialSensor.Status == KinectStatus.Connected) {
                    this.Sensor = potentialSensor;
                    break;
                }
            }

            if (this.Sensor != null) {
                var parameters = new TransformSmoothParameters {
                    Smoothing = 0.0f,
                    Correction = 0.0f,
                    Prediction = 0.0f,
                    JitterRadius = 0.0f,
                    MaxDeviationRadius = 0.0f
                };
                
                this.Sensor.SkeletonStream.Enable(parameters);
                this.Sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);                

                try {
                    this.Sensor.Start();
                } catch (System.IO.IOException ex) {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    this.Sensor = null;
                }
            }

            if (this.Sensor == null) {
                throw new Exception("Keine Kinect verfügbar!");
            }

            this.CurrentBitmap = Assets.NumberBackgroundSprite;
        }

        void ProcessColorFrame() {
            using (var cif = this.Sensor.ColorStream.OpenNextFrame(0)) {
                if (cif != null ) {
                    Byte[] pixelData = new Byte[cif.PixelDataLength];
                    cif.CopyPixelDataTo(pixelData);
                    
                    Byte[] bgraPixelData = new Byte[cif.PixelDataLength];
                    for (int i = 0; i < pixelData.Length; i += 4) {
                        bgraPixelData[i] = pixelData[i + 2];
                        bgraPixelData[i + 1] = pixelData[i + 1];
                        bgraPixelData[i + 2] = pixelData[i];
                        bgraPixelData[i + 3] = (Byte)255;
                    }
                    CurrentBitmap = new Texture2D(this.graphicsDevice, cif.Width, cif.Height); 
                    CurrentBitmap.SetData(bgraPixelData);
                }
            }
        }

        void ProcessSkeletonFrame() {
            using (var skeletonFrame = this.Sensor.SkeletonStream.OpenNextFrame(0)) {
                if (skeletonFrame != null) {
                    Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);

                    Skeletons.Clear();
                    foreach (Skeleton skel in skeletons) {
                        if (skel.TrackingState == SkeletonTrackingState.Tracked) {
                            Skeletons.Add(skel);             
                        }
                    }
                }
            }
        }

        public Point SkeletonPointToScreen(SkeletonPoint skelpoint) {            
            ColorImagePoint colorPoint = Sensor.CoordinateMapper.MapSkeletonPointToColorPoint(skelpoint, ColorImageFormat.RgbResolution640x480Fps30);
            return new Point((int)(colorPoint.X / MainGame.KinectScaleX) + MainGame.KinectOffsetX, (int)(colorPoint.Y / MainGame.KinectScaleY) + MainGame.KinectOffsetY);
        }

        public Skeleton GetSkeletonById(int id) {
            return (from Skeleton s in Skeletons where s.TrackingId == id select s).FirstOrDefault();
        }

        public Skeleton GetFirstSkeleton() {
            return Skeletons.FirstOrDefault<Skeleton>();
        }

        public Skeleton GetLeftSkeleton() { 
            return (from Skeleton s in Skeletons orderby s.Position.X ascending select s).FirstOrDefault();
        }

        public Skeleton GetRightSkeleton() {
            return (from Skeleton s in Skeletons where s != GetLeftSkeleton() orderby s.Position.X descending select s).FirstOrDefault();            
        }

        public void Update() {
            ProcessColorFrame();
            ProcessSkeletonFrame();
        }
    }
}