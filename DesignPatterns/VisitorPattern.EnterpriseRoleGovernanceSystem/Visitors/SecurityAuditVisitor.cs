using VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteRoles;
using VisitorPattern.EnterpriseRoleGovernanceSystem.VisitorInterface;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.Visitors
{
    public class SecurityAuditVisitor : IRoleVisitor
    {
        public void Visit(Administrator administrator)
        {
            Console.WriteLine("Checking privileged access controls for Administrator.");
        }

        public void Visit(Manager manager)
        {
            Console.WriteLine("Reviewing approval privileges for Manager.");
        }

        public void Visit(Auditor auditor)
        {
            Console.WriteLine("Validating audit independence for Auditor.");
        }
    }
}
