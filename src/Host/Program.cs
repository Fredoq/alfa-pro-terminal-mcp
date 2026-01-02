using Fredoqw.Alfa.ProTerminal.Mcp.Host.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

AppSignal signal = new();
AppServerName name = new("alfa-pro-terminal-mcp");
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
await using AlfaProTerminal terminal = new(new Config
                                            (new EnvironmentVariablesPart
                                                    (new JsonFilesPart
                                                        (new BasePathPart
                                                            (new ConfigurationBuilder(), new AppBasePath()))))
                                        .Root());
await using McpSession mcpSession = new(name, factory, new OptionsSet
                    (new ServerInfo
                        (name, new ApplicationTitle("Alfa Pro Terminal MCP"), new McpVersion()),
                     new HooksSet(terminal, factory, new Content())),
                signal);
await using TerminalSession trmSession = new(terminal, mcpSession, signal);
await using App app = new(signal, trmSession);
await app.Run();
