# Architecture

## Implemented through Stage 01

The solution contains:
- **App (net10.0-windows)**: STA entry point, PerMonitorV2 DPI policy, visual styles and a programmatic MainForm.
- **Core (net10.0)**: independent scaffold; no domain types or project/package dependencies.
- **Core.Tests (net10.0)**: xUnit infrastructure; zero domain test cases.
- **App.Tests (net10.0-windows)**: nine native UI assertions/cases using App.

Dependencies are App → Core, Core.Tests → Core and App.Tests → App. Core is unchanged by Stage 01 and has no Windows Forms, drawing, timer or file-dialog dependency.

## Interface ownership and behavior

MainForm owns the control tree and two custom point-unit fonts, disposed when the Form is disposed. Small private construction methods organize the layout; no separate presenter, recording session or export service is needed yet.

A root TableLayoutPanel has automatic rows for heading, status, actions, availability and scope notes, plus a remaining-height row for the workspace. Two equal-width columns contain native GroupBoxes for practice and recorded events. A wrapping FlowLayoutPanel holds the four disabled buttons. Labels size to text and wrap within the available width.

The practice TextBox is multiline, accepts Return, and leaves Tab for navigation. It supports ordinary native editing without application handlers inspecting its text. The event ListBox cannot be edited through typing; it starts empty and its separate count remains zero. Start/Stop/Clear/Export have no handlers and remain disabled. Visible stage notes explain availability without depending on tooltips.

Accessible names/descriptions, group mnemonics and the practice label accompany native keyboard focus. Tab/Shift+Tab traverse the usable practice and event controls; disabled actions are skipped. System colors and ordinary Windows text keep the visual design restrained. Screen-reader and high-contrast user acceptance are still pending.

## Resizing and DPI

Stage 01 replaces the fixed 640 × 360 initialization window with a resizable window. The requested design client is 960 × 720, with a design minimum window size of 820 × 680 at the 96-DPI baseline. Native nonclient metrics and DPI scaling affect actual dimensions.

Construction suspends layout until the full control tree is attached, then resumes/executes layout so initial DPI scaling applies consistently. PerMonitorV2 remains configured at entry. Dock, percentages, AutoSize, wrapping and point-unit fonts handle available space; no custom pixel renderer or scaling transform is introduced.

At the observed 120 DPI (125%), the preview reported a 1200 × 900 client and 1218 × 947 full window. Minimum and wider captures were also inspected. Those checks do not establish physical 100%/150% monitor transitions; they remain pending.

## Planned responsibilities — not implemented

| Component | Responsibility | Intended owner |
| --- | --- | --- |
| KeyEventRecord | Immutable normalized key name, event type, timestamp with offset | Core |
| RecordingSession | Encapsulated active/stopped state, records, start/stop/clear | Core |
| MainForm integration | Future local event wiring into the existing interface | App |
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
