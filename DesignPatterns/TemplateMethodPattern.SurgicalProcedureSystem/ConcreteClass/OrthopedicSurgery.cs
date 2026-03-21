
using TemplateMethodPattern.SurgicalProcedureSystem.AbstractClass;
using TemplateMethodPattern.SurgicalProcedureSystem.Models;

namespace TemplateMethodPattern.SurgicalProcedureSystem.ConcreteClass
{
    public class OrthopedicSurgery : SurgeryProcedure
    {
        protected override void Start(Patient patient)
        {
            LogMessage("Repairing bone structure and installing implants.");
        }
    }
}
