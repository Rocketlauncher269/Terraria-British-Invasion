using BritishInvasion.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Weapons.Ranged
{
    public class BayonetRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(10, AmmoID.Bullet, singleShotTime: 40, shotVelocity: 10f, hasAutoReuse: false);
            Item.damage = 31;
            Item.knockBack = 4.5f;
            Item.width = 54;
            Item.height = 14;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
        }

        public override bool AltFunctionUse(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<BayonetRifleProjectile>()] < 1;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2)
            {
                Item.noUseGraphic = true;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BayonetRifleProjectile>(), damage * 2, knockback * 1.4f, Main.myPlayer);
                return false;
            }

            Item.noUseGraphic = false;
            return true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-12, 0);
    }
}
