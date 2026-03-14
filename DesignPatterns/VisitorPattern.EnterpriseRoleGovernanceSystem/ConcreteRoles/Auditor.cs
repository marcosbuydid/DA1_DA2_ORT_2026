using VisitorPattern.EnterpriseRoleGovernanceSystem.UserRoleInterface;
using VisitorPattern.EnterpriseRoleGovernanceSystem.VisitorInterface;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteRoles
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
