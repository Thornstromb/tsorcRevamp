﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Accessories.Melee
{

    [AutoloadEquip(EquipType.Shield)]

    public class IronShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Greater defense for melee warriors" +
                                "\nReduces damage taken by 4% " +
                                "\nbut also reduces non-melee damage by 20% and movement speed by 10%" +
                                "\nCan be upgraded with 2000 Dark Souls.");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.defense = 2;
            Item.rare = ItemRarityID.Blue;
            Item.value = PriceByRarity.Blue_1;
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.04f;
            player.moveSpeed *= 0.9f;
            player.GetDamage(DamageClass.Ranged) -= 0.2f;
            player.GetDamage(DamageClass.Magic) -= 0.2f;
            player.GetDamage(DamageClass.Summon) -= 0.2f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 4);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 600);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
