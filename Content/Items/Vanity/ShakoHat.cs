using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class ShakoHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
        }

        public override void AddRecipes()
        {
        }
    }
}