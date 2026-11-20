# Security boundary and data handling

This is an educational recorder of events delivered to its own focused practice TextBox. It is not a system monitor. The review on 2026-11-20 covers the code and tests described below; it is not a penetration-test certificate.

## Data inventory and trust boundary

| Data | Source | Lifetime / destination |
| --- | --- | --- |
| Key name, Down/Up type, timestamp/offset | Local WinForms events after Start and App TimeProvider | In-memory session and read-only event table; explicit TXT snapshot only |
| Practice text | Native TextBox editing | Native control memory; never read by recording/export logic or included in exported data |
| Export path | Explicit owned SaveFileDialog | Used for a local write; success feedback shows the selected destination |
| Export errors | Recoverable exceptions | Mapped to fixed guidance; no raw exception message, stack trace or diagnostic log |
| Test/capture content | Synthetic messages in test-owned windows | Isolated temporary/ignored folders; only synthetic demonstration images intended for owner review are tracked |

Key names are not anonymized. A sequence of names/times can reveal sensitive input even though the application does not reconstruct characters. Use sample text and never enter credentials or personal information in the practice area.

## Recording controls

- Startup is stopped. Start is an explicit action and retains earlier records.
- Only practice KeyDown/KeyUp subscriptions exist. Collection also requires active Form state, focus inside that Form and practice focus.
- Deactivation, minimize and closing stop recording. Return/restore does not resume it.
- Other controls, other applications, existing practice text, paste contents and password controls are not inspected as data sources. Local shortcut key events may still be delivered.
- No global hook, polling, autostart, hidden monitor, application clipboard-reader code, file import, event transmission, network client or automatic saving is implemented.
- The application is a normal user process; it does not request administrator privileges or alter firewall/system settings.

Native Down repeats are real independent events; a press and release normally create two rows. This is not two edits. Start/Stop/Clear/focus/capacity boundaries can leave unmatched events.

## Retention and resource limits

The production session retains at most 10,000 events. Its final accepted event stops recording; Start is disabled at capacity, export remains available and the UI explains how to continue. No older event is silently evicted. Clear releases capacity and retains the stopped state.

Native user editing is limited to 4,096 characters. This bounds ordinary entry/paste, not programmatic TextBox assignment or all Windows memory usage. The session capacity is the domain resource limit.

Clear events removes event references and visible rows but deliberately preserves practice text. Native TextBox editing/undo and managed/OS memory are not secure-erasure facilities. Closing releases the process's in-memory data; no claim is made about pagefiles, dumps, OS history or forensic recovery. Explicitly exported files remain.

## Explicit file output

The picker stops recording before it opens, uses overwrite confirmation and owns/disposes its dialog. App takes a stable read-only snapshot; export never serializes display strings or practice text.

Destination validation accepts an existing normal local drive-letter folder and a .txt filename. It rejects UNC/device paths, unsupported/network drives, invalid/reserved names including superscript COM/LPT aliases, and reparse points on the root/ancestors/file. Names are checked before drive/file lookup. See [Microsoft's naming rules](https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file).

A unique same-folder temporary is created with CreateNew and exclusive sharing, written as strict UTF-8 without BOM, flushed/closed and moved over the selected destination. Formatting, encoding and staging failures occur before replacement; tests verify old-file preservation. Cleanup targets only the operation's owned temporary.

Limits:

- Files are ordinary plaintext, not encrypted. They inherit normal destination permissions; no custom ACL is installed.
- Validation is not a handle-based filesystem sandbox. A malicious local process can race path changes; this implementation does not guarantee protection against that actor.
- The application cannot control separate OS sync/backup software. A local-looking destination can still be managed by another service.
- Cleanup can fail and leave an owned temporary. There is no universal crash/power-loss/concurrent-writer guarantee.
- Synchronous local export can briefly pause the UI. The event cap bounds normal session size but does not guarantee drive latency.

Cancel/error/success keep records and remain stopped. Error text is deliberately controlled; selected success paths remain available in the UI/accessibility tooltip. See the exact [export contract](EXPORT.md).

## Dependencies and development data

App/Core have no third-party packages. Test tooling versions are explicit in project files. NuGet.Config pins package and audit sources to official nuget.org. Directory.Build.props enables all-dependency auditing from low severity, with warnings as errors; local check and CI both restore.

On 2026-11-20, dotnet list KeyEventLogger.slnx package --vulnerable --include-transitive reported no known vulnerable packages in the configured source. This is advisory data at that time, not proof of absence of vulnerabilities. See [NuGet audit documentation](https://learn.microsoft.com/en-us/nuget/concepts/auditing-packages).

Keep development exports under ignored exports/ or use *.keyevents.txt. Arbitrary TXT files are not globally ignored; review staged content before publication. Never add real input, credentials or exported logs to issues, PRs or screenshots. A narrow credential-pattern scan is only an additional check.

## Evidence and remaining checks

[Session tests](../tests/KeyEventLogger.Core.Tests/RecordingSessionTests.cs) cover retention/state/snapshot ownership. [Native UI tests](../tests/KeyEventLogger.App.Tests/MainFormTests.cs) cover local focus gates, explicit resume, count consistency and capacity. [File tests](../tests/KeyEventLogger.App.Tests/TextLogExporterTests.cs) cover real bytes/overwrite/failure preservation and path policy. [Export UI tests](../tests/KeyEventLogger.App.Tests/ExportUiTests.cs) cover stopping, snapshots, cancellation, retry, disposal and error redaction with controlled pickers.

Physical external-application switching, native Save/overwrite/error review, physical display/F5, high contrast/assistive technology and another Windows computer remain separately tracked in [SUBMISSION](SUBMISSION.md). The owner's reported Space/Back Down/Up observation is not blanket acceptance of those checks.

For a reproducible issue, report the tested commit, Windows/SDK, synthetic steps and observed result. Do not attach real event exports or credentials. No external security reporting service or contact has been configured.
