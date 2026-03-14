using VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteRoles;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.VisitorInterface
{
    public interface IRoleVisitor
    {
        void Visit(Administrator administrator);
        void Visit(Manager manager);
        void Visit(Auditor auditor);
    }
}
