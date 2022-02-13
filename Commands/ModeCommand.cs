using Hosslers.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Hosslers.Commands
{
    public class ModeCommand : ModCommand
    {
        public override string Command => "mode";

        public override CommandType Type => CommandType.World;

        public override string Usage => "/mode 0 or 1";

        public override string Description => "Switch dev mode";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args[0].Length <= 0) return;

            if(args[0].Trim().ToLower() == "1")
            {
                Settings.Mode = ToolMode.Dev;
                Settings.Talk("Dev mode", 0,255,0);
            }
            else
            {
                Settings.Mode=ToolMode.Normal;
                Settings.Talk("Normal mode", 0, 255, 0);
            }
        }
    }
}
