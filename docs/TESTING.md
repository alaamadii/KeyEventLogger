# Testing strategy and evidence

## Stage 00 test scope

Core currently contains no domain behavior and Core.Tests contains no test cases. The test runner must be exercised, but “no tests available” is not a passing behavioral test suite. Do not add empty facts, template assertions, or feature implementations just to report coverage.

Build/restore/format checks, reference/target inspection and actual executable lifecycle checks provide the appropriate initialization evidence. They do not validate future recording behavior.

## Executed evidence — 2026-11-19

| Check | Actual result |
| --- | --- |
| `./scripts/dev.ps1 doctor` | Passed: SDK 10.0.402, Windows 10.0.22631, WindowsDesktop runtime 10.0.13; Git available |
| `./scripts/dev.ps1 check` | Passed restore, Release build, test-runner invocation and formatting verification; zero build warnings/errors |
| Core.Tests discovery | **0 test cases**; runner reports “No test is available”; TRX counters confirm zero. This is not behavioral coverage |
| Actual Release EXE | Native probe found the titled main window and exact initialization Label; normal close exited 0 |
| Probe window DPI | Reported 120 DPI (125%); this is not physical display-transition or visual acceptance evidence |
| Configuration | JSON files parsed; target frameworks and App → Core / Tests → Core references verified; Core has no package/project/UI dependency |
| Documentation | 20 local Markdown targets across 10 Markdown files resolved |
| Git exclusions | bin/obj/.vs/.local/TestResults, personal settings and designated exports ignored; source/docs/screenshots/shared settings remain eligible |

The ignored native diagnostic helper uses target-window text inspection only, never keyboard capture. It is not a permanent test suite. There were no application build failures; zero test discovery is expected at this stage. Hosted results are recorded separately on the initialization PR once published.

Physical keyboard/focus tests, display transitions at 100%/125%/150%, VS Code F5, human visual acceptance, second-computer testing and screenshots are **not performed**. The future features to which many of those checks apply do not exist yet.

## Planned Core tests — Stage 02

- Start/stop idempotence and appends only while Recording.
- Key name/type/time preservation and stable event ordering.
- Clear semantics and snapshots that callers cannot mutate.
- Documented repeated-key/modifier policies.
- Stop/resume boundaries and invalid inputs without partial mutation.

Use synthetic records and controlled timestamps; never collect test input from other applications.

## Planned native UI checks — Stages 01–02

- Controls, tab order, clear status and usable empty state.
- Local events from the intended practice area.
- Held keys, modifiers and navigation according to the selected policy.
- Deactivation, minimizing and focus on another application stop recording.
- Returning does not resume without Start.
- Buttons/status reflect model state; repeated start/stop/clear remains stable.
- Closing the window terminates normally.
- Actual 100%/125%/150% display scaling and VS Code F5.

Where automated native tests are added, isolate process-wide UI state and use an STA thread. Record scripted checks separately from physical input/visual review.

## Planned export tests — Stage 03

- Deterministic text format and timestamp/Unicode preservation.
- Explicit destination, empty log policy, normal cancellation.
- Inaccessible/locked destination and recoverable file errors.
- In-memory data survives failures; no automatic writes on paint/stop/exit.
- No network transmission; isolated temporary paths only.

## CI and final QA

Windows GitHub Actions runs check on PRs targeting main and pushes to main, then uploads any TRX artifact. Configuration alone does not establish success; associate results with an exact commit/run.

Before submission: clean-clone restore/build/run, authentic screenshots, physical focus/input, supported scaling/debugger checks and another Windows computer with date, Windows/SDK, tested commit and outcomes.

Use [SUBMISSION](SUBMISSION.md) to track owner acceptance. No manual checkbox should be completed from scripted evidence alone.
