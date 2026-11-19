# Requirements and acceptance criteria

Source reviewed on **2026-11-19**: the supplied `YMH219_Ek_Uygulama_Odevleri_Ogrenci_Foyu.pdf`, pages 1, 2–3 and 5, plus the owner's classroom announcement. The PDF is reference material. Its local absolute path is not required to build the repository.

## Instructor requirements

| ID | Requirement | Acceptance criterion | Stage/status |
| --- | --- | --- | --- |
| FR-001 | Local keyboard events | Key names and times shown only for input within the active application | 02 implemented, native controlled-window tests; physical desktop check pending |
| FR-002 | Start/Stop | Explicit controls govern recording, with no appends while stopped | 02 implemented/tested |
| FR-003 | Clear | Clear removes displayed records | 02 implemented/tested; state/practice text retained |
| FR-004 | TXT export | User can save records to a local text file | 03, planned |
| FR-005 | Status | Visible recording state accurately reflects actual capture | 02 actual recording/stopped status and count implemented/tested |
| NFR-001 | Desktop/OOP | Working C#/.NET WinForms app with clear responsibilities and error handling | 02 UI/session/values implemented/tested; export errors pending |
| NFR-002 | File resilience | Export errors are reported without an application crash | 03, planned |
| SEC-001 | Active-window scope | Other applications and inactive/minimized periods yield no recorded input | 02 gates/stop behavior tested with owned windows; external-app physical check pending |
| SEC-002 | No system collection | No global hooks, hidden/background operation or Windows autostart | Required throughout |
| SEC-003 | No credential collection | No password/credential capture or other-app collection | Required throughout |
| SEC-004 | No transmission | Recorded events are never sent across a network | Required throughout |
| SUB-001 | Public separate repository | Source, .gitignore and README accessible on student's own account | Existing public KeyEventLogger selected; delivery tracked separately |
| SUB-002 | README | Purpose, technologies/packages, run steps, features, limitations and student details | Draft implemented; owner exception below |
| SUB-003 | Screenshots | At least two authentic screenshots embedded in final README | Two authentic Stage 02 captures embedded; owner/final refresh pending |
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
- [ ] FR-001 through FR-005 implemented and tested.
- [ ] Active-window/privacy boundaries verified with real focus changes.
- [ ] TXT success/cancel/failure checks complete.
- [ ] Two real screenshots and final README accepted.
- [ ] Second-computer check and owner sign-off.
- [ ] Final reviewed work published before deadline.
