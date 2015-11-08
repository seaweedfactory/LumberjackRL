using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Map
{
    public class Building: IStoreObject, ICopyObject
    {
        public const int WOOD_ROOF=0;
        public const int BEAM_ROOF=1;

        private String name;                //What is the building called?
        private int x, y;                   //position
        private int width, height;          //dimensions
        private BuildingType buildingType;  //type of building
        private Position door;              //where is front door?
        private bool lit;                //is the building self-lighted?
        private int roofType;               //what does the roof look like?


        public Building()
        {
            name = "";
            x = 0;
            y = 0;
            width = 0;
            height = 0;
            buildingType = BuildingType.NULL;
            door = new Position();
            lit = true;
            roofType = WOOD_ROOF;
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(getName());
            outStream.WriteLine(x + "");
            outStream.WriteLine(y + "");
            outStream.WriteLine(width + "");
            outStream.WriteLine(height + "");
            outStream.WriteLine(door.x + "");
            outStream.WriteLine(door.y + "");
            outStream.WriteLine(isLit() + "");
            outStream.WriteLine(getRoofType()+"");
            outStream.WriteLine(EnumUtil.EnumName<BuildingType>(buildingType));
        }

        public void LoadObject(StreamReader inStream)
        {
            setName(inStream.ReadLine());
            x = Int32.Parse(inStream.ReadLine());
            y = Int32.Parse(inStream.ReadLine());
            width = Int32.Parse(inStream.ReadLine());
            height = Int32.Parse(inStream.ReadLine());
            door.x = Int32.Parse(inStream.ReadLine());
            door.y = Int32.Parse(inStream.ReadLine());
            setLit(Boolean.Parse(inStream.ReadLine()));
            setRoofType(Int32.Parse(inStream.ReadLine()));
            buildingType = (BuildingType)Enum.Parse(typeof(BuildingType), inStream.ReadLine());
        }

        public object CopyObject()
        {
            Building newBuild = new Building();
            newBuild.setName(this.getName());
            newBuild.setX(this.x);
            newBuild.setY(this.y);
            newBuild.setWidth(this.width);
            newBuild.setHeight(this.height);
            newBuild.setBuildingType(this.buildingType);
            newBuild.setDoor(this.door.x, this.door.y);
            newBuild.setLit(this.isLit());
            newBuild.setRoofType(this.roofType);
            return newBuild;
        }

        public void setPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /**
         * @return the x
         */
        public int getX() {
            return x;
        }

        /**
         * @param x the x to set
         */
        public void setX(int x) {
            this.x = x;
        }

        /**
         * @return the y
         */
        public int getY() {
            return y;
        }

        /**
         * @param y the y to set
         */
        public void setY(int y) {
            this.y = y;
        }

        /**
         * @return the width
         */
        public int getWidth() {
            return width;
        }

        /**
         * @param width the width to set
         */
        public void setWidth(int width) {
            this.width = width;
        }

        /**
         * @return the height
         */
        public int getHeight() {
            return height;
        }

        /**
         * @param height the height to set
         */
        public void setHeight(int height) {
            this.height = height;
        }

        /**
         * @return the buildingType
         */
        public BuildingType getBuildingType() {
            return buildingType;
        }

        /**
         * @param buildingType the buildingType to set
         */
        public void setBuildingType(BuildingType buildingType) {
            this.buildingType = buildingType;
        }

        /**
         * @return the door
         */
        public Position getDoor() {
            return door;
        }

        public void setDoor(int x, int y)
        {
            door.x = x;
            door.y = y;
        }

        /**
         * @return the lit
         */
        public bool isLit() {
            return lit;
        }

        /**
         * @param lit the lit to set
         */
        public void setLit(bool lit) {
            this.lit = lit;
        }

        /**
         * @return the roofType
         */
        public int getRoofType() {
            return roofType;
        }

        /**
         * @param roofType the roofType to set
         */
        public void setRoofType(int roofType) {
            this.roofType = roofType;
        }

        /**
         * @return the name
         */
        public String getName() {
            return name;
        }

        /**
         * @param name the name to set
         */
        public void setName(String name) {
            this.name = name;
        }
    }
}
