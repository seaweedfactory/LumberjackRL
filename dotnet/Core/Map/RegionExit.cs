using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Map
{
    public class RegionExit : IStoreObject
    {
        private int x, y;                   //origin of the exit
        private int dx, dy;                 //destination of the exit
        private String destinationRegionID; //what region does the exit lead to?
        private RegionExitDecoratorType exitDecorator; //what does the exit look like?

        public RegionExit()
        {
            x = 0;
            y = 0;
            dx = 0;
            dy = 0;
            destinationRegionID = "";
            exitDecorator = RegionExitDecoratorType.NONE;
        }

        public RegionExit(int x, int y, int dx, int dy, String destinationRegionID, RegionExitDecoratorType exitDecorator)
        {
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
            this.destinationRegionID = destinationRegionID;
            this.exitDecorator = exitDecorator;
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(x+"");
            outStream.WriteLine(y+"");
            outStream.WriteLine(getDx()+"");
            outStream.WriteLine(getDy()+"");
            outStream.WriteLine(getDestinationRegionID());
            outStream.WriteLine(exitDecorator.ToString());
        }

        public void LoadObject(StreamReader inStream)
        {
            setX(Int32.Parse(inStream.ReadLine()));
            setY(Int32.Parse(inStream.ReadLine()));
            setDx(Int32.Parse(inStream.ReadLine()));
            setDy(Int32.Parse(inStream.ReadLine()));
            setDestinationRegionID(inStream.ReadLine());
            setExitDecorator((RegionExitDecoratorType)Enum.Parse(typeof(RegionExitDecoratorType),inStream.ReadLine()));
        }

        /**
         * @return the x
         */
        public int getX() {
            return x;
        }

        /**
         * @return the y
         */
        public int getY() {
            return y;
        }

        /**
         * @return the destinationRegionID
         */
        public String getDestinationRegionID() {
            return destinationRegionID;
        }

        /**
         * @return the dy
         */
        public int getDy() {
            return dy;
        }

        /**
         * @param dy the dy to set
         */
        public void setDy(int dy) {
            this.dy = dy;
        }

        /**
         * @return the dx
         */
        public int getDx() {
            return dx;
        }

        /**
         * @param dx the dx to set
         */
        public void setDx(int dx) {
            this.dx = dx;
        }

        /**
         * @param destinationRegionID the destinationRegionID to set
         */
        public void setDestinationRegionID(String destinationRegionID) {
            this.destinationRegionID = destinationRegionID;
        }

        /**
         * @param x the x to set
         */
        public void setX(int x) {
            this.x = x;
        }

        /**
         * @param y the y to set
         */
        public void setY(int y) {
            this.y = y;
        }

        /**
         * @return the exitDecorator
         */
        public RegionExitDecoratorType getExitDecorator() 
        {
            return exitDecorator;
        }

        /**
         * @param exitDecorator the exitDecorator to set
         */
        public void setExitDecorator(RegionExitDecoratorType exitDecorator) 
        {
            this.exitDecorator = exitDecorator;
        }
    }
}
