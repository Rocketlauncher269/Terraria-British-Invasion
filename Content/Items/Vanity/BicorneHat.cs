using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class BicorneHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TricorneHat>();

            ArmorIDs.Head.Sets.DrawHatHair[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 10;
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
        }
    }
}