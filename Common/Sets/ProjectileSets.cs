using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace BritishInvasion.Common.Sets
{
    public static class ProjectileSets
    {
        public static bool[] HandMortarProjectiles { get; } = ProjectileID.Sets.Factory.CreateBoolSet
        (
            ProjectileID.Grenade,
            ProjectileID.StickyGrenade,
            ProjectileID.BouncyGrenade,

            ProjectileID.Bomb,
            ProjectileID.StickyBomb,
            ProjectileID.BouncyBomb,
            ProjectileID.BombFish,
            ProjectileID.DirtBomb,
            ProjectileID.DirtStickyBomb,
            ProjectileID.WetBomb,
            ProjectileID.LavaBomb,
            ProjectileID.HoneyBomb,

            ProjectileID.Dynamite,
            ProjectileID.StickyDynamite,
            ProjectileID.BouncyDynamite
        );
    }
}
