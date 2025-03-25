using BritishInvasion.Content.Sounds;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Weapons.Magic
{
    public class Fife : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 12;

            Item.damage = 25;
            Item.crit = 0;
            Item.knockBack = 0;

            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 10;
            Item.useTime = 10;

            Item.shoot = ProjectileID.QuarterNote;
            Item.shootSpeed = 5.5f;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 5;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 30);

            Item.holdStyle = 1;

            Item.UseSound = null;
        }

        public override void HoldItem(Player player)
        {
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 aim = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter);

            player.direction = Math.Sign(aim.X);
            player.itemLocation.X = player.Center.X - 5 * player.direction;
            player.itemLocation.Y = player.MountedCenter.Y - 6;
            player.itemRotation = aim.ToRotation() + (player.direction > 0 ? 0 : MathHelper.Pi);

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, aim.ToRotation() + (player.direction > 0 ? -MathHelper.PiOver2 - MathHelper.Pi / 16 : -MathHelper.PiOver2 + MathHelper.Pi / 16));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type += Main.rand.Next(3);

            Vector2 aim = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter);
            float dist = aim.Length();

            float screenY = Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
            dist /= (screenY / 2f);
            if (dist > 1f) dist = 1f;

            velocity = aim.SafeNormalize(default) * Item.shootSpeed * dist;
            Projectile note = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);

            note.scale *= Main.rand.NextFloat(0.6f, 0.8f);
            note.ai[1] = 0f; // don't play harp note

            dist = MathHelper.Clamp(dist * 2f - 2f, -1f, 1f); // 
            dist = (float)Math.Round(dist * Player.musicNotes);
            dist /= Player.musicNotes;
            Main.musicPitch = dist;
            note.ai[0] = dist;

            NetMessage.SendData(MessageID.SyncProjectile, number: note.whoAmI);

            SoundEngine.PlaySound(new SoundStyle("BritishInvasion/Assets/Sounds/" + "FifeBlow") with
            {
                Volume = 0.5f,
                MaxInstances = 1,
                Pitch = Main.musicPitch,
                SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
            },
            player.Center, updateCallback: (sound) =>
            {
                sound.Pitch = MathHelper.Lerp(sound.Pitch, Main.musicPitch, 0.6f);
                sound.Position = player.Center;
                return (player.HeldItem.type == Type || Main.mouseItem.type == type) && player.ItemAnimationActive;
            });
            return false;
        }
    }
}
