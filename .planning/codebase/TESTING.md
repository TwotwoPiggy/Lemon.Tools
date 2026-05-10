# Testing Strategy

## Frameworks
- MSTest or xUnit (inferred from `.Tests` project naming conventions).

## Test Projects
- `Twotwo.Agent.Tests`: Contains unit/integration tests for the AI agent functionalities.
- `CommonTools.Tests`: Tests for general utilities and helper functions.
- `FormTest` / `ReaLTaiizorTest`: UI or WinForms related control testing.
- `ProjectTest`: General solution-level tests.

## Coverage
- Broad integration coverage across distinct domains (UI, Agents, Common Utils).
- Testing involves mock endpoints for AI, physical UI testing for `FormTest`, and basic assertion logic.
