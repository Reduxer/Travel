using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;

namespace Travel.WebApi.Helpers
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach(var parameter in operation.Parameters)
            {
                var paramDescription = apiDescription.ParameterDescriptions.First(pd => pd.Name == parameter.Name);

                parameter.Description ??= paramDescription.ModelMetadata.Description;

                if(parameter.Schema.Default == null && paramDescription.DefaultValue != null)
                {
                    parameter.Schema.Default = new OpenApiString(paramDescription.DefaultValue.ToString());
                }

                parameter.Required |= paramDescription.IsRequired;
            }
        }
    }
}
