using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.UI;

namespace BatchSeller
{
	public class BatchSeller : Mod
	{
		internal static string AUTO_SELLER_CURSOR_NAME = "AUTO_SELLER_CUSOR";
		internal static ModHotKey batchSellHotKey;
		private int autoSellCursorOverride;

		public override void Load(){
			if(!Main.dedServ){
				Texture2D cursorTexure = ModContent.GetTexture("BatchSeller/cursor/batchSell");
				autoSellCursorOverride = rebuildCursorTextures(cursorTexure);

				batchSellHotKey = RegisterHotKey("batchSell", "B");
				
				AddPlayer("BatchSellerPlayer", new BatchSellerPlayer(autoSellCursorOverride));
			}
		}

		private int rebuildCursorTextures(Texture2D newCusorTexture){
			Texture2D[] largerCursorArray = new Texture2D[Main.cursorTextures.Length + 1];
			Main.cursorTextures.CopyTo(largerCursorArray, 0);
			largerCursorArray[Main.cursorTextures.Length] = newCusorTexture;
			Main.cursorTextures = largerCursorArray;
			return Main.cursorTextures.Length - 1;
		}

		public override void Unload(){
			batchSellHotKey = null;
		}
	}
}