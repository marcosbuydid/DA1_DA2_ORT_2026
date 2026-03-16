using VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteElements;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.Visitor
{
    public interface IRoleVisitor
    {
        void Visit(Administrator administrator);
        void Visit(Manager manager);
        void Visit(Auditor auditor);
    }
}
