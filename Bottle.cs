using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipsy_bartender
{
    internal class Bottle : Sprite
    {
        public Vector2 origin;
        public bool isTaken = false;

        public Bottle(Texture2D texture, Vector2 position, float layer) : base(texture, position, layer)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(isTaken == true)
            {
                position.Y += 150 + texture.Height;
                position.X -= 50;
                isTaken = false;
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // bottom middle of an object
            origin = new Vector2(texture.Width / 2, texture.Height);

            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, layer);
        }
    }
}
