using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Hosslers.Shared;

namespace Hosslers.Commands
{
    internal class Search : ModCommand
    {
        public override string Command => "search";

        public override CommandType Type => CommandType.World;

        public override string Usage => "/search <search_key>";

        public override string Description => "Allows you to search a block id by its name";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args == null || args.Length <= 0) return;


            
            Dictionary<string, int> myDic = TileID.Search.Names.Where(a => a.ToLower().Contains(args[0].ToLower().Trim())).ToDictionary(t => t.ToString(), t => TileID.Search.GetId(t.ToString()));
            
            Dictionary<string, int> myDic2 = new Dictionary<string, int>();

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, int> kvp in myDic)
            {
                if (kvp.Key.ToLower().Contains(args[0].ToLower().Trim()))
                {
                    myDic2.Add(kvp.Key, kvp.Value); 
                    sb.Append(kvp.Key + "(" + kvp.Value+") ;");                
                }
            }

            Settings.Talk( myDic2.Count+" RESULTS: " + sb.ToString());


        }
    }
}
