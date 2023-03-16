using BloodCenterManagementSystem.Web.Queries;
using GraphQL;

namespace BloodCenterManagementSystem.Web.Schema
{
    public class ProjectSchema: GraphQL.Types.Schema
    {
        public ProjectSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            
        }
    }
}
