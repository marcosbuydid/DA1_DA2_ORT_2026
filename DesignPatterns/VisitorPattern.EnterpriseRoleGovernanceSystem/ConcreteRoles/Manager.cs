using VisitorPattern.EnterpriseRoleGovernanceSystem.UserRoleInterface;
using VisitorPattern.EnterpriseRoleGovernanceSystem.VisitorInterface;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteRoles
{
    public class Manager : IUserRole
    {
        public int TeamSize { get; }

        public Manager(int teamSize)
        {
            TeamSize = teamSize;
        }
        public void Accept(IRoleVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
