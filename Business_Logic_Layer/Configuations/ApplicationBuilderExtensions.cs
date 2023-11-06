using Microsoft.AspNetCore.Builder;

namespace Business_Logic_Layer.Configuations;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddGlobalErrorHandeling(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<GlobalExceptionHandelingMiddleware>();
}
