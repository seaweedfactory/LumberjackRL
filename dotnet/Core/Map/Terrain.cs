using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Map
{
    public class Terrain : IStoreObject
    {
        private TerrainCode code;
        private int water;  //how much water is here?
        private Dictionary<TerrainParameter,String> parameters;

        public Terrain()
        {
            code = TerrainCode.DIRT;
            water = 0;
            parameters = new Dictionary<TerrainParameter,String>();
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(EnumUtil.EnumName<TerrainCode>(code));
            outStream.WriteLine(getWater().ToString());
            outStream.WriteLine(getParameters().Count.ToString());
            foreach(TerrainParameter tempKey in getParameters().Keys)
            {
                outStream.WriteLine(EnumUtil.EnumName<TerrainParameter>(tempKey));
                outStream.WriteLine(parameters[tempKey]);
            }
        }

        public void LoadObject(StreamReader inStream)
        {
            code = (TerrainCode)Enum.Parse(typeof(TerrainCode), inStream.ReadLine());
            setWater(Int32.Parse(inStream.ReadLine()));
            getParameters().Clear();
            int parameterSize = Int32.Parse(inStream.ReadLine());
            for (int i = 0; i < parameterSize; i++)
            {
                TerrainParameter newKey = (TerrainParameter)Enum.Parse(typeof(TerrainParameter),inStream.ReadLine());
                String newValue = inStream.ReadLine();
                getParameters().Add(newKey, newValue);
            }
        }

        /**
         * @return the code
         */
        public TerrainCode getCode() {
            return code;
        }

        /**
         * @return the parameters
         */
        public Dictionary<TerrainParameter, String> getParameters() {
            return parameters;
        }

        /**
         * @param code the code to set
         */
        public void setCode(TerrainCode code) {
            this.code = code;
        }

        /**
         * @return the water
         */
        public int getWater() {
            return water;
        }

        /**
         * @param water the water to set
         */
        public void setWater(int water) {
            this.water = water;
        }
    }
}
