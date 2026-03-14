using VisitorPattern.EnterpriseRoleGovernanceSystem.ConcreteRoles;
using VisitorPattern.EnterpriseRoleGovernanceSystem.UserRoleInterface;
using VisitorPattern.EnterpriseRoleGovernanceSystem.Visitors;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<IUserRole> userRoles = new List<IUserRole>();
            Administrator administrator = new Administrator(13);
            Manager manager = new Manager(8);
            Auditor auditor = new Auditor("Finance");
            userRoles.Add(administrator);
            userRoles.Add(manager);
            userRoles.Add(auditor);

            PermissionReportVisitor permissionReportVisitor = new PermissionReportVisitor();
            SecurityAuditVisitor securityAuditVisitor = new SecurityAuditVisitor();

            foreach (IUserRole userRole in userRoles)
            {
                userRole.Accept(permissionReportVisitor);
            }

            Console.WriteLine();

            foreach (IUserRole userRole in userRoles)
            {
                userRole.Accept(securityAuditVisitor);
            }
        }
    }
}
