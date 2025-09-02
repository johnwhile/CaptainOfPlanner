using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

namespace CaptainOfPlanner
{
    public class RecipeCollection : IEnumerable<Recipe>
    {
        class CompareByName : IComparer<Recipe>
        {
            public int Compare(Recipe x, Recipe y) => x.Display.CompareTo(y.Display);
        };

        List<Recipe> recipes;
        Dictionary<string, Recipe> dictionary;

        internal RecipeCollection()
        {
            recipes = new List<Recipe>();
            dictionary = new Dictionary<string, Recipe>();
        }

        public Recipe this[int index] 
        { 
            get => recipes[index];
        }

        public int Count => recipes.Count;

        public bool TryGetByEncoded(string encoded, out Recipe recipe) =>
            dictionary.TryGetValue(encoded, out recipe);

        public bool TryAdd(Recipe recipe)
        {
            if (!Contains(recipe))
            {
                recipes.Add(recipe);
                dictionary.Add(recipe.Encoded, recipe);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            recipes.Clear();
            dictionary.Clear();
        }

        /// <summary>
        /// Sort by name
        /// </summary>
        public void Sort(int startindex)
        {
            recipes.Sort(startindex, recipes.Count - startindex, new CompareByName());
        }

        public bool Contains(Recipe recipe) => dictionary.ContainsKey(recipe.Encoded);
        
        /// <summary>
        /// Filter by resource
        /// </summary>
        public IEnumerator<Recipe> GetEnumerator(Resource resource)
        {
            foreach (var recipe in recipes)
                if (recipe.Contains(resource, out _))
                    yield return recipe;
        }
        public IEnumerator<Recipe> GetEnumerator()
        {
            foreach(var recipe in recipes)
                yield return recipe;
        }

        public int IndexOf(Recipe item)=>
            recipes.IndexOf(item);

        public bool Remove(Recipe item)=>
            dictionary.Remove(item.Encoded) && recipes.Remove(item);
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class RecipesManager
    {
        public static int MaxRecipesFormattedNameLenght;
        public static int max_resource_count;

        public static RecipeCollection Recipes;

        static RecipesManager()
        {
            Recipes = new RecipeCollection();
        }


        
        /// <summary>
        /// ResourceManager must be load before to check item validity
        /// </summary>
        public static bool Load(string xml = "Recipes.xml")
        {
            Recipes.Clear();

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

            Recipes.TryAdd(Recipe.Empty);

            var doc = new XmlDocument();
            doc.Load(xml);

            //Resources must be the only one root node
            if (doc.DocumentElement.Name == "Recipes")
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    Recipe recipe = ReadRecipe(node);
                    if (recipe != null)
                        if (!Recipes.TryAdd(recipe))
                            throw new ArgumentException("Recipe already exist " + recipe.ToString());
                    
                }

            Recipes.Sort(1);

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
