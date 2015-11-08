using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace LumberjackRL.Core.UI
{
    public class GraphicsPack
    {
        public Image[] images;
        public int graphicsCode;
        public int variations;

        public GraphicsPack(int graphicsCode)
        {
            images = new Image[16];
            this.graphicsCode = graphicsCode;
        }

        public Image getImage(int variation)
        {
            return images[variation];
        }

        public void load(String filename)
        {
            //load the first image
            images[0] = loadImage(filename + ".PNG");
            variations = 1;

            //check for any more variations
            int varCounter = 1;
            while (varCounter < 16 && File.Exists(filename + "_" + (varCounter + 1) + ".PNG"))
            {
                images[varCounter] = loadImage(filename + "_" + (varCounter + 1) + ".PNG");
                variations = (varCounter + 1);
                varCounter++;
            }
        }

        public Image loadImage(String filename)
        {
            return new Bitmap(filename);
        }
    }
}
