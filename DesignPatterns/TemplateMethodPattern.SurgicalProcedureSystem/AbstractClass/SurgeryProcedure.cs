
using TemplateMethodPattern.SurgicalProcedureSystem.Models;

namespace TemplateMethodPattern.SurgicalProcedureSystem.AbstractClass
{
    public abstract class SurgeryProcedure
    {
        //template method
        public void PerformSurgery(Patient patient)
        {
            try
            {
                LogMessage($"Beginning surgery for patient {patient.Id}");

                PreOperationPreparation(patient);
                HandleInduction(patient);
                Start(patient);
                HandlePostOperation(patient);

                if (RequiresICU(patient))
                {
                    TransferToICU(patient);
                }

                MonitorRecovery(patient);

                LogMessage($"Surgery completed for patient {patient.Id}");
            }
            catch (Exception ex)
            {
                HandleComplication(ex, patient);
            }
        }

        protected virtual void PreOperationPreparation(Patient patient)
        {
            LogMessage("Preparing patient and sterilizing equipment.");

            if (patient.IsHighRisk)
            {
                LogMessage("Applying high-risk surgical protocol.");
            }
        }

        protected virtual void HandleInduction(Patient patient)
        {
            LogMessage("Administering anesthesia.");
        }

        protected abstract void Start(Patient patient);

        protected virtual void HandlePostOperation(Patient patient)
        {
            LogMessage("Stabilizing patient after surgery...");
        }

        protected virtual void MonitorRecovery(Patient patient)
        {
            LogMessage("Monitoring patient recovery...");
        }

        //hook method
        protected virtual bool RequiresICU(Patient patient) => false;

        protected virtual void TransferToICU(Patient patient)
        {
            LogMessage("Transferring patient to ICU...");
        }

        protected virtual void HandleComplication(Exception exception, Patient patient)
        {
            LogMessage($"[EMERGENCY] Complication during surgery: {exception.Message}");
        }

        protected void LogMessage(string message)
        {
            Console.WriteLine($"[LOG] {message}");
        }
    }
}
