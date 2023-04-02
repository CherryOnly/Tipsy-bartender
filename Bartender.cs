using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipsy_bartender
{
    public class Bartender : Sprite
    {
        public Vector2 origin;
        public float speed = 4f;
        SpriteEffects spriteEffects = SpriteEffects.None;
        bool isFlipped = false;
        public Sprite Target { get; set; }

        Vector2 startingPosition;

        private bool hasInteracted;
        private float pourTimer;
        private float targetInteractionDistance = 10f;

        private Texture2D reachingTexture;




        public Bartender(Texture2D texture, Vector2 position, float layer) : base(texture, position, layer)
        {
            startingPosition = position;
        }

        public override void Update(GameTime gameTime)
        {
           /* if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position.X -= speed;
                if (isFlipped == false)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    isFlipped = true;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += speed;
                if (isFlipped == true)
                {
                    spriteEffects = SpriteEffects.None;
                    isFlipped = false;
                }
            }
            UpdateMovement(gameTime);*/
        }

        private void UpdateMovement(GameTime gameTime)
        {
            if (Target == null)
            {
                MoveToStartingPosition();
                return;
            }

            // Move the bartender towards the target object
            Vector2 direction = Target.position - position;
            direction.Normalize();
            position += direction * speed;

            // When close enough, perform the action (e.g., pour the drink)
            if (Vector2.Distance(position, Target.position) < 50)
            {
                // Perform action, like pouring the drink into the shaker
                Target.position.Y -= 5;
                if (Target.position.Y < 100)
                {
                    Target.position.Y = 200; // Reset the bottle position after pouring
                    Target = null; // Clear the target after the action is completed
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // middle of an object
            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, spriteEffects, layer);
        }

        public void Move(Sprite sprite, GameTime gameTime)
        {
            // textures bounds (rectangles around textures)
            Rectangle bartenderRect;
            Rectangle spriteRect;

            while (true)
            {
                bartenderRect = new Rectangle((int)(position.X - (texture.Width / 2)), (int)(position.Y - (texture.Height / 2)), texture.Width, texture.Height);
                spriteRect = new Rectangle((int)(sprite.position.X - (sprite.texture.Width / 2)), (int)(sprite.position.Y - sprite.texture.Height), sprite.texture.Width, sprite.texture.Height);

                if (bartenderRect.Intersects(spriteRect)) { break; }

                Vector2 direction = sprite.position - position;
                direction.Normalize();
                position += direction * speed;
            }
        }

        public void MoveToTarget(GameTime gameTime)
        {
          
            if (Target == null) return;

            // Move the bartender towards the target object
            Vector2 direction = Target.position - position;
            direction.Normalize();
            position += direction * speed;

            // When close enough, perform the action (e.g., pour the drink)
            if (Vector2.Distance(position, Target.position) < 50)
            {
                // Perform action, like pouring the drink into the shaker
                Target.position.Y -= 5;
                if (Target.position.Y < 100)
                {
                    Target.position.Y = 200; // Reset the bottle position after pouring
                    Target = null; // Clear the target after the action is completed
                }
            }
        }

        public void MoveToStartingPosition()
        {
            Vector2 direction = startingPosition - position;
            float distance = Vector2.Distance(position, startingPosition);
            if (distance > 1)
            {
                direction.Normalize();
                position += direction * speed;
            }
            else
            {
                position = startingPosition;
            }
        }

        public void MoveLeft(int barLeftBorder)
        {
            if (position.X - (texture.Width / 2) > barLeftBorder)
            {
                position.X -= 5;
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public void MoveRight(int barRightBorder)
        {
            if (position.X + (texture.Width / 2) < barRightBorder)
            {
                position.X += 5;
                spriteEffects = SpriteEffects.None;
            }
        }
        public void PourDrink(GameTime gameTime)
        {
        
        }

        public void ShakeDrink(GameTime gameTime)
        {
            
        }



    }
}
