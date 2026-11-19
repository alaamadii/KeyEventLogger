# TXT export contract

## User flow and data retention

Record practice-area events, then choose Export TXT. The action stops recording before taking a stable snapshot or showing the owned SaveFileDialog. Export is disabled with no events, while busy, or while closing.

The dialog filters .txt, suggests KeyEventLog.keyevents.txt, adds the extension, checks the path and requests overwrite confirmation. Cancel writes nothing. Success, cancel and recoverable failure all preserve records/count/practice text and leave recording stopped. Start is explicit. There is no export on Stop, Clear, focus loss or exit.

Feedback is separate from recording state. Success includes count and destination only after the write/commit succeeds. Long results are bounded visually; the full text is available through accessibility and tooltip. Failures show a readable reason and allow retry without stack traces.

## Format

UTF-8 without BOM, strict encoding, CRLF on every line including the final line. The first three lines are application name, invariant event count and tab-separated column names. Rows preserve arrival order/repeats and use yyyy-MM-dd HH:mm:ss.fff zzz timestamps.

Synthetic example, not execution evidence (columns contain actual tab characters):
```text
Key Event Logger
Events: 2
Timestamp	EventType	KeyName
2026-11-19 12:34:56.789 +03:00	KeyDown	A
2026-11-19 12:34:56.790 +03:00	KeyUp	A
```

The Markdown example does not establish file bytes/line endings; exact-byte tests do. Key-name validation excludes tabs/newlines/control characters. Empty formatting produces headers with count zero, but the UI does not offer empty export. No practice text, machine/user metadata, author/student fields or export-time clock value is added.

## Supported destinations

Use an existing normal local folder and a fully qualified drive-letter .txt path. UNC/device paths, network/unsupported drives, invalid/reserved names, and reparse-point folders/files are rejected. This conservative policy can exclude linked folders and cloud placeholders. Missing folders are not created.

DriveInfo supplies real production drive metadata; network-drive tests use a metadata substitute without making network connections. This is not a filesystem security sandbox or control over separate OS sync software/concurrent path changes.

## Staging and errors

1. Validate destination and format the stable snapshot.
2. Create a uniquely named temporary in the destination folder with CreateNew.
3. Write strict UTF-8, flush the writer and request a stream disk flush.
4. Close the file before File.Move with overwrite commits it.
5. On failure, attempt deletion only of a temporary created by this operation.

The existing destination is never deleted as preparation. Tests verify preservation on encoding, lock and read-only failures. Cleanup failures are surfaced and can leave an owned temporary; no power-loss durability, concurrent-writer coordination or universal atomicity is promised.

I/O is synchronous for this student application. Long logs/slow drives can briefly block UI. Busy actions prevent reentry, and a closed owner after destination selection cannot start writing or receive disposed-control feedback.

## Validation and remaining acceptance

The full local check passes 72 cases (26 Core, 46 App). Tests inspect real UTF-8 bytes, overwrite/failure retention and temporary cleanup, as well as UI cancellation, retry, snapshots and layout with picker substitutes.

The real native SaveFileDialog was opened/canceled. Native Save/overwrite automation was attempted but was unreliable and is not claimed as passing. Physical save/overwrite/error checks and owner acceptance remain pending. See [testing](TESTING.md).

Development logs must stay under ignored exports/ or use *.keyevents.txt. Other chosen .txt names inside the repository are not automatically ignored; never stage captured records. Do not broaden exclusions to hide documentation/source text.

## Stage 04 follow-up

Baseline regression still passes all 72 cases with unchanged writer/format. A bounded external-controller UI Automation follow-up could not reliably discover the owned Save dialog; it is not a successful save/overwrite result. No production change was made merely to satisfy automation. Follow the [owner native-dialog procedure](SUBMISSION.md) and record actual outcomes.
