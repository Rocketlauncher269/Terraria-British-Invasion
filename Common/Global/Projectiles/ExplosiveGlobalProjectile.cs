using BritishInvasion.Common.Sets;
using BritishInvasion.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Common.Global.Projectiles
{
    public class ExplosiveGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => ProjectileSets.HandMortarProjectiles[entity.type];

        private bool shotByHandMortar = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource)
            {
                if (itemSource.Item.type == ModContent.ItemType<HandMortar>())
                    projectile.GetGlobalProjectile<ExplosiveGlobalProjectile>().shotByHandMortar = true;
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.GetGlobalProjectile<ExplosiveGlobalProjectile>().shotByHandMortar)
            {
                projectile.velocity *= 0f;
                projectile.alpha = 255;
                projectile.timeLeft = 3;
                return false;
            }

            return base.OnTileCollide(projectile, oldVelocity);
        }
    }
}
