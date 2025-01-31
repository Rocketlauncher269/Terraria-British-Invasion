using BritishInvasion.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Projectiles.Weapons
{
    public class BayonetRifleProjectile : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<BayonetRifle>().Texture;

        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 46f;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear);
            Projectile.width = 92;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.scale = 1f;
        }

        private bool spawned;
        public override bool PreAI()
        {
            if (!spawned)
            {
                spawned = true;
            }

            Player player = Main.player[Projectile.owner];  
            int duration = player.itemAnimationMax;  

            player.heldProj = Projectile.whoAmI; 

            if (Projectile.timeLeft > duration)
                Projectile.timeLeft = duration;

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.velocity.X < 0)
            {
                Projectile.spriteDirection = -1;
                Projectile.rotation += MathHelper.Pi;
            }

            float halfDuration = duration * 0.5f;
            float progress;

            if (Projectile.timeLeft < halfDuration)
                progress = Projectile.timeLeft / halfDuration;
            else
                progress = (duration - Projectile.timeLeft) / halfDuration;

            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            return false;
        }
    }
}
