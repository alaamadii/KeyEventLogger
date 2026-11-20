# Submission readiness and owner checklist

## Current status

**Required application features are implemented; final submission acceptance remains pending.** The owner merged [PR #5](https://github.com/alaamadii/KeyEventLogger/pull/5), then requested further interface/security/code/documentation work. The current quality review on feat/usability-security-review needs fresh owner review; merges and CI do not supply missing human evidence.

Assignment source reviewed on 2026-11-19; the current implementation/security review is dated 2026-11-20: course PDF pages 1, 2–3 and 5. [REQUIREMENTS](REQUIREMENTS.md) maps implementation/evidence to the instructor's criteria.

Author: **Alaa Madi**, as supplied. The owner directs omission of the student number; do not request/publish it. The PDF's original name/number requirement remains recorded; this is not an instructor waiver.

See [USER_GUIDE](USER_GUIDE.md) for everyday use and [SECURITY](SECURITY.md) for data handling and limits.

The selected public repository is [alaamadii/KeyEventLogger](https://github.com/alaamadii/KeyEventLogger); the instructor recommends YMH219-KeyEventLogger. Do not change visibility.

## Technical delivery

- [x] Independent Core and programmatic Windows Forms App.
- [x] Local explicit recording, timestamps, status, Stop/Clear and count.
- [x] Local TXT snapshot export, cancellation/error retention and retry.
- [x] Current regression: 94 cases (30 Core + 64 App), zero build warnings/errors/failures/skips.
- [x] Two authentic redesigned-interface screenshots from 4725220; controlled picker explicitly disclosed; new owner approval pending.
- [x] Source-linked [code walkthrough](CODE_GUIDE.md).
- [x] Current single-press local/hosted/checkout evidence is tracked in [TESTING](TESTING.md) and PR #6; earlier 72bc356 / d3bdfe5 / 9254884 evidence is historical.
- [x] Current quality-review hosted evidence tracked on [draft PR #6](https://github.com/alaamadii/KeyEventLogger/pull/6), including the exact final-head SHA/run/result. PR #5 results are historical.
- [ ] Owner manual results, image approval and second-computer evidence.
- [ ] Owner approves final PR and course submission readiness.

## Record evidence before testing

Record date, tested commit, Windows version, SDK version and whether each step was physical, scripted or automated. Use only synthetic input and a disposable local folder. Export only owned test files; never choose existing personal files or network locations.

For each group below, report PASS with actual observations, FAIL with reproducible steps, or NOT PERFORMED. Empty fields remain pending.

## Owner manual procedure

1. **Startup and recording.** Launch using SETUP. Confirm Stopped/0 events before Start. Type synthetic text while stopped and confirm no rows. Start, type demo, then hold an ordinary key and use a modifier. One short press must add one KeyDown record; release must add none. Two intentional G presses must produce two records. Count delivered Down/repeat/modifier events, not characters. Stop and type again: rows must stay fixed. Start again: old text must not be backfilled.
2. **Clear and navigation.** Clear while stopped and while recording. Events/count clear; practice text remains and state is unchanged. Check Tab/Shift+Tab, button keyboard activation and mnemonics. Shortcut/navigation delivery must match the documented policy.
3. **Focus boundary.** While recording, switch physically to another application and type only harmless outside-demo text there. Return: no outside events, Stopped status, explicit Start required. Repeat with minimize/restore. Never use passwords or real private input.
4. **Native Save flow.** Create a disposable normal local folder first. Record events and choose Export TXT. Confirm Stopped before selection; save an owned .keyevents.txt file, inspect format/count, and verify records/practice text stay. Cancel a second export and confirm no changes. Use only that owned file to check overwrite No/Cancel and Yes. After every outcome, recording must remain stopped until Start.
5. **Recoverable file error.** Use an owned locked/read-only test target or unsupported destination. Depending on Windows, the native dialog may reject it before App writing; record which layer reports the error. Confirm no crash/data loss and retry a valid local target. Do not change system permissions or touch personal files.
6. **Display/debugging.** Inspect default/minimum/resized layouts and long result text. Test available physical 100%/125%/150% settings and monitor transitions, recording which were actually used. Press F5 in VS Code from the root. Command-line launch alone is not F5 evidence.
7. **Normal close and second computer.** Close normally with/without records; no automatic save. On a different Windows computer, clone the tested branch/commit, run doctor/check/run and repeat representative recording/focus/export steps. A fresh clone on this computer is not a second-computer check.
8. **Capacity.** The normal limit is 10,000 events, not 10,000 presses. A held key or enough synthetic practice events eventually stops the session without evicting earlier rows; Export remains available and Start is disabled. Export if needed, Clear events and confirm recording remains stopped until Start. The small-capacity automated tests cover this boundary without requiring a long physical run. Do not mark a physical capacity run performed unless actually done.
9. **Screenshots and explanation.** Review both README images and capture disclosures; approve them explicitly or request replacements. Use CODE_GUIDE to explain state, scope, snapshot ownership, file staging and limitations.

## Owner evidence register

| Check | Required evidence | Status |
| --- | --- | --- |
| Physical recording/navigation | Date, commit, observed repeats/modifiers/start/stop/clear | NOT PERFORMED |
| External-app focus/minimize | Date, commit, observed stopped state/no outside rows | NOT PERFORMED |
| Native Save/overwrite/cancel/error | Date, commit, actual file/dialog outcomes and retry | Human review NOT PERFORMED; scripted open/cancel and separate real I/O tests recorded |
| Display/F5/accessibility | Actual scales, transitions, debugger and available assistive checks | NOT PERFORMED |
| Second Windows computer | Date, Windows/SDK, tested commit, commands and outcomes | NOT PERFORMED |
| Screenshot approval | Explicit acceptance of current images/disclosures | NOT PROVIDED |
| Current quality-review PR approval | Owner decision after current evidence review | PENDING |

Do not replace this register with assumed results. Keep the current quality/final acceptance PR draft until its required evidence is complete. The reported Space/Back Down/Up observation confirms those row types only, with no supplied exact environment/commit; it is not blanket manual acceptance.

## Course delivery

The deadline is 2026-11-30 at 23:59; this working schedule uses Asia/Riyadh, with institutional timezone confirmation outstanding if different. Later commits may count as late.

The owner submits their GitHub profile link through Classroom; the instructor reviews five separate public repositories. This repository covers only Key Event Logger. The assistant does not merge automatically, change visibility, select a license or submit anything to Classroom.
