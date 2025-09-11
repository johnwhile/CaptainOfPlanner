
using System;
using System.Runtime.CompilerServices;

namespace CaptainOfPlanner
{
    /// <summary>
    /// max 2 bit
    /// </summary>
    [Flags]
    public enum FlowFlag : byte
    {
        Requested = 1,
        a = 2
    }

    /// <summary>
    /// max 3 bit
    /// </summary>
    public enum ResourceState : byte
    {
        Undefined = 0,
        Fluid = 1,
        Loose = 2,
        Unit = 3,
        Molten = 4,
        Virtual = 5,
        unused_6 = 6,
        unised_7 = 7
    }
    /// <summary>
    /// max 3 bit
    /// </summary>
    public enum ResourceOrigin : byte
    {
        Undefined = 0,
        Natural = 1,
        Semi = 2,
        Crafted = 3,
        Food = 4,
        Petrochemical = 5,
        Power = 6,
        Waste = 7
    }


    public enum ResourceState2 : byte
    {
        Fluid = (byte)'F',
        Loose = (byte)'L',
        Unit = (byte)'U',
        Molten = (byte)'M',
        Virtual = (byte)'V',
        Undefined = (byte)'\0',

    }
    public enum ResourceOrigin2 : byte
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

    /// <summary>
    /// 2byte struct
    /// </summary>
    public struct Resource
    {
        byte info;

        /// <summary>
        /// The unique identifier number.
        /// </summary>
        /// <value>0 = reserved for undefined</value>
        public byte ID { get; internal set; }
        
        
        public FlowFlag Flags 
        {
            get => (FlowFlag)(info >> 6);
            set => info = (byte) ( (((byte)value & 0x3) <<6) | (info & 0x3F));
        }
        /// <summary>
        /// Optional to sort resource by its game's state
        /// </summary>
        public ResourceState State
        {
            get => (ResourceState)(info & 0x7);
            private set => info = (byte)((info & 0xF8) | ((byte)value & 0x7));
        }
        /// <summary>
        /// Optional to sort resource by its game's origin
        /// </summary>
        public ResourceOrigin Origin
        {
            get => (ResourceOrigin)(info >> 3 & 0x7);
            private set => info = (byte)((info & 0xC7) | ((byte)value & 0x7 << 3));
        }
        /// <summary>
        /// Get the resource's name. The resources in <see cref="ResourcesManager"/> must be loaded
        /// </summary>
        public string Name { get; }
        public Resource(byte id, ResourceState state, ResourceOrigin origin) : this()
        {
            ID = id;
            State = state;
            Origin = origin;
            Name = ResourcesManager.TryGetName(ID);
        }
        public static Resource Undefined => new Resource(0, ResourceState.Undefined, ResourceOrigin.Undefined);
        public bool IsUndefined => ID == 0;
        /// <summary>
        /// Compare using id
        /// </summary>
        public bool IsCompatible(Resource other) => ID == other.ID;
        public static int CompareState(Resource x, Resource y) => x.State.CompareTo(y.State);
        public static int CompareOrigin(Resource x, Resource y) => x.Origin.CompareTo(y.Origin);
        public static int CompareName(Resource x, Resource y) => ResourcesManager.TryGetName(x.ID).CompareTo(ResourcesManager.TryGetName(y.ID));
        public override string ToString() => Name;

        public static implicit operator string(Resource resource) => resource.Name;
    } 
}
