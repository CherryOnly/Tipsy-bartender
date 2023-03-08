using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tipsy_bartender
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        int screenWidth = 1440;
        int screenHeight = 900;

        Texture2D barShelves;
        Texture2D barTable;
        Texture2D bartender;
        Texture2D shaker;
        /*Texture2D glass;
        Texture2D bottle1;
        Texture2D bottle2;
        Texture2D bottle3;*/

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
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
            barTable = Content.Load<Texture2D>("bar-table"); //bar-table-sprite-big
            bartender = Content.Load<Texture2D>("bartender-white-shirt");
            shaker = Content.Load<Texture2D>("shaker");
            /*glass = Content.Load<Texture2D>("glass");
            bottle1 = Content.Load<Texture2D>("bottle1");
            bottle2 = Content.Load<Texture2D>("bottle2");
            bottle3 = Content.Load<Texture2D>("bottle3");*/
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

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            // Layers (back to front):
            // room, shelves/other objects in a room,
            // bottles/glasses, bartender, bar table/anything on a bar
            // bar chairs, clients
            // ** Object layer changes after certain actions (e.g. client sits, chair goes behind him)
            spriteBatch.Draw(barShelves, new Vector2(screenWidth / 2 - (barShelves.Width / 2),screenHeight / 2 - (barShelves.Height / 2) - 100), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(barTable, new Vector2(screenWidth / 2 - (barTable.Width / 2), screenHeight / 2 - (barTable.Height / 2) + 200), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.4f);
            spriteBatch.Draw(bartender, new Vector2(screenWidth / 2 - (bartender.Width / 2), screenHeight / 2 - (bartender.Height / 2)), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            spriteBatch.Draw(shaker, new Vector2(screenWidth / 2 + 50, screenHeight / 2 + 80), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.5f);
            //spriteBatch.Draw(glass, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            //spriteBatch.Draw(bottle1, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            //spriteBatch.Draw(bottle2, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            //spriteBatch.Draw(bottle3, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}