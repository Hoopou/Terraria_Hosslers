using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace Hosslers.Shared
{
    public class BlockPlacer
    {
        public static bool ReplaceTile(int x, int y, ushort tileType)
        {
            if (x < 0 || y < 0 || x > Main.maxTilesX || y > Main.maxTilesY) return false;

            if (TileID.Sets.Grass[tileType] && TileID.Sets.Conversion.MergesWithDirtInASpecialWay[tileType])
            {
                WorldGen.PlaceTile(x, y, TileID.Dirt, false, true);
                WorldGen.SpreadGrass(x, y, TileID.Dirt, tileType);
            }
            else
            {
                WorldGen.PlaceTile(x, y, tileType, false, true);
            }

            return true;
        }
    }
}
