using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;

using Legends.Content.Items;

namespace Legends.Content.Projectiles
{
    internal class SunbeamProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;


            Projectile.damage = 1000;
            
            Projectile.timeLeft = 1200;
            Projectile.light = 0.5f;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

        }

        public override void AI()
        {
            if (Projectile.velocity.Y < 7f)
                Projectile.velocity.Y += 0.05f;

            Lighting.AddLight(Projectile.Center, 1f, 0.9f, 0.4f);
            
            //rastro
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    0f,
                    0f,
                    100,
                    default,
                    1.3f
                );
                
                d.noGravity = true;
                d.velocity *= 0.3f; 
            }

        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && Main.dayTime) // só o servidor/singleplayer decide
            {
                Item.NewItem(
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    ModContent.ItemType<Sunbeams>(),
                    1);
            }
        }



    }
}