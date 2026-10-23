using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

namespace Legends.Content.Projectiles
{   

    
    internal class SunbeamStaffProjectile : ModProjectile
    {

        private const int PauseTime =60; //fase lenta
        private const int MaxLife = 180; //morre aqui 
        private const float FastSpeed = 12f; //velocidade final
        private const float SpinSpeed = 0.35f;


        private const float StartScale = 0.4f;  // tamanho inicial (bem pequeno)
        private const float EndScale = 2.2f;    // tamanho final da bola de fogo
        private float VisualScale()
        {
            // 0 a 1 durante a fase lenta, depois fica em 1
            float p = MathHelper.Clamp(Projectile.ai[0] / PauseTime, 0f, 1f);

            // suaviza: cresce rápido no começo e desacelera
            p = 1f - (1f - p) * (1f - p);

            return MathHelper.Lerp(StartScale, EndScale, p);
        }



        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.scale = 1.4f;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.aiStyle = -1;

            Projectile.penetrate = 1;
        }

        

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 origin = tex.Size() / 2f;
            float scale = VisualScale();

            // Brilho laranja atrás (efeito de bola de fogo)
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null,
                new Color(255, 120, 20, 0) * 0.6f, Projectile.rotation, origin,
                scale * 1.25f, SpriteEffects.None, 0);

            // Sprite principal
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null,
                Color.White, Projectile.rotation, origin,
                scale, SpriteEffects.None, 0);

            return false;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            //gira em torno de si
            Projectile.rotation += SpinSpeed * (Projectile.direction == 0 ? 1 : Projectile.direction);

            // Depois da pausa: velocidade fixa, sem acelerar
            if (Projectile.ai[0] == PauseTime)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * FastSpeed;
            }

            if (Projectile.ai[0] >= MaxLife)
            {
                Projectile.Kill();
                return;
            }

            // float rotateSpeed = 0.35f * (float)Projectile.direction;
            // Projectile.rotation += rotateSpeed;

            Lighting.AddLight(Projectile.Center, 0.75f, 0.75f, 0.75f);

            float s = VisualScale();
            float radius = 8f * s; // área onde as chamas nascem cresce junto

            if (Main.rand.NextBool(2))
            {
                int numToSpawn = 1 + Main.rand.Next(2);
                for (int i = 0; i < numToSpawn; i++)
                {
                    Vector2 pos = Projectile.Center + Main.rand.NextVector2Circular(radius, radius);
                    Dust fire = Dust.NewDustPerfect(pos, DustID.Torch,
                        Projectile.velocity * 0.1f, 0, default,
                        Main.rand.NextFloat(0.7f, 1.1f) * s);
                    fire.noGravity = true;
                }
            }

            if (Main.rand.NextBool(4))
            {
                Vector2 pos = Projectile.Center + Main.rand.NextVector2Circular(radius, radius);
                Dust smoke = Dust.NewDustPerfect(
                    pos,
                    DustID.CopperCoin,
                    new Vector2(Main.rand.NextFloat(-0.5f, 0.5f),
                    Main.rand.NextFloat(-1.5f, -0.5f)),
                    150, 
                    default,
                    0.9f);

                smoke.noGravity = true;
            }

        }

        public override void OnKill(int timeLeft)
        {
            //  Efeito Explosão usando só recursos do Terraria
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < 25; i++)
            {
                Dust smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Smoke, 0f, 0f, 100, default, 2f);
                smoke.velocity *= 1.4f;
            }

            for (int i = 0; i < 40; i++)
            {
                Dust fire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, 0f, 0f, 100, default, 3f);
                fire.noGravity = true;
                fire.velocity *= 5f;

                Dust fire2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, 0f, 0f, 100, default, 1.5f);
                fire2.velocity *= 3f;
            }

            for (int g = 0; g < 2; g++)
            {
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center,
                    Main.rand.NextVector2Circular(2f, 2f), Main.rand.Next(61, 64));
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300); //queimação
        }

    }

}