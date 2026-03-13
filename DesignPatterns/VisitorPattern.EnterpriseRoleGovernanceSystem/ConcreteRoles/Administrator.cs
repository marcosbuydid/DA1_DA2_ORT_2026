using VisitorPattern.EnterpriseRoleGovernanceSystem.UserRoleInterface;
using VisitorPattern.EnterpriseRoleGovernanceSystem.VisitorInterface;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteRoles
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
