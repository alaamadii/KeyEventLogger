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

## ADR-011 — Delivered key events and immutable values — 2026-11-19

Context: recording must show names/times without reconstructing TextBox content.
Decision: use immutable validated KeyEventRecord values, KeyDown/KeyUp types, private session storage and read-only copied snapshots. Count delivered events, including repeats/modifiers.
Rationale: preserve explainable arrival semantics and collection ownership without Windows types in Core.
Consequences: count differs from characters/presses; boundaries can leave unmatched events. Clock offsets/order are preserved. Pure local UI display formatting does not define future TXT export.

## ADR-012 — Activation and focused practice gates — 2026-11-19

Context: capture is restricted to the active practice area. Initial invisible UI tests could not use ActiveForm because it queries the desktop foreground window.
Decision: combine WinForms Activated/Deactivate tracking, ContainsFocus, practice focus, session state and minimized/closing/disposed gates. Deactivation/minimize/closing stop; activation never resumes.
Rationale: use the Form's native lifecycle and keep desktop focus out of isolated automation. The [WinForms source](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Form.cs) documents ActiveForm's foreground query and activation callbacks.
Consequences: test-owned native SetActiveWindow calls operate within their STA thread, with real window messages and local input handlers. They do not establish physical foreground switching between external applications; that remains manual. No production P/Invoke, global polling or input hook is added.

## ADR-013 — Explicit controls and clock boundary — 2026-11-19

Context: enabled actions must preserve existing data and remain testable.
Decision: Start retains history/focuses practice, Stop retains history, Clear preserves state/practice text, Export remains disabled. App supplies TimeProvider timestamps; Core only stores supplied values.
Rationale: make collection and time ownership explicit, with deterministic native timestamp tests.
Consequences: current data is memory-only and discarded at process close. Canceled closing leaves recording stopped. Inactive timestamps/rows are never backfilled.

## ADR-014 — Deterministic TSV text — 2026-11-19

Context: exported records must preserve time/offset/order and open in ordinary editors.
Decision: Core formats a short header/count and tab-separated Timestamp/EventType/KeyName rows; invariant milliseconds/offset, CRLF and a final newline. App writes strict UTF-8 without BOM.
Rationale: no runtime/machine metadata or typed-text reconstruction is needed. Core key-name validation already excludes tabs/newlines.
Consequences: empty formatting produces headers with count zero, while UI export requires records. No parser/import or extra package.

## ADR-015 — Local staged commit and path scope — 2026-11-19

Context: failed exports must not truncate existing files and cannot deliver to a network destination.
Decision: validate normal local .txt paths/drive metadata; reject UNC/device/network drives, invalid/reserved names and directory/file reparse redirects. Stage with CreateNew in the selected folder, flush/close, then [File.Move with overwrite](https://learn.microsoft.com/en-us/dotnet/api/system.io.file.move?view=net-10.0).
Rationale: formatting/write errors occur before commit; only an owned temporary is cleaned up.
Consequences: no folder creation or delete-before-replace. Linked/cloud-placeholder destinations may be unsupported. No guarantee against power loss, concurrent path changes or OS-managed synchronization; cleanup errors are surfaced and a temporary can remain if removal fails.

## ADR-016 — Stop-before-dialog and synchronous export — 2026-11-19

Context: owned modal dialogs must not depend on deactivation timing, and tests should isolate selection from disk work.
Decision: stop explicitly, take a stable snapshot and disable actions before calling an owned/disposed picker. Use two small App interfaces for selection/write tests. Keep short local I/O synchronous with a busy guard and bounded separate feedback.
Rationale: preserve simple lifetime/state behavior and avoid background writes after closing. [OverwritePrompt](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog.overwriteprompt?view=windowsdesktop-10.0) provides normal user confirmation.
Consequences: cancel/error/success retain records and remain stopped; Start is always explicit. A closed owner after selection cannot write. Large exports may briefly pause UI. Native save/overwrite manual acceptance remains pending; substituted-dialog tests are labeled separately.
