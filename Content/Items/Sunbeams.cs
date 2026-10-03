using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;


namespace Legends.Content.Items
{
    internal class Sunbeams : ModItem
    {
        public override void SetStaticDefaults()
        {   
            // Não funciona mais na versão 1.14:
                // DisplayName.SetDefault("Sunbeams");
                // Tooltip.SetDefault("A powerful beam of sunlight\nIt can be used to craft powerful items");
            // ------------------------

            // acesso do item no modo criativo
            // configura a quantidade de itens necessarios para desbloquear o item no modo criativo 
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

        }

        public override void SetDefaults()
        {
            Item.width = 16;    // Hitbox width from Bottom Center.
            Item.height = 16;   // Hitbox height from Bottom Center.

            Item.value = Item.buyPrice(silver: 1); //Value of the Item 120 = Silver, Cooper: 20
            Item.maxStack = 99;

            Item.rare = ItemRarityID.Blue;
        }   

    }   
}