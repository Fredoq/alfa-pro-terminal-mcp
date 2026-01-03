using Fredoqw.Alfa.ProTerminal.Mcp.Host.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Catalog;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Config;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Content;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Identity;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Signal;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

AppSignal signal = new();
AlfaProTerminalProfile profile = new("alfa-pro-terminal-mcp", "Alfa Pro Terminal MCP");
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
await using AlfaProTerminal terminal = new(new Config
                                            (new EnvironmentVariablesPart
                                                    (new JsonFilesPart
                                                        (new BasePathPart
                                                            (new ConfigurationBuilder(), new AppBasePath()))))
                                        .Root());
await using McpSession mcpSession = new(profile, factory, new HooksSet(terminal, factory, new Content()), signal);
await using TerminalSession trmSession = new(terminal, mcpSession, signal);
await using App app = new(signal, trmSession);
await app.Run();
