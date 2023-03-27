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

        public Bartender(Texture2D texture, Vector2 position, float layer) : base(texture, position, layer)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position.X -= speed;
                if(isFlipped == false)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    isFlipped = true;
                }
                    
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += speed;
                if(isFlipped == true)
                {
                    spriteEffects = SpriteEffects.None;
                    isFlipped = false;
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
    }
}
