# Developer Conventions

## Language
- **C#**: Features up to C# 10 or higher are used, relying on `<LangVersion>10.0</LangVersion>` in older .NET Standard projects.
- **Implicit Usings**: Enabled where possible (`<ImplicitUsings>enable</ImplicitUsings>`).
- **Nullable Reference Types**: Enabled (`<Nullable>enable</Nullable>`).

## Structuring
- Each major functionality is isolated in its own `.csproj` library (e.g., `ExcelTools` separate from `ScreenshotTools`).
- Shared/Common logic goes to `CommonTools` or `Tools.Core`.

## Naming
- PascalCase for namespaces and public entities.
- Private fields prefixed with `_`.
- Async methods suffixed with `Async`.
