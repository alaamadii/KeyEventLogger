# Development workflow and log

Use one dedicated branch per stage. Inspect status, preserve user work, validate, review the diff, make meaningful commits, then publish/open a PR when authorized. The owner approves merging. Stage numbers and actual GitHub PR numbers differ.

## 2026-11-19 — Stage 00 initialization

Branch: chore/initialize-project.

Objective: prepare a professional, documented .NET 10 Windows Forms foundation without recording functionality.

### Preparation and implemented work

- Verified the intended keyEventLoger directory was empty and writable, with no applicable ancestor/local AGENTS.md. The owner explicitly authorized working in this separate directory; FlappyBird is untouched.
- Confirmed the existing alaamadii/KeyEventLogger repository is public and has no remote branches. Verified Git identity/authentication and installed SDK 10.0.402.
- Reviewed PDF pages 1, 2–3 and 5, separating instructor rules from owner/project choices.
- Created minimal bootstrap commit 35e28f7 on main and pushed it as explicitly authorized, then created chore/initialize-project for substantive work.
- Added App/Core/Core.Tests, a single programmatic initialization window, SDK pin, development commands, VS Code configuration, formatting settings and output exclusions.
- Added professional README and guides for requirements, plan, architecture, decisions, setup, testing and submission.
- Planned future local recording/export without implementing handlers, session types, controls, file writes or statistics.
- Added Windows CI and a review template. Removed the need for template/empty test cases; Core intentionally has no behavior yet.
- Preserved the owner-directed student-number omission and supplied author name.

### Validation and delivery

- `./scripts/dev.ps1 doctor` passed with SDK 10.0.402 and WindowsDesktop runtime 10.0.13 on Windows 10.0.22631.
- `./scripts/dev.ps1 check` passed restore, zero-warning/error Release build, test-runner invocation and formatting. Core.Tests discovered zero cases, as expected for a behavior-free scaffold; this is not claimed as a passing unit-test suite.
- The ignored native probe launched the actual Release EXE, inspected its title and exact Label at reported 120 DPI, then requested normal close and observed exit 0. No input was captured.
- JSON, project boundaries, 20 local Markdown links across 10 files, and intended Git ignore behavior passed inspection. Actual TRX counters and the staged diff are checked before publication.
- No application defect was encountered. Future-stage tests, human review, physical display/input, F5 and another-computer checks remain pending.
- Local evidence is detailed in [TESTING](TESTING.md); exact hosted CI/publication results will be linked from the PR. This entry does not imply owner acceptance or an automatic merge.

### Publication

- Pushed implementation/tooling `8beaa58` and documentation/evidence `955dfcc` to `chore/initialize-project`; remote head matched `955dfccb81a7dbbff6ca17137774793ad5af659c`.
- Opened [PR #1](https://github.com/alaamadii/KeyEventLogger/pull/1) against main. This follow-up records the actual publication rather than predicting a PR number.
- Final-head hosted results are tracked on the PR. It remains open for owner review; no merge, visibility change, Classroom submission or Stage 01 implementation was performed.

## 2026-11-19 — Stage 01 application interface

Branch: feat/logger-ui.

- Confirmed owner merge of PR #1 at 0d2909d. Fetched and fast-forwarded main to d347fc0, preserving the owner's README title change, then created the dedicated branch from a clean tree.
- Replaced the placeholder with a title/purpose, explicit nonrecording status, four disabled actions, visible stage availability, ordinary multiline practice TextBox, empty ListBox/count and scope guidance.
- Added native resizable layout, a sensible minimum, accessibility names/mnemonics and usable tab order. Suspended construction layout before the completed initial DPI pass; disposed Form-owned point-unit fonts.
- Added net10.0-windows App.Tests and nine passing cases. Core source and domain test count remain unchanged; no lifecycle, input subscription, model, export or statistics implementation.
- Executed `./scripts/dev.ps1 test`, formatting, then the final `./scripts/dev.ps1 check` after the last code change: restore/build/test/format passed, zero build warnings/errors, nine App passes, zero skips/failures and zero Core cases.
- The actual Release EXE title/status and normal-close exit 0 passed a native probe. A separate actual-MainForm diagnostic exercised focus-away/return and minimize/restore without leaving the nonrecording/empty state.
- Inspected actual default, minimum and wide target-window captures at reported 120 DPI. Committed one blank actual interface capture with source/hash provenance; no practice text, fake records or unrelated desktop content is included.
- Application/test implementation is commit 57d0ecb. Validated 24 local targets across 11 Markdown files, output exclusions, screenshot hash/provenance, production source boundary and final diff; hosted results will be associated with the final PR head.
- Physical keyboard/accessibility review, actual 100%/150% and cross-monitor transitions, VS Code F5, second-computer work and owner acceptance remain pending. No merge or Stage 02 work was performed.

### Stage 01 publication

- Pushed implementation/tests 57d0ecb and documentation/capture d39cbde to feat/logger-ui. Verified remote head d39cbdedcdf509dd8c15d0c082c84baf619a4142.
- Opened [PR #2](https://github.com/alaamadii/KeyEventLogger/pull/2) targeting main. This follow-up records its actual link; exact final-head hosted checks are recorded on the PR.
- Stage 01 remains awaiting owner review. No automatic merge, Stage 02 implementation, repository visibility change or Classroom submission was performed.

## 2026-11-19 — Stage 02 local keyboard events

Branch: feat/local-key-events.

- Confirmed owner merge of PR #2 at 2664bf9, inspected a clean tree, fetched/fast-forwarded main and created the stage branch. Preserved the owner's README title.
- Added validated immutable event values and an encapsulated session with idempotent Start/Stop, count, Clear and protected stable snapshots. Core remains independent of Windows/I/O.
- Connected only practice KeyDown/KeyUp, virtual-key names and App-owned TimeProvider timestamps. Added activation/focus/minimized/closing gates, immediate stopping and explicit restart; practice text is never inspected/backfilled.
- Implemented enabled control states and incremental event rows/count/empty-state updates. Clear retains practice text and recording state. Export remains disabled; no Stage 03 functionality.
- Core tests passed first. Corrected a test-message pointer-width constant. Initial hidden App tests exposed ActiveForm's foreground dependency; ADR-012 records the switch to native activation/focus gates and controlled test-thread activation without desktop focus theft.
- Final ./scripts/dev.ps1 check passed restore/build/test/format: 19 Core + 19 App = 38 cases, no failures/skips, zero build warnings/errors.
- Actual Release EXE launched with Stopped status and closed normally with exit 0. A separate actual-MainForm diagnostic verified synthetic native input, Stop/Clear including a mid-pair clear, deactivation/return, minimize/restore and close while recording.
- Source commit 3494e0a produced the two unedited actual-window captures: synthetic DEMO input generated eight real events through the production handlers/session and system clock. Inspected default/minimum/wide captures at 120 DPI; no human input or desktop focus acceptance is claimed.
- Updated current documentation and kept the Stage 01 screenshot explicitly historical. Physical keyboard/external-app focus, screen-reader/high-contrast, actual 100%/150% transitions, F5 and a second computer remain pending.
- Validated 28 local targets across 11 Markdown files, all current/historical image hashes, output exclusions, Core independence, production API inspection and final whitespace/diff. Publication/hosted results are tracked separately. No merge, visibility change, Classroom action or Stage 03 work.

### Stage 02 publication

- Pushed implementation/tests 3494e0a and documentation/captures 8880eb1 to feat/local-key-events. Remote head matched 8880eb106c3c2a28b2503640d2c06f21dfaa3cad.
- Opened [PR #3](https://github.com/alaamadii/KeyEventLogger/pull/3) targeting main. This follow-up records its actual link; exact final-head CI results are tracked in the PR description.
- Stage 02 remains awaiting owner review. No automatic merge, Stage 03 work, visibility change or Classroom submission was performed.
