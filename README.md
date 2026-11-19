# YMH219 : Key Event Logger

An educational Windows Forms application for **Nesne Tabanlı Programlama (YMH219)**, developed by **Alaa Madi** using C#/.NET 10.

The application demonstrates keyboard events, recording state and timestamps inside its active practice area. Recorded events can be exported explicitly to a user-selected local TXT file.

**Current status: Stage 03 TXT export, awaiting owner review.** Start, Stop and Clear manage in-memory records; Export TXT saves a stable snapshot to a local file. Recording is restricted to the practice TextBox while this window is active; leaving or minimizing the window stops it.

## Application screenshots

![Recording with eight synthetic demonstration events and Export TXT enabled](docs/screenshots/recording-export.png)

*Recording: four synthetic demonstration keys produce eight delivered Down/Up events.*

![Successful export of eight events with records retained and recording stopped](docs/screenshots/export-success.png)

*Export complete: the actual file was written, all eight events remain, and recording stays stopped.*

These actual MainForm captures were made at 125% DPI on November 19, 2026. Synthetic native input created the events; a controlled destination picker exercised the real formatter and file exporter. The success image is backed by an actual checked file. These are not native SaveFileDialog, human keyboard or desktop-focus test evidence. [Capture provenance and checksums](docs/screenshots/README.md) explain the method.

## Controls and behavior

| Control | Behavior |
| --- | --- |
| Start | Begin recording and focus Practice text; retain previous events |
| Stop | Stop collection immediately; retain records |
| Clear | Remove event records/count, preserving recording state and practice text |
| Practice text | Ordinary multiline editing; only its delivered Down/Up events are recorded after Start |
| Event display | Noneditable rows with time, event type and key name |
| Export TXT | Stop recording, choose a local .txt destination and save the current snapshot |
| Leave or minimize | Stop recording; return/restore requires Start again |

Existing practice text is never inspected or backfilled into records. Starting again does not log what was typed while stopped. Closing discards in-memory data; explicitly exported files remain at their selected destinations.

## Event policy

Each delivered **KeyDown** or **KeyUp** is one event, including native held-key Down repeats. An ordinary press can produce two rows; the counter counts events, not characters or unique key presses. Delivered modifier events are retained.

Names come from Windows Forms virtual-key names, not translated typed characters. Navigation, shortcuts, keyboard layouts and input methods affect delivery. Tab remains navigation. Down/Up pairs may be incomplete across start, stop, focus or clear boundaries; missing events are not invented.

Rows preserve arrival order and show `yyyy-MM-dd HH:mm:ss.fff zzz` timestamps from the system clock. The domain preserves supplied offsets and does not reorder records if the clock moves backward.

## Exporting a log

1. Record some practice-area events.
2. Choose **Export TXT**. Recording stops before the Save dialog opens.
3. Choose a normal local .txt destination; confirm replacement if it exists.
4. Inspect the separate success/error feedback. Cancellation and failure keep the records and practice text.
5. Press **Start** explicitly to record again.

Export is disabled with no records or while another export/closing is in progress. A full destination remains available through feedback accessibility text and its tooltip if the visible line is shortened.

The file uses UTF-8 without BOM, CRLF, a header/event count and tab-separated Timestamp, EventType and KeyName columns. A complete temporary write precedes destination replacement. Network/device paths, mapped network drives and linked/redirected destinations are unsupported. See [export format and behavior](docs/EXPORT.md).

Statistics and date/time filtering are deferred.

## Recording scope

This is an application-local event demonstration. It will not use global hooks, observe other applications, run hidden, start with Windows, capture passwords or credentials, save logs automatically, or transmit recorded input. Export requires an explicit local user action. No events are accepted before Start, outside the practice area or after deactivation.

## Getting started

Requirements: Windows 10/11, Git, and the **.NET 10.0.402 SDK**, or a newer stable patch within the 10.0.4xx feature band. The SDK is required; the runtime alone cannot build. The first restore needs access to NuGet. No API key, database, or additional workload is required.

From PowerShell:

```powershell
git clone https://github.com/alaamadii/KeyEventLogger.git
cd KeyEventLogger
./scripts/dev.ps1 doctor
./scripts/dev.ps1 check
./scripts/dev.ps1 run
```

The SDK policy is pinned in `global.json`. The script resolves `dotnet` from PATH, sets DOTNET_ROOT for that invocation, and keeps CLI state under ignored `.local/dotnet`. It restores inherited environment values when finished. See [setup and troubleshooting](docs/SETUP.md).

Open the repository root in VS Code and install the recommended extensions. Use **Ctrl+Shift+B** to build, the **check** task to validate, and **F5** to debug.

## Structure and technology

```text
src/
  KeyEventLogger.App/          Windows Forms UI, local input, lifecycle and export
  KeyEventLogger.Core/         Records, session and pure TXT formatting
tests/
  KeyEventLogger.Core.Tests/   Record/session behavior tests
  KeyEventLogger.App.Tests/    Native STA input/lifecycle/layout tests
scripts/dev.ps1                doctor, restore, build, test, check, run
docs/                          Requirements, design, plan, evidence, submission
.github/                       Windows CI and PR template
.vscode/                       Shared tasks, debugger, extensions
KeyEventLogger.slnx             Solution
global.json                    SDK selection
```

App references Core; Core.Tests references Core; App.Tests references App. Core has no WinForms, drawing, timer, or file-dialog dependency. The UI is programmatic and does not require Designer files.

App and Core have no third-party package dependencies. Both test projects reference `Microsoft.NET.Test.Sdk` 17.14.1, `xunit` 2.9.3, and `xunit.runner.visualstudio` 3.1.4.

## Validation

`./scripts/dev.ps1 check` restores, builds Release, runs available tests, and verifies formatting. Windows CI runs the same command on PRs targeting main and pushes to main.

**72 tests pass: 26 Core and 46 App.** Coverage includes recording rules, native control delivery, formatting/cultures, actual file bytes, overwrite/failure retention, cancellation, retry, busy/closing behavior and layouts. Scripted UI checks use only test-owned windows and synthetic input; they do not establish physical desktop-focus or keyboard acceptance. Executed local checks, UI lifecycle evidence, exact hosted results, and pending manual checks are recorded in [TESTING.md](docs/TESTING.md).

## Development and documentation

Work progresses through one stage branch and PR at a time. The owner reviews and approves merging; the assistant does not merge automatically.

| Guide | Purpose |
| --- | --- |
| [Plan](docs/PLAN.md) | Five stages, deliverables, and acceptance gates |
| [Requirements](docs/REQUIREMENTS.md) | Instructor requirements, owner choices, acceptance criteria |
| [Architecture](docs/ARCHITECTURE.md) | Current scaffold and planned responsibilities |
| [Decisions](docs/DECISIONS.md) | Design rationale and consequences |
| [Setup](docs/SETUP.md) | Tools, commands, SDK policy, troubleshooting |
| [Testing](docs/TESTING.md) | Actual results and planned verification |
| [TXT export](docs/EXPORT.md) | Format, encoding, destinations, cancellation and errors |
| [Development log](docs/DEVELOPMENT.md) | Dated implementation and review history |
| [Submission](docs/SUBMISSION.md) | Course deliverables and remaining owner actions |
| [Working agreement](AGENTS.md) | Repository rules |

## Screenshots and limitations

Two authentic Stage 03 captures are included above. Earlier Stage 01/02 images are explicitly historical in the capture guide. Owner screenshot approval and final submission review remain pending.

Statistics and filtering are not implemented. Records are retained only in memory unless explicitly exported; no automatic saving occurs. Export uses synchronous local I/O, so a large log or slow drive can briefly pause the UI. Physical Save dialog save/overwrite review remains pending; native opening/cancellation was checked, while file/interaction logic was tested independently. Human UI/DPI acceptance and another-computer testing remain pending. No distribution package is provided and no license has been selected.

## Course submission

This public repository covers Key Event Logger only. The instructor recommends the repository name `YMH219-KeyEventLogger`; the owner-selected `KeyEventLogger` name is retained.

The supplied assignment specifies **November 30, 2026, 23:59**. Development planning uses Asia/Riyadh; the institution's timezone has not been independently confirmed. The owner submits their GitHub profile link through Classroom. The [submission checklist](docs/SUBMISSION.md) records outstanding requirements and the owner-directed student-number omission.
