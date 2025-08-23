using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Windows.Forms.AxHost;

namespace CaptainOfPlanner
{


    public static class RecipeManager
    {
        public static List<Recipe> Recipes;

        static RecipeManager()
        {

        }

        /// <summary>
        /// ResourceManager must be load before to check item validity
        /// </summary>
        public static bool Load(string xml = "Recipes.xml")
        {
            if (ResouceManager.Resources.Count < 1) return false;

            var resBufferIn = new Resource[4];
            var resBufferOut = new Resource[4];
            var rateBufferIn = new int[4];
            var rateBufferOut = new int[4];

            bool ReadResource(XmlNode node, out Resource resource, out int rate)
            {
                resource = Resource.Undefined;
                string name = node.Attributes["name"]?.Value.TrimStart().TrimEnd();
                if (!int.TryParse(node.Attributes["rate"]?.Value, out rate)) return false;

                if (!ResouceManager.TryGetResource(name, out resource))
                {
                    Console.WriteLine($"the resource {name} not found in the list");
                    return false;
                }

                return true;
            }

            Recipe ReadRecipe(XmlNode node, ref byte rec_id)
            {
                if (node.Name != "recipe") return null;
                
                Recipe recipe = new Recipe();
                
                int inputcount = 0;
                int outcount = 0;

                foreach (XmlNode child in node.ChildNodes)
                {
                    switch(child.Name)
                    {
                        case "input":
                            if (!ReadResource(child, out resBufferIn[inputcount], out rateBufferIn[inputcount]))
                            {
                                Console.WriteLine("Error reading recipe input");
                                return null;
                            }
                            inputcount++;
                            
                            break;
                        case "output":
                            if (!ReadResource(child, out resBufferOut[outcount], out rateBufferOut[outcount]))
                            {
                                Console.WriteLine("Error reading recipe output");
                                return null;
                            }
                            outcount++;
                            break;
                    }
                }

                int.TryParse(node.Attributes["time"]?.Value, out recipe.Time);
                recipe.Id = rec_id++;
                recipe.ItemIn = new Resource[inputcount];
                recipe.ItemOut = new Resource[outcount];
                recipe.RateIn = new int[inputcount];
                recipe.RateOut = new int[outcount];

                Array.Copy(resBufferIn, recipe.ItemIn, inputcount);
                Array.Copy(resBufferOut, recipe.ItemOut, outcount);
                Array.Copy(rateBufferIn, recipe.RateIn, inputcount);
                Array.Copy(rateBufferOut, recipe.RateOut, outcount);

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
