using Fredoqw.Alfa.ProTerminal.Mcp.Host.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

AppSignal signal = new();
AppServerName name = new();
await using AlfaProTerminal terminal = new(new Config
                                            (new EnvironmentVariablesPart
                                                    (new JsonFilesPart
                                                        (new BasePathPart
                                                            (new ConfigurationBuilder(), new AppBasePath()))))
                                        .Root());
ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
await new App
        (signal, new Scope
            (factory, new TerminalSession
                (terminal, new EndpointSession
                    (new TransportLink(name, factory), new EndpointGate
                        (new OptionsSet
                            (new ServerInfo
                                (name, new ApplicationTitle("Alfa Pro Terminal MCP"), new McpVersion(new ProcessPath())),
                            new CapabilitiesSet(),
                            new HooksSet(terminal, factory, new Content())),
                        factory),
                    signal),
                signal)))
        .Run();
