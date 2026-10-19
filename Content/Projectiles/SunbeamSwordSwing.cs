using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


/*
Rastro: como o ângulo agora é suave e o código preenche todos os ângulos entre um frame e outro, o arco sai contínuo. Para mais densidade, aumente o 2 do for (int i...) ou diminua o 6f em steps.
Direção do golpe: StartAngle e EndAngle controlam de onde a espada parte e onde termina. -125 e 55 fazem um golpe de cima para baixo.
Duração: vem do useAnimation do item (50), então o golpe continua lento. Valores menores deixam mais rápido.
*/


namespace Legends.Content.Items.Weapons.Melee
{
    internal class SunbeamSwordSwing : ModProjectile
    {
        // Usa a mesma textura da espada
        public override string Texture => "Legends/Content/Items/Weapons/Melee/SunbeamSword";

        private const float ItemScale = 1.7f;

        // Ângulos do golpe (graus, olhando para a direita; y positivo = para baixo)
        private const float StartAngle = -125f;
        private const float EndAngle = 55f;

        private Player Owner => Main.player[Projectile.owner];
        private float Timer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }

        private float lastAngle;
        private bool hasLast;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.ownerHitCheck = true;

            // Cada inimigo é atingido uma vez por golpe
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        //tamanho da espada
        private float BladeLength()
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            return new Vector2(tex.Width, tex.Height).Length() * ItemScale;
        }

        // Ângulo da lâmina para um progresso p (0 a 1), já espelhado pela direção do jogador
        private Vector2 BladeDirection(float p)
        {
            // Começa devagar, acelera no meio e termina rápido (golpe com peso)
            float ease = p * p * (3f - 2f * p);
            float deg = MathHelper.Lerp(StartAngle, EndAngle, ease);
            float rad = MathHelper.ToRadians(deg);
            return new Vector2(Owner.direction * (float)Math.Cos(rad), (float)Math.Sin(rad));
        }

        public override void AI()
        {
            Player player = Owner;
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            int duration = Math.Max(1, player.itemAnimationMax);
            float p = Timer / duration;

            if (p >= 1f)
            {
                Projectile.Kill();
                return;
            }

            Vector2 hand = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.Center = hand;

            Vector2 dir = BladeDirection(p);
            Projectile.rotation = dir.ToRotation();

            player.heldProj = Projectile.whoAmI;
            player.itemRotation = (dir * player.direction).ToRotation();

            float length = BladeLength();
            float angle = dir.ToRotation();

            // ---- RASTRO CONTÍNUO: preenche todos os ângulos entre o frame anterior e o atual ----
            float delta = hasLast ? MathHelper.WrapAngle(angle - lastAngle) : 0f;
            int steps = hasLast ? Math.Max(2, (int)(Math.Abs(delta) * length / 6f)) : 1;

            for (int s = 1; s <= steps; s++)
            {
                float f = s / (float)steps;
                float a = hasLast ? lastAngle + delta * f : angle;
                Vector2 d = a.ToRotationVector2();

                for (int i = 0; i < 4; i++)
                {
                    // Mais chamas perto da ponta
                    float t = 1f - (float)Math.Pow(Main.rand.NextFloat(), 2f) * 0.65f;
                    Vector2 pos = hand + d * length * t;

                    float scale = MathHelper.Lerp(0.9f, 1.7f, t) * Main.rand.NextFloat(0.9f, 1.25f);

                    Dust fire = Dust.NewDustPerfect(pos, DustID.Torch, Vector2.Zero, 110, default, scale);

                    fire.noGravity = true;
                    fire.velocity = new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.7f, -0.1f));
                }

                // Fagulhas espalhadas ao longo do arco (não só nos pontos)
                if (Main.rand.NextBool(6))
                {
                    Vector2 pos = hand + d * length * Main.rand.NextFloat(0.6f, 1f);
                    Dust spark = Dust.NewDustPerfect(pos, DustID.CopperCoin, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-2f, -0.8f)), 150, default, 0.9f);
                    spark.noGravity = false;
                }
            }

            Lighting.AddLight(hand + dir * length, 1f, 0.5f, 0.1f);

            lastAngle = angle;
            hasLast = true;
            Timer++;
        }

        // Hitbox em forma de linha (da mão até a ponta)
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * BladeLength();
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 24f, ref point);
        }

        //efeito da espada
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDust(target.position, target.width, target.height, DustID.Torch, 0f, 0f, 150, default, 1.7f);
                // t_Meteor
                // Firefly
                // FlameBurst
                // Lava

            target.AddBuff(BuffID.OnFire, 300);
            // 300 significa 300 ticks = 5 segundos
            // 60 ticks = 1 segundo
        }

        //sprite da espada
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            int dir = Owner.direction;

            // Sprite na diagonal (ponta no canto superior direito): origem no canto inferior da empunhadura
            Vector2 origin = dir == 1 ? new Vector2(0, tex.Height) : new Vector2(tex.Width, tex.Height);
            float rot = Projectile.rotation + (dir == 1 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f);
            SpriteEffects fx = dir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, rot, origin, ItemScale, fx, 0);
            return false;
        }
    }
}