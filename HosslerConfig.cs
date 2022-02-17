using Hosslers.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace Hosslers
{
    public class HosslerConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [NullAllowed]
        public ServerConfig Configs = null;


        public override void OnChanged()
        {
            
            if(Configs != null)
            {
                Settings.Mode = Configs.DevMode ? ToolMode.Dev : ToolMode.Normal;
                base.OnChanged();
            }
            else
            {
                Settings.Talk("No changes applied");
            }

        }

        public class ServerConfig
        {

            [Label("Dev mode")]
            [Tooltip("Enable or disable mod dev mode")]
            [DefaultValue(false)]
            public bool DevMode { get; set; }

            //[SeparatePage]
            [Label("Block swap list")]
            [Tooltip("Edit the block swappability")]
            public List<BlockSwapConfig> Swaps { get; set; } = new List<BlockSwapConfig>()
            {
                new BlockSwapConfig(TileID.Grass, TileID.Dirt)
            };
        }

        public class BlockSwapConfig
        {
            [DefaultValue(0)]
            [Range(0, 625)]
            [Increment(1)]
            public int InitialTileId { get; set; }

            [Range(0, 625)]
            [DefaultValue(0)]
            [Increment(1)]
            public int TargetTileId { get; set; }

            public BlockSwapConfig()
            {}
            public BlockSwapConfig(ushort initialTileId, ushort targetTileId)
            {
                InitialTileId = initialTileId;
                TargetTileId = targetTileId;
            }
        }
    }

    public class HosslerConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
    }
}
