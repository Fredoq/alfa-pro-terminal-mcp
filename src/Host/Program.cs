using Fredoqw.Alfa.ProTerminal.Mcp.Host.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

AppSignal signal = new();
AppServerName name = new();
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
await using AlfaProTerminal terminal = new(new Config
                                            (new EnvironmentVariablesPart
                                                    (new JsonFilesPart
                                                        (new BasePathPart
                                                            (new ConfigurationBuilder(), new AppBasePath()))))
                                        .Root());
await new App
        (signal, new TerminalSession
            (terminal, new EndpointSession
                (name, factory, new OptionsSet
                    (new ServerInfo
                        (name, new ApplicationTitle("Alfa Pro Terminal MCP"), new McpVersion(new ProcessPath())),
                    new CapabilitiesSet(),
                    new HooksSet(terminal, factory, new Content())),
                signal),
            signal))
        .Run();
