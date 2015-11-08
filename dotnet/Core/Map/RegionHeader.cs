using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LumberjackRL.Core.Map
{
    public class RegionHeader : IStoreObject
    {
        private String id;                 //ID of the region, used for linking
        private List<RegionExit> exits;    //Exit data
        private Region region;             //Terrain level data, loaded/saved as needed
        private String name;               //Name of the region, if possible
        
        public RegionHeader(String id)
        {
            this.id = id;
            exits = new List<RegionExit>();
            region = null;
            name = "";
        }

        public bool regionIsLoaded()
        {
            return (getRegion() != null);
        }

        public void storeRegion(bool unloadRegionAfterSave)
        {
            if(regionIsLoaded())
            {
                //Remove expired or copied items
                getRegion().removeExpiredObjects();

                //Write to region file
                using (StreamWriter writer = new StreamWriter(this.getId() + ".region"))
                {
                    getRegion().SaveObject(writer);
                }

                if (unloadRegionAfterSave)
                {
                    setRegion(null);
                }
            }
            else
            {
                throw new Exception("tried to store null region");
            }
        }

        public void recallRegion()
        {
            if(!regionIsLoaded())
            {
                setRegion(new Region(1, 1));
                using (StreamReader reader = new StreamReader(this.getId() + ".region"))
                {
                    getRegion().LoadObject(reader);
                }
            }
            else
            {
                throw new Exception("tried to recall non-null region");
            }
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(getId());
            outStream.WriteLine(getName());
            outStream.WriteLine(getExits().Count.ToString());
            foreach(RegionExit tempExit in getExits())
            {
                tempExit.SaveObject(outStream);
            }
        }

        public void LoadObject(StreamReader inStream)
        {
            id = inStream.ReadLine();
            setName(inStream.ReadLine());
            exits = new List<RegionExit>();
            int exitsSize = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < exitsSize; i++)
            {
                RegionExit newExit = new RegionExit();
                newExit.LoadObject(inStream);
                getExits().Add(newExit);
            }
        }

        /**
         * @return the id
         */
        public String getId() {
            return id;
        }

        /**
         * @return the exits
         */
        public List<RegionExit> getExits() {
            return exits;
        }

        /**
         * @return the region
         */
        public Region getRegion() {
            return region;
        }

        /**
         * @param region the region to set
         */
        public void setRegion(Region region) {
            this.region = region;
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
