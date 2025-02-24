param(
    [string]$Name
)
$InformationPreference="Continue"
Import-Module $"$PSScriptRoot\submodules\NetDaemonPs" -Force
Set-NetDaemonEnvRoot "$PSScriptRoot\Environments"
Set-NetDaemonCodeGen (Resolve-Path "$PsScriptRoot\..\submodules\netdaemon\src\HassModel\NetDaemon.HassModel.CodeGenerator\bin\Debug\net9.0\NetDaemon.HassModel.CodeGenerator.exe")
Get-NetDaemon $Name