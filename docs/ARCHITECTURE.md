# Architecture

## Implemented through Stage 03

App targets net10.0-windows and owns the STA entry point, PerMonitorV2 DPI, controls, local keyboard events and lifecycle. Core targets net10.0 and owns immutable event values and recording state. Core.Tests and App.Tests cover their respective behavior.

Dependencies remain App → Core, Core.Tests → Core and App.Tests → App. Core has no WinForms, drawing, timer, Windows API, file-dialog or third-party dependency.

## Core values and collection ownership

KeyEventType has KeyDown and KeyUp. KeyEventRecord is a sealed immutable record containing a trimmed key name without control characters, a defined event type and DateTimeOffset. Validation precedes assignment. The type does not know Windows Keys or a clock.

RecordingSession owns a private List. Start/Stop are idempotent; TryRecord validates its argument and appends only while recording. Count derives from the list. Clear removes records without changing state. GetSnapshot returns a read-only copy: callers cannot mutate the session or observe later changes through an earlier snapshot.

Arrival order is retained, including repeated identical records and backward clock changes. No Down/Up pairing, sorting or unique-key counting is performed. This small session is UI-thread-owned, not a concurrent collection.

## App event path

MainForm.cs constructs the native layout. MainForm.Recording.cs is another part of the same Form, grouping control references, event wiring and lifecycle. No separate presenter is introduced; export ownership is described below.

Only practiceText.KeyDown/KeyUp are subscribed. A delivered event is accepted when the session is recording, WinForms has activated this Form, the Form contains focus, Practice text is focused, and the window is neither minimized nor closing/disposed. Other controls have no recording subscriptions.

App maps KeyEventArgs.KeyCode.ToString() into a Core name and supplies TimeProvider.GetLocalNow(). Production uses TimeProvider.System; the constructor also accepts a TimeProvider for deterministic timestamp tests. Application logic never reads the practice TextBox.Text value.

Each accepted event appends one ListBox row, scrolls to the newest row and updates count/empty-state/button presentation. It does not rebind the full collection or copy snapshots on each key. The invariant-culture display is time with milliseconds/offset, type and key name; it is not yet an export format.

## Recording and focus lifecycle

Start is explicit and focuses practice without clearing history. Stop keeps records. Clear removes model/display records but preserves recording state and practice text; while recording it returns focus to practice.

Activated/Deactivate track window activity. Deactivation stops the session; resize-to-minimized also stops it. Activation/restore never calls Start. Moving focus within the same Form does not stop the session, but the practice-focus gate prevents input elsewhere from being recorded.

Closing stops collection before native shutdown. A canceled close leaves recording stopped and permits an explicit new Start. Disposal stops the session and removes local input/button subscriptions before disposing controls and owned fonts.

Start is enabled while stopped, Stop while recording, Clear when records exist; all are disabled while closing. Export is enabled when records exist and no close/export is in progress. Export feedback is separate from recording status.

## Delivery semantics

Every delivered Down/Up, including native Down repeats and modifier events, is independent. Native editing/shortcuts are not suppressed; Tab remains navigation. Some navigation, shortcut, layout and input-method behavior is handled by Windows before local key events. Names identify virtual keys, not reconstructed text.

A boundary may leave unmatched events. No missing Down/Up is fabricated, and practice text typed before Start is not backfilled.

## Layout, DPI and resources

The resizable root TableLayoutPanel retains automatic instruction/status rows and a proportional two-column workspace. FlowLayoutPanel holds actions; native GroupBoxes contain multiline practice and noneditable events. Layout is suspended during construction before the initial DPI pass.

The 96-DPI design client remains 960 × 720, minimum design window 820 × 680. Actual 120-DPI captures have a 1200 × 900 client and 1218 × 947 window. Minimum/wide state layouts are checked; physical 100%/150% transitions remain pending.

Form-owned point-unit fonts are disposed; system colors, native focus, accessible names and distinct mnemonics remain. Practice label uses Alt+I, group Alt+P, event group Alt+E, Start Alt+S, Stop Alt+T, Clear Alt+C; Export uses Alt+X when records are available.

## TXT export ownership — Stage 03

TextLogFormatter in Core creates a deterministic header/count and tab-separated rows with CRLF and a final newline. It has no encoding, clock, file or dialog dependency. App supplies strict UTF-8 without BOM.

MainForm.Export groups the explicit export action. It sets a busy guard, stops recording before selection and obtains RecordingSession.GetSnapshot. IExportDestinationPicker separates modal selection from UI tests; its production implementation owns/disposes SaveFileDialog with TXT/default extension, overwrite confirmation and path validation. ITextLogExporter separates file writing from dialog/lifetime tests; production is TextLogExporter.

LocalExportPath accepts normal fully qualified local-drive .txt paths. It rejects UNC/device paths, mapped network/unsupported drives, invalid/reserved names, and reparse-point directories/files to avoid redirection. It creates no destination folders. DriveInfo is real in production; a small optional drive-type delegate allows network-drive policy tests without network connections.

TextLogExporter formats the supplied snapshot before staging. A CreateNew, same-folder GUID temporary is written with strict UTF-8, flushed and closed, then File.Move(overwrite: true) commits it. No delete-before-replace occurs. The finally block deletes only a temporary successfully created by that operation. Cleanup failures propagate as errors; cleanup is best effort, not a power-loss guarantee.

MainForm displays success only after Export returns. Cancellation skips writing. Recoverable path/I/O/access/encoding/native-dialog errors produce separate nonmodal feedback, preserving records/count/practice text. All outcomes remain stopped. Long feedback is bounded with ellipsis, full accessible description and a Form-owned/disposed tooltip.

Export is synchronous, with actions disabled while selection/write is active. Start/Clear/reentrant Export are guarded; a closed/disposed owner after selection cannot start a write or receive feedback updates. No asynchronous work outlives the window. Large logs/slow local drives can pause the UI.

No persistent sessions, autosave, import, statistics/filtering or network delivery are added. See [export contract](EXPORT.md), [decisions](DECISIONS.md) and [testing](TESTING.md).
