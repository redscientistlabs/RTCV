param (
    [switch]$Release = $false, # Run with the Release build configurations
    [switch]$WhatIf = $false # Just print the commands that would be run
)

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
    $SolutionPath = (Join-Path $ScriptDirectory -ChildPath (Join-Path "../.." $project.RelativePathToSln))
    if (-not (Test-Path $SolutionPath))
    {
        Write-Host "$($project.PrintFriendlyName) not found. Skipping..." -ForegroundColor Yellow
        continue
    }

    $restoreCommand = "msbuild '$SolutionPath' /t:restore /consoleloggerparameters:ErrorsOnly /nologo -m"
    $compilationCommand = "msbuild '$SolutionPath' $($project.MSBuildArgs -Split " ") /consoleloggerparameters:ErrorsOnly /nologo -m"
    if ($WhatIf)
    {
        Write-Host $restoreCommand
        Write-Host $compilationCommand
        continue
    }

    Write-Progress -id 0 -activity "$($project.PrintFriendlyName)" -Status "Restoring"
    Invoke-Expression $restoreCommand

    Write-Progress -id 0 -activity "$($project.PrintFriendlyName)" -Status "Building"
    Invoke-Expression $compilationCommand
    if (-not ($LastExitCode -eq 0))
    {
        Write-Host "$($project.PrintFriendlyName) BUILD FAILED" -ForegroundColor Red
    }
    else
    {
        Write-Host "$($project.PrintFriendlyName) BUILD PASSED" -ForegroundColor Green
    }

    Write-Progress -id 0 -activity "$($project.PrintFriendlyName)" -Completed
}
