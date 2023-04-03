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
        Hand hand;
        Shaker shaker;
        Glass glass;
        Bottle bottle1;
        Bottle bottle2;
        Bottle bottle3;

        List<Sprite> Objects = new List<Sprite>();
        List<Bottle> Bottles = new List<Bottle>();

        // Y coords lines (height)
        public float bottomShelf, topShelf, walkingLine, servingLine, barLine;

        Sprite target = null;

        public Vector2 bottleOriginalCoords, shakerOriginalCoords;

        // Pouring times
        public readonly TimeSpan pouringTime = TimeSpan.FromSeconds(2);
        public TimeSpan pouringStart;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            barShelves = Content.Load<Texture2D>("bar-shelves");
            bottomShelf = barShelves.Height - 180;
            topShelf = barShelves.Height - 320;

            barTable = Content.Load<Texture2D>("bar-table");

            bartender = new Bartender(Content.Load<Texture2D>("bartender"), new Vector2(screenWidth / 2, screenHeight / 2 + 230), 0.3f);
            LoadBartenderAttributes();
            walkingLine = bartender.position.Y;
            servingLine = walkingLine + 50;

            hand = new Hand(Content.Load<Texture2D>("bartender-hand"), new Vector2(-100, -100), 0.3f);

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

            Bottles.Add(bottle1);
            Bottles.Add(bottle2);
            Bottles.Add(bottle3);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            bartender.Update(gameTime);

            foreach (Bottle bottle in Bottles)
            {
                bottle.Update(gameTime);
            }

            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Console.Write(Mouse.GetState().X + " " + Mouse.GetState().Y + "\n");
                HandleMouseClick(Mouse.GetState(), gameTime);
            }

            if(bartender.shakerTaken == true)
            {
                shakerOriginalCoords = target.position;
                shaker.layer = 0;
                Objects.Remove(shaker);

                bartender = new Bartender(Content.Load<Texture2D>("bartender-holding-shaker"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.isHoldingShaker = true;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                spriteBatch.End();

                bartender.shakerTaken = false;
            }

            if(bartender.bottleTaken == true)
            {
                bottleOriginalCoords = target.position;

                bartender = new Bartender(Content.Load<Texture2D>("bartender-pouring"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.isHoldingShaker = true;
                bartender.isHoldingBottle = true;
                bartender.isPouring = true;

                Bottles[0] = new Bottle(Content.Load<Texture2D>("bottle1-rotated"), Bottles[0].position, 0.3f);
                Bottles[0].isTaken = true;
                target = Bottles[0];

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                Bottles[0].Draw(spriteBatch);
                spriteBatch.End();

                bartender.bottleTaken = false;

                pouringStart = gameTime.TotalGameTime;
            }

            if (bartender.isPouring == true)
            {
                hand = new Hand(Content.Load<Texture2D>("bartender-hand"), new Vector2(bartender.position.X + bartender.texture.Width / 2 - 20, target.position.Y - target.texture.Height), 0.4f);

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                hand.Draw(spriteBatch);
                spriteBatch.End();
            }

            // Pouring time passed
            if(bartender.isPouring == true && (pouringTime + pouringStart) < gameTime.TotalGameTime)
            {
                Bottles[0] = new Bottle(Content.Load<Texture2D>("bottle1"), bottleOriginalCoords, 0.2f);

                hand = new Hand(Content.Load<Texture2D>("bartender-hand"), new Vector2(-100, -100), 0.3f);

                bartender = new Bartender(Content.Load<Texture2D>("bartender-holding-shaker"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.isHoldingShaker = true;
                bartender.isHoldingBottle = false;
                bartender.isPouring = false;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                Bottles[0].Draw(spriteBatch);
                hand.Draw(spriteBatch);
                bartender.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            spriteBatch.Draw(barShelves, new Vector2(screenWidth / 2, screenHeight - 260), null, Color.White, 0f, new Vector2(barShelves.Width / 2, barShelves.Height), 1f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(barTable, new Vector2(screenWidth / 2, screenHeight - 20), null, Color.White, 0f, new Vector2(barTable.Width / 2, barTable.Height), 1f, SpriteEffects.None, 0.4f);
            bartender.Draw(spriteBatch);
            hand.Draw(spriteBatch);
            shaker.Draw(spriteBatch);
            glass.Draw(spriteBatch);

            foreach(Bottle bottle in Bottles)
            {
                bottle.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void HandleMouseClick(MouseState mousePos, GameTime gameTime)
        {
            foreach (Sprite sprite in Objects)
            {
                // mouse cursor is in the area of an object when pressed
                if (
                    mousePos.X >= sprite.position.X - (sprite.texture.Width / 2) &&
                    mousePos.X <= sprite.position.X + (sprite.texture.Width / 2) &&
                    mousePos.Y >= sprite.position.Y - sprite.texture.Height &&
                    mousePos.Y <= sprite.position.Y && 
                    !bartender.takeShaker &&
                    !bartender.takeBottle &&
                    !bartender.isPouring
                    )
                {
                    if (sprite.GetType() == typeof(Shaker))
                    {
                        Console.WriteLine("Shaker");
                        target = sprite;
                        shakerOriginalCoords = target.position;
                        bartender.target = target;
                        bartender.takeShaker = true;
                    }
                    if(sprite.GetType() == typeof(Bottle) && bartender.isHoldingShaker == true)
                    {
                        Console.WriteLine("Bottle");
                        target = sprite;
                        bottleOriginalCoords = target.position;
                        bartender.target = target;
                        bartender.takeBottle = true;
                    }
                }
            }
        }

    public void LoadBartenderAttributes()
        {
            bartender.target = target;
            bartender.walkingLine = walkingLine;
            bartender.servingLine = servingLine;
            bartender.leftBounds = + 375;
            bartender.rightBounds = screenWidth - 375;
        }
    }
}