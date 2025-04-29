using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.IO;
using BritishInvasion.Common.Utils;
using BritishInvasion.Content.Items.Vanity;
using BritishInvasion.Content.Items.Weapons.Magic;
using BritishInvasion.Content.Items.Weapons.Summon;
using BritishInvasion.Content.Items.Weapons.Ranged;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace BritishInvasion.Content.NPCs.Enemies
{
    public class RedcoatGrenadier : ModNPC
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = 20;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            NPC.width = 32;
            NPC.height = 44;
            NPC.damage = 20;
            NPC.defense = 4;
            NPC.lifeMax = 100;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = -1;
        }
       
        bool Walking=true;
        int shootTimer=0;
        int WalkTimer=0;
        bool runAwayOrDie=false;
        public override void ModifyNPCLoot(NPCLoot loot)
        {
            loot.Add(ItemDropRule.Common(ModContent.ItemType<BicorneHat>(), 500));
            loot.Add(ItemDropRule.Common(ModContent.ItemType<RedCoat>(), 500));
            loot.Add(ItemDropRule.Common(ModContent.ItemType<RedcoatTrousers>(), 500));
            loot.Add(ItemDropRule.Common(ModContent.ItemType<BayonetRifle>(), 500));
            loot.Add(ItemDropRule.Common(ModContent.ItemType<HandMortar>(), 500));
            loot.Add(ItemDropRule.Common(ModContent.ItemType<Blunderbuss>(), 500));
            loot.Add(ItemDropRule.Common(ModContent.ItemType<Fife>(), 500));
            loot.Add(ItemDropRule.Common(ModContent.ItemType<RottenDentures>(), 500));
            loot.Add(ItemDropRule.Common(ItemID.Teacup, 10));
            loot.Add(ItemDropRule.Common(ItemID.Grenade, 2));
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if(Walking||WalkTimer<40){
                Utility.AIFighter(NPC, ref NPC.ai, player.Center, accelerationFactor: runAwayOrDie ? -1.4f:0.08f, velMax: 2f, maxJumpTilesX: 3, maxJumpTilesY: 4);
                Walking=true;
                shootTimer=0;
                if(runAwayOrDie&&Vector2.Distance(player.Center,NPC.Center)>300f)
                {
                    runAwayOrDie=false;
                }
                if(Vector2.Distance(player.Center,NPC.Center)<300f&&!runAwayOrDie)
                {
                    WalkTimer++;
                }
                if(WalkTimer==40)
                {
                    Walking=false;
                }
            }
            else
            {
                int frameHeight=60;
                Walking=false;
                NPC.velocity.X*=0.9f;
                
                shootTimer++;
                NPC.frame.Y = frameHeight * (14+(int)(Math.Floor((double)(shootTimer/31))));//lazy but it works kind of
                
                if(shootTimer>30){
                    Projectile p =Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center+((player.Center-NPC.Center).SafeNormalize(Vector2.UnitX)*30f), (player.Center-NPC.Center).SafeNormalize(Vector2.UnitX)*8f, ModContent.ProjectileType<RedcoatGrenadeProj>(), (int)(NPC.damage/2), 0f, -1);
                    WalkTimer=0;
                    Walking=true;
                    runAwayOrDie=true;
                }
                                  
            }
            NPC.spriteDirection =-NPC.direction*(runAwayOrDie ? -1 : 1);
        }

        private readonly Range walkingFrames = 0..12;
        private readonly int airFrame = 14;
        private readonly Range shootingFrames = 15..20;

        public override void FindFrame(int frameHeight)
        {
            Player player = Main.player[NPC.target];

            int frameIndex = NPC.frame.Y / frameHeight;
            NPC.frameCounter++;
            if (Walking)
            {
                if (NPC.velocity.Y == 0)
                {
                    // Reset walking 
                    if (!walkingFrames.Contains(frameIndex))
                        NPC.frame.Y = frameHeight * walkingFrames.Start.Value;

                    // Walking animation frame counter, accounting for walk speed
                    NPC.frameCounter += Math.Abs(NPC.velocity.X);

                    // Update frame
                    if (NPC.frameCounter > 5.0)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0.0;
                    }

                    if (frameIndex >= walkingFrames.End.Value)
                        NPC.frame.Y = frameHeight * walkingFrames.Start.Value;
                }
                else if(MathF.Abs(NPC.velocity.Y) > 1f)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y = frameHeight * airFrame;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    int dustType =DustID.Blood;

                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, dustType);
                    dust.velocity.X *= (dust.velocity.X + +Main.rand.Next(0, 100) * 0.015f) * hit.HitDirection;
                    dust.velocity.Y = 3f + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    dust.noGravity = true;
                }
            }

            if (Main.dedServ)
                return; // don't run on the server

            if (NPC.life <= 0)
            {
                var entitySource = NPC.GetSource_Death();

                Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GrenadierGore1").Type);
                Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GrenadierGore2").Type);
                Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GrenadierGore3").Type);
                Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GrenadierGore2").Type);
                Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GrenadierGore3").Type);
            }
        }
    }

    public class RedcoatGrenadeProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
			Projectile.height = 14; 
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 180;
            ProjectileID.Sets.Explosive[Type] = true;
			Projectile.usesLocalNPCImmunity = true;
	
        }

        public override void AI() {
			// If timeLeft is <= 3, then explode the grenade.
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
				Projectile.PrepareBombToBlow();
			}
			else {
				// Spawn a smoke dust.
				var smokeDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100);
				smokeDust.scale *= 1f + Main.rand.Next(10) * 0.1f;
				smokeDust.velocity *= 0.2f;
				smokeDust.noGravity = true;
			}

			Projectile.ai[0] += 1f;
			// Wait 15 ticks until applying friction and gravity.
			if (Projectile.ai[0] > 15f) {
				// Slow down if on the ground.
				if (Projectile.velocity.Y == 0f) {
					Projectile.velocity.X *= 0.95f;
				}

				// Fall down. Remember, positive Y is down.
				Projectile.velocity.Y += 0.2f;
			}

			// Rotate the grenade in the direction it is moving.
			Projectile.rotation += Projectile.velocity.X * 0.1f;
		}
        
        public override bool OnTileCollide(Vector2 oldVelocity) {
			// Bounce off of tiles.
			if (Projectile.velocity.X != oldVelocity.X) {
				Projectile.velocity.X = oldVelocity.X * -0.4f;
			}

			if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 0.7f) {
				Projectile.velocity.Y = oldVelocity.Y * -0.4f;
			}

			// Return false so the projectile doesn't get killed. If you do want your projectile to explode on contact with tiles, do not return true here.
			// If you return true, the projectile will die without being resized (no blast radius).
			// Instead, set `Projectile.timeLeft = 3;` like the Example Rocket Projectile.
			return false;
		}
        public override void PrepareBombToBlow() {
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the grenade explodes on slopes.
			Projectile.alpha = 255; // Make the grenade invisible.
            Projectile.friendly = true;
            Projectile.hostile = true;
			// Resize the hitbox of the projectile for the blast "radius".
			// Rocket I: 128, Rocket III: 200, Mini Nuke Rocket: 250
			// Measurements are in pixels, so 128 / 16 = 8 tiles.
			Projectile.Resize(128, 128);
			// Set the knockback of the blast.
			// Rocket I: 8f, Rocket III: 10f, Mini Nuke Rocket: 12f
			Projectile.knockBack = 8f;
		}
        public override void OnKill(int timeLeft) {
			// Play an exploding sound.
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle.
			// Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50
			Projectile.Resize(22, 22);

			// Spawn a bunch of smoke dusts.
			for (int i = 0; i < 30; i++) {
				var smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
				smoke.velocity *= 1.4f;
			}

			// Spawn a bunch of fire dusts.
			for (int j = 0; j < 20; j++) {
				var fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3.5f);
				fireDust.noGravity = true;
				fireDust.velocity *= 7f;
				fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
				fireDust.velocity *= 3f;
			}

			// Spawn a bunch of smoke gores.
			for (int k = 0; k < 2; k++) {
				float speedMulti = 0.4f;
				if (k == 1) {
					speedMulti = 0.8f;
				}

				var smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity -= Vector2.One;
			}

			// To make the explosion destroy tiles, take a look at the commented out code in Example Rocket Projectile.
		}
       

    }
}