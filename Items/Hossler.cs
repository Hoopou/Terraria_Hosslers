using Hosslers.Shared;
using IL.Terraria.Localization;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			Item.width = 40;
			Item.height = 40;
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
				var TargetTile = Main.tile[Player.tileTargetX, Player.tileTargetY];
				var angle = new Vector2(player.Center.ToTileCoordinates().X, player.Center.ToTileCoordinates().Y).DirectionTo(new Vector2(Player.tileTargetX, Player.tileTargetY));
				int radius = 2;

				//bool isVertical = Math.Abs(angle.Y) > Math.Abs(angle.X);
				bool isTop = angle.Y < 0;//Player.tileTargetY - player.Center.ToTileCoordinates().Y > 0 ? false : true;
				bool isRigth = angle.X >= 0;

				//Settings.Talk("Block info: " + Player.tileTargetX + "," + Player.tileTargetY + " : " + TargetTile.ToString() + "  / Angle (" + angle.X + " ; " + angle.Y + ")" + " " + (isTop ? "Top-":"Bottom-")+(isRigth?"Rigth":"Left") + " " + WorldGen.CheckTileBreakability(Player.tileTargetX, Player.tileTargetY));
				if (Settings.Mode == ToolMode.Dev)
				{	
					Settings.Talk("Block info: " + "Tyle type:" + TargetTile.TileType);
					return;
				}
				

				if (TargetTile.TileType == TileID.Stone)
				{
					Settings.Talk("This block is a stone");
				}
				else if (TargetTile.TileType == TileID.Grass)
				{
					//Settings.Talk("Player position: (" + player.Center.ToTileCoordinates().X+" ; " + player.Center.ToTileCoordinates().Y + ")  /  Cursor location: (" + Player.tileTargetX + " ; " + Player.tileTargetY + ") / Angle (" + angle + ")");

					//tile.type = TileID.Dirt;

					for (int y = Player.tileTargetY; (isTop ? (y > Player.tileTargetY - radius) : (y < Player.tileTargetY + radius)); y += (isTop ? -1 : 1))
					{
						Tile tile = Main.tile[Player.tileTargetX, y];
						if (tile.TileType == TileID.Grass && WorldGen.CheckTileBreakability(Player.tileTargetX, y) == 0)
						{
							//Main.tile[Player.tileTargetX, y].type = TileID.Dirt;
							//WorldGen.ReplaceTile(Player.tileTargetX, y, TileID.Dirt, 0);
							WorldGen.PlaceTile(Player.tileTargetX, y, TileID.Dirt, false, true);
						}
						else
						{
							break;
						}
					}
					for (int x = (isRigth ? Player.tileTargetX + 1 : Player.tileTargetX - 1); (isRigth ? (x < Player.tileTargetX + radius) : (x > Player.tileTargetX - radius)); x += (isRigth ? 1 : -1))
					{
						Tile tile = Main.tile[x, Player.tileTargetY];
						if (tile.TileType == TileID.Grass && WorldGen.CheckTileBreakability(x, Player.tileTargetY) == 0)
						{

							//Main.tile[x, Player.tileTargetY].type = TileID.Dirt;
							WorldGen.PlaceTile(x, Player.tileTargetY, TileID.Dirt, false, true);
						}
						else
						{
							Settings.Talk(tile.TileType.ToString(), 150, 250, 150);
							break;
						}
					}
				}

				//this.Talk("Use Style: " + Player.tileTargetX + "," + Player.tileTargetY + "item time: " + player.itemTime + "(max: " + Item.useTime + ")");
			}

			//base.UseStyle(player, heldItemFrame);
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