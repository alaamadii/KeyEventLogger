# Submission checklist

## Source and identity

The course PDF was reviewed on 2026-11-19: pages 1, 2–3 and 5. It requests five separate public repositories, source/.gitignore/README, two screenshots, meaningful commits, student details, and a second-computer check. This repository covers Key Event Logger only.

Author: **Alaa Madi**, supplied in the project brief. The owner explicitly excludes the student number from public documentation. The instructor's original name/number requirement remains recorded in [REQUIREMENTS](REQUIREMENTS.md); this omission is not an instructor waiver. Do not request or publish the number.

The PDF recommends YMH219-KeyEventLogger; the owner selected [alaamadii/KeyEventLogger](https://github.com/alaamadii/KeyEventLogger). It was public at the initialization metadata check. Do not change visibility.

## Implementation and repository

- [x] Initial solution and nonrecording window; PR #1 merged.
- [x] Stage 01 interface merged in PR #2.
- [x] SDK policy, local commands, VS Code configuration and exclusions.
- [x] Requirements, architecture, decisions and completion plan.
- [x] Stage 02 local recording/status/start/stop/clear implemented with 38 passing cases; owner/physical acceptance pending.
- [x] Stage 03 export success/cancel/failure implemented with 72 total cases; native/manual review pending.
- [ ] Recording boundaries verified.
- [ ] Final exact-head local/hosted checks pass.
- [x] Two authentic Stage 03 recording/export screenshots embedded in README; picker substitution disclosed and owner approval pending.
- [ ] README accurately describes final behavior and limitations.
- [ ] Final diff excludes generated output, personal data and captured logs.
- [ ] Owner accepts final PR.

## Manual evidence — owner to provide after implementation

| Check | Evidence required | Status |
| --- | --- | --- |
| Recording lifecycle | Actual key input, start/stop/clear, repeats/modifiers | Not performed |
| Focus scope | Switch to another app/minimize; no events; return requires Start | Not performed |
| Export | Native destination/save/overwrite/cancel/error review | Native open/cancel scripted; file/UI logic tested separately; human save/overwrite review pending |
| Display/debugging | Physical scaling and VS Code F5 | Not performed |
| Second computer | Date, Windows/SDK, commit and results | Not performed |
| Screenshots | Authentic captures and owner approval | Two actual Stage 03 captures with controlled picker and actual writer; owner approval pending |

## Delivery

The course deadline is 2026-11-30 at 23:59; later commits may count as late. Working schedules use Asia/Riyadh; confirm the institution's timezone if different. The owner submits only their GitHub profile link through Classroom. The assistant must not submit or merge without explicit authorization.

Stages 00–03 implement recording and explicit export. Stage 04 final QA and owner submission acceptance remain outstanding. CI success does not replace manual acceptance.
