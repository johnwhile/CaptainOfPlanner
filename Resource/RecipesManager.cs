using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace CaptainOfPlanner
{
    /// <summary>
    /// IList interface for combobox.datasource compatibility.
    /// </summary>
    public class RecipeCollection : IEnumerable<Recipe> , IList
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
            set => throw new NotImplementedException();
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
        public void Sort(int startindex)=>
            recipes.Sort(startindex, recipes.Count - startindex, new CompareByName());
        
        public bool Contains(Recipe recipe) =>
            dictionary.ContainsKey(recipe.Encoded);

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
            foreach (var recipe in recipes)
                yield return recipe;
        }

        public bool Remove(Recipe item) =>
            dictionary.Remove(item.Encoded) && recipes.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region IList
        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = value as Recipe;
        }

        public int Add(object value)
        {
            if (value is Recipe recipe && TryAdd(recipe)) return recipes.Count - 1;
            return -1;
        }

        public bool Contains(object value) =>
            value is Recipe recipe && Contains(recipe);
        
        public int IndexOf(object value)
        {
            if (value is Recipe recipe) return recipes.IndexOf(recipe);
            return -1;
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly => true;

        public bool IsFixedSize => true;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        #endregion
    }

    public static class RecipesManager
    {
        public static int MaxRecipesFormattedNameLenght;
        public static int max_resource_quantity;

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

            var input_buffer = new List<(Resource,byte)>();
            var output_buffer = new List<(Resource, byte)>();

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

                input_buffer.Clear();
                output_buffer.Clear();

                foreach (XmlNode child in node.ChildNodes)
                {
                    if (ReadResource(child, out Resource resource, out byte quantity))
                    {
                        switch (child.Name)
                        {
                            case "input": input_buffer.Add((resource, quantity)); break;
                            case "output": output_buffer.Add((resource, quantity)); break;
                        }
                    }
                }

                input_buffer.Sort((x, y) => x.Item1.ID.CompareTo(y.Item1.ID));
                output_buffer.Sort((x, y) => x.Item1.ID.CompareTo(y.Item1.ID));

                Recipe recipe = new Recipe(Time, input_buffer, output_buffer);

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
                    foreach (var input in recipe.InputCollection)
                        max_resource_quantity = Math.Max(max_resource_quantity, input.Item2);
                if (recipe.OutPuts != null)
                    foreach (var ouput in recipe.OutputCollection)
                        max_resource_quantity = Math.Max(max_resource_quantity, ouput.Item2);
            }
            return true;
        }
    }
}
