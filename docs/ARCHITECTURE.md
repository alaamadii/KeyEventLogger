# Architecture

## Projects and responsibility

App targets net10.0-windows and owns the STA entry point, PerMonitorV2 DPI, controls, local input, clock and file selection/I/O. Core targets net10.0 and owns immutable event values, bounded recording state and pure TXT formatting.

References: App -> Core, Core.Tests -> Core, App.Tests -> App. Core has no WinForms, drawing, Windows API, timer, clock, file-dialog or third-party dependency. App/Core have no third-party packages.

## Values, retention and snapshots

[KeyEventRecord](../src/KeyEventLogger.Core/KeyEventRecord.cs) validates key name/type and stores get-only values with the supplied DateTimeOffset. It does not know Windows Keys or a clock.

[RecordingSession](../src/KeyEventLogger.Core/RecordingSession.cs) owns a private list and positive capacity, default 10,000. Count derives from the list. Start is idempotent while room exists; it refuses a full session. TryRecord validates, rejects stopped appends, accepts the last available event and stops exactly at capacity. No eviction, sorting or pairing occurs.

Clear removes records and preserves state. A full stopped session stays stopped after Clear until Start. GetSnapshot copies to an array and wraps it read-only; immutable elements and the separate array keep earlier snapshots stable. This is a UI-thread-owned model, not a concurrent collection.

## Form organization and native presentation

MainForm is one object split across:

- [MainForm.cs](../src/KeyEventLogger.App/Forms/MainForm.cs): constructor/composition and resource disposal.
- [MainForm.Layout.cs](../src/KeyEventLogger.App/Forms/MainForm.Layout.cs): control construction, cards, actions and event columns.
- [MainForm.Recording.cs](../src/KeyEventLogger.App/Forms/MainForm.Recording.cs): local event subscriptions, focus/state gates and presentation updates.
- [MainForm.Export.cs](../src/KeyEventLogger.App/Forms/MainForm.Export.cs): explicit stop/snapshot/picker/write flow.
- [UiTheme](../src/KeyEventLogger.App/Forms/UiTheme.cs): presentation colors with high-contrast system-color fallback.

The root TableLayoutPanel has automatic header/status/action/instruction/footer rows and a proportional workspace. Flat native panels divide practice (38%) from records (62%). An [EventListBox](../src/KeyEventLogger.App/Forms/EventListBox.cs), derived from the required native ListBox, draws local timestamp/offset, captured KeyDown and virtual-key columns. Each private immutable display row wraps its model record and supplies complete text through ToString for native accessibility. Export never reads those rows.

One accepted event adds one table item and scrolls into view. It does not rebuild or snapshot the complete session per key. Alternating row colors and separate header labels aid reading; state is also explicit text, never color alone.

## Input and lifecycle

Only practiceText.KeyDown is subscribed; release is not a recording source. RecordEvent requires a recording session, native activation state, Form focus, practice focus and a live nonminimized/nonclosing window. App maps e.KeyCode.ToString() and supplies TimeProvider.GetLocalNow(). The default provider is System; test providers fix timestamps.

Recording/export never reads practiceText.Text. Native editing remains native; MaxLength bounds ordinary user entry/paste to 4,096 characters. Each delivered KeyDown is retained, including intentional consecutive presses and native held-key repeats; KeyUp is ignored. Tab stays navigation and no key/time deduplication is applied. Count means events, not edits.

Start retains history and focuses practice. Stop retains records. Clear clears model/rows/count while preserving practice text/state. At capacity, UI derives a specific stopped reason from the model, disables Start and keeps Clear/Export available. Export/cancellation/deactivation do not hide the full-session explanation.

Deactivate/minimize stop; Activated/restore never start. Within-Form focus moves do not stop the session, but only focused-practice events are accepted. Closing stops before shutdown; canceled close permits an explicit new Start. Disposal stops and detaches subscriptions before disposing native controls, tooltip and all three owned fonts.

## Layout, DPI and accessibility

Design client: 1040x720 at 96 DPI; minimum window: 900x680. At the captured 120 DPI, the default client is 1300x900 and full window 1318x947. The layout is suspended during construction before the completed initial DPI/layout pass.

Segoe UI body/title/emphasis fonts use point units. Header labels use DPI-scaled layout widths; native ListBox drawing updates row height/horizontal extent on font/DPI changes. Selection and scrolling stay native; columns are fixed logical widths, not draggable headers. Button text/mnemonics, accessible names, ordinary Tab navigation and label-backed status remain. Start has a primary enabled style and a separate disabled appearance. High-contrast colors are selected from system colors; live OS theme-transition acceptance remains pending.

Practice uses Alt+I; Start Alt+S, Stop Alt+T, Clear events Alt+C and Export Alt+X. The former GroupBox-specific Alt+P/Alt+E shortcuts are removed with those controls.

## Explicit TXT export

[TextLogFormatter](../src/KeyEventLogger.Core/TextLogFormatter.cs) defines invariant header/count/TSV rows with CRLF/final newline, no I/O/encoding/clock. App supplies strict UTF-8 without BOM.

Export sets a busy guard and stops before taking a stable snapshot or opening [the owned picker](../src/KeyEventLogger.App/Export/SaveFileDestinationPicker.cs). The dialog supplies TXT/default extension, overwrite confirmation and path checks. Cancel skips the writer. A closed/disposed owner after selection cannot begin writing.

[LocalExportPath](../src/KeyEventLogger.App/Export/LocalExportPath.cs) validates normal local-drive .txt paths, invalid/reserved/device names and reparse points including the root. It rejects unsupported/network drives and creates no folders. Production uses DriveInfo; tests may substitute only drive classification.

[TextLogExporter](../src/KeyEventLogger.App/Export/TextLogExporter.cs) formats before staging, creates an exclusive same-folder GUID temporary, writes/flushes/closes and commits with File.Move(overwrite: true). It never deletes the destination as preparation. Finally cleans only a temporary created by that operation; cleanup failure may leave it.

[ExportFailure](../src/KeyEventLogger.App/Export/ExportFailure.cs) maps recoverable errors to fixed guidance without raw exception details. Success includes the user-selected path. Bounded feedback, accessible description and tooltip are separate from recording status. All outcomes preserve data and remain stopped.

I/O remains synchronous, guarded against reentry. A 10,000-event limit bounds ordinary session data but not drive latency. Staging/path validation are not universal crash durability or a filesystem sandbox.

## Evidence boundary

94 current cases (30 Core, 64 App) cover model/session, native input/layout and export/file behavior. UI probes/captures use synthetic local messages and a controlled picker with the real formatter/writer. They do not establish human native Save/desktop focus/display/F5 or second-computer acceptance.

See [security boundary](SECURITY.md), [code walkthrough](CODE_GUIDE.md), [testing](TESTING.md) and [owner acceptance](SUBMISSION.md).
