using Microsoft.Xna.Framework;
using BritishInvasion.Content.Buffs.Minions;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using BritishInvasion.Common.Utils;

namespace BritishInvasion.Content.Projectiles.Weapons
{
    public class RottenDenturesMinion : ModProjectile
    {


        public override void SetStaticDefaults()
        {

            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 4;
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
        }
        public sealed override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 20;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            // These below are needed for a minion weapon
            Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.minion = true; // Declares this as a minion (has many effects)
            Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
            Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.aiStyle = ProjAIStyleID.Pet;
            AIType = ProjectileID.BabySlime;
        }
        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return true;
        }

        // The AI of this minion is split into multiple methods to avoid bloat. This method just passes values between calls actual parts of the AI.
        NPC targetNPC;
        bool Sticking;
        int timer;
        float Offset;
        bool init=false;
        int intialDamage;

        private bool CheckUnder()
        {
            float Dist1 = Utility.CastLength(Projectile.Center, new Vector2(0, 1), 20f, true);
            return Dist1 < 17f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            targetNPC=target;
            timer=2;
            Sticking=true;
        }
        public override void AI()
        {
            CheckActive(Main.player[Projectile.owner]);
            if(!init)
            {
                init=true;
                Offset=Main.rand.NextFloat(0f,10f);
                intialDamage=Projectile.damage;
            }
            if(Sticking){
                Projectile.damage=(int)(intialDamage/2);
            }
            else
            {
                Projectile.damage=intialDamage;
            }
            if(targetNPC is null||!targetNPC.active)
            {
                timer=0;
                Sticking=false;
            }
            else
            {
                Projectile.Center=targetNPC.Center+(new Vector2(10,0).RotatedBy(Offset));
            }
            Visuals();
        }
        public override void PostAI()
        {
            timer--;
            if(timer<1)
                Sticking=false;
        }

        // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
        private bool CheckActive(Player Owner)
        {
            if (Owner.dead || !Owner.active)
            {
                Owner.ClearBuff(ModContent.BuffType<RottenDenturesBuff>());
                return false;
            }

            if (Owner.HasBuff(ModContent.BuffType<RottenDenturesBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }


        int ProjFrame;
        int jumpTimer;
        private void Visuals()
        {
            if(!CheckUnder())
            {
                jumpTimer++;
                if(jumpTimer<30)
                    Projectile.frame=2;
                else
                    Projectile.frame=3;
            }
            else
            {
                jumpTimer=0;
            }
        }
    }
}
