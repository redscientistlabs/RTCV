param (
    [switch]$Release = $false, # Run with the Release build configurations
    [switch]$Clean = $false, # Clean each project before building it
    [switch]$NoRestore = $false, # Don't restore each project before building it (without this flag, default behavior is to restore each project)
    [switch]$WhatIf = $false # Just print the commands that would be run
)

class RTCVCommand {
    [string]$PrintFriendlyName
    [string]$Command

    RTCVCommand(
        [string]$n,
        [string]$c
    ){
        $this.PrintFriendlyName = $n
        $this.Command = $c
    }
}

class Project {
    [string]$PrintFriendlyName
    [string]$RelativePathToSln
    [string]$MSBuildArgs

    Project(
        [string]$n,
        [string]$p,
        [string]$b,
        [boolean]$release
    ){
        $this.PrintFriendlyName = $n
        $this.RelativePathToSln = $p
        if ($release)
        {
            $this.MSBuildArgs = "$b /property:Configuration=Release"
        }
        else
        {
            $this.MSBuildArgs = "$b /property:Configuration=Debug"
        }
    }

    [RTCVCommand] GetCleanCommand([string]$fullPath)
    {
        return [RTCVCommand]::new("Cleaning",`
                                  "msbuild '$($fullPath)' /t:clean /consoleloggerparameters:ErrorsOnly /nologo -m")
    }

    [RTCVCommand] GetRestoreCommand([string]$fullPath)
    {
        return [RTCVCommand]::new("Restoring",`
                                  "msbuild '$($fullPath)' /t:restore /consoleloggerparameters:ErrorsOnly /nologo -m")
    }

    [RTCVCommand] GetBuildCommand([string]$fullPath)
    {
        return [RTCVCommand]::new("Building",`
                                  "msbuild '$fullPath' $($this.MSBuildArgs -Split " ") /consoleloggerparameters:ErrorsOnly /nologo -m")
    }
}

Set-Variable -Name PROJECTS -Option ReadOnly -Value @(`
     [Project]::new("RTCV"       , "RTCV/RTCV.sln"                                               , "", $Release)`
    ,[Project]::new("Bizhawk"    , "Bizhawk-Vanguard\Real-Time Corruptor\BizHawk_RTC\BizHawk.sln", "", $Release)`
    ,[Project]::new("CemuStub"   , "CemuStub-Vanguard"                                           , "", $Release)`
    ,[Project]::new("FileStub"   , "FileStub-Vanguard\FileStub-Vanguard.sln"                     , "", $Release)`
    ,[Project]::new("UnityStub"  , "UnityStub-Vanguard\UnityStub-Vanguard.sln"                   , "", $Release)`
    ,[Project]::new("ProcessStub", "ProcessStub-Vanguard\ProcessStub-Vanguard.sln"               , "", $Release)`
    ,[Project]::new("melonDS"    , "melonDS-Vanguard\out\build\x64-Debug\melonDS.sln"            , "", $Release)`
    ,[Project]::new("dolphin"    , "dolphin-vanguard\Source\dolphin-emu.sln"                     , "/property:Platform=x64 /p:TreatWarningAsError=false", $true)` # Debug flavor of dolphin doesn't build correctly
    ,[Project]::new("pcsx2"      , "pcsx2-Vanguard\PCSX2_suite.sln"                              , "/property:Platform=Win32", $Release)`
    ,[Project]::new("citra"      , "citra-vanguard\build\citra.sln"                              , "", $Release)`
    ,[Project]::new("Dosbox"     , "DosboxStub-Vanguard\DosboxStub-Vanguard.sln"                 , "", $Release)`
    )

$ScriptDirectory = Split-Path -parent $PSCommandPath
foreach ($project in $PROJECTS)
{
    # A developer may not have all of the projects locally, or they may not be set up properly, so skip anything that's not found
    $SolutionPath = (Join-Path $ScriptDirectory -ChildPath (Join-Path "../.." $project.RelativePathToSln))
    if (-not (Test-Path $SolutionPath))
    {
        Write-Host "$($project.PrintFriendlyName) not found. Skipping..." -ForegroundColor Yellow
        continue
    }

    # Get all of the commands that should be run
    $commandsToRun = @()
    if ($Clean)
    {
        $commandsToRun = $commandsToRun + @($project.GetCleanCommand($SolutionPath))
    }

    if (-not $NoRestore)
    {
        $commandsToRun = $commandsToRun + @($project.GetRestoreCommand($SolutionPath))
    }

    $commandsToRun = $commandsToRun + @($project.GetBuildCommand($SolutionPath))

    if ($WhatIf)
    {
        foreach($cmd in $commandsToRun)
        {
            Write-Host $cmd.Command
        }
        continue
    }

    foreach($cmd in $commandsToRun)
    {
        Write-Progress -id 0 -activity "$($project.PrintFriendlyName)" -Status $cmd.PrintFriendlyName
        Invoke-Expression $cmd.Command
        if (-not ($LastExitCode -eq 0))
        {
            Write-Host "$($project.PrintFriendlyName) FAILED" -ForegroundColor Red
            continue;
        }
    }

    if ($LastExitCode -eq 0)
    {
        Write-Host "$($project.PrintFriendlyName) PASSED" -ForegroundColor Green
    }

    Write-Progress -id 0 -activity "$($project.PrintFriendlyName)" -Completed
}
