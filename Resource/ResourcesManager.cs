using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace CaptainOfPlanner
{
    public enum ResourceState : byte
    {
        Fluid = (byte)'F',
        Loose = (byte)'L',
        Unit = (byte)'U',
        Molten = (byte)'M',
        Virtual = (byte)'V',
        Undefined = (byte)'\0',

    }
    public enum ResourceOrigin : byte
    {
        Natural = (byte)'N',
        Semi = (byte)'S',
        Crafted = (byte)'C',
        Food = (byte)'F',
        Petrochemical = (byte)'P',
        Power = (byte)'E',
        Waste = (byte)'W',
        Undefined = (byte)'\0',
    }
    public enum SaveResourceOption
    {
        SortByOrigin,
        SortByState,
        SortByNameThenOrigin,
        SortByNameThenState
    }

    /// <summary>
    /// </summary>
    public struct Resource
    {
        /// <summary>
        /// ID = 0 reserved for undefined. 
        /// </summary>
        public byte ID;
        public ResourceState State;
        public ResourceOrigin Origin;

        /// <summary>
        /// Get the resource's name
        /// </summary>
        public string Name => ResourcesManager.TryGetName(ID);

        public override string ToString() => Name;

        public string ToFormatString(int numchars)
        {
            char[] result = new char[numchars];
            for (int i = 0; i < numchars; i++) result[i] = '\t';

            var name = ResourcesManager.TryGetName(ID);
            for (int i = 0; i < Math.Min(name.Length, numchars); i++) result[i] = name[i];

            return new string(result);
        }



        public static Resource Undefined =>
            new Resource()
            {
                ID = 0,
                State = ResourceState.Undefined,
                Origin = ResourceOrigin.Undefined
            };

        public bool IsCompatible(Resource other) => ID == other.ID;

        public static int CompareState(Resource x, Resource y) => x.State.CompareTo(y.State);
        public static int CompareOrigin(Resource x, Resource y) => x.Origin.CompareTo(y.Origin);
        public static int CompareName(Resource x, Resource y) => ResourcesManager.TryGetName(x.ID).CompareTo(ResourcesManager.TryGetName(y.ID));

    }
    class ComparerByOrigin : IComparer<Resource>
    {
        public int Compare(Resource x, Resource y)
        {
            int result = x.Origin.CompareTo(y.Origin);
            if (result == 0) result = x.State.CompareTo(y.State);
            if (result == 0) result = ResourcesManager.TryGetName(x.ID).CompareTo(ResourcesManager.TryGetName(y.ID));
            return result;
        }
    }
    class ComparerByState : IComparer<Resource>
    {
        public int Compare(Resource x, Resource y)
        {
            int result = x.State.CompareTo(y.State);
            if (result == 0) result = x.Origin.CompareTo(y.Origin);
            if (result == 0) result = ResourcesManager.TryGetName(x.ID).CompareTo(ResourcesManager.TryGetName(y.ID));
            return result;
        }
    }
    class ComparerByNameThenOrigin : IComparer<Resource>
    {
        public int Compare(Resource x, Resource y)
        {
            int result = ResourcesManager.TryGetName(x.ID).CompareTo(ResourcesManager.TryGetName(y.ID));
            //impossible because name must be unique
            if (result == 0) result = x.Origin.CompareTo(y.Origin);
            if (result == 0) result = x.State.CompareTo(y.State);
            return result;
        }
    }
    class ComparerByNameThenState : IComparer<Resource>
    {
        public int Compare(Resource x, Resource y)
        {
            int result = ResourcesManager.TryGetName(x.ID).CompareTo(ResourcesManager.TryGetName(y.ID));
            //impossible because name must be unique
            if (result == 0) result = x.State.CompareTo(y.State);
            if (result == 0) result = x.Origin.CompareTo(y.Origin);
            return result;
        }
    }


    /// <summary>
    /// 184 resources
    /// </summary>
    public static class ResourcesManager
    {
        public static List<string> ResourcesName;
        public static List<Resource> Resources;

        static ResourcesManager()
        {


        }
        public static string TryGetName(byte id) => ResourcesName[id];

        public static bool TryGetResource(byte id, out Resource resource)
        {
            resource = Resource.Undefined;
            if (Resources.Count > id)
            {
                resource = Resources[id];
                return true;
            }
            return false;
        }
        public static bool TryGetResource(string name, out Resource resource)
        {
            resource = Resource.Undefined;
            int index = ResourcesName.IndexOf(name);

            // index 0 reserved for undefined resources
            if (index > 0)
            {
                resource = Resources[index];
                return true;
            }
            return false;
        }

        public static bool Save(string xml = "Resource_out.xml", SaveResourceOption sortby = SaveResourceOption.SortByOrigin)
        {
            var sortedId = Enumerable.Range(0, Resources.Count).Select(i => (byte)i++).ToArray();
            var names = ResourcesName.ToArray();
            var resources = Resources.ToArray();

            //first resources are reserved and must not be sorted
            switch (sortby)
            {
                case SaveResourceOption.SortByOrigin:
                    Array.Sort(resources, sortedId, 1, resources.Length - 1, new ComparerByOrigin()); break;
                case SaveResourceOption.SortByState:
                    Array.Sort(resources, sortedId, 1, resources.Length - 1, new ComparerByState()); break;
                case SaveResourceOption.SortByNameThenOrigin:
                    Array.Sort(resources, sortedId, 1, resources.Length - 1, new ComparerByNameThenOrigin()); break;
                case SaveResourceOption.SortByNameThenState:
                    Array.Sort(resources, sortedId, 1, resources.Length - 1, new ComparerByNameThenState()); break;
                default: throw new ArgumentException();
            }

            var doc = new XmlDocument();
            var root = doc.CreateElement("Resouces");
            doc.AppendChild(root);

            XmlNode PopulateOrigin(ref int r_index)
            {
                if (r_index >= Resources.Count) return null;

                var resource = resources[r_index];
                var prev_origin = resource.Origin;
                var prev_state = resource.State;
                var origin_node = doc.CreateElement(prev_origin.ToString());
                var state_node = doc.CreateElement(prev_state.ToString());

                while (resource.Origin == prev_origin)
                {
                    prev_origin = resource.Origin;

                    if (resource.State != prev_state)
                    {
                        origin_node.AppendChild(state_node);
                        prev_state = resource.State;
                        state_node = doc.CreateElement(prev_state.ToString());
                    }

                    var item = doc.CreateElement("item");
                    item.InnerText = TryGetName(resource.ID);
                    state_node.AppendChild(item);

                    if (++r_index >= Resources.Count) break;
                    resource = resources[r_index];
                }
                origin_node.AppendChild(state_node);
                return origin_node;
            }

            XmlNode PopulateState(ref int r_index)
            {
                if (r_index >= Resources.Count) return null;

                var resource = resources[r_index];
                var prev_origin = resource.Origin;
                var prev_state = resource.State;
                var origin_node = doc.CreateElement(prev_origin.ToString());
                var state_node = doc.CreateElement(prev_state.ToString());

                while (resource.State == prev_state)
                {
                    prev_state = resource.State;

                    if (resource.Origin != prev_origin)
                    {
                        state_node.AppendChild(origin_node);
                        prev_origin = resource.Origin;
                        origin_node = doc.CreateElement(prev_origin.ToString());
                    }

                    var item = doc.CreateElement("item");
                    item.InnerText = TryGetName(resource.ID);
                    origin_node.AppendChild(item);

                    if (++r_index >= Resources.Count) break;
                    resource = resources[r_index];
                }

                state_node.AppendChild(origin_node);
                return state_node;
            }

            int index = 1;
            XmlNode node = null;

            switch (sortby)
            {
                case SaveResourceOption.SortByState:
                case SaveResourceOption.SortByNameThenState:
                    while ((node = PopulateState(ref index)) != null) root.AppendChild(node);
                    break;

                case SaveResourceOption.SortByOrigin:
                case SaveResourceOption.SortByNameThenOrigin:
                    while ((node = PopulateOrigin(ref index)) != null) root.AppendChild(node);
                    break;

            }

            File.Create(xml).Dispose();
            doc.Save(xml);
            return true;
        }
        public static bool Load(string xml = "Resources.xml")
        {
            void ReadNode(XmlNode node, ref byte rec_id, ref ResourceOrigin rec_origin, ref ResourceState rec_state)
            {
                //its a Origin node list
                if (Enum.TryParse(node.Name, out ResourceOrigin try_origin))
                {
                    rec_origin = try_origin;
                    foreach (XmlNode child in node.ChildNodes) ReadNode(child, ref rec_id, ref rec_origin, ref rec_state);
                }
                else if (Enum.TryParse(node.Name, out ResourceState try_state))
                {
                    rec_state = try_state;
                    foreach (XmlNode child in node.ChildNodes) ReadNode(child, ref rec_id, ref rec_origin, ref rec_state);
                }
                else
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        Resources.Add(new Resource { ID = rec_id++, Origin = rec_origin, State = rec_state });
                        ResourcesName.Add(child.InnerText.TrimEnd().TrimStart());
                    }

            }

            try
            {
                Resources = new List<Resource>() { Resource.Undefined };
                ResourcesName = new List<string>() { "undefined" };
                var doc = new XmlDocument();
                doc.Load(xml);

                byte id = 1;
                ResourceOrigin origin = ResourceOrigin.Undefined;
                ResourceState state = ResourceState.Undefined;

                //Resources must be the only one root node
                if (doc.DocumentElement.Name == "Resouces")
                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                        ReadNode(node, ref id, ref origin, ref state);


                Resources.Sort(1, Resources.Count - 1, new ComparerByNameThenOrigin());

                var resource_name_tmp = new List<string>(ResourcesName.Count) { ResourcesName[0] };

                for (byte i = 1; i < ResourcesName.Count; i++)
                {
                    var res = Resources[i];
                    resource_name_tmp.Add(ResourcesName[res.ID]);
                    res.ID = i;
                    Resources[i] = res;
                }
                ResourcesName = resource_name_tmp;

            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Resources.xml", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Resources = new List<Resource>() { Resource.Undefined };
                ResourcesName = new List<string>() { "undefined" };
                return false;
            }
            return true;
        }

    }
}
