using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Projectiles.Enemy.Okiku;

namespace tsorcRevamp.NPCs.Bosses.Okiku.FirstForm
{
	public class DamnedSoul : ModNPC
	{
		private bool initiate;

		public int TimerHeal;

		public float TimerAnim;

		public override void SetDefaults()
		{
			npc.alpha = 50;
			npc.width = 50;
			npc.height = 50;
			npc.aiStyle = -1;
			npc.damage = 40;
			npc.defense = 18;
			npc.noGravity = true;
			npc.boss = true;
			npc.noTileCollide = true;
			npc.lifeMax = 20000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			Main.npcFrameCount[npc.type] = 4;
			despawnHandler = new NPCDespawnHandler(54);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Damned Soul");
		}

		public int ObscureShotDamage = 30;
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
		}

		NPCDespawnHandler despawnHandler;
		public override void AI()
		{
			despawnHandler.TargetAndDespawn(npc.whoAmI);
			if (!initiate)
			{
				npc.ai[3] = -Main.rand.Next(200);
				initiate = true;
			}
			TimerAnim += 1f;
			if (TimerAnim > 10f)
			{
				if (Main.rand.Next(2) == 0)
				{
					npc.spriteDirection *= -1;
				}
				TimerAnim = 0f;
			}
			int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 62, 0f, 0f, 100, Color.White);
			Main.dust[dust].noGravity = true;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].active && Main.npc[i].realLife == npc.whoAmI)
				{
					Main.npc[i].life = npc.life;
				}
			}
			if (Main.npc[(int)npc.ai[1]].life <= 1000)
			{
				return;
			}
			npc.ai[3] += 1f;
			if (npc.ai[3] >= 0f)
			{
				if (npc.life > 1000)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float speed = 0.5f;
						Vector2 position = new Vector2(npc.position.X + (float)(npc.width / 2), npc.position.Y + (float)(npc.height / 2));
						float rotation2 = (float)Math.Atan2(position.Y - (Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f), position.X - (Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f));
						rotation2 += (float)(Main.rand.Next(-50, 50) / 100);
						Projectile.NewProjectile(position.X, position.Y, (float)(Math.Cos(rotation2) * (double)speed * -1.0), (float)(Math.Sin(rotation2) * (double)speed * -1.0), ModContent.ProjectileType<ObscureShot>(), ObscureShotDamage, 0f, Main.myPlayer);
					}
					npc.ai[3] = -200 - Main.rand.Next(200);
				}
				else
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float speed = 0.5f;
						Vector2 position = new Vector2(npc.position.X + (float)(npc.width / 2), npc.position.Y + (float)(npc.height / 2));
						float rotation = (float)Math.Atan2(position.Y - (Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f), position.X - (Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f));
						rotation += (float)(Main.rand.Next(-50, 50) / 100);
						int projectile = Projectile.NewProjectile(position.X, position.Y, (float)(Math.Cos(rotation) * (double)speed * -1.0), (float)(Math.Sin(rotation) * (double)speed * -1.0), ModContent.ProjectileType<ObscureShot>(), ObscureShotDamage, 0f, Main.myPlayer);
						Main.projectile[projectile].scale = 3f;
					}
					npc.ai[3] = -50 - Main.rand.Next(50);
				}
			}
			if (npc.life > 1000)
			{
				return;
			}
			TimerHeal++;
			if (TimerHeal < 600)
			{
				return;
			}
			npc.life = npc.lifeMax;
			TimerHeal = 0;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].active && Main.npc[i].realLife == npc.whoAmI)
				{
					Main.npc[i].life = 2000;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			int num = 1;
			if (!Main.dedServ)
			{
				num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];
			}
			num++;
			npc.frameCounter += 1.0;
			if (npc.frameCounter >= 8.0)
			{
				npc.frame.Y = npc.frame.Y + num;
				npc.frameCounter = 0.0;
			}
			if (npc.frame.Y >= num * Main.npcFrameCount[npc.type])
			{
				npc.frame.Y = 0;
			}
		}
	}
}
