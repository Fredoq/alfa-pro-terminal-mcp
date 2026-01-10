using Fredoqw.Alfa.ProTerminal.Mcp.Host.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Catalog;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Config;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Identity;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Signal;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

AppSignal signal = new();
IConfigurationRoot root = new Config(new EnvironmentVariablesPart(new JsonFilesPart(new BasePathPart(new ConfigurationBuilder(), new AppBasePath())))).Root();
AlfaProTerminalProfile profile = new("alfa-pro-terminal-mcp", "Alfa Pro Terminal MCP");
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConfiguration(root.GetSection("Logging")).AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
await using AlfaProTerminal terminal = new(root);
await using HooksSet hooks = new(terminal, factory);
await using McpSession mcpSession = new(profile, factory, hooks, signal);
await using TerminalSession trmSession = new(terminal, mcpSession, signal);
await using App app = new(signal, trmSession);
await app.Run();
