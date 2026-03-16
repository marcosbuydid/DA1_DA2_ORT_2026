using VisitorPattern.EnterpriseRoleGovernanceSystem.Element;
using VisitorPattern.EnterpriseRoleGovernanceSystem.Visitor;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteElements
{
    public class Auditor : IUserRole
    {
        public string Department { get; }

        public Auditor(string department)
        {
            Department = department;
        }
        public void Accept(IRoleVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
