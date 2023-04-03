using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;

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
        Bartender bartender;
        Shaker shaker;
        Glass glass;
        Bottle bottle1;
        Bottle bottle2;
        Bottle bottle3;

        List<Sprite> Objects = new List<Sprite>();

        // Y coords lines (height)
        public float bottomShelf, topShelf, walkingLine, servingLine, barLine;

        Sprite target = null;

        public Vector2 bottleOriginalCoords, shakerOriginalCoords;

        float rotation = MathHelper.ToRadians(4500f);

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

            bartender = new Bartender(Content.Load<Texture2D>("bartender"), new Vector2(screenWidth / 2, screenHeight / 2 + 230), 0.3f);
            LoadBartenderAttributes();
            walkingLine = bartender.position.Y;

            shaker = new Shaker(Content.Load<Texture2D>("shaker"), new Vector2(screenWidth / 2 + 200, bottomShelf), 0.2f);

            glass = new Glass(Content.Load<Texture2D>("glass"), new Vector2(screenWidth / 2 + 300, bottomShelf), 0.2f);

            bottle1 = new Bottle(Content.Load<Texture2D>("bottle1"), new Vector2(screenWidth / 2 + 200, topShelf), 0.2f);
            bottle2 = new Bottle(Content.Load<Texture2D>("bottle2"), new Vector2(screenWidth / 2 + 300, topShelf), 0.2f);
            bottle3 = new Bottle(Content.Load<Texture2D>("bottle3"), new Vector2(screenWidth / 2 + 400, topShelf), 0.2f);

            Objects.Add(shaker);
            Objects.Add(glass);
            Objects.Add(bottle1);
            Objects.Add(bottle2);
            Objects.Add(bottle3);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            bartender.Update(gameTime);
            bottle1.Update(gameTime);

            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Console.Write(Mouse.GetState().X + " " + Mouse.GetState().Y + "\n");
                HandleMouseClick(Mouse.GetState(), gameTime);
            }

            if(bartender.isNearShaker == true)
            {
                bartender = new Bartender(Content.Load<Texture2D>("bartender-reaching-shaker"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.pickShaker = true;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                spriteBatch.End();
            }

            if(bartender.pickShaker == true)
            {
                shaker.layer = 0;
                Objects.Remove(shaker);
                bartender = new Bartender(Content.Load<Texture2D>("bartender-holding-shaker"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.pickShaker = false;
                bartender.isHoldingShaker = true;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                spriteBatch.End();
            }

            if(bartender.isNearBottle == true)
            {
                bartender = new Bartender(Content.Load<Texture2D>("bartender-reaching-bottle"), bartender.position, 0.3f);
                bartender.target = target;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                spriteBatch.End();

                bartender.pickBottle = true;
            }

            if(bartender.isHoldingBottle == true)
            {
                bartender = new Bartender(Content.Load<Texture2D>("bartender-holding-shaker-and-bottle"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bottle1.isPicked = true;
                //bartender.moveBack = true;
                bottle1.move = true;
                

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                spriteBatch.End();
            }

            if(bottle1.inHand == true)
            {
                bartender = new Bartender(Content.Load<Texture2D>("bartender-pouring"), bartender.position, 0.3f);
                Objects[2] = new Bottle(Content.Load<Texture2D>("bottle1-rotated"), Objects[2].position, 0.2f);
                LoadBartenderAttributes();
                
                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                Objects[2].Draw(spriteBatch);
                spriteBatch.End();
            }

            if(bartender.moveBack == false)
            {
                bottle1.move = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            spriteBatch.Draw(barShelves, new Vector2(screenWidth / 2, screenHeight - 260), null, Color.White, 0f, new Vector2(barShelves.Width / 2, barShelves.Height), 1f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(barTable, new Vector2(screenWidth / 2, screenHeight - 20), null, Color.White, 0f, new Vector2(barTable.Width / 2, barTable.Height), 1f, SpriteEffects.None, 0.4f);
            bartender.Draw(spriteBatch);
            //bottle1.Draw(spriteBatch);

            foreach(Sprite sprite in Objects)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void HandleMouseClick(MouseState mousePos, GameTime gameTime)
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
                    if (target == shaker)
                    {
                        Console.WriteLine("Shaker");
                        target = sprite;
                        shakerOriginalCoords = target.position;
                        bartender.target = target;
                        bartender.moveTowardsShaker = true;
                    }
                    else //if(target != shaker && bartender.isHoldingShaker == true)
                    {
                        Console.WriteLine("Not shaker");
                        target = sprite;
                        bottleOriginalCoords = target.position;
                        bartender.target = target;
                        bartender.moveTowardsBottle = true;
                    }
                }
            }
        }

    public void LoadBartenderAttributes()
        {
            bartender.target = target;
            bartender.walkingLine = walkingLine;
            bartender.servingLine = bartender.walkingLine + 50;
            bartender.leftBounds = + 375;
            bartender.rightBounds = screenWidth - 375;
        }
    }
}