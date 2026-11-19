# Application captures

## Current Stage 02 captures — 2026-11-19

These are unedited PrintWindow captures of the actual MainForm hosted by an ignored STA diagnostic tool referencing App. The production Release EXE was also separately launched/closed successfully. No generated artwork, composites or fabricated rows are used.

| Item | Evidence |
| --- | --- |
| Application source | 3494e0a9cc0568dfb1014fa0bfd0ac639def56e0 |
| DPI and size | 120 (125%); 1200 × 900 client, 1218 × 947 full window |
| [recording.png](recording.png) | Recording, synthetic demo practice text, 8 actual Down/Up records |
| [stopped.png](stopped.png) | Stopped after explicit Stop; same 8 records retained |
| Method | Target-window PrintWindow after layout/update; no unrelated desktop content |

The helper activates its own Form within its STA thread using SetActiveWindow. It uses the real Start button path, then sends native Down/character/Up messages for D/E/M/O to practice. Production handlers, focus/session gates and TimeProvider.System create the rows/timestamps. It never inserts rows, edits counts or writes practice Text directly. It presses Stop before the second capture.

This is controlled synthetic event-path evidence, not a human foreground keyboard session. The helper does not call SetForegroundWindow or collect input from other applications. Additional minimum/wide/restore captures were inspected but are diagnostic only. The helper stays under ignored .local.

SHA-256:
```text
61EA799A1185F6D3CFA6166EEA7CAB8F0E8C6C9993D48551C288BAAA973C9C26  recording.png
5EEF0F2CDC91AD3FD988686009F303DA80F05B18A9E6C2012D92469E502A929D  stopped.png
```

Owner screenshot approval, physical keyboard/focus, actual display transitions, F5, second-computer testing and final refresh after export remain pending.

## Historical Stage 01 preview

[ui-preview.png](ui-preview.png) is retained as the earlier blank nonrecording interface, not current functionality. Captured 2026-11-19 from source 57d0ecb02056de24f6e4635b6e61f9c07f76a3b0 at 120 DPI, 1218 × 947 full window, using target-window PrintWindow.

SHA-256: 1841770A11B420CBF170DD69CABCACD92A63FC8E1116D4AE562668DF31970A6C.

Never edit a capture to manufacture event/state evidence. Refresh captures and this record when the application changes.
