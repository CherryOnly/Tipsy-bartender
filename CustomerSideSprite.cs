using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipsy_bartender
{
    public class CustomerSideSprite : Sprite
    {
        private ContentManager content;

        public bool walkingIn = false;
        public bool walkingOut = false;

        SpriteEffects eff = SpriteEffects.None;
        public CustomerSideSprite(Texture2D texture, Vector2 position, float layer, ContentManager content) : base(texture, position, layer)
        {
            this.content = content;
        }


        public override void Update(GameTime gameTime)
        {
            if (walkingIn == true)
                eff = SpriteEffects.FlipHorizontally;
            if (walkingOut == true)
            {
                eff = SpriteEffects.FlipHorizontally;
                //position.X += 4f;
                //WalkOut();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the customer sprite and display the selected cocktail
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height);
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1.2f, eff, layer);
        }

        public void WalkOut()
        {
            if(position.X <= 1000)
            {
                position.X += 4f;
            }
            else
            {
                //
            }
        }
    }
}
