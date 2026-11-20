# Source-linked code walkthrough

Use this guide to explain the implemented behavior during review. [USER_GUIDE](USER_GUIDE.md) describes user actions; [SECURITY](SECURITY.md) states the data boundary and limits.

## Dependency direction and composition

[App.csproj](../src/KeyEventLogger.App/KeyEventLogger.App.csproj) references [Core.csproj](../src/KeyEventLogger.Core/KeyEventLogger.Core.csproj). Core does not know WinForms, sockets, drawing, timers, a clock or file dialogs. [Core tests](../tests/KeyEventLogger.Core.Tests/KeyEventLogger.Core.Tests.csproj) reference Core; [App tests](../tests/KeyEventLogger.App.Tests/KeyEventLogger.App.Tests.csproj) reference App.

[Program.Main](../src/KeyEventLogger.App/Program.cs) configures an STA thread, PerMonitorV2 DPI and visual styles before the standard Windows message loop.

MainForm is one object across [composition/disposal](../src/KeyEventLogger.App/Forms/MainForm.cs), [layout](../src/KeyEventLogger.App/Forms/MainForm.Layout.cs), [recording](../src/KeyEventLogger.App/Forms/MainForm.Recording.cs) and [export](../src/KeyEventLogger.App/Forms/MainForm.Export.cs). Partial files organize responsibilities; they are not separate instances. [UiTheme](../src/KeyEventLogger.App/Forms/UiTheme.cs) holds presentation colors. The Form owns native controls, three fonts and its feedback tooltip.

## Validated values and bounded ownership

[KeyEventRecord](../src/KeyEventLogger.Core/KeyEventRecord.cs) is a sealed immutable value record with get-only name, [event type](../src/KeyEventLogger.Core/KeyEventType.cs) and supplied DateTimeOffset. Constructor validation rejects blank/untrimmed/control-character names and undefined types; no clock or Windows Keys lives here.

[RecordingSession](../src/KeyEventLogger.Core/RecordingSession.cs) encapsulates its private list. Capacity must be positive; production defaults to 10,000. Count is derived, IsRecording has a private setter and IsAtCapacity compares the count/limit.

Start accepts a recording state only while room exists. TryRecord rejects stopped appends, accepts the final available record, then stops. Subsequent Start/appends cannot exceed the boundary. Clear removes records without changing state; after capacity that means remaining stopped until an explicit Start.

GetSnapshot uses ToArray plus a read-only wrapper. A read-only interface alone would not copy mutable storage; this separate array and immutable elements preserve earlier snapshots after append/clear. [Session tests](../tests/KeyEventLogger.Core.Tests/RecordingSessionTests.cs) cover ownership, ordering, boundary/restart and invalid capacities.

## Trace a delivered key

1. Only practiceText.KeyDown/KeyUp subscriptions enter the record path.
2. RecordEvent requires recording, activation, Form/practice focus and a live nonminimized/nonclosing window.
3. App maps e.KeyCode.ToString() and supplies TimeProvider.GetLocalNow(); the default is System and tests can supply a fixed provider.
4. Session accepts an immutable record; the Form adds one immutable display row to [EventListBox](../src/KeyEventLogger.App/Forms/EventListBox.cs), a native ListBox that draws aligned timestamp/type/name columns. Its complete ToString preserves native item accessibility.
5. Presentation updates count/actions/state. At capacity it derives the full-session explanation from the model and disables Start while preserving Export/Clear. Export/cancel/focus changes cannot replace that reason with a less useful stopped message.

No snapshot/rebind occurs per key and no table string becomes a domain input. Recording/export never reads practiceText.Text. Native editing remains native; user entry/paste has a 4,096-character MaxLength.

Down, Up and repeated Down are independent. Space/Back tests deliver one native edit plus Down/Up and assert the text changes once. Tab remains navigation. Boundaries can leave unmatched events; no invented pairing or character reconstruction occurs.

## Stop, Clear and lifecycle

Stop preserves records. Deactivate marks the Form inactive and stops; minimize also stops. Activated/restore never calls Start. Moving focus to the event table prevents practice events but does not silently change session state.

Clear removes model/table records while preserving practice text/state; it restores a useful stopped reason after capacity and focuses practice only if still recording.

Closing stops before native shutdown. Canceled closing leaves recording stopped and permits explicit Start. Dispose detaches local/button handlers, disposes controls and releases owned resources. [Native UI cases](../tests/KeyEventLogger.App.Tests/MainFormTests.cs) use test-owned STA windows; they are not physical external-app evidence.

## Trace an export

1. ExportClicked guards empty/busy/closing calls, sets busy and stops before selection.
2. A stable model snapshot is captured; displayed rows and practice text are not serialization sources.
3. [IExportDestinationPicker](../src/KeyEventLogger.App/Export/IExportDestinationPicker.cs) separates selection for tests. [SaveFileDestinationPicker](../src/KeyEventLogger.App/Export/SaveFileDestinationPicker.cs) owns the real TXT SaveFileDialog with overwrite confirmation.
4. Cancel returns without writing. A closed/disposed owner after selection cannot begin writing.
5. [ITextLogExporter](../src/KeyEventLogger.App/Export/ITextLogExporter.cs) receives the path/snapshot. [LocalExportPath](../src/KeyEventLogger.App/Export/LocalExportPath.cs) checks names, local drive policy and reparse metadata without creating folders.
6. [TextLogFormatter](../src/KeyEventLogger.Core/TextLogFormatter.cs) produces invariant header/count/TSV rows, CRLF and final newline without I/O/clock.
7. [TextLogExporter](../src/KeyEventLogger.App/Export/TextLogExporter.cs) stages an owned same-folder file using strict UTF-8 without BOM, flushes/closes and commits through File.Move(overwrite: true). Finally cleans only its own temporary if needed.
8. [ExportFailure](../src/KeyEventLogger.App/Export/ExportFailure.cs) maps recoverable exceptions to controlled guidance without raw details. The Form restores available actions in finally but never starts recording.

[Formatter assertions](../tests/KeyEventLogger.Core.Tests/TextLogFormatterTests.cs), [real file tests](../tests/KeyEventLogger.App.Tests/TextLogExporterTests.cs) and [export UI tests](../tests/KeyEventLogger.App.Tests/ExportUiTests.cs) keep pure format, physical file behavior and controlled pickers distinct.

## OOP points and limits to explain

- Encapsulation protects state/list mutation behind a small API.
- Immutable records provide validated values and stable snapshot elements.
- Composition delegates domain and file responsibilities from the Form.
- Interface polymorphism substitutes focused pickers/exporters without a generic filesystem framework.
- Form inheritance supplies Windows lifecycle overrides; partial files only organize the same class.
- Explicit disposal and temporary ownership make resource cleanup explainable.

Be ready to trace Down followed by Stop before Up, a session filling on Down, Clear after capacity, an export failure and a canceled close. Explain why Count is not edits and why Clear does not alter an old snapshot.

Staging is not universal crash durability or a malicious-filesystem sandbox. Export is synchronous/plaintext, key names can be sensitive and Clear is not secure erasure. Physical/native/second-computer acceptance remains in [SUBMISSION](SUBMISSION.md).
