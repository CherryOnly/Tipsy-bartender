using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tipsy_bartender
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Texture2D barShelves;
        Texture2D barTable;
        Texture2D bartenderWhite;
        Texture2D bartenderBlack;
        Texture2D shaker;
        Texture2D glass;
        Texture2D bottle1;
        Texture2D bottle2;
        Texture2D bottle3;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            //graphics.IsFullScreen= true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            barShelves = Content.Load<Texture2D>("bar-shelves");
            barTable = Content.Load<Texture2D>("bar-table");
            bartenderWhite = Content.Load<Texture2D>("bartender-white-shirt");
            bartenderBlack = Content.Load<Texture2D>("bartender-black-shirt");
            shaker = Content.Load<Texture2D>("shaker");
            glass = Content.Load<Texture2D>("glass");
            bottle1 = Content.Load<Texture2D>("bottle1");
            bottle2 = Content.Load<Texture2D>("bottle2");
            bottle3 = Content.Load<Texture2D>("bottle3");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.Draw(barShelves, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(barTable, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bartenderWhite, new Vector2(0, 0), Color.White);
            //spriteBatch.Draw(bartenderBlack, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(shaker, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(glass, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bottle1, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bottle2, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bottle3, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}