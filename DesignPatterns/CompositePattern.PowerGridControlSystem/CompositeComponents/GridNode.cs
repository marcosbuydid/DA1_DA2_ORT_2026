using CompositePattern.PowerGridControlSystem.ComponentInterface;

namespace CompositePattern.PowerGridControlSystem.CompositeComponents
{
    public class GridNode : IPowerComponent
    {
        private readonly string _name;
        private readonly List<IPowerComponent> _childrenComponents = new List<IPowerComponent>();

        public GridNode(string name)
        {
            _name = name;
        }

        public void Add(IPowerComponent component)
        {
            _childrenComponents.Add(component);
        }

        public double GetLoad()
        {
            double totalPower = 0;

            foreach (IPowerComponent component in _childrenComponents)
            {
                totalPower += component.GetLoad();
            }

            return totalPower;
        }

        public void Display(string spacing)
        {
            Console.WriteLine($"{spacing}{_name}");

            foreach (IPowerComponent component in _childrenComponents)
            {
                component.Display(spacing + "   ");
            }
        }
    }
}
