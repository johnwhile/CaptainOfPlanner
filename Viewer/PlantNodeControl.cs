
namespace CaptainOfPlanner
{
    public abstract class PlantNodeControl<NODE> : PlantNodeBaseControl where NODE : PlantNode
    {
        public PlantNodeControl(PlantNode plantnode) : base(plantnode)
        {
        }
    }
}
