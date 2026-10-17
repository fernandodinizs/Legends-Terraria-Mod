using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Legends.Content.Projectiles;


namespace Legends.Content.Items.Weapons.Melee
{
    internal class SunbeamSword : ModItem
    {

        public int attackType = 0; // keeps track of which attack it is
		public int comboExpireTimer = 0; // we want the attack pattern to reset if the weapon is not used for certain period of time


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
            // Item.width = 46;    // Hitbox width from Bottom Center.
            // Item.height = 54;   // Hitbox height from Bottom Center.
            Item.Size = new Vector2(46, 54); // Hitbox size from Bottom Center.
            Item.scale = 1.7f;

            // USE AND ANIMATION STYLE
            Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
            Item.useTime = 50; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
            Item.useAnimation = 50; // The time span of the using animation of the weapon
            Item.autoReuse = false; // Significa que o item pode ser usado continuamente ao segurar o botão de uso.

            // DAMAGE VALUES
            Item.DamageType = DamageClass.Melee; // Sets the damage type to melee.
            Item.damage = 40; // The damage stat for the Weapon.
            Item.knockBack = 3.5f; // Sets the knockback of the weapon. Maximum is 20
            Item.crit = 4; // Sets the critical strike chance of the weapon. The player, by default, has a 4% critical strike chance.

            // MISC
            Item.value = Item.buyPrice(silver: 15, copper: 40); //Value of the Item 120 = Silver, Cooper: 20
            Item.maxStack = 1;

            Item.rare = ItemRarityID.Orange;
            // ITEM RARE = -1 PIOR A 11 MELHOR
            // ITENS -11, -12 E -13 SÃO PARA ITENS DE QUEST, EXPERT MODE, MASTER MODE RESPECTIVAMENTES.

            
            //SOUND
            Item.UseSound = SoundID.Item1;

            Item.noMelee = true;       // o dano agora vem do projétil
            Item.noUseGraphic = true;  // o projétil desenha a espada
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<SunbeamSwordSwing>();
        }   

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			// Using the shoot function, we override the swing projectile to set ai[0] (which attack it is)
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, attackType);
			attackType = (attackType + 1) % 2; // Increment attackType to make sure next swing is different
			comboExpireTimer = 0; // Every time the weapon is used, we reset this so the combo does not expire
			return false; // return false to prevent original projectile from being shot
		}

        public override void UpdateInventory(Player player) {
			if (comboExpireTimer++ >= 120) // after 120 ticks (== 2 seconds) in inventory, reset the attack pattern
				attackType = 0;
		}

        public override bool MeleePrefix() {
			return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
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