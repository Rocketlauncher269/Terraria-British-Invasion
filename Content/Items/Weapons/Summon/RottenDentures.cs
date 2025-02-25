using BritishInvasion.Content.Projectiles.Weapons;
using BritishInvasion.Content.Buffs.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Weapons.Summon
{
    public class RottenDentures : ModItem
    {
        public override void SetStaticDefaults()
        {

            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
            Item.buffType = ModContent.BuffType<RottenDenturesBuff>();

            Item.shoot = ModContent.ProjectileType<RottenDenturesMinion>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-12, 0);
    }
}
