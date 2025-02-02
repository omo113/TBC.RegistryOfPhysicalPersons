using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TBC.ROPP.Api;

public class AddRequiredHeadersOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Ensure parameters list exists
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        // Add the custom "culture" header parameter
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Description = "Culture header for the request (en-US or ka-GE)",
            Required = false,
            Schema = new OpenApiSchema { Type = "string" }
        });
    }
}