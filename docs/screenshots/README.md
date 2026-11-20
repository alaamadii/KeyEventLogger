# Application captures

## Current single-press behavior — 2026-11-20

Unedited target-window PrintWindow captures from actual source **4725220f1cca5a8a8873985eb1308fd3b1d0c344**, 120 DPI (125%), 1300x900 client / 1318x947 full window.

| Image | Actual state |
| --- | --- |
| [single-press-recording.png](single-press-recording.png) | Four synthetic DEMO presses create four real KeyDown records; release adds none |
| [single-press-export.png](single-press-export.png) | Real exported file contains those four records; records retained and recording stopped |

The ignored STA helper dispatches native Down/character/Up messages only to its own Form. The destination picker is controlled; recording/session/snapshot/formatter/writer are real. Actual output count/bytes/no-BOM and cancel/read-only failure/retry are checked. These are not human/native Save/desktop-focus evidence. Only synthetic own-window content appears.

SHA-256:
```text
6F621A375F5C8CE14F52DD2A025AF3C90218D4EBFEDF216714F29C87F51A6E20  single-press-recording.png
C0303CCE4BC1282B4F7403F42B402A3994E35B73B1FE1C3489410DF5F24A4D52  single-press-export.png
```



## Historical paired-event review — 2026-11-20

These unedited target-window PrintWindow images show the actual MainForm from source **c39337871269eb391c394db3ee39f71f4aaeb35f** at 120 DPI (125%). Default client: 1300x900; full window: 1318x947.

| Image | Actual state |
| --- | --- |
| [quality-recording.png](quality-recording.png) | Recording; 8 actual synthetic DEMO Down/Up events in separate timestamp/event/key columns |
| [quality-export.png](quality-export.png) | Actual local file exported; 8 records retained; recording stopped |

The ignored STA helper uses native synthetic control messages directed only to its own Form. It substitutes the destination picker with an isolated owned .local path. Recording handlers, model, snapshot, Core formatter and TextLogExporter are real. Output bytes/header/count/no-BOM were checked. Cancel/read-only failure/retry were checked separately; default/minimum/wide target-window images were inspected.

No rows/status/success feedback were inserted or edited to manufacture a result. Only the target application window and synthetic content appear. No exported TXT or unrelated desktop content is tracked.

These captures do not establish a native SaveFileDialog save/overwrite action, physical keyboard/desktop focus, monitor transition or owner approval.

SHA-256:
```text
2FEC51BBB023B6247CC22DB2B6AC1EB82EEA9C5518078CBAB1A9F6FA6086C4EF  quality-recording.png
604D0A58392DA7FB944CCE3B90C388F34487EFB5359A6F35B938443D88CF6BCC  quality-export.png
```



## Historical Stage 03 — 2026-11-19

These unedited PrintWindow captures show the actual MainForm referencing application source **40845cb354440e3dbfd345ea0fc5d14d25e7e9a2** at 120 DPI (125%), 1200 × 900 client / 1218 × 947 full window.

| Image | Actual state |
| --- | --- |
| [recording-export.png](recording-export.png) | Recording with 8 actual synthetic DEMO Down/Up events and Export enabled |
| [export-success.png](export-success.png) | Actual file exported, same 8 records retained, recording stopped |

The ignored STA capture helper uses synthetic native control messages through production recording handlers and system timestamps. Its destination picker is substituted with an isolated local path. The Core formatter, snapshot flow and TextLogExporter are real; the resulting file bytes/header/count/no-BOM were checked. Success feedback is not fabricated. The helper separately checked cancellation/read-only failure/retry and inspected minimum/wide layouts.

These images do **not** show a real SaveFileDialog interaction or human keyboard/desktop focus acceptance. Real native open/cancel was checked separately; save/overwrite automation was not counted as passing. Only the target application window appears, with synthetic demo data; no unrelated desktop content or exported TXT is committed.

SHA-256:
```text
BCE859B21C40FDE852B744881A4945C25A16727716661A817ACF014B587FAF29  recording-export.png
97755AFA208A411EDF8521FA7F8B76676EC5FD84A3C6FD88D76F3C71AF7D8B4F  export-success.png
```

## Historical captures

The earlier images are retained as history, not current export behavior:

- [recording.png](recording.png) and [stopped.png](stopped.png): Stage 02 source 3494e0a9cc0568dfb1014fa0bfd0ac639def56e0, actual native DEMO input, 8 events, export disabled.
- [ui-preview.png](ui-preview.png): Stage 01 source 57d0ecb02056de24f6e4635b6e61f9c07f76a3b0, blank nonrecording preview.

All were captured on 2026-11-19 using target-window PrintWindow at 120 DPI, 1218 × 947. Historical SHA-256:
```text
61EA799A1185F6D3CFA6166EEA7CAB8F0E8C6C9993D48551C288BAAA973C9C26  recording.png
5EEF0F2CDC91AD3FD988686009F303DA80F05B18A9E6C2012D92469E502A929D  stopped.png
1841770A11B420CBF170DD69CABCACD92A63FC8E1116D4AE562668DF31970A6C  ui-preview.png
```

Owner approval, physical display/input, F5 and another-computer checks remain pending. Never edit images to manufacture evidence; refresh images/provenance when behavior changes.

## Historical Stage 04 review — 2026-11-19

The current images were visually reinspected and retained unchanged because the final review changes documentation only; recording rows, stopped/export feedback and controls remain readable in the original captures. Their source, capture date, controlled-picker method and hashes above remain authoritative. Re-reviewing these images does not imply owner approval or a new capture. Final display/keyboard/native-dialog acceptance remains separate.
