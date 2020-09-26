using module ".\RTCVCommand.psm1"

class Project {
    [string]$PrintFriendlyName
    [string]$SolutionPath
    [string]$MSBuildArgs
    [string]$ExecutablePath

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
        $this.ExecutablePath = (Join-Path $ScriptDirectory -ChildPath (Join-Path "../.." $jsonInput.exe))
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

    [void] Run()
    {
        Start-Process $this.ExecutablePath
    }
}
