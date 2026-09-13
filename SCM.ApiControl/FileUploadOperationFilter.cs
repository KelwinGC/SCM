using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SCM.ApiControl
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var isFileUploadOperation = false;

            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                isFileUploadOperation = descriptor.MethodInfo
                    .GetParameters()
                    .Any(p => p.ParameterType == typeof(IFormFile));
            }

            if (!isFileUploadOperation)
                return;

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        "multipart/form-data",
                        new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    {
                                        "archivo",
                                        new OpenApiSchema
                                        {
                                            Type = "string",
                                            Format = "binary"
                                        }
                                    },
                                    {
                                        "usuario",
                                        new OpenApiSchema
                                        {
                                            Type = "string"
                                        }
                                    }
                                },
                                Required = new HashSet<string> { "archivo", "usuario" }
                            }
                        }
                    }
                }
            };

            // Limpiar los parámetros generados automáticamente
            operation.Parameters.Clear();
        }
    }
}