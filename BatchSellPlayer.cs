using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;

namespace BatchSeller
{
	public class BatchSellerPlayer : ModPlayer
	{
        private static Color cursorColor = new Color(255, 255, 255);

        private Color previousCursorColor;
        private BatchSellConfig batchSellConfig;
        private bool canBatchSellItem;
        private int previousCursorOverride;
        private int cursorOverride;

        public BatchSellerPlayer(int cursorOverride){
            this.canBatchSellItem = false;
            this.cursorOverride = cursorOverride;
            this.previousCursorColor = new Color(255, 255, 255);
            this.batchSellConfig = ModContent.GetInstance<BatchSellConfig>();
        }

        private bool HandleCursorRendering(Item hoveringItem) {
            if (BatchSeller.batchSellHotKey.Current && hoveringItem.IsAir == false)
            {
                if (!canBatchSellItem)
                {
                    previousCursorOverride = Main.cursorOverride;
                    previousCursorColor = Main.cursorColor;
                }
                Main.cursorOverride = cursorOverride;
                Main.cursorColor = cursorColor;
                canBatchSellItem = true;
                return true;
            }
            else
            {
                if (canBatchSellItem)
                {
                    Main.cursorOverride = previousCursorOverride;
                    Main.cursorColor = previousCursorColor;
                }
                canBatchSellItem = false;
                return false;
            }
        }

        public override void OnEnterWorld(Player player)
        {
            if (BatchSeller.batchSellHotKey.GetAssignedKeys().Capacity == 0)
            {
                Main.NewText("Batch-Seller: Hotkey is unbound please set it in the controls -> mod controls.", Color.PaleVioletRed);
            }
        }

        private void PrintSellResult(int totalSold) {
            if (totalSold > 0 && batchSellConfig.printSellSummary) {
                int copperSold = totalSold > 0 ? (totalSold % 500) / 5 : 0;
                int silverSold = totalSold > 0 ? ((totalSold / 50) / 10) % 100 : 0;
                int goldSold = totalSold > 0 ? ((((totalSold / 50) / 10)) / 100) % 100 : 0;
                int platinumSold = totalSold > 0 ? ((((totalSold / 50) / 10)) / 100) / 100 : 0;

                string builtString = "";
                if (platinumSold > 0)
                {
                    builtString += string.Format("{0} platinum ", platinumSold);
                }
                if (goldSold > 0)
                {
                    builtString += string.Format("{0} gold ", goldSold);
                }
                if (silverSold > 0)
                {
                    builtString += string.Format("{0} silver ", silverSold);
                }
                if (copperSold > 0)
                {
                    builtString += string.Format("{0} copper ", copperSold);
                }
                Main.NewText(string.Format("Batch-sell complete gained: {0}", builtString), Color.Aqua);
            }
        }

        private int SellAllMatchingItems(Player currentPlayer, Chest shop, Item[] inventory, Item matchItem) {
            int totalSold = 0;

            int matchPrefix = Convert.ToInt32(matchItem.prefix);

            for (int i = 0; i < inventory.Length; i++)
            {
                Item item = inventory[i];
                int itemPrefix = Convert.ToInt32(item.prefix);
                if (!item.favorited && matchItem.type == item.type && (!batchSellConfig.onlySellMatchingPrefix || matchPrefix == itemPrefix))
                {
                    
                    currentPlayer.SellItem(item.value, item.stack);
                    shop.AddShop(item);
                    totalSold += matchItem.value * item.stack;
                    Main.mouseItem.SetDefaults(0, false);
                    inventory[i].SetDefaults(0, false);
                }
            }

            if (matchItem.value > 0)
            {
                Main.PlaySound(18, -1, -1, 1, 1f, 0f);
            }
            else {
                Main.PlaySound(7, -1, -1, 1, 1f, 0f);
            }
            return totalSold;
        }

        public override void ProcessTriggers(Terraria.GameInput.TriggersSet triggersSet)
        {
            Item hoveringItem = Main.HoverItem;
            int npcStoreIndex = Main.npcShop;

            if (npcStoreIndex > 0 && !hoveringItem.buy && !hoveringItem.favorited)
            {
                bool canBatchSell = HandleCursorRendering(hoveringItem);
                if (canBatchSell && Main.mouseLeft)
                {
                    Player currentPlayer = Main.player[Main.myPlayer];
                    Chest npcStore = Main.instance.shop[npcStoreIndex];
                    Item[] inventory = currentPlayer.inventory;
                    int totalSold = SellAllMatchingItems(currentPlayer, npcStore, inventory, hoveringItem);
                    PrintSellResult(totalSold);
                }
            }
        }
    }
}