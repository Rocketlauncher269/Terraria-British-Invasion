using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BritishInvasion.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class BearskinHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.IsTallHat[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
        }
    }
}