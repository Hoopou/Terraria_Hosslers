using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
