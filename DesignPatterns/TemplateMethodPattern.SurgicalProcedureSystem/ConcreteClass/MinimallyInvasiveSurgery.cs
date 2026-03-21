
using TemplateMethodPattern.SurgicalProcedureSystem.AbstractClass;
using TemplateMethodPattern.SurgicalProcedureSystem.Models;

namespace TemplateMethodPattern.SurgicalProcedureSystem.ConcreteClass
{
    public class MinimallyInvasiveSurgery : SurgeryProcedure
    {
        protected override void Start(Patient patient)
        {
            LogMessage("Performing laparoscopic procedure.");

            //simulate complication
            if (patient.IsHighRisk)
            {
                throw new Exception("Unexpected internal bleeding detected.");
            }
        }

        protected override void HandlePostOperation(Patient patient)
        {
            LogMessage("Applying fast recovery protocol.");
        }

        protected override void MonitorRecovery(Patient patient)
        {
            LogMessage("Short-term monitoring with early discharge.");
        }
    }
}
