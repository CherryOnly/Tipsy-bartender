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
    public class CustomerSprite : Sprite
    {
        private Customer customer;
        private ContentManager content;
        public CustomerSprite(Texture2D texture, Vector2 position, float layer, ContentManager content) : base(texture, position, layer)
        {
            customer = new Customer();
            this.content = content;
        }


        public override void Update(GameTime gameTime)
        {
            // Add any update logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the customer sprite and display the selected cocktail
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height);
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1.2f, SpriteEffects.None, layer);
            spriteBatch.DrawString(content.Load<SpriteFont>("File"), $"I'll have a {customer.SelectedCocktail} please!", position - new Vector2(100, 550), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
        }
    }
}
