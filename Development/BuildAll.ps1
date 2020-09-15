class Project {
    [string]$PrintFriendlyName
    [string]$RelativePathToSln
    [string]$MSBuildArgs

    Project(
        [string]$n,
        [string]$p,
        [string]$b
    ){
        $this.PrintFriendlyName = $n
        $this.RelativePathToSln = $p
        $this.MSBuildArgs = $b
    }
}

Set-Variable -Name PROJECTS -Option ReadOnly -Value @(`
     [Project]::new("RTCV"       , "RTCV/RTCV.sln"                                               , "")`
    ,[Project]::new("Bizhawk"    , "Bizhawk-Vanguard\Real-Time Corruptor\BizHawk_RTC\BizHawk.sln", "")`
    ,[Project]::new("CemuStub"   , "CemuStub-Vanguard"                                           , "")`
    ,[Project]::new("FileStub"   , "FileStub-Vanguard\FileStub-Vanguard.sln"                     , "")`
    ,[Project]::new("UnityStub"  , "UnityStub-Vanguard\UnityStub-Vanguard.sln"                   , "")`
    ,[Project]::new("ProcessStub", "ProcessStub-Vanguard\ProcessStub-Vanguard.sln"               , "")`
    ,[Project]::new("melonDS"    , "melonDS-Vanguard\out\build\x64-Debug\melonDS.sln"            , "")`
    ,[Project]::new("dolphin"    , "dolphin-vanguard\Source\dolphin-emu.sln"                     , "/property:Configuration=Release /property:Platform=x64 /p:TreatWarningAsError=false")`
    ,[Project]::new("pcsx2"      , "pcsx2-Vanguard\PCSX2_suite.sln"                              , "/property:Configuration=Debug /property:Platform=Win32")`
    ,[Project]::new("citra"      , "citra-vanguard\build\citra.sln"                              , "")`
    ,[Project]::new("Dosbox"     , "DosboxStub-Vanguard\DosboxStub-Vanguard.sln"                 , "")`
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

    Write-Progress -id 0 -activity "$($project.PrintFriendlyName)" -Status "Restoring"
    msbuild $SolutionPath /t:restore /consoleloggerparameters:ErrorsOnly /nologo -m

    Write-Progress -id 0 -activity "$($project.PrintFriendlyName)" -Status "Building"
    msbuild $SolutionPath $($project.MSBuildArgs -Split " ") /consoleloggerparameters:ErrorsOnly /nologo -m
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
