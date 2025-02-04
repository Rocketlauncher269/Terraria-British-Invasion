using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Weapons.Ranged
{
    public class CaptainsFlintlockPistol : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.knockBack = 4f;
            Item.width = 38;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.DefaultToRangedWeapon(5, AmmoID.Bullet, singleShotTime: 5, shotVelocity: 10f, hasAutoReuse: false); 
        }
        int Timer =0;
        public override void UpdateInventory(Player player)
        {
            player.autoReuseAllWeapons=false;

            Timer--;
        }
        public override bool CanUseItem	(Player player)
        {
            return Timer<1 || Timer>40;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * (Item.width - 8 + HoldoutOffset()?.X ?? 0);
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;
            if(Timer<1){
            Timer=60;
            }

            for (int i = 0; i < 16; i++)
                Dust.NewDustPerfect(position, DustID.Smoke, velocity.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.2f, 0.7f), newColor: new Color(80, 80, 80), Scale: Main.rand.NextFloat(0.6f, 1f));

            return true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-6, 0);
    }
}
