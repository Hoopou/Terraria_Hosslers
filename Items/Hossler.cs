using Hosslers.Shared;
using IL.Terraria.Localization;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Hosslers.HosslerConfigServer;

namespace Hosslers.Items
{
    /// <summary>
    /// 
    /// For exemples and Dust chart : https://forums.terraria.org/index.php?threads/official-tmodloader-help-thread.28901/
    /// 
    /// TModLoader Documentation : https://docs.tmodloader.net/html_alpha/annotated.html
    /// 
    /// See ExampleMagicMirror in item exemples
    /// 
    /// 
    /// 179	0	Green Moss (placed).png	Green MossGreen Moss(on Stone)
    /// 180	0	Brown Moss(placed).png Brown MossBrown Moss(on Stone)
    /// 181	0	Red Moss(placed).png Red MossRed Moss(on Stone)
    /// 182	0	Blue Moss(placed).png Blue MossBlue Moss(on Stone)
    /// 183	0   Purple Moss (placed).png Purple MossPurple Moss(on Stone)
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    public class Hossler : ModItem
    {

        public override string Texture => Mod.Name + "/Items/BasicHossler";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BasicItem"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

            Tooltip.SetDefault("This is a basic Hossler.");
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Generic;
            Item.width = 5;
            Item.height = 5;
            Item.useTime = 20;
            Item.useAnimation = 20;            
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.holdStyle = ItemHoldStyleID.HoldFront;

        }
        //public override void HoldItem(Player player)
        //{
        //	player.itemRotation = 0f;
        //	player.itemLocation.Y = player.Center.Y;
        //	player.itemLocation.X = player.Center.X - 18 * player.direction;
        //}
        public override void HoldItemFrame(Player player)
        {
            player.itemRotation = 0f;
            player.itemLocation.Y = player.Center.Y + 8;
            player.itemLocation.X = player.Center.X + 2 * player.direction;

            player.itemWidth = 5;
            player.itemHeight = 5;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //if (Main.rand.NextBool(10))
            //{
            int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.AncientLight);
            //Main.tile[hitbox.X, hitbox.Y].ToString()
            //this.Talk("Block info: "+ Player.tileTargetX + "," + Player.tileTargetY + "item time: " + Item.buffTime);

            //}
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            //if (Main.rand.NextBool(10))
            //{
            int dust = Dust.NewDust(new Vector2(Player.tileTargetX, Player.tileTargetY), Item.width, Item.height, DustID.AncientLight);
            //Main.tile[hitbox.X, hitbox.Y].ToString()

            if (player.itemTime == 0)
            {
                player.itemTime = Item.useTime;
            }

            if (player.itemTime == Item.useTime)
            {




                //bool isVertical = Math.Abs(angle.Y) > Math.Abs(angle.X);


                //Settings.Talk("Block info: " + Player.tileTargetX + "," + Player.tileTargetY + " : " + TargetTile.ToString() + "  / Angle (" + angle.X + " ; " + angle.Y + ")" + " " + (isTop ? "Top-":"Bottom-")+(isRigth?"Rigth":"Left") + " " + WorldGen.CheckTileBreakability(Player.tileTargetX, Player.tileTargetY));
                if (Settings.Mode == ToolMode.Dev)
                {
                    OnUseToolWithDevMode(player, heldItemFrame);
                    return;
                }

                OnUseTool(player, heldItemFrame);






                //base.UseStyle(player, heldItemFrame);
            }
        }

        public void OnUseTool(Player player, Rectangle heldItemFrame)
        {


            int radius = 3;
           
            var TargetTile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            var isWall = !TargetTile.HasTile;
            var swap = ModContent.GetInstance<HosslerConfigServer>().Configs.Swaps.FirstOrDefault(c => c.InitialTileId == TargetTile.TileType);

            if (swap == null) return;
           
            CrawlAndReplace(Player.tileTargetX, Player.tileTargetY, swap, radius, isWall);
         
        }

        private void CrawlAndReplace(int x,int y, BlockSwapConfig swap, int radius, bool isWall = false)
        {
            if (!isWall && (!Main.tile[x, y].HasTile || Main.tile[x, y].TileType != swap.InitialTileId)) return;

            if(isWall && (Main.tile[x, y].WallType != swap.InitialTileId || Main.tile[x, y].WallType == 0)) return;
            
            if (radius <= 0) return;

            BlockPlacer.ReplaceTile(x, y, (ushort)swap.TargetTileId);

            radius--;

            Crawler.CrawlTile(x, y, (ushort)swap.InitialTileId, out bool top, out bool right, out bool bottom, out bool left);

            var tempradius = radius;
            if (top) CrawlAndReplace(x, y - 1, swap, tempradius, isWall);
            tempradius = radius;
            if (right) CrawlAndReplace(x+1, y, swap, tempradius, isWall);
            tempradius = radius;
            if (bottom) CrawlAndReplace(x, y+1, swap, tempradius, isWall);
            tempradius = radius;
            if (left) CrawlAndReplace(x-1, y, swap, tempradius, isWall);

        }



        public void OnUseToolWithDevMode(Player player, Rectangle heldItemFrame)
        {
            var TargetTile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            var sb = new StringBuilder();

            sb.AppendLine("TileTargetX : " + Player.tileTargetX);
            sb.AppendLine("TileTargetY : " + Player.tileTargetY);

            foreach (var attr in TargetTile.GetType().GetProperties())
            {
                try
                {
                    sb.Append(attr.Name + " : " + attr.GetValue(TargetTile) + "\n");
                }
                catch (Exception ez)
                {
                    sb.Append(attr.Name + "(" + attr.PropertyType.Name + ") : UNDEFINED " + "\n");
                }
            }
            sb.Append("\n==================================\n");

            foreach (var attr in TargetTile.GetType().GetFields())
            {
                try
                {
                    sb.Append(attr.Name + " : " + attr.GetValue(TargetTile) + "\n");
                }
                catch (Exception ez)
                {
                    sb.Append(attr.Name + "(" + attr.DeclaringType + ") : UNDEFINED " + "\n");
                }
            }

            sb.Append("\n==================================\n");

            foreach (var attr in TargetTile.GetType().GetMembers())
            {
                try
                {
                    sb.Append(attr.Name + " : " + attr.MemberType + "\n");
                }
                catch (Exception ez)
                {
                    sb.Append(attr.Name + "(" + attr.DeclaringType + ") : UNDEFINED " + "\n");
                }
            }

            Settings.LogInfo(sb.ToString());
        }




        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

    }
}