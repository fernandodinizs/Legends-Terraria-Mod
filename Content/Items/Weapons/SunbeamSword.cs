using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;


namespace Legends.Content.Items.Weapons
{
    internal class SunbeamSword : ModItem
    {
        public override void SetStaticDefaults()
        {   
            // Não funciona mais na versão 1.14:
                // DisplayName.SetDefault("Tutorial Sword");
                // Tooltip.SetDefault("A powerful sword\nIt can be used to craft powerful items");
            // ------------------------

            // acesso do item no modo criativo
            // configura a quantidade de itens necessarios para desbloquear o item no modo criativo 
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {   
            // HITBOX
            Item.width = 32;    // Hitbox width from Bottom Center.
            Item.height = 32;   // Hitbox height from Bottom Center.

            // USE AND ANIMATION STYLE
            Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
            Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
            Item.useAnimation = 20; // The time span of the using animation of the weapon
            Item.autoReuse = true; // Significa que o item pode ser usado continuamente ao segurar o botão de uso.

            // DAMAGE VALUES
            Item.DamageType = DamageClass.Melee; // Sets the damage type to melee.
            Item.damage = 20; // The damage stat for the Weapon.
            Item.knockBack = 3.5f; // Sets the knockback of the weapon. Maximum is 20
            Item.crit = 5; // Sets the critical strike chance of the weapon. The player, by default, has a 4% critical strike chance.

            // MISC
            Item.value = Item.buyPrice(silver: 15, copper: 40); //Value of the Item 120 = Silver, Cooper: 20
            Item.maxStack = 1;

            Item.rare = ItemRarityID.Blue;
            // ITEM RARE = -1 PIOR A 11 MELHOR
            // ITENS -11, -12 E -13 SÃO PARA ITENS DE QUEST, EXPERT MODE, MASTER MODE RESPECTIVAMENTES.

            //SOUND
            Item.UseSound = SoundID.Item1;
        }   

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Sunbeams>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}