
using TemplateMethodPattern.SurgicalProcedureSystem.AbstractClass;
using TemplateMethodPattern.SurgicalProcedureSystem.Models;

namespace TemplateMethodPattern.SurgicalProcedureSystem.ConcreteClass
{
    public class CardiacSurgery : SurgeryProcedure
    {
        protected override void Start(Patient patient)
        {
            LogMessage("Performing open-heart surgery with cardiopulmonary bypass.");
        }

        protected override bool RequiresICU(Patient patient) => true;

        protected override void MonitorRecovery(Patient patient)
        {
            LogMessage("Monitoring ICU recovery with continuous cardiac supervision.");
        }
    }
}
