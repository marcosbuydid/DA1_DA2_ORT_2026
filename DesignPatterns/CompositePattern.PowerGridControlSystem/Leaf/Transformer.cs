using CompositePattern.PowerGridControlSystem.Component;

namespace CompositePattern.PowerGridControlSystem.Leaf
{
    public class Transformer : IPowerComponent
    {
        private readonly string _name;
        private readonly double _load;

        public Transformer(string name, double load)
        {
            _name = name;
            _load = load;
        }

        public double GetLoad()
        {
            return _load;
        }

        public void Display(string spacing)
        {
            Console.WriteLine($"{spacing}Transformer {_name} - Load: {_load} MW");
        }
    }
}
