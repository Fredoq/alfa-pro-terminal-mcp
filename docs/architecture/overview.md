# Architecture Overview

```mermaid
flowchart LR
    Client([MCP Client])
    Host[Console MCP Host (stdio)]
    Domain[Domain Services]
    Client --> Host --> Domain
    Domain --> Host --> Client
```
