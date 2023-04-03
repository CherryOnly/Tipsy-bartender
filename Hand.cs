using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipsy_bartender
{
    internal class Hand : Sprite
    {
        public Vector2 origin;
        public bool move = false;

        public Hand(Texture2D texture, Vector2 position, float layer) : base(texture, position, layer)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(move == true)
            {
                //
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // top middle of an object
            origin = new Vector2(texture.Width / 2, 0);

            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, layer);
        }
    }
}
