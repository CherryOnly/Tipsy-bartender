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
    internal class Customer
    {

        private Customer customer;
        private ContentManager content;
        public enum Status { Happy, Sad, Annoyed }
        public Status status;
        public Dictionary<string, List<string>> recipes;
        public string SelectedCocktail { get; }

        // Constructor
        public Customer()
        {
            // Initializes the recipes dictionary with all the available cocktails and their ingredients
            recipes = new Dictionary<string, List<string>>()
        {
            /*{ "Screwdriver", new List<string>() { "Vodka", "Orange Juice" } },
            { "Whiskey and Coke", new List<string>() { "Whiskey", "Coca-Cola" } },
            { "Gin and Tonic", new List<string>() { "Gin", "Tonic Water" } },
            { "Brandy and Coke", new List<string>() { "Brandy", "Coca-Cola" } },
            { "Rum and Coke", new List<string>() { "Rum", "Coca-Cola" } },
            { "Cuba Libre", new List<string>() { "Rum", "Coca-Cola", "Lime Juice" } },*/
            { "Vodka and Sprite", new List<string>() { "Vodka", "Sprite" } },
            { "Whiskey and Sprite", new List<string>() { "Whiskey", "Sprite" } },
            //{ "Vodka Grenadine Splash", new List<string>() { "Vodka", "Sprite", "Grenadine" } }
        };

            // Select a random cocktail for the customer
            var random = new Random();
            var index = random.Next(0, recipes.Count);
            SelectedCocktail = recipes.Keys.ElementAt(index);
            Console.WriteLine($"Selected cocktail: {SelectedCocktail}");
        }

    }

}
