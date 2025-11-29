using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Extensions;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();
builder.Logging
    .AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);
builder.Services
    .Register(builder.Configuration)
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();
IHost host = builder.Build();
await host.RunAsync();
