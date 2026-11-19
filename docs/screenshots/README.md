# Application captures

## Current Stage 03 — 2026-11-19

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
