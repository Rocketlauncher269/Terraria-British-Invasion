using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Weapons.Ranged
{
    public class Blunderbuss : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(10, AmmoID.Bullet, singleShotTime: 30, shotVelocity: 7f, hasAutoReuse: false);
            Item.damage = 14;
            Item.knockBack = 4.5f;
            Item.width = 54;
            Item.height = 14;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * (Item.width - 8 + HoldoutOffset()?.X ?? 0);
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;

            for (int i = 0; i < 3; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(12));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }

            for (int i = 0; i < 16; i++)
                Dust.NewDustPerfect(position, DustID.Smoke, velocity.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.2f, 0.7f), newColor: new Color(80, 80, 80), Scale: Main.rand.NextFloat(0.6f, 1f));

            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-6, 0);
    }
}
