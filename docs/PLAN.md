# Completion plan

## Scope and review agreement

Build only the Key Event Logger assignment in this repository. Stage 00 prepares the solution and documentation; feature implementation starts after owner review. Separate branches and PRs preserve the development history. Stage numbers are not GitHub PR numbers.

The target deadline from the supplied course brief is 2026-11-30 at 23:59. This working plan uses Asia/Riyadh; confirm the institutional timezone if different. Dates below describe actual work or targets, not fabricated history.

| Stage | Branch | Deliverable | Current status |
| --- | --- | --- | --- |
| 00 | chore/initialize-project | Scaffold, docs, tooling, Windows CI | Merged in [PR #1](https://github.com/alaamadii/KeyEventLogger/pull/1) on 2026-11-19 |
| 01 | feat/logger-ui | Event-list layout, practice area, disabled controls and nonrecording status | Merged in [PR #2](https://github.com/alaamadii/KeyEventLogger/pull/2) on 2026-11-19 |
| 02 | feat/local-key-events | Application-local key events, timestamps and recording lifecycle | Merged in [PR #3](https://github.com/alaamadii/KeyEventLogger/pull/3) on 2026-11-19 |
| 03 | feat/text-export | User-selected local TXT export, formatting and file errors | Merged in [PR #4](https://github.com/alaamadii/KeyEventLogger/pull/4) on 2026-11-19 |
| 04 | docs/final-submission | Final regression/evidence audit, code guide and submission checklist | Owner merged [PR #5](https://github.com/alaamadii/KeyEventLogger/pull/5); missing human evidence remains pending |
| Quality review | feat/usability-security-review | Redesigned native table, bounded recording, export hardening and precise documentation | Implemented/locally checked; owner review pending |

## Stage 00 — Initialization

- [x] Inspect empty local/remote repository and installed SDK.
- [x] Establish a minimal main base and a dedicated initialization branch.
- [x] Add App/Core/test infrastructure and a compiling nonrecording window.
- [x] Add development commands, VS Code settings, exclusions and CI configuration.
- [x] Document assignment scope, planned architecture and future acceptance.
- [x] Owner merged PR #1; this records stage acceptance, not completion of pending physical/manual checks.

See [TESTING](TESTING.md) for executed results rather than interpreting configuration or a checklist as proof.

## Stage 01 — Usable interface

Create the event list, dedicated practice area, clear status and accessible controls. Recording/export controls must truthfully indicate that their behavior is not implemented until its stage. Establish tab order, resizing/DPI behavior and readable empty/error states. Do not add keyboard recording or export yet.

Acceptance: owner can inspect the UI; keyboard navigation and supported display checks are documented; no control suggests active capture. Include meaningful UI assertions where practical.

Progress on 2026-11-19: resizable native layout, disabled actions, practice editing, noneditable empty events, status/availability notes and accessible navigation are implemented. Nine App tests and the local check pass; actual 125% window captures and scripted lifecycle/focus/restore checks are recorded in TESTING.

- [x] Interface implemented without recording or export.
- [x] Meaningful native tests and local validation recorded.
- [x] Owner merged PR #2. Pending physical checks are not inferred complete.

## Stage 02 — Local event recording

Implement an immutable record and an encapsulated session, then connect local WinForms events. Record key names/types/timestamps rather than reconstructing typed text. Define repeated-key, modifier and event-order policies before implementing them.

Start is explicit. Stop prevents further appends. Deactivation/minimize stops recording; activation alone never resumes. Clearing removes the current records without hidden saving. Keep the UI status and session state consistent.

Acceptance: test state transitions, record ordering, timestamps, stop/clear, held keys and focus transitions. Verify that input in another application is not recorded. No global hooks, background collection or credentials.

Progress on 2026-11-19: immutable records/session, explicit controls, practice-only Down/Up events, count/presentation and activation/minimize/close stopping are implemented. Native repeats are independent events; Tab stays navigation. 38 tests pass (19 Core, 19 App). Scripted native evidence and two actual captures are recorded in TESTING.

- [x] Behavior implemented and locally validated without export.
- [x] Event policies, focus gates, actual captures and limitations documented.
- [x] Owner merged PR #3; physical input/desktop-focus checks remain pending.

## Stage 03 — Local TXT export

Implement a deterministic format and explicit SaveFileDialog destination. Export a stable record snapshot, handle cancellation as a normal action, and report file errors without losing records or crashing. Do not add autosave, transmission or export-on-exit.

Acceptance: assert formatting and Unicode/timestamp consistency, cancellation and representative failures. Use isolated test directories and synthetic input. Keep exports out of Git.

Progress on 2026-11-19: formatter, local staged writer, owned SaveFileDialog, snapshot-before-dialog stop, feedback and retry are implemented. The full local check passes 72 cases (26 Core, 46 App). The real dialog was opened/canceled; save/overwrite automation was not reliable and is not counted as a pass. Real I/O and UI with dialog substitutes are independently verified.

- [x] Export implemented without automatic saving or network collection.
- [x] Format, failure retention, evidence and pending native/manual checks documented.
- [x] Owner merged PR #4. Missing manual/native checks remain tracked in Stage 04.

## Stage 04 — QA and submission

Reconcile documentation with actual behavior. Capture at least two authentic application screenshots, covering recording/status and useful results/export. Verify a clean checkout, physical input/focus, scaling, VS Code F5, normal shutdown and another Windows computer.

Acceptance: all mandatory requirements have evidence; owner signs off manual results, authentic captures and final PR. Publish final work before the deadline. Classroom submission is the owner's action.

Progress on 2026-11-19: reviewed the source/PDF requirement mapping, reran doctor/check (72 passes), verified actual EXE normal close, and prepared the source-linked CODE_GUIDE plus precise manual procedures. The improved bounded external-controller native Save attempt could not reliably discover the dialog and is not passing evidence. Application code/configuration and authentic current images are retained unchanged; no reproducible functional defect was established.

- [x] Baseline regression, source review and evidence classification.
- [x] Documentation walkthrough and explicit owner procedure.
- [x] Published-candidate clean-checkout and public artifact receipts recorded; exact final-head hosted evidence tracked on [PR #5](https://github.com/alaamadii/KeyEventLogger/pull/5).
- [ ] Owner manual results, screenshot approval and second-computer evidence.
- [x] Owner merged PR #5; this does not complete missing manual/course evidence.

## Explicitly deferred

Statistics, date/time filtering, packaging and additional visual themes are not authorized feature stages. Do not implement them automatically.

Each PR must include its purpose, scope, exact validation, documentation updates and limitations. Do not mark a stage accepted merely because CI passes.

## Owner-requested interface/security review — 2026-11-20

After PR #5, the owner requested further interface, code-quality, security and documentation work. This is a review of the same application, without adding optional statistics/filtering or broader input collection.

- [x] Separate timestamp/event/key table, readable native layout, accessible actions and current single-press/repeat explanation.
- [x] 10,000-event stop boundary with no eviction; Clear/restart/exports remain explicit.
- [x] Native 4,096-character entry limit; reserved filename hardening and controlled export errors.
- [x] Official package/audit sources and explicit all-dependency advisory checking.
- [x] 94 local tests (30 Core, 64 App), actual EXE close, controlled-picker real-I/O probe and genuine new captures.
- [x] User/security/code/architecture guides reconciled; historical evidence retained separately.
- [x] Clean-checkout/publication receipt recorded; exact final-head hosted results tracked on [draft PR #6](https://github.com/alaamadii/KeyEventLogger/pull/6).
- [ ] Owner approves the new interface/images and supplies remaining manual/second-computer evidence.

Use a draft review PR while mandatory final acceptance evidence remains missing. No automatic merge or course submission.

### Single-press follow-up — 2026-11-20

The owner requests one row for a short press. App now records only KeyDown; release is ignored. Intentional consecutive presses, modifier presses and native held-key repeats remain. This supersedes the original paired-event App capture policy, with unchanged focus/retention/export boundaries. Current local check: 94 cases (30 Core, 64 App). User/new-image approval remains pending on PR #6.
