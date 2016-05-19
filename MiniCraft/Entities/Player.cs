using System;
using System.Collections.Generic;
using MiniRealms.Engine;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.LevelGens;
using MiniRealms.Entities.Particles;
using MiniRealms.Items;
using MiniRealms.Items.Resources;
using MiniRealms.Levels;
using MiniRealms.Levels.Tiles;
using MiniRealms.Screens;
using MiniRealms.Screens.GameScreens;

namespace MiniRealms.Entities
{
    public class Player : Mob
    {
        private readonly InputHandler _input;
        private int _attackTime, _attackDir;

        public McGame Game;
        public Inventory Inventory = new Inventory();
        public Item AttackItem;
        public Item ActiveItem;
        public int Stamina;
        public int StaminaRecharge;
        public int StaminaRechargeDelay;
        public int Score;
        public int MaxStamina = 10;
        private int _onStairDelay;
        public int InvulnerableTime;

        //Apperance
        public int FullColor { get; protected set; } //Old color: Color.Get(-1, 100, 220, 532);
        public int Outline { get; protected  set; }
        public int Shirt { get; protected set; }
        public int Skin { get; protected set; }

        public Player(McGame game, InputHandler input)
        {
            Game = game;
            _input = input;
            X = 24;
            Y = 24;
            Stamina = MaxStamina;

            Inventory.Add(new FurnitureItem(new Workbench()));
            Inventory.Add(new PowerGloveItem());

            var rnd = new Random((int) LevelGen.Seed);


            ChangePlayerColor(100, rnd.NextInt(555) + 1, rnd.NextInt(555) + 1);
#if DEBUG
            foreach (var item in Resource.AllResources)
            {
                Inventory.Add(new ResourceItem(item, 1000));
            }
#endif

        }

        public void ChangePlayerColor(int outline, int shirt, int skin)
        {
            Outline = outline;
            Shirt = shirt;
            Skin = skin;
            FullColor = Color.Get(-1, outline, shirt, skin);
        }

        public override void Tick()
        {
            base.Tick();

            if (InvulnerableTime > 0) InvulnerableTime--;
            Tile onTile = Level.GetTile(X >> 4, Y >> 4);
            if (onTile == Tile.StairsDown || onTile == Tile.StairsUp)
            {
                if (_onStairDelay == 0)
                {
                    ChangeLevel((onTile == Tile.StairsUp) ? 1 : -1);
                    _onStairDelay = 10;
                    return;
                }
                _onStairDelay = 10;
            }
            else
            {
                if (_onStairDelay > 0) _onStairDelay--;
            }

            if (Stamina <= 0 && StaminaRechargeDelay == 0 && StaminaRecharge == 0)
            {
                StaminaRechargeDelay = 40;
            }

            if (StaminaRechargeDelay > 0)
            {
                StaminaRechargeDelay--;
            }

            if (StaminaRechargeDelay == 0)
            {
                StaminaRecharge++;
                if (IsSwimming())
                {
                    StaminaRecharge = 0;
                }
                while (StaminaRecharge > 10)
                {
                    StaminaRecharge -= 10;
                    if (Stamina < MaxStamina) Stamina++;
                }
            }

            int xa = 0;
            int ya = 0;
            if (_input.Up.Down) ya--;
            if (_input.Down.Down) ya++;
            if (_input.Left.Down) xa--;
            if (_input.Right.Down) xa++;
            if (IsSwimming() && TickTime%60 == 0)
            {
                if (Stamina > 0)
                {
                    Stamina--;
                }
                else
                {
                    Hurt(this, 1, Dir ^ 1);
                }
            }

            if (StaminaRechargeDelay%2 == 0)
            {
                Move(xa, ya);
            }

            if (_input.Attack.Clicked)
            {
                if (Stamina != 0)
                {
                    Stamina--;
                    StaminaRecharge = 0;
                    Attack();
                }
            }
            if (_input.Menu.Clicked && !Use())
                Game.SetMenu(new InventoryMenu(this));
            if (_attackTime > 0) _attackTime--;

        }

        private bool Use()
        {
            int yo = -2;
            if (Dir == 0 && Use(X - 8, Y + 4 + yo, X + 8, Y + 12 + yo)) return true;
            if (Dir == 1 && Use(X - 8, Y - 12 + yo, X + 8, Y - 4 + yo)) return true;
            if (Dir == 3 && Use(X + 4, Y - 8 + yo, X + 12, Y + 8 + yo)) return true;
            if (Dir == 2 && Use(X - 12, Y - 8 + yo, X - 4, Y + 8 + yo)) return true;

            int xt = X >> 4;
            int yt = (Y + yo) >> 4;
            int r = 12;
            if (_attackDir == 0) yt = (Y + r + yo) >> 4;
            if (_attackDir == 1) yt = (Y - r + yo) >> 4;
            if (_attackDir == 2) xt = (X - r) >> 4;
            if (_attackDir == 3) xt = (X + r) >> 4;

            if (xt < 0 || yt < 0 || xt >= Level.W || yt >= Level.H) return false;
            return Level.GetTile(xt, yt).Use(Level, xt, yt, this, _attackDir);
        }

        private void Attack()
        {
            WalkDist += 8;
            _attackDir = Dir;
            AttackItem = ActiveItem;
            bool done = false;

            if (ActiveItem != null)
            {
                _attackTime = 10;
                const int yo = -2;
                const int range = 12;
                if (Dir == 0 && Interact(X - 8, Y + 4 + yo, X + 8, Y + range + yo)) done = true;
                if (Dir == 1 && Interact(X - 8, Y - range + yo, X + 8, Y - 4 + yo)) done = true;
                if (Dir == 3 && Interact(X + 4, Y - 8 + yo, X + range, Y + 8 + yo)) done = true;
                if (Dir == 2 && Interact(X - range, Y - 8 + yo, X - 4, Y + 8 + yo)) done = true;
                if (done) return;

                int xt = X >> 4;
                int yt = (Y + yo) >> 4;
                int r = 12;
                if (_attackDir == 0) yt = (Y + r + yo) >> 4;
                if (_attackDir == 1) yt = (Y - r + yo) >> 4;
                if (_attackDir == 2) xt = (X - r) >> 4;
                if (_attackDir == 3) xt = (X + r) >> 4;

                if (xt >= 0 && yt >= 0 && xt < Level.W && yt < Level.H)
                {
                    if (ActiveItem.InteractOn(Level.GetTile(xt, yt), Level, xt, yt, this, _attackDir))
                    {
                        done = true;
                    }
                    else
                    {
                        if (Level.GetTile(xt, yt).Interact(Level, xt, yt, this, ActiveItem, _attackDir))
                        {
                            done = true;
                        }
                    }
                    if (ActiveItem.IsDepleted())
                    {
                        ActiveItem = null;
                    }
                }
            }

            if (done) return;

            if (ActiveItem == null || ActiveItem.CanAttack())
            {
                _attackTime = 5;
                int yo = -2;
                int range = 20;
                if (Dir == 0) Hurt(X - 8, Y + 4 + yo, X + 8, Y + range + yo);
                if (Dir == 1) Hurt(X - 8, Y - range + yo, X + 8, Y - 4 + yo);
                if (Dir == 3) Hurt(X + 4, Y - 8 + yo, X + range, Y + 8 + yo);
                if (Dir == 2) Hurt(X - range, Y - 8 + yo, X - 4, Y + 8 + yo);

                int xt = X >> 4;
                int yt = (Y + yo) >> 4;
                int r = 12;
                if (_attackDir == 0) yt = (Y + r + yo) >> 4;
                if (_attackDir == 1) yt = (Y - r + yo) >> 4;
                if (_attackDir == 2) xt = (X - r) >> 4;
                if (_attackDir == 3) xt = (X + r) >> 4;

                if (xt >= 0 && yt >= 0 && xt < Level.W && yt < Level.H)
                {
                    Level.GetTile(xt, yt).Hurt(Level, xt, yt, this, Random.NextInt(3) + 1, _attackDir);
                }
            }

        }

        private bool Use(int x0, int y0, int x1, int y1)
        {
            List<Entity> entities = Level.GetEntities(x0, y0, x1, y1);
            for (int i = 0; i < entities.Size(); i++)
            {
                Entity e = entities.Get(i);
                if (e == this) continue;
                if (e.Use(this, _attackDir)) return true;
            }
            return false;
        }

        private bool Interact(int x0, int y0, int x1, int y1)
        {
            List<Entity> entities = Level.GetEntities(x0, y0, x1, y1);
            for (int i = 0; i < entities.Size(); i++)
            {
                Entity e = entities.Get(i);
                if (e == this) continue;
                if (e.Interact(this, ActiveItem, _attackDir)) return true;
            }
            return false;
        }

        private void Hurt(int x0, int y0, int x1, int y1)
        {
            List<Entity> entities = Level.GetEntities(x0, y0, x1, y1);
            for (int i = 0; i < entities.Size(); i++)
            {
                Entity e = entities.Get(i);
                if (e != this) e.Hurt(this, GetAttackDamage(e), _attackDir);
            }
        }

        private int GetAttackDamage(Entity e)
        {
            int dmg = Random.NextInt(3) + 1;
            if (AttackItem == null) return dmg;
            dmg += AttackItem.GetAttackDamageBonus(e);
            return dmg;
        }

        public override void Render(Screen screen)
        {
            int xt = 0;
            int yt = 14;

            int flip1 = (WalkDist >> 3) & 1;
            int flip2 = (WalkDist >> 3) & 1;

            if (Dir == 1)
            {
                xt += 2;
            }
            if (Dir > 1)
            {
                flip1 = 0;
                flip2 = ((WalkDist >> 4) & 1);
                if (Dir == 2)
                {
                    flip1 = 1;
                }
                xt += 4 + ((WalkDist >> 3) & 1)*2;
            }

            int xo = X - 8;
            int yo = Y - 11;
            if (IsSwimming())
            {
                yo += 4;
                int waterColor = Color.Get(-1, -1, 115, 335);
                if (TickTime/8%2 == 0)
                {
                    waterColor = Color.Get(-1, 335, 5, 115);
                }
                screen.Render(xo + 0, yo + 3, 5 + 13*32, waterColor, 0);
                screen.Render(xo + 8, yo + 3, 5 + 13*32, waterColor, 1);
            }

            if (_attackTime > 0 && _attackDir == 1)
            {
                screen.Render(xo + 0, yo - 4, 6 + 13*32, Color.Get(-1, 555, 555, 555), 0);
                screen.Render(xo + 8, yo - 4, 6 + 13*32, Color.Get(-1, 555, 555, 555), 1);
                AttackItem?.RenderIcon(screen, xo + 4, yo - 4);
            }
            var col = HurtTime > 0 ? Color.Get(-1, 555, 555, 555) : FullColor;

            if (ActiveItem is FurnitureItem)
            {
                yt += 2;
            }
            screen.Render(xo + 8*flip1, yo + 0, xt + yt*32, col, flip1);
            screen.Render(xo + 8 - 8*flip1, yo + 0, xt + 1 + yt*32, col, flip1);
            if (!IsSwimming())
            {
                screen.Render(xo + 8*flip2, yo + 8, xt + (yt + 1)*32, col, flip2);
                screen.Render(xo + 8 - 8*flip2, yo + 8, xt + 1 + (yt + 1)*32, col, flip2);
            }

            if (_attackTime > 0 && _attackDir == 2)
            {
                screen.Render(xo - 4, yo, 7 + 13*32, Color.Get(-1, 555, 555, 555), 1);
                screen.Render(xo - 4, yo + 8, 7 + 13*32, Color.Get(-1, 555, 555, 555), 3);
                AttackItem?.RenderIcon(screen, xo - 4, yo + 4, Screen.BitMirrorX);
            }
            if (_attackTime > 0 && _attackDir == 3)
            {
                screen.Render(xo + 8 + 4, yo, 7 + 13*32, Color.Get(-1, 555, 555, 555), 0);
                screen.Render(xo + 8 + 4, yo + 8, 7 + 13*32, Color.Get(-1, 555, 555, 555), 2);
                AttackItem?.RenderIcon(screen, xo + 8 + 4, yo + 4);
            }
            if (_attackTime > 0 && _attackDir == 0)
            {
                screen.Render(xo + 0, yo + 8 + 4, 6 + 13*32, Color.Get(-1, 555, 555, 555), 2);
                screen.Render(xo + 8, yo + 8 + 4, 6 + 13*32, Color.Get(-1, 555, 555, 555), 3);
                AttackItem?.RenderIcon(screen, xo + 4, yo + 8 + 4, Screen.BitMirrorX | Screen.BitMirrorY);
            }

            var item = ActiveItem as FurnitureItem;
            if (item != null)
            {
                Furniture furniture = item.Furniture;
                furniture.X = X;
                furniture.Y = yo;
                furniture.Render(screen);
            }
        }

        public override void TouchItem(ItemEntity itemEntity)
        {
            itemEntity.Take(this);
            Inventory.Add(itemEntity.Item);
        }

        public override bool CanSwim() => true;

        public override bool FindStartPos(Level level)
        {
            while (true)
            {
                int x = Random.NextInt(level.W);
                int y = Random.NextInt(level.H);
                if (level.GetTile(x, y) != Tile.Grass) continue;
                X = x*16 + 8;
                Y = y*16 + 8;
                return true;
            }
        }

        public bool PayStamina(int cost)
        {
            if (cost > Stamina) return false;
            Stamina -= cost;
            return true;
        }

        public void ChangeLevel(int dir)
        {
            Game.ScheduleLevelChange(dir);
        }

        public override int GetLightRadius()
        {
            int r = 2;
            var item = ActiveItem as FurnitureItem;
            if (item == null) return r;
            int rr = item.Furniture.GetLightRadius();
            if (rr > r) r = rr;
            return r;
        }

        protected override void Die()
        {
            base.Die();
            SoundEffectManager.Play("death");
        }

        public override void TouchedBy(Entity entity)
        {
            if (!(entity is Player))
            {
                entity.TouchedBy(this);
            }
        }

        protected override void DoHurt(int damage, int attackDir)
        {
            if (HurtTime > 0 || InvulnerableTime > 0) return;

            SoundEffectManager.Play("playerhurt");
            Level.Add(new TextParticle("" + damage, X, Y, Color.Get(-1, 504, 504, 504)));
            Health -= damage;
            if (attackDir == 0) YKnockback = +6;
            if (attackDir == 1) YKnockback = -6;
            if (attackDir == 2) XKnockback = -6;
            if (attackDir == 3) XKnockback = +6;
            HurtTime = 10;
            InvulnerableTime = 30;
        }

        public void GameWon()
        {
            Level.Player.InvulnerableTime = 60*5;
            Game.Won();
        }
    }
}
