using DatingApi.Errors;
using DatingApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingApi.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) {

            services.AddAutoMapper(typeof(MyMapper));
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToArray();

                    var errorResponse = new ApiValidationErrorResponse();
                    errorResponse.Errors = errors;
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
