using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipsy_bartender
{
    internal class Recipe
    {
        public class Recipes
        {
            public string Name { get; set; }
            public List<Ingredient> Ingredients { get; set; }

            public Recipes(string name, List<Ingredient> ingredients)
            {
                Name = name;
                Ingredients = ingredients;
            }
        }

    }
}
