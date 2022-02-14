using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Hosslers.Shared;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Hosslers.Commands
{
    public class PlaceBlocks : ModCommand
    {
        public override string Command => "place";

        public override CommandType Type => CommandType.World;

        public override string Usage => "/place <Block ID> <Type ID> length";

        public override string Description => "Places the blocks infront of the player";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length <= 0 ) WriteError("invalid args");

            int tileType = 1;
            int typeId = 0;
            int length = 0;



            if(args.Length == 1)
            {
                try
                {
                    length = int.Parse(args[0]);
                }
                catch (Exception)
                {
                    WriteError("invalid args");
                    return;
                }
            }

            if (args.Length == 2)
            {
                try
                {
                    tileType = int.Parse(args[0]);
                    length = int.Parse(args[1]);
                }
                catch (Exception)
                {
                    WriteError("invalid args");
                    return;
                }
            }

            if (args.Length >= 3)
            {
                try
                {
                    tileType = int.Parse(args[0]);
                    typeId = int.Parse(args[1]);
                    length = int.Parse(args[2]);
                }
                catch (Exception)
                {
                    WriteError("invalid args");
                    return;
                }
            }

            int startPosition = caller.Player.Center.ToTileCoordinates().X + caller.Player.direction;
            int endPosition = caller.Player.Center.ToTileCoordinates().X + (caller.Player.direction * (length+1));

            if(startPosition > endPosition)
            {
                var tempStart = startPosition;
                startPosition = endPosition;
                endPosition = tempStart;
            }

            Settings.Talk("Placing " + length + " blocks of " + tileType + " with type " + typeId + " from x:" + startPosition + "  ; to  x:"+ endPosition);
            for(int i = startPosition; i < endPosition; i++)
            {
                if (args.Length <= 2)
                {
                   
                    WorldGen.PlaceTile(i, caller.Player.Center.ToTileCoordinates().Y, TileID.Grass, false, true);
                    
                }
                else
                {
                    WorldGen.PlaceTile(i, caller.Player.Center.ToTileCoordinates().Y, tileType, false, true,style: typeId);                    
                }
            }
            
            //WorldGen.PlaceTile(x, Player.tileTargetY, TileID.Dirt, false, true);

            //foreach (var i in Main.item)
            //{
            //    i.Refresh();
            //}
        }

        private void WriteError(string message)
        {
            Settings.Talk(message, 255,0,0,true);
        }
    }
}
