using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Items.Accessories.Forces;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.Projectiles.Champions;

namespace FargowiltasSouls.NPCs.Champions
{
    public class TimberChampion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Timber");
        }

        /*public override bool Autoload(ref string name)
        {
            return false;
        }*/

        public override void SetDefaults()
        {
            npc.width = 340;
            npc.height = 400;
            npc.damage = 100;
            npc.defense = 50;
            npc.lifeMax = 180000;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 10);
            //npc.boss = true;
            music = MusicID.TheTowers;
            musicPriority = MusicPriority.BossMedium;

            npc.buffImmune[BuffID.Chilled] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.buffImmune[mod.BuffType("Lethargic")] = true;
            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;
        }

        public override void AI()
        {
            Player player = Main.player[npc.target];
            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;

            switch ((int)npc.ai[0])
            {
                case 0: //jump at player
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[1] == 60)
                    {
                        npc.TargetClosest();

                        const float gravity = 0.4f;
                        const float time = 90f;
                        Vector2 distance = player.Center - npc.Center;
                        distance.Y -= npc.height;

                        distance.X = distance.X / time;
                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                        npc.velocity = distance;
                        npc.netUpdate = true;

                        if (Main.netMode != 1) //explosive jump
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.DD2OgreSmash, npc.damage / 4, 0, Main.myPlayer);
                        }

                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);

                        for (int k = -2; k <= 2; k++) //explosions
                        {
                            Vector2 dustPos = npc.position;
                            int width = npc.width / 5;
                            dustPos.X += width * k + Main.rand.NextFloat(-width, width);
                            dustPos.Y += npc.height - width / 2 + Main.rand.NextFloat(-width, width) / 2;

                            for (int i = 0; i < 30; i++)
                            {
                                int dust = Dust.NewDust(dustPos, 32, 32, 31, 0f, 0f, 100, default(Color), 3f);
                                Main.dust[dust].velocity *= 1.4f;
                            }

                            for (int i = 0; i < 20; i++)
                            {
                                int dust = Dust.NewDust(dustPos, 32, 32, 6, 0f, 0f, 100, default(Color), 3.5f);
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].velocity *= 7f;
                                dust = Dust.NewDust(dustPos, 32, 32, 6, 0f, 0f, 100, default(Color), 1.5f);
                                Main.dust[dust].velocity *= 3f;
                            }

                            float scaleFactor9 = 0.5f;
                            for (int j = 0; j < 4; j++)
                            {
                                int gore = Gore.NewGore(dustPos, default(Vector2), Main.rand.Next(61, 64));
                                Main.gore[gore].velocity *= scaleFactor9;
                                Main.gore[gore].velocity.X += 1f;
                                Main.gore[gore].velocity.Y += 1f;
                            }
                        }
                    }
                    else if (npc.ai[1] > 60)
                    {
                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y += 0.4f;

                        if (npc.ai[1] > 60 + 90)
                        {
                            npc.TargetClosest();
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    else //less than 60
                    {
                        npc.velocity.X *= 0.95f;

                        if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 2500f)
                        {
                            npc.TargetClosest();
                            if (npc.timeLeft > 30)
                                npc.timeLeft = 30;

                            npc.noTileCollide = true;
                            npc.noGravity = true;
                            npc.velocity.Y -= 1f;
                        }
                        else
                        {
                            npc.timeLeft = 600;
                        }
                    }
                    break;

                case 1: //acorn sprays
                    npc.velocity.X *= 0.95f;
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[2] > 35)
                    {
                        npc.ai[2] = 0;
                        const float gravity = 0.2f;
                        float time = 60f;
                        Vector2 distance = player.Center - npc.Center;// + player.velocity * 30f;
                        distance.X = distance.X / time;
                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                        for (int i = 0; i < 20; i++)
                        {
                            Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-0.5f, 0.5f) * 3,
                                ModContent.ProjectileType<Acorn>(), npc.damage / 5, 0f, Main.myPlayer);
                        }
                    }
                    
                    if (++npc.ai[1] > 120)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    break;

                case 2:
                    goto case 0;

                case 3: //snowball barrage
                    npc.velocity.X *= 0.95f;
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[2] > 5)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1 && npc.ai[1] > 30 && npc.ai[1] < 120)
                        {
                            Projectile.NewProjectile(npc.Center + new Vector2(Main.rand.NextFloat(-100, 100), Main.rand.NextFloat(-150, 0)),
                                Vector2.UnitY * -16f, ModContent.ProjectileType<Snowball>(), npc.damage / 4, 0f, Main.myPlayer);
                        }
                    }

                    if (++npc.ai[1] > 150)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    break;

                case 4:
                    goto case 0;

                case 5: //spray squirrels
                    npc.velocity.X *= 0.95f;
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[2] > 6)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<LesserSquirrel>());
                            if (n != Main.maxNPCs)
                            {
                                Main.npc[n].velocity.X = Main.rand.NextFloat(-10, 10);
                                Main.npc[n].velocity.Y = Main.rand.NextFloat(-20, -10);
                                Main.npc[n].netUpdate = true;
                                if (Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                    }

                    if (++npc.ai[1] > 180)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    break;

                case 6:
                    goto case 0;

                /*case 7:
                    goto case 3;

                case 8:
                    goto case 0;

                case 9: //grappling hook
                    npc.velocity.X *= 0.95f;
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (npc.ai[2] == 0)
                    {
                        
                    }
                    break;*/

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
        }

        /*public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }*/

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.Size, ModContent.ItemType<TimberForce>());
        }
    }
}