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

## 2026-11-19 — Stage 03 local TXT export

Branch: feat/text-export.

- Confirmed owner merge of PR #3 at d9fdd60, fast-forwarded main from a clean tree and created the branch. Preserved README title and identity omission.
- Added a pure Core formatter, App local destination policy and same-directory staged UTF-8 writer. Existing destination is not deleted first; owned temporary cleanup follows failures.
- Added owned SaveFileDialog selection and two focused App boundaries for UI tests. Export stops before snapshot/selection, guards reentry/closing, preserves data and reports results separately from recording state.
- Added bounded accessible/tooltip feedback and retained practice-only focus/lifecycle behavior.
- Corrected an unsupported SaveFileDialog property during the first build, and adjusted the locked-file test to accept the OS's IO/access exception categories without weakening data-retention/cleanup assertions.
- Final ./scripts/dev.ps1 check passed: zero build warnings/errors, 26 Core + 46 App = 72 cases, no failures/skips, formatting clean. Actual Release EXE title/Stopped status and normal close exited 0.
- A scripted real SaveFileDialog opened/canceled successfully. Save/overwrite automation did not complete reliably and was not counted as passing; only the verified temporary helper process was closed. Native/manual save/overwrite checks remain pending.
- A separate actual-MainForm probe with a controlled picker and real exporter verified synthetic DEMO events, actual UTF-8 bytes, success/cancel/read-only failure/retry and data/state retention. Inspected 125% default/minimum/wide captures.
- Source 40845cb produced two new direct window captures. Provenance explicitly identifies the substituted picker and real file writer; no exported TXT or unrelated desktop data is committed.
- Validated 35 local targets across 12 Markdown files, five current/historical image hashes, narrow exclusions, Core/API boundaries and final whitespace/diff. Publication and hosted results are tracked separately. No merge, Stage 04 work, visibility change or Classroom action.

### Stage 03 publication

- Pushed implementation/tests 40845cb and documentation/captures 7977fe8 to feat/text-export; remote matched 7977fe8efc5e52242da130d16721bb90ad35df3b.
- Opened [PR #4](https://github.com/alaamadii/KeyEventLogger/pull/4) targeting main. This follow-up records the actual link; exact final-head hosted results are recorded in its description.
- Native/manual Save/overwrite and owner acceptance remain pending. No automatic merge, Stage 04 work, visibility change or Classroom action.

## 2026-11-19 — Stage 04 final review preparation

Branch: docs/final-submission.

- Confirmed owner merge of PR #4 at 8e01e08, preserved the clean working tree/title, fast-forwarded main and created the final branch.
- Reread the assignment PDF pages 1, 2–3 and 5, source/test responsibilities and evidence. Mapped required features to source/test links; no missing optional feature was implemented.
- Doctor confirmed SDK 10.0.402 / WindowsDesktop 10.0.13 on Windows 10.0.22631. Baseline check passed restore/build/test/format: 26 Core + 46 App, zero failures/skips/build warnings/errors. Actual Release EXE title/Stopped/normal close exit 0 passed.
- Made one improved bounded native-dialog attempt with UI Automation separated into a controller process, exact isolated target validation and owned cleanup. Dialog discovery was still unreliable, so no Save/overwrite pass or human acceptance is claimed.
- No reproducible functional defect was established; application code/configuration/tests remain unchanged and no placeholder tests were added.
- Added a source-linked CODE_GUIDE and precise owner procedures/evidence register. Retained authentic current screenshots because UI/source behavior is unchanged, preserving controlled-picker disclosure and hashes.
- Owner physical keyboard/external-app focus/native Save/display/F5/accessibility, screenshot approval and second-computer evidence remain missing. Final PR must stay draft.
- Validated 80 local targets across 13 Markdown files, five unchanged capture hashes, Core/API boundaries, narrow exclusions and a limited tracked-file credential-pattern scan with no matches. Public metadata remains public.
- Published candidate 9254884ce285517ade1ea947af1defc3ae12c4bf was cloned into a fresh isolated directory: documented doctor/check passed with the same 72 cases, no failures/skips/build warnings/errors and clean formatting. The actual cloned EXE started Stopped and closed normally with exit 0; the clone stayed clean, without needing local configuration or build/run diagnostic helpers. This was the same computer, not a second Windows computer.
- Anonymous HTTP 200 confirmed public metadata (private=false), candidate README/CODE_GUIDE and both current README images. Opened [draft PR #5](https://github.com/alaamadii/KeyEventLogger/pull/5); exact final-head hosted SHA/run/results are recorded in its description separately from the clean-checkout candidate. No automatic merge, visibility/license change or Classroom submission.

## 2026-11-20 — Owner-requested interface and security review

Branch: feat/usability-security-review. Based on owner-merged PR #5 (main 06045f3). Existing local changes were absent; fetched and fast-forwarded main before branching.

- Returned to Key Event Logger at the owner's request; Chat work is not being continued in this review.
- Replaced the long-string ListBox with a timestamp/event/key table and separated layout helpers. Added native accessible controls, readable Segoe UI typography, calmer surfaces and explicit press/release guidance.
- Added a 10,000-event retained-session boundary that stops without evicting records; native practice input is limited to 4,096 characters. Clear remains explicit and does not erase practice text.
- Hardened Windows device-name validation, including superscript aliases/space variants and the drive root. Replaced raw export exception details with actionable stable messages.
- Added explicit official NuGet sources and all-dependency advisory audit policy; no new runtime package or external UI framework.
- Regression includes capacity/clear/restart, real Space/Back editing with separate Down/Up rows, native layouts, reserved aliases and error-detail redaction. An initial new editing test placed its caret at the beginning; corrected the test setup rather than changing native editing.
- Executed full check: 30 Core + 61 App = 91 passed, zero failures/skips/build warnings/errors, formatting verified. The vulnerability query found no known vulnerable packages in configured sources; results are time/source dependent.
- A preliminary actual-MainForm capture probe checked synthetic input, real export bytes, cancel/read-only failure/retry and default/minimum/wide windows with a controlled picker. Final source-linked captures and full documentation reconciliation follow. Human native Save/overwrite, physical display/F5 and a second computer remain pending.

### Current review captures and documentation

- Preliminary source c954743d3dcdd070b698c34c62bc4b9ffddc6c84 produced exploratory target-window images on 2026-11-20, 120 DPI (125%), 1318x947 full window. Native synthetic DEMO input generated 8 actual events; a controlled picker used the real formatter/writer and checked file bytes. Inspected default/minimum/wide and success images; no human native-dialog or monitor-transition result is inferred.
- Reran doctor and the advisory query with the repository's official source; no known vulnerable packages were reported. The actual production EXE title/Stopped/normal-close exit 0 was checked separately.
- Added USER_GUIDE and SECURITY with data inventory, retention/focus/export boundaries, plaintext/permissions/sync/erasure/race limits and precise verification steps. Reconciled README, architecture, code guide, setup, export, plan, submission and current/historical evidence. Retained old captures and checksums as history.
- Publication/current-head CI and new-candidate clean-checkout receipts follow actual execution. No automatic merge, visibility change or Classroom action.

- Final course-sheet reconciliation retained the specified native ListBox family through EventListBox column drawing and complete accessible row text, superseding the preliminary ListView choice. The full 91-case check passed again; an actual-Form probe also passed with the final ListBox. Final screenshots/provenance will identify that source separately.

- Final capture source c39337871269eb391c394db3ee39f71f4aaeb35f: reran the actual-Form controlled-picker probe after committing the final ListBox implementation, then replaced only the new review images. The earlier c954743 exploration is not the published screenshot source. Provenance/hashes identify the final images exactly.

- Static repository review passed: 123 local Markdown targets across 15 files, all seven screenshot hashes, narrow output exclusions, production API/Core boundaries and whitespace. Historical images remain unchanged. No generated output or event exports are included.

### Quality-review publication

- Published code c954743, ListBox compliance c393378 and documentation/captures d3bdfe5 on feat/usability-security-review. Opened [draft PR #6](https://github.com/alaamadii/KeyEventLogger/pull/6) targeting main; no automatic merge.
- Fresh isolated checkout of d3bdfe56748115a8880089be7224084fc2253a5b passed doctor/check: 91 cases, zero failures/skips/build warnings/errors, formatting clean. Its actual EXE title/Stopped/normal close exit 0 passed and the tree stayed clean. Same computer only; no local helper/configuration required to build/run.
- Anonymous public metadata and README/USER_GUIDE/SECURITY/new-image URLs returned 200. Candidate Windows CI run 44683520537 passed. Exact final-head CI SHA/run/results are in PR #6; the later receipt commits change documentation only.

### Final capacity-message regression

- Kept the full-session stop explanation derived from current model state so an export/cancel or focus event cannot hide why Start remains disabled. Added an export-at-capacity regression and min-size full-state layout coverage. Current full check passes 30 Core + 62 App = 92, zero failures/skips/build warnings/errors and clean formatting.
- An intermediate native export fixture lacked a stable active/focused Start setup in one run; it now explicitly activates/focuses the owned Form and asserts native active-window identity before Start. Production capture gates remain unchanged. The full suite passed after this fixture correction.
- The existing c393378 images are retained because the depicted non-full recording/export states and layout are unchanged. Updated clean-checkout/final-head receipts follow actual execution.
