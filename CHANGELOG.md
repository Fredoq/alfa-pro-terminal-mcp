# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2026-01-09
### Added
- Root entries composition for schema-based output mapping
- Tool-level tests for accounts, assets, positions, archive, and balance tools
- Schema match helper for infrastructure tests
### Changed
- Entries, filters, and schema rules for account, asset, position, and archive responses
- MCP tools and terminal clients to align with updated entries and schema rules
- Host composition, hooks, and configuration parts for simpler app wiring
### Removed
- Description layer and its tests for accounts, positions, archive, and asset info
- Legacy app services/content abstractions

## [0.8.0] - 2026-01-02
### Added
- Explicit MCP tool catalog with per-tool schemas, annotations, and structured responses
- JSON value wrappers and generic entries/filters with schema-based output mapping for accounts, assets, positions, and archive payloads
- Application composition primitives (signals, sessions, configuration parts) and expanded schema/description tests
### Changed
- Host bootstrap now uses explicit app sessions and tool catalog instead of Host builder discovery
- Terminal query processing now maps payloads through schema/entries pipelines with fallback handling for archive payloads
### Removed
- Attribute-based MCP tool wrappers and legacy JsonElement extensions and entries interfaces in favor of unified entries

## [0.7.0] - 2025-12-16
### Added
- Terminal endpoint and timeout configuration
- Additional test coverage for message flow and outbox
### Changed
- Incoming message processing and logging structure
### Removed
- Outbound pipeline and private pump implementation

## [0.6.0] - 2025-12-08
### Changed
- Improved MCP method metadata with clearer names and descriptions

## [0.5.0] - 2025-12-03
### Added
- MCP tool for fetching instrument information by ticker

## [0.4.0] - 2025-12-02
### Added
- MCP tool `History` for archive candles with parameter hints
- Archive query transport and parsing for OHLCV and MPV responses with field descriptions
- Tests covering archive payload serialization, routing, parsing, and WsArchive integration

## [0.3.0] - 2025-11-30
### Added
- Nuget publish and dnx support for mcp
### Removed
- Docker publish

## [0.2.0] - 2025-11-30
### Added
- MCP tool for fetching balance
- MCP tool for fetching position for specific account
- MCP tool for fetching asset info

## [0.1.0] - 2025-11-29
### Added
- MCP stdio host wired to Alfa PRO Terminal router with configurable endpoint
- MCP tool for fetching client accounts list
- CI pipeline and container publish workflow
