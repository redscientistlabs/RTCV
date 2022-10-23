using module ".\RTCVCommand.psm1"

class Project {
    [string]$PrintFriendlyName
    [string]$SolutionPath
    [string]$MSBuildArgs
    [string]$DefaultExecutablePath
    [string]$DebugExecutablePath
    [string]$ReleaseExecutablePath

    static [Project[]] LoadFromJson([string]$ScriptDirectory)
    {
        $jsonProjects = Get-Content (Join-Path $ScriptDirectory "Projects.json") | ConvertFrom-Json
        [System.Collections.ArrayList]$projects = @()
        foreach ($jsonProject in $jsonProjects)
        {
            $projects.Add([Project]::new($jsonProject, $ScriptDirectory))
        }

        return $projects;
    }

    Project([object]$jsonInput, [string]$ScriptDirectory)
    {
        $this.PrintFriendlyName = $jsonInput.name
        $this.SolutionPath = (Join-Path $ScriptDirectory -ChildPath (Join-Path "../.." $jsonInput.solutionPath))

        if ($jsonInput.exe)
        {
            $this.DefaultExecutablePath = (Join-Path $ScriptDirectory -ChildPath (Join-Path "../.." $jsonInput.exe))
        }

        if ($jsonInput.debugExe)
        {
            $this.DebugExecutablePath = (Join-Path $ScriptDirectory -ChildPath (Join-Path "../.." $jsonInput.debugExe))
        }

        if ($jsonInput.releaseExe)
        {
            $this.ReleaseExecutablePath = (Join-Path $ScriptDirectory -ChildPath (Join-Path "../.." $jsonInput.releaseExe))
        }

        $this.MSBuildArgs = $($jsonInput.additionalBuildArguments)
        if ($jsonInput.forceReleaseFlavor)
        {
            $this.MSBuildArgs += " /property:Configuration=Release"
        }
        else
        {
            $this.MSBuildArgs += " /property:Configuration=Debug"
        }
    }

    [boolean] CheckSolutionExists()
    {
        return (Test-Path $this.SolutionPath);
    }

    [RTCVCommand] GetCleanCommand()
    {
        return [RTCVCommand]::new("Cleaning",`
                                  "msbuild '$($this.SolutionPath)' /t:clean /consoleloggerparameters:ErrorsOnly /nologo -m")
    }

    [RTCVCommand] GetRestoreCommand()
    {
        return [RTCVCommand]::new("Restoring",`
                                  "msbuild '$($this.SolutionPath)' /t:restore /consoleloggerparameters:ErrorsOnly /nologo -m")
    }

    [RTCVCommand] GetBuildCommand()
    {
        return [RTCVCommand]::new("Building",`
                                  "msbuild '$($this.SolutionPath)' $($this.MSBuildArgs -Split " ") /consoleloggerparameters:ErrorsOnly /nologo -m")
    }

    [void] Run([bool]$Release)
    {
        if ($Release -and $this.ReleaseExecutablePath)
        {
            Start-Process $this.ReleaseExecutablePath
        }
        elseif (!$Release -and $this.DebugExecutablePath)
        {
            Start-Process $this.DebugExecutablePath
        }
        elseif ($this.DefaultExecutablePath)
        {
            Start-Process $this.DefaultExecutablePath
        }
        else
        {
            Write-Host "$($this.PrintFriendlyName) FAILED. No executable available for configuration" -ForegroundColor Red
        }
    }
}
