using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireBall_Royale
{
    class TileMap
    {
        public List<MapRow> Rows = new List<MapRow>();
        public int MapWidth = 50;
        public int MapHeight = 50;
        public TileMap()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    thisRow.Columns.Add(new MapCell(64));
                }
                Rows.Add(thisRow);
            }

            #region Map Data
            Rows[0].Columns[3].TileID = 96;
            Rows[0].Columns[4].TileID = 96;
            for (int j =5; j < 15; j++)
            {
                Rows[0].Columns[j].TileID = 32;
            }

            Rows[1].Columns[3].TileID = 96;
            for (int j = 4; j < 15; j++)
            {
                Rows[1].Columns[j].TileID = 32;
            }

            Rows[2].Columns[2].TileID = 96;
            for (int j = 3; j < 15; j++)
            {
                Rows[2].Columns[j].TileID = 32;
            }

            Rows[3].Columns[2].TileID = 96;
            Rows[3].Columns[3].TileID = 32;
            Rows[3].Columns[4].TileID = 32;
            Rows[3].Columns[5].TileID = 0;
            Rows[3].Columns[6].TileID = 0;
            Rows[3].Columns[7].TileID = 0;
            for (int j = 8; j < 13; j++)
            {
                Rows[3].Columns[j].TileID = 96;
            }

            Rows[4].Columns[2].TileID = 96;
            Rows[4].Columns[3].TileID = 32;
            Rows[4].Columns[4].TileID = 32;
            Rows[4].Columns[5].TileID = 0;
            Rows[4].Columns[6].TileID = 0;
            Rows[4].Columns[7].TileID = 0;
            for (int j = 8; j < 13; j++)
            {
                Rows[4].Columns[j].TileID = 96;
            }

            Rows[5].Columns[2].TileID = 96;
            Rows[5].Columns[3].TileID = 32;
            Rows[5].Columns[4].TileID = 32;
            for (int j = 5; j < 13; j++)
            {
                Rows[5].Columns[j].TileID = 0;
            }
            #endregion

            doAutoTransition();
        }

        private int GetTileBaseHeight(int tileID)
        {
            // Returns the Base Height of a Tile, we have 32 Tiles for one Terrain so divide with 32
            // 1 -> 0, 16 -> 0, 33 -> 1       
            return tileID / 32;
        }

        private int getBaseTile(int y, int x)
        {
            if (x < 0 || y < 0 ||
                x >= MapWidth || y >= MapHeight)
            {
                return 0;
            }

            return Rows[y].Columns[x].TileID;
        }

        private int CalculateTransistionTileEdge(int y, int x, int iHeight)
        {
            int temp = 0;

            if (GetTileBaseHeight(getBaseTile(y, x - 1)) == iHeight)
            {
                // Left
                temp += 1;
            }

            if (GetTileBaseHeight(getBaseTile(y - 1, x)) == iHeight)
            {
                // Top
                temp += 2;
            }

            if (GetTileBaseHeight(getBaseTile(y, x + 1)) == iHeight)
            {
                // Right
                temp += 4;
            }
            if (GetTileBaseHeight(getBaseTile(y + 1, x)) == iHeight)
            {
                // bottom
                temp += 8;
            }

            if (temp > 0)
            {
                return temp;
            }

            return -1;
        }

        private int CalculateTransistionTileCorner(int y, int x, int iHeight)
        {
            int temp = 0;

            if (GetTileBaseHeight(getBaseTile(y - 1, x - 1)) == iHeight)
            {
                // Left top
                temp += 1;
            }
            if (GetTileBaseHeight(getBaseTile(y - 1, x + 1)) == iHeight)
            {
                // Top right
                temp += 2;
            }
            if (GetTileBaseHeight(getBaseTile(y + 1, x + 1)) == iHeight)
            {
                // bottem Right
                temp += 4;
            }
            if (GetTileBaseHeight(getBaseTile(y + 1, x - 1)) == iHeight)
            {
                // bottom left
                temp += 8;
            }
            if (temp > 0)
            {
                return temp;
            }

            return -1;
        }

        private void doAutoTransition()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int height = getBaseTile(y, x);
                    int start = GetTileBaseHeight(height) + 1;
                    for (int i = start; i < 4; i++)
                    {

                        int tileID = CalculateTransistionTileEdge(y, x, i);
                        if (tileID > -1)
                        {
                            Rows[y].Columns[x].AddBaseTile(i * 32 + tileID);
                        }

                        tileID = CalculateTransistionTileCorner(y, x, i);
                        if (tileID > -1)
                        {
                            Rows[y].Columns[x].AddBaseTile(i * 32 + 16 + tileID);
                        }
                    }
                }
            }
        }
    }

    class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }
}
