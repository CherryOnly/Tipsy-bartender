using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tipsy_bartender.Recipe;

namespace Tipsy_bartender
{
    public class KNearestNeighbor
    {
        private Dictionary<string, List<string>> recipes;

        public int K { get; set; }


        public KNearestNeighbor(int k)
        {
            // Initialize the recipes dictionary with all the available cocktails and their ingredients
            recipes = new Dictionary<string, List<string>>()
        {
        { "Screwdriver", new List<string>() { "Vodka", "Orange Juice" } },
        { "Whiskey and Coke", new List<string>() { "Whiskey", "Coca-Cola" } },
        { "Gin and Tonic", new List<string>() { "Gin", "Tonic Water" } },
        { "Brandy and Coke", new List<string>() { "Brandy", "Coca-Cola" } },
        { "Rum and Coke", new List<string>() { "Rum", "Coca-Cola" } },
        { "Cuba Libre", new List<string>() { "Rum", "Coca-Cola", "Lime Juice" } },
        { "Vodka and Sprite", new List<string>() { "Vodka", "Sprite" } },
        { "Whiskey and Sprite", new List<string>() { "Whiskey", "Sprite" } },
        { "Vodka Grenadine Splash", new List<string>() { "Vodka", "Sprite", "Grenadine" } }
        };


            // Define the difficulty ratings for each recipe
            Dictionary<string, int> difficulty = new Dictionary<string, int>()
            {
                { "Screwdriver", 2 },
                { "Whiskey and Coke", 3 },
                { "Gin and Tonic", 4 },
                { "Brandy and Coke", 2 },
                { "Rum and Coke", 2 },
                { "Cuba Libre", 5 },
                { "Vodka and Sprite", 1 },
                { "Whiskey and Sprite", 1 },
                { "Vodka Grenadine Splash", 6 }
            };

            int rating = difficulty["Screwdriver"];

            K = k;

        }

        public bool CheckMatch(List<string> list1, List<string> list2)
        {
           
            // Calculate the similarity between the two lists
            int matches = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] == list2[i])
                {
                    matches++;
                }
            }

            // Check if the number of matching elements is greater than or equal to k
            return matches >= K;
        }

        public double CalculateAccuracy(List<string> order, List<string> served)
        {
            double accuracy = 0.0;

            // Calculate the similarity between the test drink and all the training drinks
            Dictionary<string, double> similarity = new Dictionary<string, double>();
            for (int i = 0; i < served.Count; i++)
            {
                int matchingIngredients = 0;
                for (int j = 0; j < order.Count; j++)
                {
                    if (served[i].Contains(order[j]))
                    {
                        matchingIngredients++;
                    }
                }
                double drinkSimilarity = (double)matchingIngredients / served[i].Count();
                similarity.Add(served[i], drinkSimilarity);
            }

            // Sort the training drinks in descending order of similarity to the test drink
            var sorted = similarity.OrderByDescending(x => x.Value);

            // Select the k-nearest training drinks
            int count = 0;
            List<string> kNearest = new List<string>();
            foreach (var item in sorted)
            {
                kNearest.Add(item.Key);
                count++;
                if (count >= K)
                {
                    break;
                }
            }

            // Calculate the accuracy by averaging the accuracies of the k-nearest drinks
            foreach (var item in kNearest)
            {
                int matchingIngredients = 0;
                for (int i = 0; i < order.Count; i++)
                {
                    if (item.Contains(order[i]))
                    {
                        matchingIngredients++;
                    }
                }
                double drinkAccuracy = (double)matchingIngredients / item.Count();
                accuracy += drinkAccuracy;
            }
            accuracy /= kNearest.Count;

            // Convert the accuracy to a percentage
            accuracy *= 100.0;

            return accuracy;
        }


    }
}
