# Testing strategy and evidence

## Current interface/security review — 2026-11-20

Branch: feat/usability-security-review. Based on owner-merged PR #5, main 06045f3. Capture source: c39337871269eb391c394db3ee39f71f4aaeb35f. Current runtime/test SHA and hosted results are recorded on PR #6.

Environment: Windows 10.0.22631 x64, SDK 10.0.402, WindowsDesktop 10.0.13, Git 2.53.0.windows.1.

| Check | Executed result |
| --- | --- |
| ./scripts/dev.ps1 doctor | SDK/runtime/Git available |
| ./scripts/dev.ps1 check | Restore/Release build/test/format pass; 30 Core + 62 App = 92; 0 failures/skips/build warnings/errors |
| Capacity regression | Final event stops without eviction; full Start rejected; Clear releases capacity and requires explicit restart; the full-session explanation survives export cancellation |
| Native Space / Back | One native edit, separate Down/Up rows, correct count; caret explicitly positioned for the test |
| Existing lifecycle/layout regression | Local focus/start/stop/clear/cancel-close gates, table presentation and minimum/resized bounds pass |
| File policy and errors | Reserved superscript/whitespace aliases rejected before metadata access; controlled errors retain data and omit raw details |
| Dependency query | dotnet list KeyEventLogger.slnx package --vulnerable --include-transitive: no known vulnerable packages in official configured source on this date |
| Actual production EXE | Correct title/Stopped state; normal close exit 0 |
| Actual-MainForm probe | Synthetic DEMO creates 8 real records; real export bytes/header/count/no BOM checked; controlled picker success/cancel/read-only failure/retry pass |
| Visual/scripted review | Actual target-window captures inspected at 120 DPI (125%), default/minimum/wide; new README recording/export captures from c393378 |
| Repository/publication | 123 local targets / 15 Markdown files, 7 capture hashes, exclusions/Core boundary and public checks pass; current receipt and exact hosted evidence below / on PR #6 |

The package query is an advisory snapshot, not a vulnerability-free guarantee. Restore uses the repository's official package/audit source and explicit all-dependency audit policy. No runtime dependency was added.

The new editing tests initially assumed an end-positioned caret after setting text; the native control actually placed it at the start. Corrected that test setup and reran the complete check. This was not an application double-edit defect.

The owner reported seeing one Down and one Up for Space/Back in the list. That confirms the described observation only; it is not a complete physical test register. Current automated tests independently verify editing once.

The capture helper uses an actual Form, native synthetic input and a substituted destination picker with the real formatter/writer. It does not prove native Save/overwrite or physical keyboard/focus acceptance. Previous native automation limitations remain historical; no repeated hanging Save attempt was made in this review.

Human native Save/overwrite/error review, physical keyboard/external-app switching, display transitions/F5/high contrast/assistive technology, a second Windows computer and new-image approval remain pending unless supplied with actual evidence. The current quality review does not turn previous merges into those results.

See [security boundary](SECURITY.md), [user guide](USER_GUIDE.md), [capture provenance](screenshots/README.md) and [owner procedure](SUBMISSION.md).



The final capacity-message regression raised the current count to 92 (30 Core, 62 App). One intermediate export test failed its Start setup when native activation/focus was not established; the fixture now explicitly activates/focuses its owned Form and asserts native active-window identity before Start. The complete check passed afterward; production focus gates were not loosened. The min-size full-session layout is covered too. Existing source-c393378 screenshots still match their depicted non-full states.

## Earlier candidate publication receipt — 2026-11-20

[Draft PR #6](https://github.com/alaamadii/KeyEventLogger/pull/6) targets main from feat/usability-security-review.

- Published clean-checkout candidate: d3bdfe56748115a8880089be7224084fc2253a5b; runtime/test/capture source c39337871269eb391c394db3ee39f71f4aaeb35f.
- Fresh isolated clone on the same Windows 10.0.22631 development computer, SDK 10.0.402 / WindowsDesktop 10.0.13: doctor/check pass, 30 Core + 61 App = 91, no failures/skips/build warnings/errors, formatting clean.
- The actual cloned EXE title/Stopped/normal-close exit 0 passed. The clone stayed clean and required no untracked local configuration or diagnostic helper to build/run. A separate observer performed the lifecycle check.
- Anonymous HTTP 200 confirmed public repository metadata, candidate README/USER_GUIDE/SECURITY and both new README images.
- Static review: 123 local Markdown targets across 15 files, seven capture hashes, narrow exclusions, App/Core boundaries, whitespace and a limited tracked-file credential-pattern scan with no matches.
- Candidate hosted run 44683520537 succeeded. The exact final evidence-commit SHA/run/results are recorded on PR #6 separately, avoiding a self-referential receipt commit.
- Later receipt changes are documentation only; the final head's code/configuration is compared with the clean-checkout candidate before delivery.

This is same-computer validation, not the required second Windows computer or physical/native acceptance. PR #6 stays draft for owner evidence.


## Historical Stage 04 review — 2026-11-19

| Check | Executed result |
| --- | --- |
| Baseline | PR #4 owner-merged; main 8e01e08 |
| ./scripts/dev.ps1 doctor | SDK 10.0.402, Windows 10.0.22631, WindowsDesktop 10.0.13, Git available |
| ./scripts/dev.ps1 check | Restore/Release build/test/format pass; 26 Core + 46 App = 72, zero failures/skips/build warnings/errors |
| Actual production EXE | Correct title/Stopped state; normal main-window close exits 0 |
| Native export follow-up | One improved, bounded external-controller attempt failed to discover the owned Save dialog reliably; not a passing Save/overwrite result |
| Code/configuration | No changes; no reproducible application defect established in executed checks |
| Screenshots | Current Stage 03 images retained because behavior/source is unchanged; controlled picker remains disclosed |
| Documentation/repository | 80 local targets across 13 Markdown files, five unchanged capture hashes, Core/API boundaries, narrow exclusions and limited tracked-file credential scan passed |
| Published clean checkout | 9254884ce285517ade1ea947af1defc3ae12c4bf: doctor/check pass; 26 Core + 46 App, zero failures/skips/build warnings/errors, formatting clean; actual cloned EXE title/Stopped and normal close exit 0 |
| Anonymous public artifacts | HTTP 200 without credentials: repo metadata (private=false), candidate README/CODE_GUIDE and both current README images |
| Final PR / hosted CI | [Draft PR #5](https://github.com/alaamadii/KeyEventLogger/pull/5) records the exact final-head SHA, run and confirmed result separately from the clean-checkout candidate |

The published candidate was cloned into a new isolated directory on the same Windows 10.0.22631 development computer, using SDK 10.0.402 / WindowsDesktop 10.0.13. The documented commands restored and built without inherited project output, local configuration or diagnostic helpers. A separate bounded observer launched the actual cloned executable and requested normal close. The clone's working tree remained clean. Later evidence-only documentation commits do not change code/configuration; their exact hosted results belong to PR #5. This is not second-computer, physical-input, native Save or F5 evidence.

Source/PDF audit maps required behavior to code/tests in [REQUIREMENTS](REQUIREMENTS.md). Current checks cover session invariants, native synthetic Down/Up/repeat/modifier delivery, scope/stop gates, Clear/no backfill, cancel/retry/snapshot/closing, exact byte format and local file preservation. Existing real I/O and dialog-substitute tests remain distinct from actual native dialog acceptance.

The revised diagnostic separated UI Automation into another process, validated the exact isolated destination, used deadlines and limited cleanup to its own helper windows/processes. It still could not reliably locate the Save dialog, so no native Save/overwrite result is inferred. No further unmodified hanging retries were made.

Physical keyboard, switching to another desktop application, native Save/overwrite/error review, real 100%/150% and monitor transitions, F5, assistive technology/high contrast, a second Windows computer and screenshot approval have **not been supplied/performed**. The final PR remains draft for those owner checks; automated success does not accept Stage 04.

See [precise owner steps and evidence fields](SUBMISSION.md) and the [source walkthrough](CODE_GUIDE.md). The historical records below are retained separately.

## Historical Stage 03 evidence — 2026-11-19

| Check | Actual result |
| --- | --- |
| Final ./scripts/dev.ps1 check | Restore/build/test/format passed; zero build warnings/errors |
| Core.Tests | 26 passed, 0 failures/skips |
| App.Tests | 46 passed, 0 failures/skips; total 72 |
| Actual Release EXE | Title/Stopped state verified; normal close exited 0 |
| Real native SaveFileDialog | Opened and canceled; recording stayed stopped |
| Native Save/overwrite automation | Attempted but did not complete reliably; not recorded as passing |
| Real MainForm + controlled picker + actual exporter | 8 synthetic events; file bytes checked; success/cancel/read-only failure/retry preserve data and stopped state |
| Visual checks | Actual 120-DPI default/minimum/wide windows inspected |
| Screenshots | Current captures identify source 40845cb and controlled picker; [provenance](screenshots/README.md) |
| Documentation/source checks | 35 local targets across 12 Markdown files, five capture hashes, narrow exclusions and Core/API boundaries passed |
| Hosted CI | Exact final-head hosted outcome is tracked on [PR #4](https://github.com/alaamadii/KeyEventLogger/pull/4); owner review pending |

Formatter assertions cover exact header/count/tabs/CRLF/final newline, milliseconds/offsets, Arabic/Turkish cultures, Unicode, repeats/backward clock order, empty input and nulls. Real file tests verify no-BOM bytes, new/overwrite, missing folders, locks, read-only retry, encoding failure retention/cleanup, invalid snapshots/names, UNC/device paths and substituted network-drive metadata. No real network drive is contacted.

UI tests use picker/exporter substitutes only where specified, with separate real-writer integration. They verify disabled-empty/busy states, stop before selection, cancel/no-write, stable model snapshot, success/failure/retry retention, close during selection and long feedback bounds. Existing serialized STA native recording/layout cases remain.

Initial build/test assumptions were corrected: SaveFileDialog has no singular SupportMultiDottedExtension property, and Windows reports some locked replacement failures as UnauthorizedAccessException rather than IOException. No final test was skipped or replaced by a placeholder.

Physical Save dialog save/overwrite confirmation and error review remain pending, as do physical keyboard/external-app focus, actual 100%/150% monitor transitions, assistive technology/high contrast, F5 and a second computer. GUI automation attempts are not human/manual evidence.


## Historical Stage 02 evidence — 2026-11-19

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
| Hosted CI | Exact final-head hosted evidence is tracked on [PR #3](https://github.com/alaamadii/KeyEventLogger/pull/3); owner review is pending |

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
