using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace BritishInvasion.Common.Sets
{
    public static class ItemSets
    {
        public static bool[] HandMortarAmmo { get; } = ItemID.Sets.Factory.CreateBoolSet
        (
            ItemID.Grenade,
            ItemID.StickyGrenade,
            ItemID.BouncyGrenade,

            ItemID.Bomb,
            ItemID.StickyBomb,
            ItemID.BouncyBomb,
            ItemID.BombFish,
            ItemID.DirtBomb,
            ItemID.DirtStickyBomb,
            ItemID.WetBomb,
            ItemID.LavaBomb,
            ItemID.HoneyBomb,

            ItemID.Dynamite,
            ItemID.StickyDynamite,
            ItemID.BouncyDynamite
        );
    }
}
