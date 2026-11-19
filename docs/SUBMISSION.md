# Submission readiness and owner checklist

## Current status

**Application implementation is complete; final submission acceptance is pending.** Stages 00–03 are merged through [PR #4](https://github.com/alaamadii/KeyEventLogger/pull/4). Stage 04 prepares technical evidence and a draft final PR; missing human evidence is not inferred from merge/CI results.

Source reviewed again on 2026-11-19: course PDF pages 1, 2–3 and 5. [REQUIREMENTS](REQUIREMENTS.md) maps implementation/evidence to the instructor's criteria.

Author: **Alaa Madi**, as supplied. The owner directs omission of the student number; do not request/publish it. The PDF's original name/number requirement remains recorded; this is not an instructor waiver.

The selected public repository is [alaamadii/KeyEventLogger](https://github.com/alaamadii/KeyEventLogger); the instructor recommends YMH219-KeyEventLogger. Do not change visibility.

## Technical delivery

- [x] Independent Core and programmatic Windows Forms App.
- [x] Local explicit recording, timestamps, status, Stop/Clear and count.
- [x] Local TXT snapshot export, cancellation/error retention and retry.
- [x] Regression baseline: 72 cases, zero build warnings/errors/failures/skips.
- [x] Two authentic current screenshots; controlled picker explicitly disclosed.
- [x] Source-linked [code walkthrough](CODE_GUIDE.md).
- [ ] Published candidate clean-checkout evidence recorded.
- [ ] Exact final-head hosted CI and public artifact checks recorded.
- [ ] Owner manual results, image approval and second-computer evidence.
- [ ] Owner approves final PR and course submission readiness.

## Record evidence before testing

Record date, tested commit, Windows version, SDK version and whether each step was physical, scripted or automated. Use only synthetic input and a disposable local folder. Export only owned test files; never choose existing personal files or network locations.

For each group below, report PASS with actual observations, FAIL with reproducible steps, or NOT PERFORMED. Empty fields remain pending.

## Owner manual procedure

1. **Startup and recording.** Launch using SETUP. Confirm Stopped/0 events before Start. Type synthetic text while stopped and confirm no rows. Start, type demo, then hold an ordinary key and use a modifier. Count delivered Down/Up/repeat rows, not characters. Stop and type again: rows must stay fixed. Start again: old text must not be backfilled.
2. **Clear and navigation.** Clear while stopped and while recording. Events/count clear; practice text remains and state is unchanged. Check Tab/Shift+Tab, button keyboard activation and mnemonics. Shortcut/navigation delivery must match the documented policy.
3. **Focus boundary.** While recording, switch physically to another application and type only harmless outside-demo text there. Return: no outside events, Stopped status, explicit Start required. Repeat with minimize/restore. Never use passwords or real private input.
4. **Native Save flow.** Create a disposable normal local folder first. Record events and choose Export TXT. Confirm Stopped before selection; save an owned .keyevents.txt file, inspect format/count, and verify records/practice text stay. Cancel a second export and confirm no changes. Use only that owned file to check overwrite No/Cancel and Yes. After every outcome, recording must remain stopped until Start.
5. **Recoverable file error.** Use an owned locked/read-only test target or unsupported destination. Depending on Windows, the native dialog may reject it before App writing; record which layer reports the error. Confirm no crash/data loss and retry a valid local target. Do not change system permissions or touch personal files.
6. **Display/debugging.** Inspect default/minimum/resized layouts and long result text. Test available physical 100%/125%/150% settings and monitor transitions, recording which were actually used. Press F5 in VS Code from the root. Command-line launch alone is not F5 evidence.
7. **Normal close and second computer.** Close normally with/without records; no automatic save. On a different Windows computer, clone the tested branch/commit, run doctor/check/run and repeat representative recording/focus/export steps. A fresh clone on this computer is not a second-computer check.
8. **Screenshots and explanation.** Review both README images and capture disclosures; approve them explicitly or request replacements. Use CODE_GUIDE to explain state, scope, snapshot ownership, file staging and limitations.

## Owner evidence register

| Check | Required evidence | Status |
| --- | --- | --- |
| Physical recording/navigation | Date, commit, observed repeats/modifiers/start/stop/clear | NOT PERFORMED |
| External-app focus/minimize | Date, commit, observed stopped state/no outside rows | NOT PERFORMED |
| Native Save/overwrite/cancel/error | Date, commit, actual file/dialog outcomes and retry | Human review NOT PERFORMED; scripted open/cancel and separate real I/O tests recorded |
| Display/F5/accessibility | Actual scales, transitions, debugger and available assistive checks | NOT PERFORMED |
| Second Windows computer | Date, Windows/SDK, tested commit, commands and outcomes | NOT PERFORMED |
| Screenshot approval | Explicit acceptance of current images/disclosures | NOT PROVIDED |
| Final PR approval | Owner decision after evidence review | PENDING |

Do not replace this register with assumed results. Keep the final PR draft until its required acceptance evidence is complete.

## Course delivery

The deadline is 2026-11-30 at 23:59; this working schedule uses Asia/Riyadh, with institutional timezone confirmation outstanding if different. Later commits may count as late.

The owner submits their GitHub profile link through Classroom; the instructor reviews five separate public repositories. This repository covers only Key Event Logger. The assistant does not merge automatically, change visibility, select a license or submit anything to Classroom.
