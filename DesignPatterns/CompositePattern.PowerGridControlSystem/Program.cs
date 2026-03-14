using CompositePattern.PowerGridControlSystem.CompositeComponents;
using CompositePattern.PowerGridControlSystem.LeafComponents;

namespace CompositePattern.PowerGridControlSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Transformer transformer1 = new Transformer("T1", 51);
            Transformer transformer2 = new Transformer("T2", 77);
            Transformer transformer3 = new Transformer("T3", 43);

            GridNode substationA = new GridNode("Substation A");
            substationA.Add(transformer1);
            substationA.Add(transformer2);

            GridNode substationB = new GridNode("Substation B");
            substationB.Add(transformer3);

            GridNode northRegion = new GridNode("North Region");
            northRegion.Add(substationA);

            GridNode eastRegion = new GridNode("East Region");
            eastRegion.Add(substationB);

            northRegion.Display("");

            Console.WriteLine();
            Console.WriteLine($"Total load: {northRegion.GetLoad()} MW");
            Console.WriteLine();

            eastRegion.Display("");

            Console.WriteLine();
            Console.WriteLine($"Total load: {eastRegion.GetLoad()} MW");
        }
    }
}
