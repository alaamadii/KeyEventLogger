# Project working agreement

## Scope and workflow

- Build the YMH219 C#/.NET 10 Windows Forms assignment incrementally.
- Inspect Git status before edits and preserve existing user changes.
- Work on a dedicated stage branch; never put feature work directly on main.
- Use meaningful commits reflecting actual work; do not rewrite history.
- Publish, push, or create remote resources only when the task authorizes it.
- Open a PR for authorized completed work; never merge automatically.
- Finish the current stage and obtain owner review before starting the next.

## Design and recording boundary

- Keep Core independent of WinForms, drawing, timers, controls and file dialogs.
- App owns Windows input, UI, and file selection; keep classes small and explainable.
- Do not implement future-stage features or speculative abstractions.
- Recording is explicit and limited to the application's active window.
- On deactivation stop recording; returning requires Start again.
- Never add global hooks, other-application capture, hidden/background monitoring,
  autostart, password capture, automatic log saving, or event transmission.
- Export is an explicit user-selected local TXT operation.

## Validation and documentation

- Run ./scripts/dev.ps1 check after code/configuration changes.
- Add meaningful assertions for changed behavior; no placeholder tests.
- Report only executed checks; distinguish local, CI and manual evidence.
- Update English documentation with behavior/architecture changes.
- Record decisions in docs/DECISIONS.md and actual dates/work in docs/DEVELOPMENT.md.
- Never fabricate screenshots, identity details, results, commits, or dates.
- Keep bin, obj, .vs, .local, TestResults, personal settings and captured input out of Git.
- Store development exports under ignored exports/ or use *.keyevents.txt.
- Do not publish or request the owner's student number.
