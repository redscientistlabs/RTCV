using module ".\Modules\RTCVCommand.psm1"
using module ".\Modules\Project.psm1"

param (
    [switch]$Release = $false, # Run with the Release build configurations
    [switch]$Clean = $false, # Clean each project before building it
    [switch]$NoRestore = $false, # Don't restore each project before building it (without this flag, default behavior is to restore each project)
    [switch]$WhatIf = $false # Just print the commands that would be run
)

$ScriptDirectory = Split-Path -parent $PSCommandPath
$projects = [Project]::LoadFromJson($ScriptDirectory)
foreach ($project in $projects)
{
    # A developer may not have all of the projects locally, or they may not be set up properly, so skip anything that's not found
    if (-not $project.CheckSolutionExists())
    {
        Write-Host "$($project.PrintFriendlyName) not found. Skipping..." -ForegroundColor Yellow
        continue
    }

    # Get all of the commands that should be run
    $commandsToRun = @()
    if ($Clean)
    {
        $commandsToRun = $commandsToRun + @($project.GetCleanCommand())
    }

    if (-not $NoRestore)
    {
        $commandsToRun = $commandsToRun + @($project.GetRestoreCommand())
    }

    $commandsToRun = $commandsToRun + @($project.GetBuildCommand())

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
