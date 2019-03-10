using System;
using System.Collections.Generic;
using System.Linq;
using restapi.Models;
using System.Drawing;

namespace restapi {
    public static class Database {
        private static ImageProcess ImageDB {get; set;}
        
        public static void AddImg(ImageProcess img) {
            ImageDB = img;
        }

        public static ImageProcess GetImg() {
            return ImageDB;
        }

        public static void DeleteImg() {
            ImageDB = null;
        }
    }
}