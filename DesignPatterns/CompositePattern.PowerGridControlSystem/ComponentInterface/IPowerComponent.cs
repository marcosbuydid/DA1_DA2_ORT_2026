namespace CompositePattern.PowerGridControlSystem.ComponentInterface
{
    public interface IPowerComponent
    {
        double GetLoad();
        void Display(string spacing);
    }
}
