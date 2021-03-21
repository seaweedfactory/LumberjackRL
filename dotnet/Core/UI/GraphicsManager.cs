using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumberjackRL.Core.UI
{
    public class GraphicsManager
    {
        public Form Parent { get; set; }

        public GraphicsPack[] images;
        public FontPack font;

        private int _tileSize = 16;
    
        public const int TRANSPARENT=0;
        public const int GRASS=1;
        public const int STONE_FLOOR=2;
        public const int STONE_WALL=3;
        public const int DIRT=5;
        public const int ROOF=6;
        public const int DOOR=7;
        public const int LUMBERJACK=8;
        public const int SPONGE=9;
        public const int GHOST=10;
        public const int SLIME=11;
        public const int TREE=12;
        public const int FLAPJACKS=13;
        public const int APPLE=14;
        public const int KEY=15;
        public const int AXE=16;
        public const int COINS=17;
        public const int JACKET=18;
        public const int HAT=19;
        public const int BOOTS=20;
        public const int LOG=21;
        public const int LANTERN=22;
        public const int SIGN=23;
        public const int FIRE=24;
        public const int ASH=25;
        public const int BUCKET=26;
        public const int SHOVEL=27;
        public const int WATER=28;
        public const int PICKAXE=29;
        public const int ROCKFACE=30;
        public const int TORCH=31;
        public const int GEM=32;
        public const int BACON=33;
        public const int MUSHROOM=34;
        public const int SPARKLE=35;
        public const int BED=36;
        public const int GRAVE=37;
        public const int DEER=38;
        public const int PIG=39;
        public const int CLOVER=40;
        public const int CORN=41;
        public const int PUMPKIN=42;
        public const int MOSS=43;
        public const int FLOODGATE=44;
        public const int MOP=45;
        public const int BONES=46;
        public const int EXIT=47;

        public GraphicsManager(Form parent)
        {
            Parent = parent;

            //TODO: Determine tile size from scaling
            _tileSize = 32;

            images = new GraphicsPack[64];
            this.loadPack(GraphicsManager.GRASS, "graphics/grass");
            this.loadPack(GraphicsManager.STONE_FLOOR, "graphics/stone_floor");
            this.loadPack(GraphicsManager.STONE_WALL, "graphics/stone_wall");
            this.loadPack(GraphicsManager.TRANSPARENT, "graphics/transparent");
            this.loadPack(GraphicsManager.DIRT, "graphics/dirt");
            this.loadPack(GraphicsManager.ROOF, "graphics/roof");
            this.loadPack(GraphicsManager.DOOR, "graphics/door");
            this.loadPack(GraphicsManager.SIGN, "graphics/sign");
            this.loadPack(GraphicsManager.LUMBERJACK, "graphics/lumberjack");
            this.loadPack(GraphicsManager.SPONGE, "graphics/sponge");
            this.loadPack(GraphicsManager.GHOST, "graphics/ghost");
            this.loadPack(GraphicsManager.SLIME, "graphics/slime");
            this.loadPack(GraphicsManager.TREE, "graphics/tree");
            this.loadPack(GraphicsManager.FLAPJACKS, "graphics/flapjacks");
            this.loadPack(GraphicsManager.APPLE, "graphics/apple");
            this.loadPack(GraphicsManager.KEY, "graphics/key");
            this.loadPack(GraphicsManager.AXE, "graphics/axe");
            this.loadPack(GraphicsManager.COINS, "graphics/coin");
            this.loadPack(GraphicsManager.JACKET, "graphics/jacket");
            this.loadPack(GraphicsManager.HAT, "graphics/hat");
            this.loadPack(GraphicsManager.BOOTS, "graphics/boots");
            this.loadPack(GraphicsManager.LOG, "graphics/log");
            this.loadPack(GraphicsManager.LANTERN, "graphics/lantern");
            this.loadPack(GraphicsManager.FIRE, "graphics/fire");
            this.loadPack(GraphicsManager.ASH, "graphics/ash");
            this.loadPack(GraphicsManager.BUCKET, "graphics/bucket");
            this.loadPack(GraphicsManager.SHOVEL, "graphics/shovel");
            this.loadPack(GraphicsManager.WATER, "graphics/water");
            this.loadPack(GraphicsManager.PICKAXE, "graphics/pickaxe");
            this.loadPack(GraphicsManager.ROCKFACE, "graphics/rockface");
            this.loadPack(GraphicsManager.TORCH, "graphics/torch");
            this.loadPack(GraphicsManager.GEM, "graphics/gem");
            this.loadPack(GraphicsManager.BACON, "graphics/bacon");
            this.loadPack(GraphicsManager.MUSHROOM, "graphics/mushroom");
            this.loadPack(GraphicsManager.SPARKLE, "graphics/sparkle");
            this.loadPack(GraphicsManager.BED, "graphics/bed");
            this.loadPack(GraphicsManager.GRAVE, "graphics/grave");
            this.loadPack(GraphicsManager.DEER, "graphics/deer");
            this.loadPack(GraphicsManager.PIG, "graphics/pig");
            this.loadPack(GraphicsManager.CLOVER, "graphics/clover");
            this.loadPack(GraphicsManager.CORN, "graphics/corn");
            this.loadPack(GraphicsManager.PUMPKIN, "graphics/pumpkin");
            this.loadPack(GraphicsManager.MOSS, "graphics/moss");
            this.loadPack(GraphicsManager.FLOODGATE, "graphics/floodgate");
            this.loadPack(GraphicsManager.MOP, "graphics/mop");
            this.loadPack(GraphicsManager.BONES, "graphics/bones");
            this.loadPack(GraphicsManager.EXIT, "graphics/exit");


            font = new FontPack();
            font.load("graphics/font/pixel");

        }

        public Image getImage(int code)
        {
            return images[code].getImage(0);
        }

        public Image getImage(int code, int variation)
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

        public Image getLetter(char letter)
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
            return _tileSize;
        }
    }
}
