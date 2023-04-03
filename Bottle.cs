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
        public bool isPicked { get; set; }
        public bool move { get; set; }
        public bool inHand { get; set; }

        public Bottle(Texture2D texture, Vector2 position, float layer) : base(texture, position, layer)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(isPicked == true)
            {
                position.Y += 100;
                isPicked = false;
                inHand = true;
            }

            if(move == true && isPicked == false)
            {
                position.Y += 4;
            }

            if(inHand == true)
            {
                //position.Y +=
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
