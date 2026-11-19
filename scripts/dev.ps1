param(
    [ValidateSet('doctor', 'restore', 'build', 'test', 'check', 'run')]
    [string]$Command = 'doctor'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$previousDotnetRoot = $env:DOTNET_ROOT
$previousCliHome = $env:DOTNET_CLI_HOME

function Invoke-Dotnet {
    param([string[]]$Arguments)
    & $script:dotnetPath @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet $($Arguments -join ' ') failed with exit code $LASTEXITCODE."
    }
}

Push-Location $projectRoot
try {
    $script:dotnetPath = (Get-Command dotnet -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source
    $env:DOTNET_ROOT = Split-Path -Parent $script:dotnetPath
    $env:DOTNET_CLI_HOME = Join-Path $projectRoot '.local/dotnet'

    switch ($Command) {
        'doctor' {
            Write-Output "Project: $projectRoot"
            Write-Output "SDK executable: $script:dotnetPath"
            Write-Output "Session DOTNET_ROOT: $env:DOTNET_ROOT"
            Write-Output "Session DOTNET_CLI_HOME: $env:DOTNET_CLI_HOME"
            Invoke-Dotnet -Arguments @('--info')
            Invoke-Dotnet -Arguments @('--version')
            & git --version
            if ($LASTEXITCODE -ne 0) { throw 'Git is required.' }
        }
        'restore' { Invoke-Dotnet -Arguments @('restore', 'KeyEventLogger.slnx') }
        'build' { Invoke-Dotnet -Arguments @('build', 'KeyEventLogger.slnx', '-c', 'Release') }
        'test' { Invoke-Dotnet -Arguments @('test', 'KeyEventLogger.slnx', '-c', 'Release', '--logger', 'trx', '--results-directory', 'TestResults') }
        'check' {
            Invoke-Dotnet -Arguments @('restore', 'KeyEventLogger.slnx')
            Invoke-Dotnet -Arguments @('build', 'KeyEventLogger.slnx', '-c', 'Release', '--no-restore')
            Invoke-Dotnet -Arguments @('test', 'KeyEventLogger.slnx', '-c', 'Release', '--no-build', '--logger', 'trx', '--results-directory', 'TestResults')
            Invoke-Dotnet -Arguments @('format', 'KeyEventLogger.slnx', '--verify-no-changes', '--no-restore')
        }
        'run' { Invoke-Dotnet -Arguments @('run', '--project', 'src/KeyEventLogger.App', '-c', 'Release') }
    }
}
finally {
    $env:DOTNET_ROOT = $previousDotnetRoot
    $env:DOTNET_CLI_HOME = $previousCliHome
    Pop-Location
}
