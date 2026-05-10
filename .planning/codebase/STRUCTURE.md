# Codebase Structure

## Directory Layout
- **Agent/AI**: `Twotwo.Agent` (core agent logic), `GptApi`.
- **Core Libs**: `Tools.Core`, `SQLite.Core`.
- **General Utilities**: `CommonTools`, `CommonDlls`, `ExcelTools`, `ScreenshotTools`, `AutoCursorTool`, `Win32Api`.
- **UI/Frontend**: `Lemon.UI.Controls`, `CustomControllersLib`.
- **Network**: `HttpManager`, `HttpClient`.
- **Tests**: `Test`, `TestProject`, `FormTest`, `Twotwo.Agent.Tests`, `CommonTools.Tests`.

## Solution File
`Lemon.sln` serves as the root orchestration for MSBuild, defining build configurations and platforms (`x64`, `x86`, `ARM64`, `AnyCPU`).
