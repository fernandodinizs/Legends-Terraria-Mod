using System;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Legends.Content.Projectiles;

namespace Legends.Content.Items.Weapons.Mage
{

public class SunbeamStaff : ModItem
{
    public override void SetStaticDefaults()
        {   
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
           Item.Size = new Vector2(40, 40);
           Item.scale = 1f;

           Item.useStyle = ItemUseStyleID.Shoot;
           Item.useAnimation = 60;
           Item.useStyle = 60;

           Item.DamageType = DamageClass.Magic;
           Item.damage = 50;
           Item.knockBack = 2.0f;
           Item.mana = 10;

           Item.shoot =  ModContent.ProjectileType<SunbeamStaffProjectile>();
           Item.shootSpeed = 2f;

           Item.UseSound = SoundID.Item71;

           Item.rare = ItemRarityID.Orange;

           Item.noMelee = true;
           Item.noUseGraphic = true;
           Item.channel = true;
           
        }


    public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Sunbeams>(), 17)
                .AddTile(TileID.Anvils)
                .Register();
        }


}

}