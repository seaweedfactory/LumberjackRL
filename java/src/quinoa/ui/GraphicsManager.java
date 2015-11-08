package quinoa.ui;

import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;

public class GraphicsManager
{
    private class GraphicsPack
    {
        public BufferedImage[] images;
        public int graphicsCode;
        public int variations;

        public GraphicsPack(int graphicsCode)
        {
            images = new BufferedImage[16];
            this.graphicsCode = graphicsCode;
        }

        public BufferedImage getImage(int variation)
        {
            return images[variation];
        }

        public void load(String filename)
        {
            //load the first image
            images[0] = loadImage(filename + ".PNG");
            variations = 1;

            //check for any more variations
            int varCounter=1;
            while(varCounter < 16 && new File(filename + "_" + (varCounter+1) + ".PNG").exists())
            {
                images[varCounter] = loadImage(filename + "_" + (varCounter+1) + ".PNG");
                variations = (varCounter+1);
                varCounter++;
            }
        }

        public BufferedImage loadImage(String filename)
        {
            try
            {
                return ImageIO.read(new File(filename));
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
            return null;
        }
    }

    private class FontPack
    {
        public static final int LETTER_COUNT=75;
        public static final int WIDTH=5;
        public static final int HEIGHT=7;
        public static final double SPACING_PERCENT=1.25;

        public BufferedImage[] images;

        public FontPack()
        {
            images = new BufferedImage[LETTER_COUNT];
        }

        public BufferedImage getImage(int variation)
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

        public BufferedImage loadImage(String filename)
        {
            try
            {
                return ImageIO.read(new File(filename));
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
            return null;
        }
    }

    public GraphicsPack[] images;
    public FontPack font;
    
    public static final int TRANSPARENT=0;
    public static final int GRASS=1;
    public static final int STONE_FLOOR=2;
    public static final int STONE_WALL=3;
    public static final int DIRT=5;
    public static final int ROOF=6;
    public static final int DOOR=7;
    public static final int LUMBERJACK=8;
    public static final int SPONGE=9;
    public static final int GHOST=10;
    public static final int SLIME=11;
    public static final int TREE=12;
    public static final int FLAPJACKS=13;
    public static final int APPLE=14;
    public static final int KEY=15;
    public static final int AXE=16;
    public static final int COINS=17;
    public static final int JACKET=18;
    public static final int HAT=19;
    public static final int BOOTS=20;
    public static final int LOG=21;
    public static final int LANTERN=22;
    public static final int SIGN=23;
    public static final int FIRE=24;
    public static final int ASH=25;
    public static final int BUCKET=26;
    public static final int SHOVEL=27;
    public static final int WATER=28;
    public static final int PICKAXE=29;
    public static final int ROCKFACE=30;
    public static final int TORCH=31;
    public static final int GEM=32;
    public static final int BACON=33;
    public static final int MUSHROOM=34;
    public static final int SPARKLE=35;
    public static final int BED=36;
    public static final int GRAVE=37;
    public static final int DEER=38;
    public static final int PIG=39;
    public static final int CLOVER=40;
    public static final int CORN=41;
    public static final int PUMPKIN=42;
    public static final int MOSS=43;
    public static final int FLOODGATE=44;
    public static final int MOP=45;
    public static final int BONES=46;
    public static final int EXIT=47;
    

    public GraphicsManager()
    {
        images = new GraphicsPack[64];
        this.loadPack(this.GRASS, "graphics/grass");
        this.loadPack(this.STONE_FLOOR, "graphics/stone_floor");
        this.loadPack(this.STONE_WALL, "graphics/stone_wall");
        this.loadPack(this.TRANSPARENT, "graphics/transparent");
        this.loadPack(this.DIRT, "graphics/dirt");
        this.loadPack(this.ROOF, "graphics/roof");
        this.loadPack(this.DOOR, "graphics/door");
        this.loadPack(this.SIGN, "graphics/sign");
        this.loadPack(this.LUMBERJACK, "graphics/lumberjack");
        this.loadPack(this.SPONGE, "graphics/sponge");
        this.loadPack(this.GHOST, "graphics/ghost");
        this.loadPack(this.SLIME, "graphics/slime");
        this.loadPack(this.TREE, "graphics/tree");
        this.loadPack(this.FLAPJACKS, "graphics/flapjacks");
        this.loadPack(this.APPLE, "graphics/apple");
        this.loadPack(this.KEY, "graphics/key");
        this.loadPack(this.AXE, "graphics/axe");
        this.loadPack(this.COINS, "graphics/coin");
        this.loadPack(this.JACKET, "graphics/jacket");
        this.loadPack(this.HAT, "graphics/hat");
        this.loadPack(this.BOOTS, "graphics/boots");
        this.loadPack(this.LOG, "graphics/log");
        this.loadPack(this.LANTERN, "graphics/lantern");
        this.loadPack(this.FIRE, "graphics/fire");
        this.loadPack(this.ASH, "graphics/ash");
        this.loadPack(this.BUCKET, "graphics/bucket");
        this.loadPack(this.SHOVEL, "graphics/shovel");
        this.loadPack(this.WATER, "graphics/water");
        this.loadPack(this.PICKAXE, "graphics/pickaxe");
        this.loadPack(this.ROCKFACE, "graphics/rockface");
        this.loadPack(this.TORCH, "graphics/torch");
        this.loadPack(this.GEM, "graphics/gem");
        this.loadPack(this.BACON, "graphics/bacon");
        this.loadPack(this.MUSHROOM, "graphics/mushroom");
        this.loadPack(this.SPARKLE, "graphics/sparkle");
        this.loadPack(this.BED, "graphics/bed");
        this.loadPack(this.GRAVE, "graphics/grave");
        this.loadPack(this.DEER, "graphics/deer");
        this.loadPack(this.PIG, "graphics/pig");
        this.loadPack(this.CLOVER, "graphics/clover");
        this.loadPack(this.CORN, "graphics/corn");
        this.loadPack(this.PUMPKIN, "graphics/pumpkin");
        this.loadPack(this.MOSS, "graphics/moss");
        this.loadPack(this.FLOODGATE, "graphics/floodgate");
        this.loadPack(this.MOP, "graphics/mop");
        this.loadPack(this.BONES, "graphics/bones");
        this.loadPack(this.EXIT, "graphics/exit");


        font = new FontPack();
        font.load("graphics/font/pixel");

    }

    public BufferedImage getImage(int code)
    {
        return images[code].getImage(0);
    }

    public BufferedImage getImage(int code, int variation)
    {
        return images[code].getImage(variation);
    }

    public int getVariations(int code)
    {
        return images[code].variations;
    }

    public void loadPack(int code, String filename)
    {
        images[code] = new GraphicsPack(code);
        images[code].load(filename);
    }

    public BufferedImage getLetter(char letter)
    {
        int index=0;
        switch(letter)
        {
            case 'A': index = 0; break;
            case 'B': index = 1; break;
            case 'C': index = 2; break;
            case 'D': index = 3; break;
            case 'E': index = 4; break;
            case 'F': index = 5; break;
            case 'G': index = 6; break;
            case 'H': index = 7; break;
            case 'I': index = 8; break;
            case 'J': index = 9; break;
            case 'K': index = 10; break;
            case 'L': index = 11; break;
            case 'M': index = 12; break;
            case 'N': index = 13; break;
            case 'O': index = 14; break;
            case 'P': index = 15; break;
            case 'Q': index = 16; break;
            case 'R': index = 17; break;
            case 'S': index = 18; break;
            case 'T': index = 19; break;
            case 'U': index = 20; break;
            case 'V': index = 21; break;
            case 'W': index = 22; break;
            case 'X': index = 23; break;
            case 'Y': index = 24; break;
            case 'Z': index = 25; break;
            case 'a': index = 26; break;
            case 'b': index = 27; break;
            case 'c': index = 28; break;
            case 'd': index = 29; break;
            case 'e': index = 30; break;
            case 'f': index = 31; break;
            case 'g': index = 32; break;
            case 'h': index = 33; break;
            case 'i': index = 34; break;
            case 'j': index = 35; break;
            case 'k': index = 36; break;
            case 'l': index = 37; break;
            case 'm': index = 38; break;
            case 'n': index = 39; break;
            case 'o': index = 40; break;
            case 'p': index = 41; break;
            case 'q': index = 42; break;
            case 'r': index = 43; break;
            case 's': index = 44; break;
            case 't': index = 45; break;
            case 'u': index = 46; break;
            case 'v': index = 47; break;
            case 'w': index = 48; break;
            case 'x': index = 49; break;
            case 'y': index = 50; break;
            case 'z': index = 51; break;
            case '0': index = 52; break;
            case '1': index = 53; break;
            case '2': index = 54; break;
            case '3': index = 55; break;
            case '4': index = 56; break;
            case '5': index = 57; break;
            case '6': index = 58; break;
            case '7': index = 59; break;
            case '8': index = 60; break;
            case '9': index = 61; break;
            case '.': index = 62; break;
            case '?': index = 63; break;
            case '!': index = 64; break;
            case ':': index = 65; break;
            case '/': index = 66; break;
            case ',': index = 67; break;
            case '\'': index = 68; break;
            case '-': index = 69; break;
            case '$': index = 70; break;
            case '%': index = 71; break;
            case '(': index = 72; break;
            case ')': index = 73; break;
            case '=': index = 74; break;
            default: return null;
        }

        return font.getImage(index);
    }

    public int getLetterWidth()
    {
        return FontPack.WIDTH;
    }

    public int getLetterHeight()
    {
        return FontPack.HEIGHT;
    }

    public double getLetterSpacingPercent()
    {
        return FontPack.SPACING_PERCENT;
    }

    public int getTileSize()
    {
        return 16;
    }

    
}
