using VisitorPattern.EnterpriseRoleGovernanceSystem.Element;
using VisitorPattern.EnterpriseRoleGovernanceSystem.Visitor;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteElements
{
    public class Administrator : IUserRole
    {
        public int ManagedSystems { get; }

        public Administrator(int systems)
        {
            ManagedSystems = systems;
        }

        public void Accept(IRoleVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
