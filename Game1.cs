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

        Texture2D background;
        Texture2D barShelves;
        Table barTable;
        Bartender bartender;
        Hand hand;
        Shaker shaker;
        Glass glass;
        Bottle bottle1;
        Bottle bottle2;
        Bottle bottle3;
        Button shakeButton;

        CustomerSprite Customersprite;
        Texture2D CustomerTexture;
        Texture2D customerSideTexture;
        CustomerSideSprite customerSideSprite;
        bool customerArrived = false;

        List<Sprite> Objects = new List<Sprite>();
        List<Bottle> Bottles = new List<Bottle>();

        // Y coords lines (height)
        public float bottomShelf, topShelf, walkingLine, servingLine, tableLine;

        Sprite target = null;

        public Vector2 bottleOriginalCoords, shakerOriginalCoords, glassOriginalCoords;

        // Times
        public readonly TimeSpan pouringTime = TimeSpan.FromMilliseconds(1500);
        public TimeSpan pouringStartTime;
        public readonly TimeSpan buttonPressedTime = TimeSpan.FromMilliseconds(100);
        public TimeSpan buttonPressedStartTime;
        public readonly TimeSpan shakingInterval = TimeSpan.FromMilliseconds(300);
        public TimeSpan shakingStartTime;
        public int shakeTimes = 4;
        public int timesShaked = 0;
        public readonly TimeSpan fillingGlassTime = TimeSpan.FromMilliseconds(1500);
        public TimeSpan fillingGlassStartTime;
        public readonly TimeSpan customerDrinkingTime = TimeSpan.FromMilliseconds(5000);
        public TimeSpan customerDrinkingStartTime;

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

            background = Content.Load<Texture2D>("background");

            barShelves = Content.Load<Texture2D>("bar-shelves");
            bottomShelf = barShelves.Height - 180;
            topShelf = barShelves.Height - 320;

            barTable = new Table(Content.Load<Texture2D>("bar-table"), new Vector2(screenWidth / 2, screenHeight - 20), 0.4f);
            tableLine = barTable.position.Y - (barTable.texture.Height / 2) - 30;

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

            shakeButton = new Button(Content.Load<Texture2D>("shake-button-unpressed"), new Vector2(150, screenHeight - 50), 0.7f);

            Objects.Add(shaker);
            Objects.Add(glass);
            Objects.Add(bottle1);
            Objects.Add(bottle2);
            Objects.Add(bottle3);
            Objects.Add(shakeButton);
            Objects.Add(barTable);

            Bottles.Add(bottle1);
            Bottles.Add(bottle2);
            Bottles.Add(bottle3);

            CustomerTexture = Content.Load<Texture2D>("customer-back");
            Customersprite = new CustomerSprite(CustomerTexture, new Vector2(400, 890), 0.8f, Content);

            customerSideTexture = Content.Load<Texture2D>("customer-side");
            customerSideSprite = new CustomerSideSprite(CustomerTexture, new Vector2(-100, 890), 0.8f, Content);

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

            // Shaker actions
            if(bartender.shakerTaken == true)
            {
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

            // Bottle actions
            if(bartender.bottleTaken == true)
            {
                //bottleOriginalCoords = target.position;

                bartender = new Bartender(Content.Load<Texture2D>("bartender-pouring"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.isHoldingShaker = true;
                bartender.isHoldingBottle = true;
                bartender.isPouring = true;
                shaker.isEmpty = false;

                Bottles[0] = new Bottle(Content.Load<Texture2D>("bottle1-rotated"), Bottles[0].position, 0.3f);
                Bottles[0].isTaken = true;
                target = Bottles[0];

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                Bottles[0].Draw(spriteBatch);
                spriteBatch.End();

                bartender.bottleTaken = false;

                pouringStartTime = gameTime.TotalGameTime;
            }

            // Hand actions
            if (bartender.isPouring == true)
            {
                hand = new Hand(Content.Load<Texture2D>("bartender-hand"), new Vector2(bartender.position.X + bartender.texture.Width / 2 - 20, target.position.Y - target.texture.Height), 0.4f);

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                hand.Draw(spriteBatch);
                spriteBatch.End();
            }

            // Pouring time
            if(bartender.isPouring == true && (pouringTime + pouringStartTime) < gameTime.TotalGameTime)
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

            // Shake button actions
            if(shakeButton.isPressed == true && (buttonPressedTime + buttonPressedStartTime) > gameTime.TotalGameTime)
            {
                shakeButton = new Button(Content.Load<Texture2D>("shake-button-pressed"), new Vector2(150, screenHeight - 50), 0.7f);
                shakeButton.isPressed = true;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                shakeButton.Draw(spriteBatch);
                spriteBatch.End();
            }
            if(shakeButton.isPressed == true && (buttonPressedTime + buttonPressedStartTime) < gameTime.TotalGameTime)
            {
                shakeButton = new Button(Content.Load<Texture2D>("shake-button-unpressed"), new Vector2(150, screenHeight - 50), 0.7f);

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                shakeButton.Draw(spriteBatch);
                spriteBatch.End();
            }

            // Shaking actions
            if(bartender.startShaking == true)
            {
                if (timesShaked != shakeTimes)
                {
                    if (shakingInterval + shakingStartTime < gameTime.TotalGameTime)
                    {
                        if (2 * shakingInterval + shakingStartTime > gameTime.TotalGameTime)
                        {
                            bartender = new Bartender(Content.Load<Texture2D>("bartender-shaking-up"), bartender.position, 0.3f);
                            LoadBartenderAttributes();
                            bartender.isHoldingShaker = true;
                            shaker.isEmpty = false;
                            bartender.startShaking = true;

                            spriteBatch.Begin(SpriteSortMode.FrontToBack);
                            bartender.Draw(spriteBatch);
                            spriteBatch.End();
                        }
                        else
                        {
                            shakingStartTime = gameTime.TotalGameTime;
                            timesShaked++;
                        }
                    }
                    else
                    {
                        bartender = new Bartender(Content.Load<Texture2D>("bartender-shaking-down"), bartender.position, 0.3f);
                        LoadBartenderAttributes();
                        bartender.isHoldingShaker = true;
                        shaker.isEmpty = false;
                        bartender.startShaking = true;

                        spriteBatch.Begin(SpriteSortMode.FrontToBack);
                        bartender.Draw(spriteBatch);
                        spriteBatch.End();
                    }
                }
                else
                {
                    bartender = new Bartender(Content.Load<Texture2D>("bartender-holding-shaker"), bartender.position, 0.3f);
                    LoadBartenderAttributes();
                    bartender.isHoldingShaker = true;
                    shaker.isEmpty = false;
                    bartender.startShaking = false;
                    bartender.readyToServe = true;

                    spriteBatch.Begin(SpriteSortMode.FrontToBack);
                    bartender.Draw(spriteBatch);
                    spriteBatch.End();
                }
            }

            if(bartender.glassTaken == true)
            {
                glass.layer = 0;
                Objects.Remove(glass);

                bartender = new Bartender(Content.Load<Texture2D>("bartender-filling-glass"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.isHoldingShaker = true;
                bartender.isFillingGlass = true;
                glass.isTaken = true;

                fillingGlassStartTime = gameTime.TotalGameTime;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                bartender.Draw(spriteBatch);
                spriteBatch.End();

                bartender.glassTaken = false;
            }

            if(bartender.isFillingGlass == true && (fillingGlassTime + fillingGlassStartTime) < gameTime.TotalGameTime)
            {
                //glass = new Glass(Content.Load<Texture2D>("glass"), glassOriginalCoords, 0.2f);
                shaker = new Shaker(Content.Load<Texture2D>("shaker"), shakerOriginalCoords, 0.2f);
                //Objects.Add(glass);
                Objects.Add(shaker);

                bartender = new Bartender(Content.Load<Texture2D>("bartender-holding-glass"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.isDrinkFinished = true;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                //glass.Draw(spriteBatch);
                shaker.Draw(spriteBatch);
                bartender.Draw(spriteBatch);
                spriteBatch.End();
            }

            if(bartender.getBack == true)
            {
                glass = new Glass(Content.Load<Texture2D>("glass"), new Vector2(bartender.position.X, tableLine), 0.5f);

                customerDrinkingStartTime = gameTime.TotalGameTime;

                bartender = new Bartender(Content.Load<Texture2D>("bartender"), bartender.position, 0.3f);
                LoadBartenderAttributes();
                bartender.getBack = true;

                shakeButton.isPressed = false;
                timesShaked = 0;

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                glass.Draw(spriteBatch);
                bartender.Draw(spriteBatch);
                spriteBatch.End();
            }

            if(bartender.drinkServed == true && (customerDrinkingTime + customerDrinkingStartTime) < gameTime.TotalGameTime)
            {
                glass = new Glass(Content.Load<Texture2D>("glass"), glassOriginalCoords, 0.2f);
                Objects.Add(glass);

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                glass.Draw(spriteBatch);
                spriteBatch.End();
            }

            if (!customerArrived && customerSideSprite.position.X <= 400)
            {
                customerSideSprite.position.X += 2f;
            }
            else
            {
                customerArrived = true;

                // Remove the customerSideSprite from the Objects list
                Objects.Remove(customerSideSprite);

                // Replace it with the Customersprite
                Objects.Add(Customersprite);

                Customersprite.position = new Vector2(400, 890);
                Customersprite.texture = Content.Load<Texture2D>("customer-back");
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            spriteBatch.Draw(background, new Vector2(screenWidth / 2, screenHeight / 2), null, Color.White, 0f, new Vector2(background.Width / 2, background.Height / 2), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(barShelves, new Vector2(screenWidth / 2, screenHeight - 260), null, Color.White, 0f, new Vector2(barShelves.Width / 2, barShelves.Height), 1f, SpriteEffects.None, 0.1f);
            barTable.Draw(spriteBatch);
            bartender.Draw(spriteBatch);
            hand.Draw(spriteBatch);
            shaker.Draw(spriteBatch);
            glass.Draw(spriteBatch);

            // Draw the customer only when arrived
            if (customerArrived)
            {
                Customersprite.Draw(spriteBatch);
            }
            else
            {
                customerSideSprite.Draw(spriteBatch);
            }

            foreach (Bottle bottle in Bottles)
            {
                bottle.Draw(spriteBatch);
            }

            shakeButton.Draw(spriteBatch);

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
                    if(sprite.GetType() == typeof(Bottle) && bartender.isHoldingShaker == true && bartender.readyToServe == false)
                    {
                        Console.WriteLine("Bottle");
                        target = sprite;
                        bottleOriginalCoords = target.position;
                        bartender.target = target;
                        bartender.takeBottle = true;
                    }
                    if(sprite.GetType() == typeof(Button) && bartender.isHoldingShaker == true && shaker.isEmpty == false)
                    {
                        Console.WriteLine("Shake button");
                        shakeButton.isPressed = true;
                        buttonPressedStartTime = gameTime.TotalGameTime;
                        bartender.startShaking = true;
                        shakingStartTime = gameTime.TotalGameTime;
                    }
                    if(sprite.GetType() == typeof(Glass) && bartender.readyToServe == true)
                    {
                        Console.WriteLine("Glass");
                        target = sprite;
                        glassOriginalCoords = target.position;
                        bartender.target = target;
                        bartender.takeGlass = true;
                    }
                    if (sprite.GetType() == typeof(Table) && bartender.isDrinkFinished == true)
                    {
                        Console.WriteLine("Table");
                        bartender.serveDrink = true;
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