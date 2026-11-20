# User guide

## Start a safe demonstration

1. Launch using [SETUP](SETUP.md). The title is Key Event Logger and recording starts Stopped.
2. Choose Start recording (Alt+S). Focus moves to Practice text.
3. Type a short synthetic sample such as demo.
4. Read the timestamp, event type and key name in the table.
5. Choose Stop (Alt+T) when finished. Previous records stay visible.

Only the focused practice area in the active application is a recording source. Moving focus to another control prevents practice events being accepted. Leaving/minimizing the window stops the session; returning/restoring always requires Start.

Use sample content. Key-event names can reveal what was typed; do not enter private data. See [data handling](SECURITY.md).

## Why a key appears twice

A short press normally delivers KeyDown when pressed and KeyUp when released. Space therefore normally has two rows, as does Back (Windows Forms' name for Backspace). The native text editor still inserts one space or removes one character for a short press.

Held keys may generate multiple KeyDown repeats before one KeyUp. Count means delivered events, not characters, unique keys or paired presses. A stop, clear, focus change or capacity boundary can leave an unmatched event; no artificial partner is added.

The table separates:

- Timestamp: local system date/time, milliseconds and UTC offset.
- Event: KeyDown or KeyUp.
- Key: Windows virtual-key name, not translated text.

The order is arrival order, even if the system clock changes. Navigation, modifiers, shortcuts, layouts and input methods affect event delivery. Tab/Shift+Tab remain navigation; not every input method maps to a simple character press.

Pasting may deliver shortcut key events, but the pasted content is not inspected or backfilled. Text entered before Start is not reconstructed.

## Clear events and session capacity

Clear events (Alt+C) removes stored events, rows and count. It preserves practice text and the current recording state. You can edit/delete practice text using native editing; clearing the event table is not an erasure of that text.

At 10,000 events, recording automatically stops and Start becomes unavailable. Records are retained for export. Export if needed, then Clear events and explicitly Start again. There is no automatic rolling history or silent eviction.

Practice entry/paste is limited to 4,096 characters. The application is designed for short educational demonstrations.

## Export a local TXT snapshot

1. Record at least one event.
2. Choose Export TXT (Alt+X). Recording stops before the Save dialog opens.
3. Choose an existing normal local folder and a .txt filename.
4. For an existing file, review Windows' overwrite confirmation carefully.
5. Read success feedback only after the actual write completes.
6. Press Start explicitly if you want another recording session.

Success shows count and selected path. Long feedback is bounded; its tooltip/accessibility description holds the full message. Cancel writes nothing. Failure keeps records/count/practice text and allows another destination/retry. Every outcome remains stopped.

The file is plaintext UTF-8 without BOM, with a header/count and tab-separated timestamp, event type and key-name columns. It contains no practice-text body. See [format and path details](EXPORT.md).

Network/device/linked destinations and invalid/reserved filenames are unsupported. A selected local folder may still be synchronized by separate Windows software; choose a destination you control.

## Troubleshooting

| Observation | Meaning / next step |
| --- | --- |
| Two rows for Space or Back | Check Event: one Down and one Up is normal |
| Multiple KeyDown rows while held | Native key repeat; expected |
| Start unavailable with full session | Export/clear records, then Start |
| Typing outside Practice adds no rows | Recording boundary is working |
| Return from another app shows Stopped | Press Start explicitly |
| Clear left typed text | Clear events intentionally preserves practice text |
| Access denied or file in use | Use an owned writable local target; close a conflicting program and retry |
| Destination unsupported | Choose a normal local .txt path, avoiding linked/network/device destinations |
| Window closes with unexported records | In-memory records are discarded; no autosave |

Do not use a real private file to test overwrite or permissions. The [owner validation procedure](SUBMISSION.md) describes isolated tests and the evidence still needed.
