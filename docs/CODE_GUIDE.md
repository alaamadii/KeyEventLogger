# Source-linked code walkthrough

This guide explains the implemented application for review and the course's oral check. It does not describe optional statistics/filtering or claim missing manual acceptance.

## Project direction

[App.csproj](../src/KeyEventLogger.App/KeyEventLogger.App.csproj) references [Core.csproj](../src/KeyEventLogger.Core/KeyEventLogger.Core.csproj). Core never references App, WinForms, drawing, timers or file dialogs. [Core tests](../tests/KeyEventLogger.Core.Tests/KeyEventLogger.Core.Tests.csproj) reference Core; [App tests](../tests/KeyEventLogger.App.Tests/KeyEventLogger.App.Tests.csproj) reference App.

[Program.Main](../src/KeyEventLogger.App/Program.cs) establishes an STA UI thread, PerMonitorV2 DPI and visual styles, then runs MainForm in the Windows message loop.

MainForm is one object split across [layout](../src/KeyEventLogger.App/Forms/MainForm.cs), [recording](../src/KeyEventLogger.App/Forms/MainForm.Recording.cs) and [export](../src/KeyEventLogger.App/Forms/MainForm.Export.cs) partial files. Partial files are not separate instances. The Form owns native controls, fonts and tooltip disposal.

## Values and encapsulation

[KeyEventRecord](../src/KeyEventLogger.Core/KeyEventRecord.cs) is a sealed immutable value record: key name, event type and supplied DateTimeOffset are get-only. Constructor validation rejects blank/untrimmed/control-character names and undefined [event types](../src/KeyEventLogger.Core/KeyEventType.cs). It stores the supplied offset; it does not consult a clock or Windows Keys.

[RecordingSession](../src/KeyEventLogger.Core/RecordingSession.cs) owns its private list. IsRecording has a private setter; Count is derived. Start/Stop are idempotent. TryRecord validates then rejects a stopped append. Clear empties records without changing state.

GetSnapshot calls ToArray and wraps the copy read-only. A read-only interface alone would not provide a stable copy; this implementation protects ownership, and immutable record elements remain safe to share. Later appends/clear do not alter a prior snapshot. See [session assertions](../tests/KeyEventLogger.Core.Tests/RecordingSessionTests.cs) and [value assertions](../tests/KeyEventLogger.Core.Tests/KeyEventRecordTests.cs).

## Trace a delivered key

1. Only the practice TextBox's KeyDown/KeyUp subscriptions enter PracticeKeyDown/PracticeKeyUp.
2. RecordEvent requires recording, native activation state, Form/practice focus, and a live nonminimized/nonclosing window.
3. App maps e.KeyCode.ToString() and reads TimeProvider.GetLocalNow(). Production uses System; tests can supply a fixed provider.
4. Session.TryRecord appends the immutable value. The Form adds one formatted row, scrolls and refreshes count/status/button/empty-state presentation.

The application never reads practiceText.Text to reconstruct or backfill typed content. Each delivered Down/Up/repeat is independent, so one press can create two rows. Modifier events are retained when delivered. Native shortcuts/navigation/input methods affect delivery; Tab remains navigation. A boundary can leave an unmatched event; the code does not invent its missing partner.

## Trace Stop, Clear and focus

StopRecording stops the model, retains rows and records a readable reason. Deactivate marks the window inactive and stops; minimizing also stops. Activated changes only activity state, so returning never resumes capture.

Clear removes model/display records while preserving state and practice text. When still recording it focuses practice again. Export/closing busy guards prevent conflicting actions.

OnFormClosing stops before shutdown; a canceled close leaves recording stopped and permits a later explicit Start. Dispose removes subscriptions and disposes native/UI resources. [Native UI cases](../tests/KeyEventLogger.App.Tests/MainFormTests.cs) exercise synthetic messages, state/focus transitions and layouts; they are not a human desktop-focus check.

## Trace an explicit export

1. ExportClicked rejects empty/busy/closing calls, sets the busy guard and stops recording before any dialog.
2. GetSnapshot captures stable model records; ListBox strings and practice text are not serialization inputs.
3. [IExportDestinationPicker](../src/KeyEventLogger.App/Export/IExportDestinationPicker.cs) selects a path. [SaveFileDestinationPicker](../src/KeyEventLogger.App/Export/SaveFileDestinationPicker.cs) owns/disposes the real SaveFileDialog, configured for TXT and overwrite confirmation. Null means cancellation.
4. A closed/disposed owner cannot begin writing after selection. Otherwise [ITextLogExporter](../src/KeyEventLogger.App/Export/ITextLogExporter.cs) receives that snapshot.
5. [LocalExportPath](../src/KeyEventLogger.App/Export/LocalExportPath.cs) validates supported local paths and metadata without creating missing folders.
6. [TextLogFormatter](../src/KeyEventLogger.Core/TextLogFormatter.cs) creates invariant text with header/count/tabs/CRLF/final newline; it has no I/O or clock.
7. [TextLogExporter](../src/KeyEventLogger.App/Export/TextLogExporter.cs) creates an owned same-folder temporary, uses strict UTF-8 without BOM, flushes/closes, then moves it over the approved destination. The finally block cleans only its own temporary after failure.
8. MainForm reports success only after return, or readable cancel/error feedback. The finally block restores available controls but never starts recording.

[Formatter tests](../tests/KeyEventLogger.Core.Tests/TextLogFormatterTests.cs), [real file tests](../tests/KeyEventLogger.App.Tests/TextLogExporterTests.cs) and [export UI tests](../tests/KeyEventLogger.App.Tests/ExportUiTests.cs) keep pure formatting, physical file behavior and dialog substitutes distinct. See [EXPORT](EXPORT.md) for the contract and limits.

## OOP and review points

- Encapsulation keeps list/state mutation behind a small session API.
- Immutable records provide validated value equality and stable elements.
- Composition keeps the Form responsible for presentation while delegating domain rules and writing.
- Interface polymorphism supplies production pickers/exporters or focused test substitutes; no generic filesystem framework is needed.
- Form inheritance provides Windows lifecycle overrides. Partial source organization does not change that inheritance.
- Resource ownership is explicit through using/disposal and named temporary ownership.

Be ready to trace an A Down followed by Stop before Up, explain why Count is not characters, show why Clear cannot mutate an old snapshot, and distinguish write completion from overwrite confirmation.

Known limits: synchronous export may pause UI, linked/network destinations are unsupported, and staging is not a universal power-loss/concurrent-writer guarantee. No global input capture or automatic saving exists. Physical Save/desktop focus/display/F5, second-computer evidence and owner approval remain in [SUBMISSION](SUBMISSION.md).
