using System;
using System.Globalization;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace restapi.Models {
    public class ImageProcess {
        public Bitmap ImageGraphic {get; set;}
        public string filePath {get; set;}
        public ImageProcess(Bitmap image, string path) {
            ImageGraphic = image;
            filePath = path;
        }

        public Bitmap FlipCommand(char direction, Bitmap image) {
            //Flip image depending on direction
            if (direction == 'x' || direction == 'X') {
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                return image;
            } else if (direction == 'y' || direction == 'Y') {
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                return image;
            } else {
                return image;
            }
        }

        public Bitmap GrayScaleCommand(Bitmap original) {
            //new bitmap with same size as the original original
            Bitmap editedImg = new Bitmap(original.Width, original.Height);
            //get a graphics object from the new image
            Graphics graph = Graphics.FromImage(editedImg);

            //grayscale ColorMatrix with gray colors
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][] {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                //Image attributes allow the common data sets for graphics in an 
                //image to be edited, such as the brush used or the picture colors
                //per pixel. What we are going to do here is copy the color matrix
                //of light grey to dark grey on to the attributes for the new graphic.
                //This will allow us to then draw out the image, line by line, with 
                //a new set of attributes, which in this case will all be gray colors
                ImageAttributes attributes = new ImageAttributes();
                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                graph.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

                graph.Dispose(); //get rid of graphics obj
                return editedImg;
            }

        public Bitmap RotateCommand(float degrees, Bitmap image) {
            //Traditional rotations
            if(degrees == 90 || degrees == 180 || degrees == 270) {
                if(degrees == 90) {
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                } else if (degrees == 180) {
                    image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                } else if (degrees == 270) {
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }
                return image;
            } else {
                /*
                 * !!WARNING!!
                 * If the picture is being cropped this is why!
                 * This method is imperfect.
                 * It attempts to measure out the bitmap's hypotenuse
                 * and using a Graphics class, it draws out the picture onto a
                 * second created bitmap. The problem arises on the DrawImage function.
                 * DrawImage draws a new drawing starting from the newly created space's
                 * (0,0) coordinates which is the top left corner of the space. If an image is
                 * not perfectly square this means that during the rotation a new area of empty
                 * space is drawn out during DrawImage, which means that with every rotation the image
                 * gets larger and larger. Also, if the image were to rotate at a point beyond its 
                 * boundary then it will most likely get cropped out at the edges.
                 * My recommendation then would be to handle things such as this from a completly 
                 * different library then System.Drawing, since image processing is very basic with it.
                 */
                
                float width = image.Width, height = image.Height;
                int hypotenuse = System.Convert.ToInt32(System.Math.Floor(
                    Math.Sqrt(height*height + width * width))); //Use Pythagoras to find the hypotenuse
                Bitmap rotateImg = new Bitmap(hypotenuse, hypotenuse); //New bitmmap is both hypotenuse wide and high
                //Ensure same resolution between the two bitmaps
                rotateImg.SetResolution(image.HorizontalResolution, image.VerticalResolution); 

                Graphics graph = Graphics.FromImage(rotateImg);
                float centerX = (float)image.Width / 2, centerY = (float)image.Height /2;
                graph.TranslateTransform(centerX, centerY); //Set point to center
                graph.RotateTransform(degrees); //Rotate from center
                graph.TranslateTransform(-centerX, -centerY); //Restore transform point
                //Using overloaded DrawImage, we provide both a new x,y point to draw from
                //as well as the new width and height of the image
                graph.DrawImage(image, 0, 0, width, height);
                
                graph.Dispose(); //Get rid of graph object
                
                return rotateImg;
            }
        }

        public Bitmap ConvertCommand(Bitmap image) {
            int width = 128;
            int height = 128;
            using(image) {
                var resized = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resized)) {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);
                    //resized.Save($"resized-{file}", ImageFormat.Png);
                    //Console.WriteLine($"Saving resized-{file} thumbnail");
                    return resized;
                }       
            }            
        }

        public Bitmap ResizeCommand(int x, int y, Bitmap image) {
            int width = x;
            int height = y;
            using(image) {
                var resized = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resized)) {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);
                    //resized.Save($"resized-{file}", ImageFormat.Png);
                    //Console.WriteLine($"Saving resized-{file} thumbnail");
                    return resized;
                }
            }
        }  
    }
}