param(
    [Parameter(Mandatory)]
    [AllowEmptyString()]
    [string]$Path
)
$startPath = Split-Path $PSScriptRoot -Parent
Push-Location $startPath
$ToolsPath = Join-Path -Path $startPath -ChildPath "temp\tools"
$TmpPath = Join-Path -Path $startPath -ChildPath "temp\deploy"
$TargetPath = Join-Path -Path $startPath -ChildPath "docker\deploy"

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

Function Publish-Tool {
    param(
        [string]$TempPath,
        [string]$TargetPath
    )
    [System.IO.Directory]::CreateDirectory($TempPath) | Out-Null

    $nuget = Join-Path -Path $TempPath -ChildPath "nuget.exe"
    Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile $nuget
    & $nuget install WebConfigTransformRunner -OutputDirectory $TempPath

    $WebConfigTransformRunner = Join-Path -Path $TempPath -ChildPath 'WebConfigTransformRunner.*\tools'
    $WebConfigTransformRunner = (Get-ChildItem -Path $WebConfigTransformRunner).FullName
    Move-Item -LiteralPath $WebConfigTransformRunner -Destination $TargetPath -Force
}

Function Cerate-DeployScript {
    param(
        [string]$Path,
        [string]$Zip
    )
    $text = @"
Push-Location `$PSScriptRoot
`$Zip = '$Zip'
Get-Website | Format-Table -AutoSize
`$siteName = Read-Host 'Input "Name"'
if(Get-IISSite -Name `$siteName){
    `$site = Get-Website -Name `$siteName
    Expand-Archive -Path `$Zip -DestinationPath `$site.PhysicalPath -Force

    `$xdtPath = (Join-Path -Path `$site.PhysicalPath -ChildPath 'App_Data\Transforms\Deploy\xdts')
    if (Test-Path `$xdtPath) {
        `$xdtDirectory = (Get-ChildItem `$xdtPath -Recurse).DirectoryName
        foreach (`$xdt in `$xdtDirectory) {
            if (Get-ChildItem -Path `$xdt -Filter *.xdt) {
                Invoke-TransformXmlDocTask -RootDirectoryPath `$site.PhysicalPath -XdtDirectory `$xdt
            }
        }
    }
}
"@
    $text | Out-File -FilePath (Join-Path -Path $Path -ChildPath "deploy.ps1") -Encoding utf8
}


if (!([System.IO.Path]::IsPathRooted($Path))) {
    $Path = $TargetPath
}

$MSBuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe
if ($MSBuild) {
    $publish = (Get-ChildItem -Path . -Filter Environment.Platform.csproj -Recurse).FullName
    if ($publish.Count -eq 1) {
        & $MSBuild $publish /p:publishUrl="$TmpPath" /p:configuration=Release /p:DeployOnBuild=true /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:platform="Any CPU"
        Start-Sleep -s 1

        [System.IO.Directory]::CreateDirectory($Path) | Out-Null
        $time = (Get-Date).ToString("yyyyMMddHHmmss")
        $zipFile = "deploy_$time.zip"
        Cerate-DeployScript $Path $zipFile
        $Path = (Join-Path -Path $Path -ChildPath $zipFile)
        Add-Type -AssemblyName System.IO.Compression.FileSystem
        [System.IO.Compression.ZipFile]::CreateFromDirectory($TmpPath, $Path, [System.IO.Compression.CompressionLevel]::Optimal, $false)

        Write-Host $Path -ForegroundColor Green

        Recycle-Item $TmpPath -Force -Recurse
    }
    else {
        Write-Host "*.csproj was not found." -ForegroundColor Yellow
    }
}
else {
    Write-Host "MSBuild.exe was not found." -ForegroundColor Yellow
}
