namespace CompositePattern.PowerGridControlSystem.Component
{
    public interface IPowerComponent
    {
        double GetLoad();
        void Display(string spacing);
    }
}
