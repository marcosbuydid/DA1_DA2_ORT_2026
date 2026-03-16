using VisitorPattern.EnterpriseRoleGovernanceSystem.Element;
using VisitorPattern.EnterpriseRoleGovernanceSystem.Visitor;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteElements
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
