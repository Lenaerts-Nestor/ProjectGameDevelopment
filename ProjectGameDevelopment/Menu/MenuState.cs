﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;

namespace ProjectGameDevelopment.Menu
{
    public class MenuState : GameScreen
    {
        private List<MenuComponent> _components;

        private new Game1 Game => (Game1)base.Game;

        public SpriteBatch _spriteBatch;

        public MenuState(Game game) : base(game)
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var buttonTexture = game.Content.Load<Texture2D>("Controls\\ButtonImage");
            var buttonFont = game.Content.Load<SpriteFont>("Fonts\\Font");

            var Level1button = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "START LVL 1",

            };

            Level1button.Click += Level1Button_click;


            //LEVEL 2
            var Level2button = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "START LVL 2",
            };
            Level2button.Click += Level2Button_click;

            _components = new List<MenuComponent>
            {
                Level1button,
                Level2button,
            };
        }

        private void Level2Button_click(object sender, EventArgs e)
        {

            this.Game.stateOfGame = currentGameState.level2;
        }

        private void Level1Button_click(object sender, EventArgs e)
        {
            this.Game.stateOfGame = currentGameState.level1;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Draw(_spriteBatch, gameTime);
            }
        }
    }
}
