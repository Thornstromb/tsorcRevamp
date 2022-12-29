﻿using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace tsorcRevamp.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class HollowSoldierBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.vanity = true;
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
            Item.value = PriceByRarity.fromItem(Item);
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronChainmail);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 150);
            recipe.AddTile(TileID.DemonAltar);
            
            recipe.Register();
        }
    }
}