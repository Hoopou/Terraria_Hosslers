using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Hosslers.Shared
{
    public class Crawler
    {
        public static void CrawlTile(int x, int y,ushort tileType, out bool top, out bool right, out bool bottom, out bool left)
        {
            top = false;
            right = false;
            bottom = false;
            left = false;

            if(y-1 > 0) top = Main.tile[x,y-1].TileType == tileType;
            if(x+1 <= Main.maxTilesX) right = Main.tile[x+1, y].TileType == tileType;
            if(y+1 <= Main.maxTilesY) bottom = Main.tile[x,y+1].TileType == tileType;
            if(x-1 > 0) left = Main.tile[x-1,y].TileType == tileType;
        }
    }
}
