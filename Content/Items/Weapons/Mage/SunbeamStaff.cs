using System;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Legends.Content.Projectiles;
using Terraria;

namespace Legends.Content.Items.Weapons.Mage
{
    public class SunbeamStaff : ModItem
    {

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 2;

        public override void SetStaticDefaults()
        {   
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Type] = true;
        }

        public override void SetDefaults()
        {
           Item.Size = new Vector2(40, 40);
           Item.scale = 1f;

           Item.useStyle = ItemUseStyleID.Shoot;
           Item.useAnimation = 35;
           Item.useTime = 35;

           Item.DamageType = DamageClass.Magic;
           Item.damage = 32;
           Item.knockBack = 2.0f;
           Item.mana = 10;
           Item.autoReuse = true;

           Item.shoot =  ModContent.ProjectileType<SunbeamStaffProjectile>();
           Item.shootSpeed = 2f;

           Item.UseSound = SoundID.Item71;

           Item.rare = ItemRarityID.Red;

           Item.noMelee = true;
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