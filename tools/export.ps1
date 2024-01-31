param(
    [Parameter(Mandatory)]
    [AllowEmptyString()]
    [string]$Path
)
$startPath = Split-Path $PSScriptRoot -Parent
Push-Location $startPath
$TmpPath = Join-Path -Path $startPath -ChildPath "temp\export"
$TargetPath = Join-Path -Path $startPath -ChildPath "docker\export"

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

$DiffTree = (git diff-tree --no-commit-id --name-only -r HEAD) -Replace '/', '\'
foreach ($diff in $DiffTree) {
    $exportPath = Join-Path -Path $TmpPath -ChildPath $diff
    cmd /c "echo F | xcopy $diff $exportPath  /i /y"
}
Start-Sleep -s 1

if (!([System.IO.Path]::IsPathRooted($Path))) {
    $Path = $TargetPath
}
[System.IO.Directory]::CreateDirectory($Path) | Out-Null

$time = (Get-Date).ToString("yyyyMMddHHmmss")
$Path = Join-Path -Path $Path -ChildPath "export_$time.zip"

Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($TmpPath, $Path, [System.IO.Compression.CompressionLevel]::Optimal, $false)

Write-Host $Path -ForegroundColor Green

Recycle-Item $TmpPath -Force -Recurse

