using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tipsy_bartender
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private PouringState pouringState;

        int screenWidth = 1440;
        int screenHeight = 900;

        Texture2D barShelves;
        Texture2D barTable;
        Bartender bartender;
        Shaker shaker;
        Glass glass;
        Bottle bottle1;
        Bottle bottle2;
        Bottle bottle3;

        List<Sprite> Objects = new List<Sprite>();

        int bottomShelf, topShelf;

        int barLeftBorder = 300;
        int barRightBorder = 1140;

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
            bottomShelf = barShelves.Height - 180;
            topShelf = barShelves.Height - 320;

            barTable = Content.Load<Texture2D>("bar-table");

            bartender = new Bartender(Content.Load<Texture2D>("bartender"), new Vector2(screenWidth / 2, screenHeight / 2 - 10), 0.3f);

            shaker = new Shaker(Content.Load<Texture2D>("shaker"), new Vector2(screenWidth / 2 + 200, bottomShelf), 0.2f);

            glass = new Glass(Content.Load<Texture2D>("glass"), new Vector2(screenWidth / 2 + 300, bottomShelf), 0.2f);

            pouringState = new PouringState();

            bottle1 = new Bottle(Content.Load<Texture2D>("bottle1"), new Vector2(screenWidth / 2 + 200, topShelf), 0.2f);
            bottle2 = new Bottle(Content.Load<Texture2D>("bottle2"), new Vector2(screenWidth / 2 + 300, topShelf), 0.2f);
            bottle3 = new Bottle(Content.Load<Texture2D>("bottle3"), new Vector2(screenWidth / 2 + 400, topShelf), 0.2f);

            Objects.Add(shaker);
            Objects.Add(glass);
            Objects.Add(bottle1);
            Objects.Add(bottle2);
            Objects.Add(bottle3);

            Texture2D bartenderIdle = Content.Load<Texture2D>("bartender");
        }

        /*protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            bartender.Update(gameTime);

            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Console.Write(Mouse.GetState().X + " " + Mouse.GetState().Y + "\n");
                HandleMouseClick(Mouse.GetState(), gameTime);
            }

            base.Update(gameTime);
        }*/

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get the current keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // Move the bartender left or right based on the 'A' and 'D' key presses
            if (keyboardState.IsKeyDown(Keys.A))
            {
                bartender.MoveLeft(barLeftBorder);
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                bartender.MoveRight(barRightBorder);
            }
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            spriteBatch.Draw(barShelves, new Vector2(screenWidth / 2 - (barShelves.Width / 2),screenHeight / 2 - (barShelves.Height / 2) - 110), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(barTable, new Vector2(screenWidth / 2 - (barTable.Width / 2), screenHeight / 2 - (barTable.Height / 2) + 200), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.4f);
            bartender.Draw(spriteBatch);

            foreach(Sprite sprite in Objects)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


       /* public void HandleMouseClick(MouseState mousePos, GameTime gameTime)
        {
            foreach (Sprite sprite in Objects)
            {
                // mouse cursor is in the area of an object when pressed
                if(
                    mousePos.X >= sprite.position.X - (sprite.texture.Width / 2) && 
                    mousePos.X <= sprite.position.X + (sprite.texture.Width / 2) && 
                    mousePos.Y >= sprite.position.Y - sprite.texture.Height && 
                    mousePos.Y <= sprite.position.Y)
                {
                    // textures bounds (rectangles around textures)
                    //Rectangle bartenderRect = new Rectangle((int)(bartender.position.X - (bartender.texture.Width / 2)), (int)(bartender.position.Y - (bartender.texture.Height / 2)), bartender.texture.Width, bartender.texture.Height);
                    //Rectangle spriteRect = new Rectangle((int)(sprite.position.X - (sprite.texture.Width / 2)), (int)(sprite.position.Y - sprite.texture.Height), sprite.texture.Width, sprite.texture.Height);

                    //while (!bartenderRect.Intersects(spriteRect))
                    //{
                        //bartenderRect = new Rectangle((int)(bartender.position.X - (bartender.texture.Width / 2)), (int)(bartender.position.Y - (bartender.texture.Height / 2)), bartender.texture.Width, bartender.texture.Height);
                        //spriteRect = new Rectangle((int)(sprite.position.X - (sprite.texture.Width / 2)), (int)(sprite.position.Y - sprite.texture.Height), sprite.texture.Width, sprite.texture.Height);
                        bartender.Move(sprite, gameTime);
                        //base.Update(gameTime);
                    //}
                }
            }
        }*/
       public void HandleMouseClick(MouseState mousePos, GameTime gameTime)
        {
            foreach (Sprite sprite in Objects.OfType<Bottle>())
            {
                if (sprite is Bottle bottle &&
                    mousePos.X >= bottle.position.X - (bottle.texture.Width / 2) &&
                    mousePos.X <= bottle.position.X + (bottle.texture.Width / 2) &&
                    mousePos.Y >= bottle.position.Y - bottle.texture.Height &&
                    mousePos.Y <= bottle.position.Y)
                {
                    bottle.IsClicked = true;
                    bartender.Target = bottle;
                }
            }
        }


    }
}