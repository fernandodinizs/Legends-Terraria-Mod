using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Legends.Content.Items;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

using Legends.Content.Projectiles; //import do projectile

namespace Legends.Systems
{
    public class SunBeamsSpawnSystem : ModSystem
    {   
        // Chance de spawn por tentativa.
        // 1 em 1000 = aproximadamente 0,1%
        // 1 em 3000 = aproximadamente 0,033%
        // 1 em 5000 = aproximadamente 0,02%
        // aumentar o numero deixa mais raro, e deminuir mais comun
        // private const int SpawnChance = 1;   
        private const int SpawnChance = 2000;
        
        // Quantos ticks entre cada tentativa de spawn.
        // 60 ticks = aproximadamente 1 segundo.
        private const int SpawnInterval = 60;

        // Guarda o contador para não verificar todo frame.
        private int spawnTimer;


        // --------------------------------
        //  MARCACAO HORARIO
        // ------------------

        //VERIFICA SE É NO PERIODO MIDDAY
        private static bool IsMidday()
        {
            /*
             * Sunrise = 0
             * Midday  = 27000
             * Sunset  = 54000
             */

            if (!Main.dayTime)
                return false;

            return Main.time >= 27000 && Main.time < 54000;
        }

        // --------------------------------
        //  FIM MARCACAO HORARIO
        // ------------------

        // --------------------------------
        //  SPAWN DE ITENS
        // ------------------
        private static void TrySpawnSunBeams()
        {   
            // Chance de spawn.
            if (!Main.rand.NextBool(SpawnChance))
                return;

            // escolhe uma posição horizontal aleatória dentro do mundo
            // -50 e para ecluir o spwan da borda do mundo
            int tileX = Main.rand.Next(50, Main.maxTilesX - 50); 

            // escolhe uma posição vertical alta dentro do mundo
            //EsCOLHE UMA POSIÇÃO ALEATORIA ENTRE 40 E 99
            //quanto maior o numero mais baixo ele spwana
            int tileY = Main.rand.Next(0, 90);

            Vector2 spawnPosition = new Vector2(
                tileX * 16,
                tileY * 16
            );

            Vector2 velocity = new Vector2(
                Main.rand.NextFloat(-2f, 2f),
                Main.rand.NextFloat(3f, 5f)
            );

            // CRIA O ITEM
            Projectile.NewProjectile(
                new EntitySource_Misc("SunBeamsSpawn"),      // tipo de source do spawn
                spawnPosition,       
                velocity,
                ModContent.ProjectileType<SunbeamProjectile>(), // tipo do item que vai spawnar
                0,
                0f,
                Main.myPlayer   
            );
        }
        // --------------------------------
        //  FIM SPAWN DE ITENS
        // ------------------

        // --------------------------------
        // UPDATE DO MUNDO
        // ------------------
        public override void PostUpdateWorld()
        {
            //so o servidor cria itens
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            //verifica se está midday
            if (IsMidday())
            {
                spawnTimer++;

                if (spawnTimer >= SpawnInterval)
                {
                    spawnTimer = 0;

                    TrySpawnSunBeams();
                }
            }
            else
            {
                spawnTimer = 0;
            }
        // --------------------------------
        // FIM UPDATE DO MUNDO
        // ------------------
        }
        
    }

}