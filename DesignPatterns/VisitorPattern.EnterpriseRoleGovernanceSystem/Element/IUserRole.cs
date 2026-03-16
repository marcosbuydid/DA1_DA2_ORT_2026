using VisitorPattern.EnterpriseRoleGovernanceSystem.Visitor;

namespace VisitorPattern.EnterpriseRoleGovernanceSystem.Element
{
    public interface IUserRole
    {
        void Accept(IRoleVisitor visitor);
    }
}
