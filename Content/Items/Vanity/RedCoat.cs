using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class RedCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PatrioticCoat>();
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
        }
    }
}