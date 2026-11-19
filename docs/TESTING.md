# Testing strategy and evidence

## Current evidence — Stage 02, 2026-11-19

| Check | Actual result |
| --- | --- |
| Final ./scripts/dev.ps1 check | Restore, Release build, test and formatting passed; zero build warnings/errors |
| Core.Tests | 19 passed, 0 failed/skipped |
| App.Tests | 19 passed, 0 failed/skipped; total 38 |
| Actual Release EXE | Title/Stopped status found; normal close exited 0 |
| Actual MainForm diagnostic | Synthetic DEMO Down/Up/character messages create 8 real records; Stop/Clear, clear mid-pair, thread deactivation/return, minimize/restore and active close passed |
| Visual inspection | Default/minimum/wide actual window images at 120 DPI (125%) inspected |
| Captures | Recording and Stopped from source 3494e0a; [provenance](screenshots/README.md) |
| Documentation/source checks | 28 local targets across 11 Markdown files, all capture hashes, output exclusions, Core independence and production API inspection passed |
| Hosted CI | Exact final-head result will be recorded on the recording PR after publication |

Core tests cover values/offsets, invalid names/types, stopped acceptance, repeated Start/Stop, repeated events, backward-clock arrival order, Clear in both states, read-only stable snapshots and invalid input without mutation.

App cases cover stopped editing/no backfill, deterministic timestamp offset, Down/Up/repeats/modifiers, Stop/restart, both Clear states, practice-only focus, deactivation/inactive rejection, minimize/restore, native Space button activation, pumped Tab plus reverse control navigation, repeated cycles, canceled close, fresh Form and four multi-state layout configurations.

All UI tests are serialized STA work and dispose Forms. Synthetic WM_KEYDOWN/UP/CHAR messages are sent only to owned control handles; Tab is posted through the normal message pump. Start/Stop/Clear usually use button PerformClick, with a separate native button-key test. No test manually invokes private record handlers or inserts ListBox rows.

Activation tests use SetActiveWindow for the test thread's own windows, not a desktop foreground change or external-app typing. An initial ActiveForm-dependent fixture failed because transparent background windows were not foreground; activation callbacks plus focus gates and thread-scoped activation resolved this. A pointer-width compile issue in test messages was also corrected before the final full check.

Physical keyboard input, desktop switching to another application, real 100%/150% and cross-monitor transitions, screen-reader/high-contrast acceptance, VS Code F5 and second-computer testing remain **not performed**. TXT export/failures are not implemented. Scripted images/messages do not establish those checks.


## Historical Stage 01 evidence — 2026-11-19

| Check | Actual result |
| --- | --- |
| `./scripts/dev.ps1 test` | Nine native App cases pass; Core has zero cases |
| Final `./scripts/dev.ps1 check` | Restore, Release build, test invocation and formatting passed; zero build warnings/errors |
| App.Tests | 9 passed, 0 failed/skipped; no placeholder assertions |
| Core.Tests | 0 discovered/executed; expected because no domain behavior is implemented |
| Actual Release EXE | Main title and nonrecording status found; normal close exits 0 |
| Actual MainForm diagnostic | Scripted focus-away/return, minimize/restore and closing preserve the empty/nonrecording preview |
| Visual inspection | Actual target-window default/minimum/wide captures inspected at 120 DPI (125%); no clipped instructions or hidden content areas observed |
| Screenshot | One authentic blank UI capture; [provenance and checksum](screenshots/README.md) |
| Documentation/exclusions | 24 local targets across 11 Markdown files, image SHA-256, source boundary inspection and generated/export exclusions passed |
| Hosted checks | Exact final-head hosted results are tracked in [PR #2](https://github.com/alaamadii/KeyEventLogger/pull/2); owner review remains pending |

App tests initialize the same DPI/visual-style defaults as Program, serialize native work, use STA threads, and dispose Forms. They assert disabled/visible future actions and status, native practice editing without collected events, noneditable empty events, tab traversal, four minimum/resized configurations with text/content bounds, and a blank fresh Form. Tests use only synthetic character messages to their own controls; production has no keyboard handlers.

No equivalent-scale simulation is claimed as a physical display check. Actual 100% and 150% displays, monitor transitions, physical keyboard/assistive-technology acceptance, VS Code F5 and a second computer remain **not performed**. Stage 02 recording/focus-stop behavior and Stage 03 export are not implemented or validated by this UI preview.

The historical Stage 00 record below is retained separately.

## Stage 00 test scope

Core currently contains no domain behavior and Core.Tests contains no test cases. The test runner must be exercised, but “no tests available” is not a passing behavioral test suite. Do not add empty facts, template assertions, or feature implementations just to report coverage.

Build/restore/format checks, reference/target inspection and actual executable lifecycle checks provide the appropriate initialization evidence. They do not validate future recording behavior.

## Stage 00 executed evidence — 2026-11-19

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

## Hosted CI and publication

The initialization is published in [PR #1](https://github.com/alaamadii/KeyEventLogger/pull/1) from `chore/initialize-project` to `main`. Implementation/tooling commit `8beaa58` and documentation commit `955dfcc` were pushed and their remote head verified.

Final Stage 00 head 237faed passed [Windows run 44545069787](https://github.com/alaamadii/KeyEventLogger/actions/runs/44545069787), including restore/build/test-runner/format/TRX upload with zero cases. PR #1 was merged by the owner on 2026-11-19. That zero-case result describes initialization, not current App test coverage.
