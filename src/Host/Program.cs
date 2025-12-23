using Fredoqw.Alfa.ProTerminal.Mcp.Host.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Signal signal = new(new CancellationTokenSource());
ServerName name = new("alfa-pro-terminal-mcp");
await using AlfaProTerminal terminal = new(new Config(new BasePath(),
                                               new EnvironmentName("DOTNET_ENVIRONMENT", "ASPNETCORE_ENVIRONMENT", "Production")).Root());
ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
Catalog catalog = new(new ToolSet(terminal, new Log(factory).Logger(), new Content()).Tools());
await new App(signal, new Scope(factory,
                new TerminalSession(terminal,
                    new EndpointSession(new TransportLink(name, factory),
                        new EndpointGate(new OptionsSet(new ServerInfo(name,
                            new ApplicationTitle("Alfa Pro Terminal MCP"),
                            new McpVersion(new ProcessPath())),
                        new CapabilitiesSet(),
                        new HooksSet(catalog,
                        new Calls(catalog))), factory), signal,
                    new Services(new ServiceCollection().BuildServiceProvider())), signal))).Run();
