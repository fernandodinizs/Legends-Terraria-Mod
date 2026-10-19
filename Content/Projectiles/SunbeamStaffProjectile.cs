using System.Drawing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Legends.Content.Projectiles
{

    internal class SunbeamStaffProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.scale = 1f;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.aiStyle = -1;

            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 60)
            {
                Projectile.velocity *= 1.01f;
            } else
            {
                Projectile.velocity *= 1.05f;
                if (Projectile.ai[0] == 180)
                {
                    Projectile.Kill();
                }
            }

            float rotateSpeed = 0.35f * (float)Projectile.direction;
            Projectile.rotation += rotateSpeed;

            Lighting.AddLight(Projectile.Center, 0.75f, 0.75f, 0.75f);

            if (Main.rand.NextBool(2))
            {
                int numToSpaw = Main.rand.Next(3);
                for(int i = 0; i < numToSpaw; i++)
                {
                    Dust.NewDust(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.Torch,
                        // ModContent.DustType<SunbeamDust>(),
                        Projectile.velocity.X * 0.1f,
                        Projectile.velocity.Y * 0.1f,
                        0, //ALPHA 0 OPAQUE 255 TRANSPARENT
                        default,
                        1f
                        );
                }
            }
        }

    }

}