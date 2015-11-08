using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Map
{
    public enum CellType 
    {
        NULL,
        FOREST, 
        MAIN_TOWN, 
        TOWN
    }

    public class OverworldCell : IStoreObject
    {
        public RegionHeader header; //non-peristant, only used during generation

        public CellType cellType;
        public bool nExit;
        public bool eExit;
        public bool sExit;
        public bool wExit;
        public int depth;

        public OverworldCell()
        {
            header = null;
            cellType = CellType.NULL;

            nExit = false;
            eExit = false;
            sExit = false;
            wExit = false;
            depth = 0;
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(EnumUtil.EnumName<CellType>(cellType));
            outStream.WriteLine(nExit.ToString());
            outStream.WriteLine(eExit.ToString());
            outStream.WriteLine(sExit.ToString());
            outStream.WriteLine(wExit.ToString());
            outStream.WriteLine(depth + "");
        }

        public void LoadObject(StreamReader inStream)
        {
            cellType = (CellType)Enum.Parse(typeof(CellType), inStream.ReadLine());
            nExit = Boolean.Parse(inStream.ReadLine());
            eExit = Boolean.Parse(inStream.ReadLine());
            sExit = Boolean.Parse(inStream.ReadLine());
            wExit = Boolean.Parse(inStream.ReadLine());
            depth = Int32.Parse(inStream.ReadLine());
        }
    }
}
