namespace HRMS.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // The domain project currently contains only entities and enums.
        return services;
    }
}
