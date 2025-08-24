using System;
using System.Collections.Generic;
using System.Xml;

namespace CaptainOfPlanner
{
    public static class RecipesManager
    {
        public static List<Recipe> Recipes;

        static RecipesManager()
        {

        }

        /// <summary>
        /// ResourceManager must be load before to check item validity
        /// </summary>
        public static bool Load(string xml = "Recipes.xml")
        {
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
                if (count32bit<1 || count32bit>255)
                {
                    Console.WriteLine($"ERROR reading resource count {count32bit}, it's not in [0,255] range");
                    return false;
                }
                count = (byte)count32bit;
                return true;
            }

            Recipe ReadRecipe(XmlNode node, ref byte rec_id)
            {
                if (node.Name != "recipe") return null;
                
                Recipe recipe = new Recipe();
                if (!int.TryParse(node.Attributes["time"]?.Value, out recipe.Time)) recipe.Time = 60;

                InputsBuffer.Clear();
                OutputBuffer.Clear();

                foreach (XmlNode child in node.ChildNodes)
                {
                    var item = new ResourceCount();
                    if (ReadResource(child, out item.Resource, out item.Count))
                    {
                        item.Rate = item.Count * 60f / recipe.Time;
                        switch (child.Name)
                        {
                            case "input": InputsBuffer.Add(item); break;
                            case "output": OutputBuffer.Add(item); break;
                        }
                    }
                }
                recipe.Inputs = InputsBuffer.ToArray();
                recipe.OutPuts = OutputBuffer.ToArray();
                return recipe;
            }

            Recipes = new List<Recipe>();

            var doc = new XmlDocument();
            doc.Load(xml);

            byte id = 0;
            //Resources must be the only one root node
            if (doc.DocumentElement.Name == "Recipes")
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    Recipe recipe = ReadRecipe(node, ref id);
                    if (recipe != null) Recipes.Add(recipe);
                }

            return true;
        }
    }
}
