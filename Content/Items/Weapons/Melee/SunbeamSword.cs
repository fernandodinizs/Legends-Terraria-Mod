using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Humanizer.Localisation.Formatters;
using Terraria.Utilities.Terraria.Utilities;


namespace Legends.Content.Items.Weapons.Melee
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
        }   

        //efeito ao girar espada
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {   
            if (player.itemAnimationMax <= 0)
                return;

            // Progresso aproximado da animação: 0 = início, 1 = final.
            float swingProgress = 1f -
                player.itemAnimation / (float)player.itemAnimationMax;

            swingProgress = MathHelper.Clamp(swingProgress, 0f, 1f);

            Vector2 handPosition = player.RotatedRelativePoint(
                player.MountedCenter,
                true
            );

            float maxDistance = new Vector2(
                hitbox.Width,
                hitbox.Height
            ).Length();

            if (maxDistance <= 0f)
                return;

            // Começa próximo à ponta e expande em direção à mão.
            float minDistance = MathHelper.Lerp(
                0.75f,
                0.10f,
                swingProgress
            ) * maxDistance;

            // Fogo: menos partículas, menores e com tamanhos variados.
            //mudar numero para aumentar densidade
            int fireCount = swingProgress < 0.25f ? 5 : 7;

            for (int i = 0; i < fireCount; i++)
            {
                Vector2 position = new Vector2(
                    Main.rand.NextFloat(hitbox.Left, hitbox.Right),
                    Main.rand.NextFloat(hitbox.Top, hitbox.Bottom)
                );

                float distance = Vector2.Distance(
                    handPosition,
                    position
                );

                // Não gera fogo na região que ainda não foi alcançada.
                if (distance < minDistance)
                    continue;

                float progress = MathHelper.Clamp(
                    distance / maxDistance,
                    0f,
                    1f
                );
    
                // A ponta tem partículas maiores, mas sem exagero.
                float scale = MathHelper.Lerp(
                    1.25f,
                    1.75f,
                    progress
                ) * Main.rand.NextFloat(1.0f, 1.4f);

                int index = Dust.NewDust(
                    position,
                    1, 1,
                    DustID.Torch,
                    Main.rand.NextFloat(0.1f, 0.7f),
                    Main.rand.NextFloat(0.1f, 0.7f),
                    120,
                    default,
                    scale
                );


                Dust fire = Main.dust[index];
                fire.noGravity = true;
                fire.velocity *= 0.25f;
            }

            // 4. FAGULHAS QUE SOBEM APENAS DA PARTE DISTANTE (Metade final da lâmina)
            if (Main.rand.NextBool(2))
            {
                float randomX = Main.rand.Next(hitbox.X, hitbox.X + hitbox.Width);
                float randomY = Main.rand.Next(hitbox.Y, hitbox.Y + hitbox.Height);
                Vector2 sparkPosition = new Vector2(randomX, randomY);

                // Só cria a fagulha se ela nascer longe da mão do jogador (na metade externa da espada)
                if (Vector2.Distance(handPosition, sparkPosition) > (new Vector2(hitbox.Width, hitbox.Height).Length() * 0.5f))
                {
                    int sparkIndex = Dust.NewDust(
                        sparkPosition, 
                        3, 3, 
                        DustID.CopperCoin, 
                        Main.rand.NextFloat(-1f, 1f), 
                        -1f, // Força a subida vertical como na foto original
                        150, 
                        default, 
                        1.0f
                    );

                    Main.dust[sparkIndex].noGravity = false;
                }
            }

        }


        //efeito ao bater
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++ )
            {
                Dust.NewDust(target.position, target.width, target.height, DustID.Torch, 0f, 0f, 150, default, 1.7f);
                // t_Meteor
                // Firefly
                // FlameBurst
                // Lava
            }

            target.AddBuff(BuffID.OnFire, 300);
            // 300 significa 300 ticks = 5 segundos
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