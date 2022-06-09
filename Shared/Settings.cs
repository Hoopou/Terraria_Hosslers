using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hosslers.Shared
{
    public static class Settings
    {
        public static ToolMode Mode { get; set; } = ToolMode.Normal;

        internal static void LogInfo(string info)
        {
            var path = @"C:\Users\Vincent\Documents\My Games\Terraria\tModLoader\ModSources\Hosslers\bin\Logs.txt";
            var TilesIdsPath = @"C:\Users\Vincent\Documents\My Games\Terraria\tModLoader\ModSources\Hosslers\bin\TileIDs.txt";

            if (!File.Exists(TilesIdsPath))
            {
                //var sb = new StringBuilder();

                //foreach (var attr in Enum.GetNames(typeof(TileID)))
                //{
                //    sb.Append(attr + " : " + TileID[attr] + "\n");
                //}

                //File.WriteAllText(path, sb.ToString());

                var sb = new StringBuilder();
                var tileids = new TileID();
                foreach (var attr in tileids.GetType().GetFields())
                {
                    try
                    {
                        sb.Append(attr.Name + " : " + attr.GetValue(tileids)?.ToString() + "" + "\n");
                    }
                    catch (Exception ez)
                    {
                        sb.Append(attr + "(" + "" + ") : UNDEFINED " + "\n");
                    }
                }

                File.WriteAllText(TilesIdsPath, sb.ToString());
            }

            File.WriteAllText(path,info);
        }

        internal static void Talk(string message, byte R = 255, byte G = 255, byte B = 255, bool force = false)
        {
            Main.NewText(message, R, G, B);
        }
    }


    public class SettingsData
    {
        public int Value { get; set; }
    }


    public enum ToolMode{
        Normal,
        Dev
    }
}
