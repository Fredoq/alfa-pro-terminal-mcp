using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Logging;

AppSignal signal = new();
AppServerName name = new();
await using AlfaProTerminal terminal = new(new Config(new AppBasePath(),
                                               new EnvironmentName("DOTNET_ENVIRONMENT", "ASPNETCORE_ENVIRONMENT", "Production")).Root());
ILog journal = new Log(LoggerFactory.Create(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace)));
Catalog catalog = new(new ToolSet(terminal, journal, new Content()).Tools());
await new App(signal, new Scope(journal,
                new TerminalSession(terminal,
                    new EndpointSession(new TransportLink(name, journal),
                        new EndpointGate(new OptionsSet(new ServerInfo(name,
                            new ApplicationTitle("Alfa Pro Terminal MCP"),
                            new McpVersion(new ProcessPath())),
                        new CapabilitiesSet(),
                        new HooksSet(catalog,
                        new Calls(catalog))), journal), signal), signal))).Run();
