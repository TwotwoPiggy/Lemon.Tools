# System Architecture

## Overview
Lemon Tools (`0.Lemon`) is a multi-project .NET solution that comprises various utility libraries, UI controls, API integrations, and agent logic. It appears to be a desktop-oriented or backend utility toolkit.

## Core Components
- **API Wrappers**: `GptApi`, `OcrApi` (Tesseract based), `HttpManager`, `Win32Api`.
- **AI Agents**: `Twotwo.Agent` (integrates with Gemini and Microsoft/Azure AI).
- **Desktop/System Tools**: `ExcelTools`, `ScreenshotTools`, `AutoCursorTool`, `SQLite.Core`.
- **UI Components**: `Lemon.UI.Controls`, `FormTest`.

## Data Flow
- Clients or scripts interact through the `CommonTools` and `Tools.Core`.
- AI requests are routed through `Twotwo.Agent` to respective backend providers (Gemini/Azure).
- System-level interop happens via `Win32Api`.

## Deployment
Likely deployed as a set of NuGet packages internally or bundled into a desktop application structure, given the presence of `Custom Nuget Packages` in output paths.
