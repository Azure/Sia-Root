# installing rerequisites and cloning repos for SIA-EventUI

# all bits are x64 version unless specified
$nodejsUrl = "https://nodejs.org/dist/v6.11.3/node-v6.11.3-x64.msi"
$dotnetCoreSDKUrl = "https://download.microsoft.com/download/0/F/D/0FD852A4-7EA1-4E2A-983A-0484AC19B92C/dotnet-sdk-2.0.0-win-x64.exe"
$dotnetCoreRuntimeUrl = "https://download.microsoft.com/download/5/6/B/56BFEF92-9045-4414-970C-AB31E0FC07EC/dotnet-runtime-2.0.0-win-x64.exe"

# registry key check to see if rerequisites are already installed
$nodejsRegKeyCheck = Get-ItemProperty 'HKLM:\SOFTWARE\Node.js' -ErrorAction SilentlyContinue
$dotnetCoreSDKRegKeyCheck = Get-ItemProperty 'HKLM:\SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedhost' -ErrorAction SilentlyContinue
$dotnetCoreRuntimeRegKeyCheck = Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\ASP.NET Core\Runtime Package Store\v2.0\RTM' -ErrorAction SilentlyContinue

function CheckIfElevated()
{
      Write-Host "Checking if PowerShell is running with elevated permissions..."
      $wid=[System.Security.Principal.WindowsIdentity]::GetCurrent()
      $prp=new-object System.Security.Principal.WindowsPrincipal($wid)
      $adm=[System.Security.Principal.WindowsBuiltInRole]::Administrator
      $IsAdmin=$prp.IsInRole($adm)
      if ($IsAdmin)
      {
        Write-Host "Verified - PowerShell is running in Admin mode"
        return $true
      }
      else
      {
        Write-Host "You are not running PowerShell with elevated permissions. Please re-launch Powershell in Administrator mode and run this script." -ForegroundColor Yellow
        Write-Host "Press any key to exit..."
        $In = Read-Host
        return $false
      }
}

function CheckIfUnrestricted()
{
      Write-Host "Checking if PowerShell is running with Unrestricted execution policy..."
      $executionPolicy = Get-ExecutionPolicy
      if($executionPolicy -eq "Unrestricted") {
        Write-Host "Verified - PowerShell Execution Policy is set to Unrestricted"
        return $true
      }
      else {
        Write-Host "You are not running PowerShell with elevated permissions. Please re-launch Powershell after executing the command 'set-executionpolicy unrestricted' " -ForegroundColor Yellow
        Write-Host "Press any key to exit..."
        $In = Read-Host
        return $false
      }
}

function invoke-download(){
	param(
		[string]$url,
		[string]$serviceName
	)
  
	$fileExtension = $url.Substring($url.Length-4)
	$currentDirectory = (Get-Location).Path
	$outputFileName = "$currentDirectory\$serviceName" + $fileExtension
	$webClient = New-Object System.Net.WebClient
	$webClient.DownloadFile($url,$outputFileName)
	
	if($fileExtension -eq ".exe")
	{
		$install = (Start-Process -FilePath $outputFileName -ArgumentList /passive -PassThru -Wait)
	}
	else
	{
		$argumentlist = "/i [application] /qb"
		$argumentlist = $argumentlist.Replace("[application]",$outputFileName)
		$install = (Start-Process -FilePath "C:\Windows\System32\msiexec.exe" -ArgumentList $argumentlist -PassThru -Wait)
	}

	if($install.ExitCode -eq 0)
	{
        Write-Output "$serviceName successfully installed"
        Remove-Item $outputFileName
	}
	else
	{
		Write-Output "$serviceName installation failed with Exit Code: $install.ExitCode"
	}
}

if(!(CheckIfElevated) -or !(CheckIfUnrestricted))
{
  exit
}

Write-Host "Checking if Node.js is installed..."
if($nodejsRegKeyCheck) {
    Write-Output "Node.js is already installed. Skipping installation..."
}
else {
    Write-Output "Downloading and installing Node.js"
    invoke-download($nodejsUrl) ("Node.js")
}

Write-Host "Checking if Dotnet Core SDK is installed..."
if($dotnetCoreSDKRegKeyCheck) {
    Write-Output "Dotnet Core SDK is already installed. Skipping installation..."
}
else {
    Write-Output "Downloading and installing Dotnet Core SDK"
    invoke-download($dotnetCoreSDKUrl) ("DotnetCoreSDK")
}

Write-Host "Checking if Dotnet Core Runtime is installed..."
if($dotnetCoreRuntimeRegKeyCheck) {
    Write-Output "Dotnet Core Runtime is already installed. Skipping installation..."
}
else {
    Write-Output "Downloading and installing Dotnet Core Runtime"
    invoke-download($dotnetCoreRuntimeUrl) ("DotnetCoreRuntime")
}

# assuming the script is running from Sia-Root directory
# moving up one level above and going back to reposDir
$reposDir = Split-Path (Get-Location).Path
Push-Location $reposDir

if (Test-Path Sia-EventUI) {
    Write-Output "Removing existing Sia-EventUI folder..."
    Remove-Item Sia-EventUI -Recurse -Force -ErrorAction Ignore
}

git clone https://github.com/Azure/Sia-EventUI.git
Push-Location Sia-EventUI

npm install

Copy-Item cfg\constExample.js cfg\localhost.const.js -Recurse -Force

Write-Output "`nSIA-EventUI is now installed with the rerequisites and cloned with the repos."
Write-Output "You may now start the UI with 'npm start', and then open http://localhost:3000 in your browser.`n"