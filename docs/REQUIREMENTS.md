# Requirements and acceptance criteria

Source reviewed on **2026-11-19**: the supplied `YMH219_Ek_Uygulama_Odevleri_Ogrenci_Foyu.pdf`, pages 1, 2–3 and 5, plus the owner's classroom announcement. The PDF is reference material. Its local absolute path is not required to build the repository.

## Instructor requirements

| ID | Requirement | Acceptance criterion | Stage/status |
| --- | --- | --- | --- |
| FR-001 | Local keyboard events | Key names and times shown only for input within the active application | 02 implemented, native controlled-window tests; physical desktop check pending |
| FR-002 | Start/Stop | Explicit controls govern recording, with no appends while stopped | 02 implemented/tested |
| FR-003 | Clear | Clear removes displayed records | 02 implemented/tested; state/practice text retained |
| FR-004 | TXT export | User can save records to a local text file | 03 implemented, real I/O and UI-substitute tests; native save/overwrite review pending |
| FR-005 | Status | Visible recording state accurately reflects actual capture | 02 actual recording/stopped status and count implemented/tested |
| NFR-001 | Desktop/OOP | Working C#/.NET WinForms app with clear responsibilities and error handling | 03 recording/format/export separation implemented/tested |
| NFR-002 | File resilience | Export errors are reported without an application crash | 03 real failure/retention and UI retry tests passed |
| SEC-001 | Active-window scope | Other applications and inactive/minimized periods yield no recorded input | 02 gates/stop behavior tested with owned windows; external-app physical check pending |
| SEC-002 | No system collection | No global hooks, hidden/background operation or Windows autostart | Required throughout |
| SEC-003 | No credential collection | No password/credential capture or other-app collection | Required throughout |
| SEC-004 | No transmission | Recorded events are never sent across a network | Required throughout |
| SUB-001 | Public separate repository | Source, .gitignore and README accessible on student's own account | Existing public KeyEventLogger selected; delivery tracked separately |
| SUB-002 | README | Purpose, technologies/packages, run steps, features, limitations and student details | Draft implemented; owner exception below |
| SUB-003 | Screenshots | At least two authentic screenshots embedded in final README | Two authentic Stage 03 captures embedded; controlled picker disclosed, owner review pending |
| SUB-004 | Meaningful history | Commits represent actual incremental work | Required throughout |
| SUB-005 | Second computer | Project opens/runs on another computer | 04, pending |
| SUB-006 | Timely delivery | Final work on GitHub by 2026-11-30 23:59 | Pending final delivery |

Assessment: application 40, OOP/code/errors 20, UI 10, Git/GitHub 15, README 10, explanation 5; total 100. The PDF recommends `YMH219-KeyEventLogger`; the owner selected `alaamadii/KeyEventLogger`. The owner submits only their GitHub profile URL through Classroom.

## Owner choices

- .NET 10, VS Code, programmatic UI, English documentation, App/Core separation, xUnit, CI and stage PRs are project practices, not extra instructor requirements.
- Deactivation stops recording and requires Start again on return; this is stronger than merely ignoring inactive-window input.
- No automatic log saving; export is explicit and local.
- Author name supplied in the brief: Alaa Madi.
- The owner directed omission of the student number from public documentation. Do not request/publish it. The PDF's original name/number requirement is recorded in SUB-002; omission is an owner exception, not an instructor waiver.

## Optional and deferred

Key-count/frequency statistics and date/time filtering are optional in the PDF. They are not part of the current implementation plan.

## Completion checklist

- [x] Nonrecording window and independent Core scaffold.
- [x] Stage 01 interface merged in PR #2.
- [x] Stage 02 recording controls, practice-only events and session tests; owner review pending.
- [x] Development commands, package versions and exclusions documented.
- [x] FR-001 through FR-005 implemented with automated coverage; native/manual and owner acceptance remain pending.
- [ ] Active-window/privacy boundaries verified with real focus changes.
- [x] TXT success/cancel/failure covered with real I/O and dialog substitutes; native save/overwrite review pending.
- [ ] Two real screenshots and final README accepted.
- [ ] Second-computer check and owner sign-off.
- [ ] Final reviewed work published before deadline.

## Final implementation/evidence audit — 2026-11-19

| Criteria | Source and automated evidence | Remaining acceptance |
| --- | --- | --- |
| FR-001, FR-005 | [Recording wiring](../src/KeyEventLogger.App/Forms/MainForm.Recording.cs), [native UI cases](../tests/KeyEventLogger.App.Tests/MainFormTests.cs) | Physical delivered keys/layout and desktop focus |
| FR-002, FR-003 | [Session](../src/KeyEventLogger.Core/RecordingSession.cs), [session cases](../tests/KeyEventLogger.Core.Tests/RecordingSessionTests.cs) and native controls | Human lifecycle/navigation |
| FR-004, NFR-002 | [Export flow](../src/KeyEventLogger.App/Forms/MainForm.Export.cs), [real file cases](../tests/KeyEventLogger.App.Tests/TextLogExporterTests.cs), [UI integration](../tests/KeyEventLogger.App.Tests/ExportUiTests.cs) | Human native Save/overwrite/error flow |
| SEC-001 through SEC-004 | Practice-only subscriptions, activity/focus/stop gates, no production hook/polling/network API; [code walkthrough](CODE_GUIDE.md) | External-app physical check; no claim of system monitoring |
| NFR-001 / explanation | App/Core boundaries, small value/session/export responsibilities, [walkthrough](CODE_GUIDE.md) | Student oral explanation |
| SUB-001 through SUB-004 | Public repo, source/README/exclusions, authentic captures and staged commit history | Current-head CI tracked on PR #5; owner image acceptance remains pending |
| SUB-005 / SUB-006 | Explicit [submission procedure](SUBMISSION.md) | Second computer, final owner acceptance and Classroom action |

All required application features are implemented. This audit does not waive the owner-directed student-number omission or mark missing manual evidence complete.

## Owner-requested quality review — 2026-11-20

The merged PR #5 is the historical preparation baseline. The owner then requested a more polished interface, stronger local safeguards, better code organization and precise documentation.

| Change | Actual evidence | Acceptance boundary |
| --- | --- | --- |
| Three-column read-only event table and Down/Up guidance | Native layout/edit/count tests and genuine source-c393378 captures | New owner visual/image approval pending |
| Bounded retention | Core/UI capacity, no-eviction and Clear/restart regression | Default 10,000 events; no silent rolling history |
| Export path/error safeguards | Superscript/whitespace aliases and raw-error redaction cases | Still ordinary plaintext and not a filesystem sandbox |
| Dependency policy | Official NuGet sources, restore audit, dated advisory query | Known-source snapshot, no blanket security claim |
| Precise explanation | [User guide](USER_GUIDE.md), [security boundary](SECURITY.md), source-linked CODE_GUIDE | Human native/focus/display/F5 and second-computer results remain separately pending |

No statistics, filtering, global input capture, network delivery or automatic saving is added. Current full check: 92 cases (30 Core + 62 App), zero failures/skips/build warnings/errors.
