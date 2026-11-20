# Setup and troubleshooting

## Requirements

Windows 10/11, Git, .NET SDK 10.0.402 or a newer stable 10.0.4xx patch, and NuGet access for the first restore. VS Code plus the recommended C# extensions is optional. App uses Windows Forms; no separate workload or third-party runtime is needed.

Open the repository root, not its parent or another assignment. The local folder may be named keyEventLoger; remote/repository and .NET project names remain KeyEventLogger.

## Commands

Run from PowerShell:

```powershell
./scripts/dev.ps1 doctor
./scripts/dev.ps1 restore
./scripts/dev.ps1 build
./scripts/dev.ps1 test
./scripts/dev.ps1 check
./scripts/dev.ps1 run
```

Doctor prints the resolved executable/SDK/runtime and Git version. Build/test use Release. Check restores, builds without another restore, tests the built solution, then verifies formatting. Run launches the App project. Output stays in ignored bin/obj/TestResults/.local directories.

The script temporarily sets DOTNET_ROOT to the selected PATH executable's directory and DOTNET_CLI_HOME to .local/dotnet, then restores prior values and the caller's location. This avoids accidentally mixing system and private runtime installations.

global.json uses latestPatch: it permits servicing within 10.0.4xx, requires at least 10.0.402, disallows prereleases, and does not silently choose another feature band.

## VS Code

Install recommended extensions from .vscode/extensions.json. Ctrl+Shift+B runs the build task. The check task runs full validation. F5 uses the shared coreclr configuration and Release DLL after building.

The command-line lifecycle probe does not prove that an owner pressed F5; actual debugger verification remains a separate manual check.

## Common problems

| Symptom | Action |
| --- | --- |
| SDK not found | Run dotnet --list-sdks; install a matching 10.0.4xx SDK and reopen the shell |
| Apphost cannot find runtime | Use scripts/dev.ps1 run so DOTNET_ROOT matches the selected dotnet |
| NuGet restore fails | Check connectivity and NuGet sources; do not add credentials to Git |
| WindowsDesktop unavailable | Use supported Windows with the matching SDK/runtime |
| PowerShell policy blocks scripts | Follow the machine's script-signing/execution policy; do not weaken system-wide policy |
| Missing test discovery | Current review has 30 Core and 61 App cases; check restore, test SDK/adapters and project targets |
| Export accidentally appears in Git | Unstage it without deleting user data; place future development exports under exports/ or *.keyevents.txt |

The application starts Stopped. Press Start, then use synthetic non-sensitive practice input to inspect Down/Up events. Stop retains records. Clear removes events while preserving practice text/state. Leaving/minimizing stops recording; return requires Start. Tab navigates normally. Export becomes available when events exist. It stops recording, requests a local .txt destination, and reports a separate result. Cancel/failure keep records; Start is required again. Use ignored exports/ or *.keyevents.txt for development exports. See [export behavior](EXPORT.md).

## Final review evidence

Use the exact candidate commit when reproducing final checks. A fresh clone's doctor/check and actual executable launch verify the documented dependencies; the ignored development probes are not required to build or run.

Record the commit, Windows and SDK with results. Physical F5/display checks and a second Windows computer remain separate from command-line or same-computer clean-clone success. See [owner procedure](SUBMISSION.md) and [code walkthrough](CODE_GUIDE.md).

## Interface/security review — 2026-11-20

While this PR is unmerged, switch to feat/usability-security-review after cloning to inspect the redesigned table and retention boundary. NuGet.Config selects official package/audit sources. Restore audits all dependencies from low severity and treats warnings as errors; failed audit retrieval is a check failure, not a clean security result. Keep source availability current; do not suppress advisories merely to make CI green.

Run dotnet list KeyEventLogger.slnx package --vulnerable --include-transitive for an explicit advisory query. No known advisories were reported on 2026-11-20; this is time/source dependent. See [security](SECURITY.md) and [user guide](USER_GUIDE.md). The new default window is 1040x720 client at 96 DPI, with a 900x680 minimum window. High-contrast colors and physical DPI/F5 still require owner review.
