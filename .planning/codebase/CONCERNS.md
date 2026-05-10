# Codebase Concerns

## Known Issues & Tech Debt
- **Target Framework Fragmentation**: Some projects might target `.net10` while others need to be compatible with `.netstandard2.0` or `.netstandard2.1` (e.g., recent downgrade of `Twotwo.Agent`).
- **Implicit Dependencies**: `IsExternalInit` and other polyfills are manually managed to support newer C# features on older standard frameworks.
- **Cache Locks**: `.csproj` modifications sometimes struggle with `obj` cache locks needing manual `dotnet clean` & deletion.

## Security
- API Keys (e.g., Gemini API keys) are passed through configuration, need to ensure they are not hardcoded or leaked in `Prompts.json` or source control.

## Performance
- Multiple projects generating DLLs might cause longer build times.
- OCR operations and AI requests might need explicit async/await cancellation token handling optimization.
