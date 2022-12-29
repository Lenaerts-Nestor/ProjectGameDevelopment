﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGameDevelopment.Characters;
using ProjectGameDevelopment.Characters.Playable;
using ProjectGameDevelopment.Map;
using TiledSharp;

namespace ProjectGameDevelopment.Level
{
    public class Level2 : LevelMaker
    {
        private new Game1 Game => (Game1)base.Game;
        public Level2(Game game) : base(game) { }
        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Map maken => 
            _map = new TmxMap("Content\\Level2.tmx");
            _tileset = Content.Load<Texture2D>("Final\\Assets\\" + _map.Tilesets[0].Name.ToString());
            _mapMaker = new MapDrawer(_map, _tileset);

            //BEWAAR DE COLLISIONS =>
            CollisionTiles = _collisionController.GetTilesCollision(_map, CollisionTiles);
            EnemyPathWay = _collisionController.GetEnemyPathWayCollision(_map, EnemyPathWay);
            RespawnZone = _collisionController.GetRespawnCollision(_map, RespawnZone);
            EndZone = _collisionController.GetEndCollision(_map, EndZone);


            Player = new Player(new Vector2(RespawnZone[0].X, RespawnZone[0].Y), true,
              Content.Load<Texture2D>("Sprite Pack 5\\2 - Lil Wiz\\Idle_(32 x 32)"), Content.Load<Texture2D>("Sprite Pack 5\\2 - Lil Wiz\\Running_(32 x 32)"),
              Content.Load<Texture2D>("Sprite Pack 5\\2 - Lil Wiz\\Ducking_(32 x 32)"), Content.Load<Texture2D>("Sprite Pack 5\\2 - Lil Wiz\\Casting_Spell_Aerial_(32 x 32)"));

            //Enemy Creatie => 
            _enemyList.Add(new Enemy(Content.Load<Texture2D>("Sprite Pack 4\\2 - Martian_Red_Running (32 x 32)"), EnemyPathWay[0], 1f, false, false, Player, new Vector2()));
            _enemyList.Add(new Enemy(Content.Load<Texture2D>("Sprite Pack 5\\5 - Moe Scotty\\Flying_(32 x 32)"), EnemyPathWay[1], 1f, false, false, Player, new Vector2()));
            _enemyList.Add(new Enemy(Content.Load<Texture2D>("Sprite Pack 5\\1 - Robo Retro\\Flying_(32 x 32)"), new Rectangle(), 3f, false, true, Player, new Vector2(RespawnZone[1].X, RespawnZone[1].Y)));

            //Bullets creatie 
            _bulletTexture = Content.Load<Texture2D>("Sprite Pack 5\\2 - Lil Wiz\\Sparkles_(8 x 8)");
        }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _collisionController.DrawLevelMap(_spriteBatch, _mapMaker); // Tekenen van de map

            //Teken de TEXT Punten en Extra's 
            _spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts\\Font"), $"POINTS : {Player.Points}", new Vector2(50, 180), Color.White);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts\\Font"), $"HP : {Player.HealthPoints}", new Vector2(50, 200), Color.White);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts\\Font"), $"3 POINTS = Win&Game over", new Vector2(50, 250), Color.White);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts\\Font"), $"END PORTAL", new Vector2(670, 45), Color.White);

            Player.Draw(_spriteBatch, gameTime);                                                //Tekenen van Player
            foreach (var enemy in _enemyList) { enemy.Draw(_spriteBatch, gameTime); }           // Tekenen van Enemie's 
            foreach (var bullet in _bullets.ToArray()) { bullet.Draw(_spriteBatch, gameTime); } //Tekenen van bu;llet

            _spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            LevelConditionControl.GetEndzone(Player, EndZone, Game);

            EnemyInitPos = LevelConditionControl.GetEnemyPosition(_enemyList, EnemyInitPos);
            PlayerInitPosition = Player.Position;

            Player.Update(gameTime);
            LevelConditionControl.GetPlayerLifeCondition(Player, _enemyList, gameTime);

            LevelConditionControl.GetPlayerCollides(Player, CollisionTiles, PlayerInitPosition);
            LevelConditionControl.GetBulletCollides(Player, _bullets, _enemyList, CollisionTiles, gameTime);
            LevelConditionControl.GetEnemyCollides(CollisionTiles, _enemyList, EnemyInitPos);
            LevelConditionControl.GetBullets(_bullets, Player, _time_x_bullet, _bulletTexture, this);
            //LevelConditionControl.GetItemBuff(this._buffItemList, this.Player);

            LevelConditionControl.GetPlayerGameState(Player, Game);
        }
    }
}