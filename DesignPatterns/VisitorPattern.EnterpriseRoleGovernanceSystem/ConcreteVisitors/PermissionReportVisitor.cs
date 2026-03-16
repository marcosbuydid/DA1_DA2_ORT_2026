using VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteElements;
using VisitorPattern.EnterpriseRoleGovernanceSystem.Visitor;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteVisitors
{
    public class PermissionReportVisitor : IRoleVisitor
    {
        public void Visit(Administrator administrator)
        {
            Console.WriteLine($"Administrator manages {administrator.ManagedSystems} systems.");
        }

        public void Visit(Manager manager)
        {
            Console.WriteLine($"Manager supervises {manager.TeamSize} employees.");
        }

        public void Visit(Auditor auditor)
        {
            Console.WriteLine($"Auditor reviews department of {auditor.Department}.");
        }
    }
}
