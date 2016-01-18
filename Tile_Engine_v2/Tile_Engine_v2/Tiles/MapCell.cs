using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireBall_Royale
{
    class MapCell
    {
        public List<int> BaseTiles = new List<int>();
        public bool Explored { get; set; }

        public MapCell(int tileID)
        {
            TileID = tileID;
            Explored = false;
        }


        public int TileID
        {
            get { return BaseTiles.Count > 0 ? BaseTiles[0] : 0; }
            set
            {
                if (BaseTiles.Count > 0)
                    BaseTiles[0] = value;
                else
                    AddBaseTile(value);
            }
        }

        public void AddBaseTile(int tileID)
        {
            BaseTiles.Add(tileID);
        }
    }
}
