[CmdletBinding()]
  
param(
[parameter(Mandatory=$true,ParameterSetName = "Set 1")]
[ValidateNotNullOrEmpty()]
[String]$ProjectPath,

[parameter(Mandatory=$true,ParameterSetName = "Set 1")]
[ValidateNotNullOrEmpty()]
[String[]]$AdditionalConfigFiles,

[parameter(Mandatory=$true,ParameterSetName = "Set 1")]
[ValidateNotNullOrEmpty()]
[String]$AdditionalConfigurations
)
  
 $appSettings = '{}' | ConvertFrom-Json
 $appSettingFile = [IO.Path]::Combine($ProjectPath, 'appsettings.json')
 Write-Host "appSettingFile: $appSettingFile"
 if (Test-Path $appSettingFile) {
	$appSettings = (Get-Content -Raw -Path $appSettingFile)
	$appSettings = ($appSettings) -join "`n" | ConvertFrom-Json
 }
 
 ForEach ($configFile in $AdditionalConfigFiles) {
   $configFile = [IO.Path]::Combine($ProjectPath, $configFile)
   $config = (Get-Content -Raw -Path $configFile) -join "`n" | ConvertFrom-Json
   $config.psobject.Properties | ForEach-Object {
     $appSettings | Add-member -MemberType $_.MemberType -Name $_.Name -Value $_.Value -Force
   }
 }
 
 $configurations = $AdditionalConfigurations | ConvertFrom-Json
 $configurations.psobject.Properties | ForEach-Object {
	 $appSettings | Add-member -MemberType $_.MemberType -Name $_.Name -Value $_.Value -Force
 }
 
 $appSettings = ($appSettings | ConvertTo-Json -Depth 10)
 Write-Host "appSettings: $appSettings" 
 
 $appSettings | out-file($appSettingFile)