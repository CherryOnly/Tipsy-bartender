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
    internal class Shaker : Sprite
    {
        public Vector2 origin;

        public Shaker(Texture2D texture, Vector2 position, float layer) : base(texture, position, layer)
        {
        }

        public override void Update(GameTime gameTime)
        {
            position.X += 100;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // bottom middle of an object
            origin = new Vector2(texture.Width / 2, texture.Height);

            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, layer);
        }

        public bool Clicked(MouseState mouseState)
        {
            return mouseState.X >= position.X - (texture.Width / 2) &&
                   mouseState.X <= position.X + (texture.Width / 2) &&
                   mouseState.Y >= position.Y - texture.Height &&
                   mouseState.Y <= position.Y;
        }
    }
}
