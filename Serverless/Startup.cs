using Budgetly.Application;
using Budgetly.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Budgetly.Serverless;

[assembly: FunctionsStartup(typeof(Startup))]
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        IConfiguration configuration = builder.GetContext().Configuration; 
        
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(configuration);
    }
}