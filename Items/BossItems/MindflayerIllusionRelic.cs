﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.BossItems
{
    class MindflayerIllusionRelic : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The final battle with Attraidies. \n" +
                "No more illusions.");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightRed;
            Item.width = 38;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.channel = true;
        }
        public override bool? UseItem(Player player)
        {
            int offset = 50 * 16;
            int effectOffset = 65;
            Vector2 spawnPoint = new Vector2(player.position.X, player.position.Y);
            int DustType = DustID.PurpleCrystalShard;
            Vector2 vfx = new Vector2(spawnPoint.X, spawnPoint.Y);
            if (player.direction == 1)
            {
                spawnPoint.X += offset;
                vfx.X += offset - effectOffset;
            }
            else
            {
                spawnPoint.X -= offset;
                vfx.X -= offset;
            }


            NPC Attraidies = Main.npc[NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)spawnPoint.X, (int)spawnPoint.Y, ModContent.NPCType<NPCs.Bosses.Okiku.FinalForm.Attraidies>())];

            for (int i = 0; i < 50; i++)
            {
                vfx = Attraidies.Center;
                Vector2 vel = Main.rand.NextVector2Circular(10, 10);
                int dust;
                dust = Dust.NewDust(vfx, 30, 30, DustType, vel.X, vel.Y, 100, default, 5f);
                Main.dust[dust].noGravity = true;
                Dust.NewDust(vfx, 30, 30, DustType, vel.X, vel.Y, 240, default, 5f);
                Main.dust[dust].noGravity = true;
                Dust.NewDust(vfx, 30, 30, DustID.Torch, vel.X, vel.Y, 200, default, 3f);

                Dust.NewDustPerfect(player.position, DustType, vel, 100, default, 5f).noGravity = true;
            }

            //Flip it turnways if the player is facing the other way
            spawnPoint.X -= 14 * player.direction;

            if (Main.netMode != NetmodeID.SinglePlayer && (player.whoAmI == Main.LocalPlayer.whoAmI))
            {

                if (!Main.dayTime) //If it's already night, we have to flip it turnways twice
                {
                    ModPacket timePacket2 = ModContent.GetInstance<tsorcRevamp>().GetPacket();
                    timePacket2.Write(tsorcPacketID.SyncTimeChange);
                    timePacket2.Send();
                }

                //If it's day, set it to night
                ModPacket timePacket = ModContent.GetInstance<tsorcRevamp>().GetPacket();
                timePacket.Write(tsorcPacketID.SyncTimeChange);
                timePacket.Send();
            }
            else
            {
                Main.dayTime = false;
                Main.time = 0;
            }
            return true;
        }        

        //Was gonna make it have to charge up for a second to activate, but... eh
        //int cast = 0;
        public override bool CanUseItem(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Okiku.FinalForm.Attraidies>()))
            {
                return false;
            }
            for (int i = 0; i < 50; i++)
            {
                Dust.NewDustPerfect(player.Center, DustID.PurpleCrystalShard, Main.rand.NextVector2Circular(10, 10), 100, default, 5f).noGravity = true;
            }
            return true;
        }
    }
}
