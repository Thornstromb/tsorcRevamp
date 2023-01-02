using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Buffs.Runeterra;
using tsorcRevamp.Projectiles.Swords.Runeterra;

namespace tsorcRevamp.Items.Weapons.Melee.Runeterra
{
    public class PlasmaWhirlwind : ModItem
    {
        public int AttackSpeedScalingDuration;
        public float DashingTimer = 0f;
        public float Invincibility = 0f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Whirlwind");
            Tooltip.SetDefault("Doubled crit chance scaling" +
                "\nThrusts on right click dealing 125% damage, cooldown scales down with attack speed" +
                "\nGain a stack of Steel Tempest upon thrusting any enemy" +
                "\nUpon reaching 2 stacks, the next right click will release a plasma whirlwind dealing double damage" +
                "\nHover your mouse over an enemy and press Special Ability to dash through the enemy");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.damage = 60;
            Item.crit = 6;
            Item.width = 52;
            Item.height = 54;
            Item.knockBack = 1f;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 6.2f;
            Item.useTurn = false;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit *= 2;
        }
        public override void HoldItem(Player player)
        {
            player.GetModPlayer<tsorcRevampPlayer>().DoubleCritChance = true;
            AttackSpeedScalingDuration = (int)(3 / player.GetTotalAttackSpeed(DamageClass.Melee) * 60); //3 seconds divided by player's melee speed
            if (AttackSpeedScalingDuration <= 80)
            {
                AttackSpeedScalingDuration = 80; //1.33 seconds minimum
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC other = Main.npc[i];

                if (other.active & !other.friendly & other.Distance(Main.MouseWorld) <= 25 & other.Distance(player.Center) <= 10000 & player.GetModPlayer<tsorcRevampPlayer>().DoubleCritChance & !player.HasBuff(ModContent.BuffType<PlasmaWhirlwindDashCooldown>()))
                {
                    if (DashingTimer > 0)
                    {
                        player.velocity = UsefulFunctions.GenerateTargetingVector(player.Center, other.Center, 15f);
                        Invincibility = 1f;
                        player.AddBuff(ModContent.BuffType<PlasmaWhirlwindDashCooldown>(), 30 * 60);
                    }
                    break;
                }
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.mouseRight & !Main.mouseLeft & player.GetModPlayer<tsorcRevampPlayer>().steeltempest >= 2 & !player.HasBuff(ModContent.BuffType<PlasmaWhirlwindThrustCooldown>()))
            {
                player.altFunctionUse = 2;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
                Item.noMelee = false;
                Item.shoot = ModContent.ProjectileType<PlasmaWhirlwindTornado>();
                player.AddBuff(ModContent.BuffType<PlasmaWhirlwindThrustCooldown>(), AttackSpeedScalingDuration);
                player.GetModPlayer<tsorcRevampPlayer>().steeltempest = 0;
            }
            else
            if (Main.mouseRight & !Main.mouseLeft & !player.HasBuff(ModContent.BuffType<SteelTempestThrustCooldown>()))
            {
                player.altFunctionUse = 2;
                Item.useStyle = ItemUseStyleID.Rapier;
                Item.noUseGraphic = true;
                Item.noMelee = true;
                player.AddBuff(ModContent.BuffType<PlasmaWhirlwindThrustCooldown>(), AttackSpeedScalingDuration);
                Item.shoot = ModContent.ProjectileType<PlasmaWhirlwindThrust>();
            }
            if (Main.mouseLeft)
            {
                player.altFunctionUse = 1;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
                Item.noMelee = false;
                Item.useTurn = false;
                Item.shoot = ModContent.ProjectileType<Projectiles.Nothing>();
            }

        }
        public override void UpdateInventory(Player player)
        {
            if (Invincibility > 0f)
            {
                player.immune = true;
            }
            if (Main.GameUpdateCount % 1 == 0)
            {
                DashingTimer -= 0.0167f;
                Invincibility -= 0.0167f;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2 || !player.HasBuff(ModContent.BuffType<PlasmaWhirlwindThrustCooldown>()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*public override bool CanShoot(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                return true;
            } return false;
        }*/

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        /*
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<SteelTempest>());
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 35000);

            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }*/
    }
}