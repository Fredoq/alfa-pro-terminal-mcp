# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

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

