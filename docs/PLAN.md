# Completion plan

## Scope and review agreement

Build only the Key Event Logger assignment in this repository. Stage 00 prepares the solution and documentation; feature implementation starts after owner review. Separate branches and PRs preserve the development history. Stage numbers are not GitHub PR numbers.

The target deadline from the supplied course brief is 2026-11-30 at 23:59. This working plan uses Asia/Riyadh; confirm the institutional timezone if different. Dates below describe actual work or targets, not fabricated history.

| Stage | Branch | Deliverable | Current status |
| --- | --- | --- | --- |
| 00 | chore/initialize-project | Scaffold, docs, tooling, Windows CI | Published in [PR #1](https://github.com/alaamadii/KeyEventLogger/pull/1); owner review pending |
| 01 | feat/logger-ui | Event-list layout, practice area, Start/Stop/Clear/Export controls and status | Planned |
| 02 | feat/local-key-events | Application-local key events, timestamps and recording lifecycle | Planned |
| 03 | feat/text-export | User-selected local TXT export, formatting and file errors | Planned |
| 04 | docs/final-submission | Usability, regression QA, authentic screenshots and final checklist | Planned |

## Stage 00 — Initialization

- [x] Inspect empty local/remote repository and installed SDK.
- [x] Establish a minimal main base and a dedicated initialization branch.
- [x] Add App/Core/test infrastructure and a compiling nonrecording window.
- [x] Add development commands, VS Code settings, exclusions and CI configuration.
- [x] Document assignment scope, planned architecture and future acceptance.
- [ ] Owner accepts the PR after reviewing validation and hosted checks.

See [TESTING](TESTING.md) for executed results rather than interpreting configuration or a checklist as proof.

## Stage 01 — Usable interface

Create the event list, dedicated practice area, clear status and accessible controls. Recording/export controls must truthfully indicate that their behavior is not implemented until its stage. Establish tab order, resizing/DPI behavior and readable empty/error states. Do not add keyboard recording or export yet.

Acceptance: owner can inspect the UI; keyboard navigation and supported display checks are documented; no control suggests active capture. Include meaningful UI assertions where practical.

## Stage 02 — Local event recording

Implement an immutable record and an encapsulated session, then connect local WinForms events. Record key names/types/timestamps rather than reconstructing typed text. Define repeated-key, modifier and event-order policies before implementing them.

Start is explicit. Stop prevents further appends. Deactivation/minimize stops recording; activation alone never resumes. Clearing removes the current records without hidden saving. Keep the UI status and session state consistent.

Acceptance: test state transitions, record ordering, timestamps, stop/clear, held keys and focus transitions. Verify that input in another application is not recorded. No global hooks, background collection or credentials.

## Stage 03 — Local TXT export

Implement a deterministic format and explicit SaveFileDialog destination. Export a stable record snapshot, handle cancellation as a normal action, and report file errors without losing records or crashing. Do not add autosave, transmission or export-on-exit.

Acceptance: assert formatting and Unicode/timestamp consistency, cancellation and representative failures. Use isolated test directories and synthetic input. Keep exports out of Git.

## Stage 04 — QA and submission

Reconcile documentation with actual behavior. Capture at least two authentic application screenshots, covering recording/status and useful results/export. Verify a clean checkout, physical input/focus, scaling, VS Code F5, normal shutdown and another Windows computer.

Acceptance: all mandatory requirements have evidence; owner signs off manual results, authentic captures and final PR. Publish final work before the deadline. Classroom submission is the owner's action.

## Explicitly deferred

Statistics, date/time filtering, packaging and additional visual themes are not authorized feature stages. Do not implement them automatically.

Each PR must include its purpose, scope, exact validation, documentation updates and limitations. Do not mark a stage accepted merely because CI passes.
