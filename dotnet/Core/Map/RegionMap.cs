using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LumberjackRL.Core.Map
{
    public class RegionMap : IStoreObject
    {
        private Dictionary<String, RegionHeader> regions;
        private String currentRegionID;
        private OverworldCell[,] overworldCells;
        private int overworldWidth;
        private int overworldHeight;
    
        public RegionMap()
        {
            regions = new Dictionary<String, RegionHeader>();
            currentRegionID = "";
            overworldWidth = Quinoa.OVERWORLD_WIDTH;
            overworldHeight = Quinoa.OVERWORLD_HEIGHT;
            overworldCells = new OverworldCell[overworldWidth,overworldHeight];

            for(int x=0; x < overworldWidth; x++)
            {
                for(int y=0; y < overworldHeight; y++)
                {
                    overworldCells[x,y] = new OverworldCell();
                }
            }
        }
    
        public void changeCurrentRegion(String newRegionID)
        {
            if(!currentRegionID.Equals(""))
            {
                RegionHeader tempHeader = regions[getCurrentRegionID()];
                if(tempHeader.regionIsLoaded())
                {
                    tempHeader.storeRegion(true);
                }
                currentRegionID = newRegionID;
                tempHeader = regions[getCurrentRegionID()];
                tempHeader.recallRegion();
            }
            else
            {
                currentRegionID = newRegionID;
                RegionHeader tempHeader = regions[getCurrentRegionID()];
                tempHeader.recallRegion();
            }
        }

        public RegionHeader getCurrentRegionHeader()
        {
            RegionHeader tempHeader = regions[getCurrentRegionID()];
            return tempHeader;
        }

        public void addRegionHeader(RegionHeader regionHeader)
        {
            if(regions.ContainsKey(regionHeader.getId()))
            {
                throw new Exception("region ID already defined");
            }
            else
            {
                regions.Add(regionHeader.getId(), regionHeader);
            }
        }

        public RegionHeader getRegionHeaderByID(String ID)
        {
            return regions[ID];
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(getCurrentRegionID());
            outStream.WriteLine(regions.Count + "");
            foreach(String regionKey in regions.Keys)
            {
                regions[regionKey].SaveObject(outStream);
            }

            outStream.WriteLine(getOverworldWidth() + "");
            outStream.WriteLine(getOverworldHeight() + "");
            for(int x=0; x < getOverworldWidth(); x++)
            {
                for(int y=0; y < getOverworldHeight(); y++)
                {
                    getOverworldCells()[x,y].SaveObject(outStream);
                }
            }
        }

        public void LoadObject(StreamReader inStream)
        {
            currentRegionID = inStream.ReadLine();
            regions.Clear();
            int regionsSize = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < regionsSize; i++)
            {
                RegionHeader tempRegionHeader = new RegionHeader("");
                tempRegionHeader.LoadObject(inStream);
                regions.Add(tempRegionHeader.getId(), tempRegionHeader);
            }
            setOverworldWidth(Int32.Parse(inStream.ReadLine()));
            setOverworldHeight(Int32.Parse(inStream.ReadLine()));
            for(int x=0; x < getOverworldWidth(); x++)
            {
                for(int y=0; y < getOverworldHeight(); y++)
                {
                    getOverworldCells()[x,y].LoadObject(inStream);
                }
            }
        }

        /**
         * @return the currentRegionID
         */
        public String getCurrentRegionID() {
            return currentRegionID;
        }

        /**
         * @return the overworldCells
         */
        public OverworldCell[,] getOverworldCells() 
        {
            return overworldCells;
        }

        /**
         * @param overworldCells the overworldCells to set
         */
        public void setOverworldCells(OverworldCell[,] overworldCells) 
        {
            this.overworldCells = overworldCells;
        }

        /**
         * @return the overworldWidth
         */
        public int getOverworldWidth() 
        {
            return overworldWidth;
        }

        /**
         * @param overworldWidth the overworldWidth to set
         */
        public void setOverworldWidth(int overworldWidth) 
        {
            this.overworldWidth = overworldWidth;
        }

        /**
         * @return the overworldHeight
         */
        public int getOverworldHeight() 
        {
            return overworldHeight;
        }

        /**
         * @param overworldHeight the overworldHeight to set
         */
        public void setOverworldHeight(int overworldHeight) 
        {
            this.overworldHeight = overworldHeight;
        }
    }
}
