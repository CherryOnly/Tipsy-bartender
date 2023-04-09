using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;

namespace Tipsy_bartender
{
    public class CustomerSprite : Sprite
    {
        //private Customer customer;
        private ContentManager content;

        public Dictionary<string, List<string>> recipes;

        public string CocktailName;
        public List<string> SelectedCocktail { get; }

        public bool served = false;

        public bool status = false;

        public string mood = "No words...";

        //public bool walkingIn = false;
        //public bool walkingOut = false;
        public CustomerSprite(Texture2D texture, Vector2 position, float layer, ContentManager content) : base(texture, position, layer)
        {
            //customer = new Customer();
            this.content = content;

            recipes = new Dictionary<string, List<string>>(){
            { "Screwdriver", new List<string>() { "vodka", "orange-juice" } },
             { "Whiskey and Coke", new List<string>() { "whiskey", "cola" } },
             { "Gin and Tonic", new List<string>() { "gin", "tonic" } },
             { "Brandy and Coke", new List<string>() { "brandy", "cola" } },
             { "Rum and Coke", new List<string>() { "rum", "cola" } },
             { "Cuba Libre", new List<string>() { "rum", "cola", "lime-juice" } },
             { "Vodka and Sprite", new List<string>() { "vodka", "sprite" } },
             { "Whiskey and Sprite", new List<string>() { "whiskey", "sprite" } },
             { "Vodka Grenadine Splash", new List<string>() { "vodka", "sprite", "grenadine" } }};

            var random = new Random();
            var index = random.Next(0, recipes.Count);
            CocktailName = recipes.Keys.ElementAt(index);
            SelectedCocktail = recipes.ElementAt(index).Value;
            Console.WriteLine($"Selected cocktail: {CocktailName}");
        }


        public override void Update(GameTime gameTime)
        {
            if (status == false)
            {
                mood = "Sad";
            }
            else
            {
                mood = "Happy";
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the customer sprite and display the selected cocktail
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height);
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1.2f, SpriteEffects.None, layer);
            //spriteBatch.DrawString(content.Load<SpriteFont>("File"), $"I'll have a {CocktailName} please!", position - new Vector2(100, 550), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);

            if(served == true)
            {
                spriteBatch.DrawString(content.Load<SpriteFont>("File"), $" I'm {mood}", position - new Vector2(100, 550), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
            }
            else
            {
                spriteBatch.DrawString(content.Load<SpriteFont>("File"), $"I'll have a {CocktailName} please!", position - new Vector2(100, 550), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
            }
        }

        
    }
}
