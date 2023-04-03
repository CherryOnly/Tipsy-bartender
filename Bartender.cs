using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Tipsy_bartender
{
    public class Bartender : Sprite
    {
        // Original sprite coords (position)
        public Vector2 origin;

        // Movement speed
        public float speed = 4f;

        // Movement bounds
        public float leftBounds, rightBounds;

        // For flipping sprite
        SpriteEffects spriteEffects = SpriteEffects.None;
        bool isFlipped = false;

        // Bartender walking lines Y coords
        public float walkingLine, servingLine;

        // Shaker/bottle
        public Sprite target = null;

        // Taking shaker
        public bool moveTowardsShaker = false;
        public bool isNearShaker = false;
        public bool pickShaker = false;
        public bool isHoldingShaker = false;

        // Taking bottles
        public bool moveTowardsBottle = false;
        public bool isNearBottle = false;
        public bool pickBottle = false;
        public bool isHoldingBottle = false;
        public bool isPouring = false;
        public bool putBottleBack = false;
        public bool moveBack;


        public Bartender(Texture2D texture, Vector2 position, float layer) : base(texture, position, layer)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // A D movement
            if(Keyboard.GetState().IsKeyDown(Keys.A) && position.X > leftBounds)
            {
                position.X -= speed;
                if(isFlipped == false)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    isFlipped = true;
                }
                    
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) && position.X < rightBounds)
            {
                position.X += speed;
                if(isFlipped == true)
                {
                    spriteEffects = SpriteEffects.None;
                    isFlipped = false;
                }
            }

            if (moveTowardsShaker == true)
                MoveTowardsShaker();

            if (moveTowardsBottle == true)
                MoveTowardsBottle();

            if(pickBottle == true)
                PickBottle();

            if (isHoldingBottle == true)
                isPouring = true;

            if (moveBack == true)
                MoveBack();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // bottom middle of an object
            origin = new Vector2(texture.Width / 2, texture.Height);

            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, spriteEffects, layer);
        }

        public void MoveTowardsShaker()
        {
            // target's x coords line
            float x = target.position.X - (target.texture.Width / 2);

            // target's y coords line
            float y = target.position.Y - target.texture.Height;

            if (position.X + (texture.Width / 2) != x)
            {
                position.X += speed;
            }
            else
            {
                moveTowardsShaker = false;
                isNearShaker = true;
                return;
            }
        }

        public void MoveTowardsBottle()
        {
            Sprite sprite = target;

            // textures bounds (rectangles around textures)
            Rectangle bartenderRect = new Rectangle((int)(position.X - (texture.Width / 2)), (int)(position.Y - texture.Height), texture.Width, texture.Height);
            Rectangle spriteRect = new Rectangle((int)(sprite.position.X - (sprite.texture.Width / 2)), (int)(sprite.position.Y - sprite.texture.Height), sprite.texture.Width, sprite.texture.Height);

            // target's x coords line
            float x = target.position.X - (target.texture.Width / 2);

            // target's y coords line
            float y = target.position.Y - target.texture.Height;

            //if (!bartenderRect.Intersects(spriteRect))
            if(position.X + (texture.Width / 2) != x)
            {
                position.X += speed;

                // Diagonal movement
                //Vector2 direction = Target.position - position;
                //direction.Normalize();
                //position += direction * speed;
            }
            else
            {
                if(position.Y - texture.Height != y)
                {
                    position.Y -= speed;
                }
                else
                {
                    target = null;
                    isNearBottle = true;
                    return;
                }
            }
        }

        public void PickBottle()
        {
            if (position.Y - texture.Height > (target.position.Y - target.texture.Height - (target.texture.Height / 3)))
            {
                position.Y -= speed;
            }
            else
            {
                pickBottle = false;
                isHoldingBottle = true;
                return;
            }
        }

        public void MoveBack()
        {
            if(position.Y != walkingLine)
            {
                position.Y += speed;
            }
            else
            {
                moveBack = false;
                return;
            }
        }
    }
}
