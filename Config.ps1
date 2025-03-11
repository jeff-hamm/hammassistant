function Config-Hammassistant(
    [string]$Name
) {
    $InformationPreference="Continue"
    $DebugPreference="Continue"
    Import-Module "$PSScriptRoot\submodules\NetDaemonPs" -Force
    Set-NetDaemonSrcRoot "$PSScriptRoot\src\"
    Set-NetDaemonEnvRoot "$PSScriptRoot\Environments"
    Set-NetDaemonCodeGen (Resolve-Path "$PsScriptRoot\submodules\netdaemon\src\HassModel\NetDaemon.HassModel.CodeGenerator\bin\Debug\net9.0\NetDaemon.HassModel.CodeGenerator.exe")
    Get-NetDaemon $Name
}