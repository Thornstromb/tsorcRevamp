﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors.Summon
{
    [AutoloadEquip(EquipType.Body)]
    public class CrystalArmor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Crystal armor vibrates with a mysterious energy\nIncreases minion damage by 10%\nIncreases your max number of minions by 1");
        }
        public override void UpdateEquip(Player player)
        {
                player.GetDamage(DamageClass.Summon) += 0.1f;
                player.maxMinions += 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 11;
            Item.rare = ItemRarityID.Pink;
            Item.value = PriceByRarity.fromItem(Item);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpiderBreastplate, 1);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 5200);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
