# Architecture

## Implemented in Stage 00

The solution contains:
- **App (net10.0-windows)**: STA entry point, explicit PerMonitorV2 DPI policy, visual styles, and programmatically constructed MainForm.
- **Core (net10.0)**: library scaffold with no domain types or project/package dependencies.
- **Core.Tests (net10.0)**: xUnit infrastructure referencing Core; no test cases yet.

The dependency direction is App → Core and Core.Tests → Core. Core must never reference App or Windows UI APIs.

MainForm owns its single Label through the WinForms control tree. It opens centered with a fixed 640 × 360 logical client area, a 96-DPI baseline, and maximize disabled. It contains no custom drawing resources, keyboard handlers, timers, session model, file access or network code. The window title is Key Event Logger; its status says recording is not implemented.

## Planned responsibilities — not implemented

| Component | Responsibility | Intended owner |
| --- | --- | --- |
| KeyEventRecord | Immutable normalized key name, event type, timestamp with offset | Core |
| RecordingSession | Encapsulated active/stopped state, records, start/stop/clear | Core |
| MainForm | Local event wiring, status, practice area, controls | App |
| TXT formatter | Stable text representation of synthetic/real records | Core if pure and justified |
| Export service | Explicit destination writing and recoverable I/O results | App |
| SaveFileDialog | Destination choice and ordinary cancellation | App |

Do not create these classes merely to match the table. Add them with the behavior that needs them. Interfaces are justified by replaceable boundaries, not by a requirement to put every class behind one.

## Planned event and recording lifecycle

A local KeyDown/KeyUp and/or KeyPress event is normalized by App and offered to the session only when recording was explicitly started and the application is active. Core does not receive WinForms Keys values, control handles or global keyboard state.

The exact event/repeat/modifier policy will be decided and tested in Stage 02. Store names and timestamps; do not accumulate a hidden text reconstruction.

The planned states are Stopped and Recording. Start permits appends. Stop and deactivation/minimize prevent appends immediately. Activation does not resume; the user must press Start. Clear removes the in-memory/displayed records without causing file writes. Resolve any pending event state at stop/focus boundaries so stale input cannot leak into another session.

## Planned export flow

An explicit Export action takes a stable record snapshot, requests a destination through SaveFileDialog, formats consistently and writes locally. Dialog focus loss also stops recording. Cancellation is normal. A file failure must preserve in-memory records and show a readable error. No automatic saving, network delivery or hidden persistence is permitted.

The timestamp format, encoding, newline policy and replacement behavior will be specified with Stage 03, not claimed as current functionality.

## OOP and maintenance

Use immutable values, encapsulated session state and small UI/I/O responsibilities. Prefer composition over unnecessary inheritance. Tests will isolate rules and formatting from the native window. Optional statistics can later consume snapshots, but are deferred.

See [decisions](DECISIONS.md), [requirements](REQUIREMENTS.md), and [test strategy](TESTING.md).
