using BritishInvasion.Common.Global.Projectiles;
using BritishInvasion.Common.Sets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Weapons.Ranged
{
    public class HandMortar : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(10, ItemID.Grenade, singleShotTime: 40, shotVelocity: 1f, hasAutoReuse: false);
            Item.damage = 5;
            Item.knockBack = 6.5f;
            Item.width = 46;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item61;
        }

        public override bool? CanChooseAmmo(Item ammo, Player player)
        {
            if (ItemSets.HandMortarAmmo[ammo.type])
                return true;

            return base.CanChooseAmmo(ammo, player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity *= 1.4f;
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 2f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            position.Y -= 6;
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * (Item.width - 8 + HoldoutOffset()?.X ?? 0);
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;

            for (int i = 0; i < 24; i++)
            {
                Dust dust = Dust.NewDustPerfect(position + Main.rand.NextVector2Circular(5, 5), DustID.Smoke, velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.1f, 0.8f), newColor: new Color(80, 80, 80), Scale: Main.rand.NextFloat(0.6f, 1f));
                dust.alpha = 127;
            }

            for (int i = 0; i < 32; i++)
            {
                Dust dust = Dust.NewDustPerfect(position + Main.rand.NextVector2Circular(8, 2), DustID.Torch, velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.1f, 1.2f), newColor: new Color(80, 80, 80), Scale: Main.rand.NextFloat(1f, 2f));
                dust.noGravity = true;
                dust.alpha = 127;
            }

            for (int i = 0; i < 4; i++)
            {
                Gore gore = Gore.NewGoreDirect(Item.GetSource_FromThis(), position + new Vector2(0, -10), velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.3f, 0.6f), GoreID.ChimneySmoke1 + i % 3, Scale: Main.rand.NextFloat(0.5f, 0.7f));
                gore.timeLeft = 80;
            }

            return true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-6, 0);
    }
}
