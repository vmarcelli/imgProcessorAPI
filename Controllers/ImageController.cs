using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using restapi.Models;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace restapi.Controllers {
    
    [Route("[controller]")]
    public class ImageController: Controller {
        
        [HttpGet("")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetImage() {
            if(Database.GetImg() == null) {
                return NotFound();
            }
            ImageProcess picture = Database.GetImg();
            string path = picture.filePath;
            Bitmap image = picture.ImageGraphic;
            image.Save($"image-{path}", ImageFormat.Png);
            return Ok();
        } 

        [HttpDelete]
        [ProducesResponseType(200)]
        public IActionResult DeleteImage() {
            Database.DeleteImg();
            return Ok();
        }

        [HttpPost("{path}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FileNotFoundError), 409)]
        public IActionResult Create(string path) { 
            
            Console.WriteLine($"Loading {path}");  
            try {
                //FileStream for png
                FileStream pngStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                var image = new Bitmap(pngStream);
                ImageProcess processor = new ImageProcess(image, path);
                Database.AddImg(processor);

                pngStream.Close();
                return Ok();
            } catch(FileNotFoundException) {
                return StatusCode(100, new FileNotFoundError());
            }                    
        }

        [HttpPost("picturebox/thumbailcmd")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult ThumbnailImage() {
            if(Database.GetImg() == null) {
                return NotFound();
            } else {
                ImageProcess processor = Database.GetImg();
                Bitmap editedImg = processor.ConvertCommand(processor.ImageGraphic);
                ImageProcess newImage = new ImageProcess(editedImg, processor.filePath);
                Database.AddImg(newImage);
                return Ok();
            }
            
        }

        [HttpPost("picturebox/rotationcmd/{degrees}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RotateImage(float degrees) {
            if(Database.GetImg() == null) {
                return NotFound();
            } else {
                ImageProcess processor = Database.GetImg();
                Bitmap editedImg = processor.RotateCommand(degrees, processor.ImageGraphic);
                ImageProcess newImage = new ImageProcess(editedImg, processor.filePath);
                Database.AddImg(newImage);
                return Ok();
            }
        }

        [HttpPost("picturebox/flippedcmd/{direction}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult FlipImage(char direction) {
            if(Database.GetImg() == null) {
                return NotFound();
            } else {
                ImageProcess processor = Database.GetImg();
                Bitmap editedImg = processor.FlipCommand(direction, processor.ImageGraphic);
                ImageProcess newImage = new ImageProcess(editedImg, processor.filePath);
                Database.AddImg(newImage);
                return Ok();
            }
        }

        [HttpPost("picturebox/graycmd")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GrayScaleCommand() {
            if(Database.GetImg() == null) {
                return NotFound();
            } else {
                ImageProcess processor = Database.GetImg();
                Bitmap editedImg = processor.GrayScaleCommand(processor.ImageGraphic);
                ImageProcess newImage = new ImageProcess(editedImg, processor.filePath);
                Database.AddImg(newImage);
                return Ok();
            }
        }

        [HttpPost("picturebox/sizecmd/<{width},{height}>")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult ResizeImage(int width, int height) {
            if(Database.GetImg() == null) {
                return NotFound();
            } else {
                ImageProcess processor = Database.GetImg();
                Bitmap editedImg = processor.ResizeCommand(width, height, processor.ImageGraphic);
                ImageProcess newImage = new ImageProcess(editedImg, processor.filePath);
                Database.AddImg(newImage);
                return Ok();
            }
        }

    }
}