using VisitorPattern.EnterpriseRoleGovernanceSystem.VisitorInterface;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.UserRoleInterface
{
    public interface IUserRole
    {
        void Accept(IRoleVisitor visitor);
    }
}
