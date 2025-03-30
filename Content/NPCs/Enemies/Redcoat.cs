using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.IO;
using BritishInvasion.Common.Utils;

namespace BritishInvasion.Content.NPCs.Enemies
{
    public class Redcoat : ModNPC
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
            NPC.damage = 10;
            NPC.defense = 4;
            NPC.lifeMax = 100;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = -1;

        }
       
        bool Walking=true;
        bool NeedToLoad=true;
        int loadTimer=0;
        int shootTimer=0;
        bool IsShooting=false;
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if(Vector2.Distance(player.Center,NPC.Center)>400f){
                Utility.AIFighter(NPC, ref NPC.ai, player.Center, accelerationFactor: 0.08f, velMax: 2f, maxJumpTilesX: 3, maxJumpTilesY: 4);
                Walking=true;
                shootTimer=0;
                NeedToLoad=true;
                loadTimer=0;
                IsShooting=false;
            }
            else
            {
                int frameHeight=58;
                Walking=false;
                NPC.velocity.X*=0.9f;

                if(NeedToLoad==true)
                    NPC.frame.Y = frameHeight *16;
                loadTimer++;
                NeedToLoad=false;
                if(loadTimer==10)
                {
                    NPC.frame.Y +=  frameHeight;
                    IsShooting=true;
                }
                
                if(IsShooting)
                {
                    shootTimer++;
                    if(shootTimer%15==0)
                        NPC.frame.Y += frameHeight;
                    if(NPC.frame.Y==frameHeight*20){
                        NPC.frame.Y=frameHeight*17;
                    }
                    if(NPC.frame.Y==frameHeight*17&&shootTimer%15==10)
                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center+new Vector2(0,-8), (player.Center-NPC.Center).SafeNormalize(Vector2.UnitX)*12f, 180, (int)(NPC.damage/2), 0f, Main.myPlayer);
                }                       

            }
            NPC.spriteDirection = -NPC.direction;
        }

        private readonly Range walkingFrames = 0..13;
        private readonly int airFrame = 15;
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

        public override void ModifyNPCLoot(NPCLoot loot)
        {
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
        }
    }
    public class RedcoatAlt1 : Redcoat{}
}