# Design decisions

## ADR-001 — .NET 10 Windows Forms, programmatic UI — 2026-11-19

Context: the assignment requires C#/.NET and Windows Forms; the owner uses VS Code.
Decision: target net10.0-windows for App and build controls in code.
Rationale: preserves the required desktop deliverable without Designer dependence.
Consequences: App requires Windows; VS Code tasks/debug settings are maintained explicitly.

## ADR-002 — Independent Core — 2026-11-19

Context: future recording rules should be explainable and testable without native UI.
Decision: Core targets net10.0 with no WinForms, drawing, timers or file dialogs.
Rationale: separates state/values from Windows input and presentation.
Consequences: App normalizes input. Empty Core in initialization is intentional; speculative types are omitted.

## ADR-003 — SDK and test tooling — 2026-11-19

Context: the machine has stable SDK 10.0.402 and the project needs reproducible checks.
Decision: global.json selects 10.0.402 with latestPatch and no prereleases; xUnit v2/test SDK versions are explicit.
Rationale: permit stable servicing within the same SDK feature band while controlling tooling changes.
Consequences: another feature band is not silently selected. Core.Tests initially has zero cases; infrastructure is not coverage, and no placeholder tests are added.

## ADR-004 — Local explicit recording — 2026-11-19

Context: the PDF limits observation to the active application and forbids system monitoring.
Decision: plan local WinForms events only, explicit Start, and Stop on deactivation with explicit restart.
Rationale: make collection visible and controllable; prevent implicit resume.
Consequences: no global hooks, background capture, autostart or credential collection. Future tests must include real focus transitions.

## ADR-005 — User-selected local export — 2026-11-19

Context: the assignment permits local text saving and prohibits event transmission.
Decision: implement explicit TXT export in Stage 03, keeping dialogs/I/O in App.
Rationale: the owner chooses when/where records leave memory.
Consequences: cancellation and failures need clear handling; no autosave/export-on-exit. Logs remain out of Git.

## ADR-006 — Incremental reviewed delivery — 2026-11-19

Context: meaningful history, documentation and explainable development are assessed.
Decision: one stage branch/PR at a time with local checks and Windows CI.
Rationale: keep changes concrete and reviewable.
Consequences: only a minimal base commit is pushed to main when bootstrapping an empty repository; substantive work stays on chore/initialize-project. No automatic merging or next-stage work.

## ADR-007 — Public identity choice — 2026-11-19

Context: the PDF requests name and student number; the owner explicitly excludes the number.
Decision: publish the supplied name Alaa Madi, omit and do not request the student number.
Rationale: follow the owner's publication instruction.
Consequences: record the instructor requirement separately; do not claim compliance with that field or a waiver. See [submission](SUBMISSION.md).

## ADR-008 — Resizable native layout and initial DPI — 2026-11-19

Context: the initialization label is being replaced with instructions, controls and two usable content areas.
Decision: use native controls, nested TableLayoutPanels, a wrapping action panel, automatic text rows and a proportional workspace. Permit resizing with a minimum, retaining PerMonitorV2 and a 96-DPI baseline.
Rationale: keep layout understandable and make available space serve the text rather than absolute coordinates.
Consequences: layout is suspended during construction before one completed initial layout/scaling pass. Two point-unit fonts are Form-owned/disposed. Minimum/resized text bounds are asserted; actual physical DPI transitions still require owner checks.

## ADR-009 — Truthful preview controls — 2026-11-19

Context: UI is Stage 01; recording and export belong to later stages.
Decision: disable all four future actions, show explicit nonrecording/stage notes and use an empty noneditable ListBox. The practice TextBox performs native local editing only.
Rationale: the interface must not imply working capture or manufacture events.
Consequences: no lifecycle/model/keyboard handlers or export I/O are added. Clear is also deferred; no faux status toggles or sample event rows.

## ADR-010 — Native UI assertions — 2026-11-19

Context: Core still has no domain behavior, while the UI now has meaningful contracts.
Decision: add Windows App.Tests referencing App; run cases serially on STA threads with matching DPI/visual-style defaults and dispose windows.
Rationale: test disabled actions, ordinary editing without collection, noneditable events, tab order and unclipped minimum/resized layouts directly.
Consequences: nine App cases cover this stage, while Core stays at zero. Synthetic WM_CHAR messages are directed only to test-owned controls; production has no keyboard handler or hook. Automated assertions do not replace physical or assistive-technology review.
