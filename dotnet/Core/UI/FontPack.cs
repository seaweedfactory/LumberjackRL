using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LumberjackRL.Core.UI
{
    public class FontPack
    {
        public const int LETTER_COUNT=75;
        public const int WIDTH=5;
        public const int HEIGHT=7;
        public const double SPACING_PERCENT=1.25;

        public Image[] images;

        public FontPack()
        {
            images = new Image[LETTER_COUNT];
        }

        public Image getImage(int variation)
        {
            return images[variation];
        }

        public void load(String filename)
        {
            //check for any more variations
            for(int varCounter=1; varCounter < LETTER_COUNT + 1; varCounter++)
            {
                images[varCounter-1] = loadImage(filename + "_" + (varCounter) + ".PNG");
            }
        }

        public Image loadImage(String filename)
        {
            try
            {
                return new Bitmap(filename);
            }
            catch (Exception)
            {
                //Ignore load error
            }
            return null;
        }
    }
}
