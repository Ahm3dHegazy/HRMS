namespace HRMS.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(
                ApiResponseFactory.ModelStateFailure(context.ModelState));
        });

        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddAutoMapper(configuration =>
            configuration.AddMaps(applicationAssembly));

        return services;
    }

    public static WebApplication MapApiDocumentation(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference();

        return app;
    }
}
