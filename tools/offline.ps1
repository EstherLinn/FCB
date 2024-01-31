$startPath = Split-Path $PSScriptRoot -Parent
Push-Location $startPath
$TmpPath = Join-Path -Path $startPath -ChildPath "temp\packages"
$NugetFolder = Join-Path -Path $startPath -ChildPath "build\nugets"

#Set-ExecutionPolicy RemoteSigned
$global:OutputEncoding = [Console]::InputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8;
$global:PSDefaultParameterValues['*:Encoding'] = 'utf8'
$global:InformationPreference = 'Continue'
$global:ErrorActionPreference = 'STOP'
$global:ProgressPreference = 'silentlyContinue'

Function Recycle-Item {
    param(
        [string]$Path
    )

    if (!([System.IO.Path]::IsPathRooted($Path))) {
        Write-Host "The string '$Path' is not a valid path." -ForegroundColor Red
    }
    elseif (Test-Path $Path) {
        Start-Sleep -s 1
        $shell = New-Object -ComObject 'Shell.Application'
        $shell.NameSpace(0).ParseName($Path).InvokeVerb('Delete')
        $shell = $null
    }
    else {
        Write-Host "The path '$Path' does not exist." -ForegroundColor Yellow
    }
}

Function Invoke-DownloadNupkg {
    param (
        [string]$packagesFolder,
        [string]$tmpFolder
    )

    dotnet restore --packages $tmpFolder
    $folders = Get-ChildItem -Path $tmpFolder -Directory
    foreach ($folder in $folders) {
        $packageName = $folder.Name
        $subFolder = Get-ChildItem -Path $folder.FullName -Directory
        $versions = $subFolder.Name
        foreach ($version in $versions) {
            # https://learn.microsoft.com/en-us/nuget/api/package-base-address-resource
            $lowerId = $packageName.ToLower()
            $lowerVersion = $version.ToLower()
            [string]$downloadUrl = ''
            $path = "$packagesFolder\$packageName.$version.nupkg"

            if ((Get-ChildItem -Path $tmpFolder -Directory -Filter "sitecore.*").Name -contains $packageName ) {
                $downloadUrl = "https://sitecore.myget.org/F/sc-packages/api/v2/package/$lowerId/$lowerVersion"
            }
            else {
                $downloadUrl = "https://api.nuget.org/v3-flatcontainer/$lowerId/$lowerVersion/$lowerId.$lowerVersion.nupkg"
            }
            try {
                if(!(Test-Path $path)) {
                    Write-Host "[Skipped] $packageName"
                } else {
                    Write-Host "[Download] $packageName"
                    Invoke-WebRequest -Uri $downloadUrl -OutFile $path
                }
            }
            catch {
                Write-Host "Failed to download $downloadUrl" -ForegroundColor Red
            }
        }
    }
}

[System.IO.Directory]::CreateDirectory($NugetFolder) | Out-Null
Invoke-DownloadNupkg $NugetFolder $TmpPath
Recycle-Item $TmpPath -Force -Recurse