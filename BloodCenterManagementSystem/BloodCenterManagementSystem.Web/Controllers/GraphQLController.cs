using BloodCenterManagmentSystem.Utilities;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace BloodCenterManagementSystem.Web.Controllers
{
    [Route("[controller]")]
    public class GraphQLController: Controller
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _documentExecuter;

        public GraphQLController(ISchema schema,
            IDocumentExecuter executer)
        {
            _schema = schema;
            _documentExecuter = executer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if(query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var inputs = query.Variables.ToInputs();

            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _documentExecuter
                .ExecuteAsync(executionOptions);

            if (result.Errors.Any())
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
