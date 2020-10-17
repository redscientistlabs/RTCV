using module ".\Modules\RTCVCommand.psm1"
using module ".\Modules\Project.psm1"

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("RTCV","Bizhawk","CemuStub","FileStub","CemuStub","UnityStub","ProcessStub","melonDS","dolphin","pcsx2","citra","Dosbox")]
    [String[]]$ProjectsToRun,

    [switch]$Release = $false # Run with the Release build configurations
)

$ScriptDirectory = Split-Path -parent $PSCommandPath
$projects = [Project]::LoadFromJson($ScriptDirectory)
foreach ($project in $projects) {
    if($ProjectsToRun | Where-Object { $_ -like $project.PrintFriendlyName })
    {
        $project.Run($Release)
    }
}
