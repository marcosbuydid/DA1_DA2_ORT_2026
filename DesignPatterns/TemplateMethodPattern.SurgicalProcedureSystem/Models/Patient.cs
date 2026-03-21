namespace TemplateMethodPattern.SurgicalProcedureSystem.Models
{
    public class Patient
    {
        public string Id { get; set; }
        public int Age { get; set; }
        public bool IsHighRisk { get; set; }

        public Patient(string id, int age, bool isHighRisk)
        {
            Id = id;
            Age = age;
            IsHighRisk = isHighRisk;
        }
    }
}
