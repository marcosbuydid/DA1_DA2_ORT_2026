using TemplateMethodPattern.SurgicalProcedureSystem.AbstractClass;
using TemplateMethodPattern.SurgicalProcedureSystem.ConcreteClass;
using TemplateMethodPattern.SurgicalProcedureSystem.Models;

namespace TemplateMethodPattern.SurgicalProcedureSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Patient patientA = new Patient("54323406", 45, true);
            Patient patientB = new Patient("27892572", 30, false);
            Patient patientC = new Patient("97652375", 74, true);

            SurgeryProcedure surgeryProcedure;

            Console.WriteLine("=== Cardiac Surgery ===");
            surgeryProcedure = new CardiacSurgery();
            surgeryProcedure.PerformSurgery(patientA);

            Console.WriteLine("\n=== Orthopedic Surgery ===");
            surgeryProcedure = new OrthopedicSurgery();
            surgeryProcedure.PerformSurgery(patientB);

            Console.WriteLine("\n=== Minimally Invasive Surgery ===");
            surgeryProcedure = new MinimallyInvasiveSurgery();
            surgeryProcedure.PerformSurgery(patientC);
        }
    }

}
