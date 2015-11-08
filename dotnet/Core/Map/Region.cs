using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;
using LumberjackRL.Core.Items;
using System.IO;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Map
{
    public class Region : IStoreObject
    {
        private int width;                  //Width of region in tiles
        private int height;                 //Height of region in tiles
        private Terrain[,] terrainField;    //Holds persistant terrain
        private List<Monster> monsters;     //Holds monster data
        private List<Item> items;           //Holds item data
        private List<Building> buildings;   //Holds building data
        private LightingModel lightingModel;//What kind of lighting to use

        public Region(int width , int height)
        {
            this.width = width;
            this.height = height;
            terrainField = new Terrain[width,height];
            monsters = new List<Monster>();
            items = new List<Item>();
            buildings = new List<Building>();
            lightingModel = LightingModel.ABOVE_GROUND;

            for(int x=0; x < width; x++)
            {
                for(int y=0; y < height; y++)
                {
                    terrainField[x,y] = new Terrain();
                }
            }
        }

        public void buildBuildings()
        {
            foreach(Building tempBuild in buildings)
            {
                BuildingManager.build(this, tempBuild.getX(), tempBuild.getY(), tempBuild);
            }
        }
      
        public void setTerrain(int x, int y, Terrain newTerrain)
        {
            terrainField[x,y] = newTerrain;
        }

        public Terrain getTerrain(int x, int y)
        {
            if(x >= 0 && x < width && y >=0 && y < height)
            {
                return terrainField[x,y];
            }
            else
            {
                return null;
            }
        }

        public Item getItem(int x, int y)
        {
            foreach(Item tempItem in items)
            {
                if(tempItem.x == x && tempItem.y == y)
                {
                    return tempItem;
                }
            }
            return null;
        }

        public void removeExpiredObjects()
        {
            //remove expired items on monsters
            foreach(Monster tempMonster in monsters)
            {
                tempMonster.inventory.removedExpiredItems();
            }
        
            //remove items on the ground
            List<Item> removeItemList = new List<Item>();
            foreach(Item tempItem in items)
            {
                if(tempItem.ShouldBeRemoved)
                {
                    tempItem.RemoveObject();
                    removeItemList.Add(tempItem);
                }
            }
            foreach(Item tempItem in removeItemList)
            {
                getItems().Remove(tempItem);
            }

            //remove monsters
            List<Monster> removeMonsterList = new List<Monster>();
            foreach(Monster tempMonster in monsters)
            {
                if(tempMonster.ShouldBeRemoved && !tempMonster.ID.Equals(MonsterActionManager.PLAYER_ID))
                {
                    tempMonster.RemoveObject();
                    removeMonsterList.Add(tempMonster);
                }
            }
            foreach(Monster tempMonster in removeMonsterList)
            {
                getMonsters().Remove(tempMonster);
            }
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(getWidth()+"");
            outStream.WriteLine(getHeight()+"");
            for(int x=0; x < getWidth(); x++)
            {
                for(int y=0; y < getHeight(); y++)
                {
                    terrainField[x,y].SaveObject(outStream);
                }
            }
            this.removeExpiredObjects();    //Remove any objects which should not be saved or have been copied
            outStream.WriteLine(monsters.Count+"");
            foreach(Monster tempMonster in getMonsters())
            {
                tempMonster.SaveObject(outStream);
            }
            outStream.WriteLine(getItems().Count+"");
            foreach(Item tempItem in getItems())
            {
                tempItem.SaveObject(outStream);
            }
            outStream.WriteLine(getBuildings().Count+"");
            foreach(Building tempBuild in getBuildings())
            {
                tempBuild.SaveObject(outStream);
            }
            outStream.WriteLine(EnumUtil.EnumName<LightingModel>(this.getLightingModel()));
        }

        public void LoadObject(StreamReader inStream)
        {
            this.width = Int32.Parse(inStream.ReadLine());
            this.height = Int32.Parse(inStream.ReadLine());
            terrainField = new Terrain[getWidth(),getHeight()];
            for(int x=0; x < getWidth(); x++)
            {
                for(int y=0; y < getHeight(); y++)
                {
                    Terrain newTerrain = new Terrain();
                    newTerrain.LoadObject(inStream);
                    terrainField[x,y] = newTerrain;
                }
            }
            getMonsters().Clear();
            int monstersSize = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < monstersSize; i++)
            {
                Monster newMonster = new Monster();
                newMonster.LoadObject(inStream);

                //do not load any monster with the player ID
                if(!newMonster.ID.Equals(MonsterActionManager.PLAYER_ID))
                {
                    getMonsters().Add(newMonster);
                }
            }
            getItems().Clear();
            int itemsSize = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < itemsSize; i++)
            {
                Item newItem = new Item();
                newItem.LoadObject(inStream);
                getItems().Add(newItem);
            }
            getBuildings().Clear();
            int buildSize = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < buildSize; i++)
            {
                Building newBuild = new Building();
                newBuild.LoadObject(inStream);
                getBuildings().Add(newBuild);
            }
            this.lightingModel = (LightingModel)Enum.Parse(typeof(LightingModel), inStream.ReadLine());
        }

        /**
         * @return the monsters
         */
        public List<Monster> getMonsters()
        {
            return monsters;
        }

        /**
         * @return the width
         */
        public int getWidth() {
            return width;
        }

        /**
         * @return the height
         */
        public int getHeight() {
            return height;
        }

        /**
         * @return the items
         */
        public List<Item> getItems() {
            return items;
        }

        /**
         * @return the buildings
         */
        public List<Building> getBuildings() {
            return buildings;
        }

        /**
         * @return the lightingModel
         */
        public LightingModel getLightingModel() {
            return lightingModel;
        }

        /**
         * @param lightingModel the lightingModel to set
         */
        public void setLightingModel(LightingModel lightingModel) {
            this.lightingModel = lightingModel;
        }
    }
}
