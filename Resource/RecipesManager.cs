using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

namespace CaptainOfPlanner
{
    public class CompareByName : IComparer<Recipe>
    {
        public int Compare(Recipe x, Recipe y) => x.Display.CompareTo(y.Display);
    };

    public static class RecipesManager
    {
        static Dictionary<string, Recipe> Dictionary;
        public static List<Recipe> Recipes;
        public static int MaxRecipesFormattedNameLenght;
        public static int max_resource_count;


        static RecipesManager()
        {
            Recipes = new List<Recipe>();
            Dictionary = new Dictionary<string, Recipe>();
        }

        public static bool TryGetValue(string encoded, out Recipe recipe) =>
            Dictionary.TryGetValue(encoded, out recipe);
        
        /// <summary>
        /// ResourceManager must be load before to check item validity
        /// </summary>
        public static bool Load(string xml = "Recipes.xml")
        {
            Dictionary.Clear();

            if (ResourcesManager.Resources.Count < 1) return false;

            List<ResourceCount> InputsBuffer = new List<ResourceCount>();
            List<ResourceCount> OutputBuffer = new List<ResourceCount>();

            bool ReadResource(XmlNode node, out Resource resource, out byte count)
            {
                resource = Resource.Undefined;
                count = 0;

                string name = node.Attributes["name"]?.Value.TrimStart().TrimEnd();
                if (!int.TryParse(node.Attributes["count"]?.Value, out int count32bit))
                {
                    Console.WriteLine("ERROR reading resource count");
                    return false;
                }

                if (!ResourcesManager.TryGetResource(name, out resource))
                {
                    Console.WriteLine($"ERROR the resource {name} not found in the list");
                    return false;
                }
                if (count32bit < 1 || count32bit > 255)
                {
                    Console.WriteLine($"ERROR reading resource count {count32bit}, it's not in [0,255] range");
                    return false;
                }
                count = (byte)count32bit;
                return true;
            }

            Recipe ReadRecipe(XmlNode node)
            {
                if (node.Name != "recipe") return null;

                if (!int.TryParse(node.Attributes["time"]?.Value, out int Time)) Time = 60;

                InputsBuffer.Clear();
                OutputBuffer.Clear();

                foreach (XmlNode child in node.ChildNodes)
                {
                    var item = new ResourceCount();
                    if (ReadResource(child, out item.Resource, out item.Count))
                    {
                        item.Rate = item.Count * 60f / Time;
                        switch (child.Name)
                        {
                            case "input": InputsBuffer.Add(item); break;
                            case "output": OutputBuffer.Add(item); break;
                        }
                    }
                }

                InputsBuffer.Sort((x, y) => x.Resource.ID.CompareTo(y.Resource.ID));
                OutputBuffer.Sort((x, y) => x.Resource.ID.CompareTo(y.Resource.ID));

                Recipe recipe = new Recipe(Time, InputsBuffer.ToArray(), OutputBuffer.ToArray());
                return recipe;
            }

            var empty = Recipe.Empty;

            Dictionary.Add(empty.Encoded, empty);

            var doc = new XmlDocument();
            doc.Load(xml);

            //Resources must be the only one root node
            if (doc.DocumentElement.Name == "Recipes")
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    Recipe recipe = ReadRecipe(node);
                    if (recipe != null)
                    {
                        if (Dictionary.ContainsKey(recipe.Encoded))
                            throw new ArgumentException("Recipe already exist " + recipe.ToString());
                        Dictionary.Add(recipe.Encoded, recipe);
                    }
                }

            Recipes = Dictionary.Values.ToList();



            Recipes.Sort(1,Recipes.Count-1, new CompareByName());

            for (int id = 0; id < Recipes.Count; id++)
            {
                var recipe = Recipes[id];
                recipe.Id = id;

                MaxRecipesFormattedNameLenght = Math.Max(MaxRecipesFormattedNameLenght, recipe.Display.Length);

                if (recipe.Inputs != null)
                    foreach (var input in recipe.Inputs)
                        max_resource_count = Math.Max(max_resource_count, input.Count);
                if (recipe.OutPuts != null)
                    foreach (var ouput in recipe.OutPuts)
                        max_resource_count = Math.Max(max_resource_count, ouput.Count);
            }
            return true;
        }
    }
}
