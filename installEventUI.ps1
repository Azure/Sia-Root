# installing prerequisites and cloning source codes for SIA-EventUI on Windows PC

# checking registry key to see if any prerequisites is already installed
# all bits are x64 Windows PC versions unless specified
$gitHash = @{
    name = "Git"
    url = "https://github.com/git-for-windows/git/releases/download/v2.14.2.windows.1/Git-2.14.2-64-bit.exe"
    regKeyCheck = "Get-ItemProperty 'HKLM:\SOFTWARE\GitForWindows' -ErrorAction SilentlyContinue"
}
$nodeHash = @{
    name = "Node.js"
    url = "https://nodejs.org/dist/v6.11.3/node-v6.11.3-x64.msi"
    regKeyCheck = "Get-ItemProperty 'HKLM:\SOFTWARE\Node.js' -ErrorAction SilentlyContinue"
}
$dotnetCoreSDKHash = @{
    name = "dotnetCoreSDK"
    url = "https://download.microsoft.com/download/0/F/D/0FD852A4-7EA1-4E2A-983A-0484AC19B92C/dotnet-sdk-2.0.0-win-x64.exe"
    regKeyCheck = "Get-ItemProperty 'HKLM:\SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedhost' -ErrorAction SilentlyContinue"
}
$dotnetCoreRuntimeHash = @{
    name = "dotnetCoreRuntime"
    url = "https://download.microsoft.com/download/5/6/B/56BFEF92-9045-4414-970C-AB31E0FC07EC/dotnet-runtime-2.0.0-win-x64.exe"
    regKeyCheck = "Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\ASP.NET Core\Runtime Package Store\v2.0\RTM' -ErrorAction SilentlyContinue"
}

# puttig all hash into a single array for looping actions later
$prereqArr = $gitHash, $nodeHash, $dotnetCoreSDKHash, $dotnetCoreRuntimeHash

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
        Write-Host "You are not running PowerShell with elevated permissions. Please re-launch Powershell in Administrator mode and run the script again." -ForegroundColor Yellow
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
        Write-Host "You are not running PowerShell with elevated permissions. Please re-launch Powershell after executing the command: `nset-executionpolicy unrestricted " -ForegroundColor Yellow
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

foreach ($prereq in $prereqArr) {
    Write-Host "Checking if $($prereq.name) is installed..."
    if(Invoke-Expression $prereq.regKeyCheck) {
        Write-Output "$($prereq.name) is already installed. Skipping installation..."
        
    }
    else {
        Write-Output "Downloading and installing $($prereq.name)"
        invoke-download($prereq.url) ($prereq.name)
    }
}

# refreshing the PowerShell prompt after Node.js installation to avoid relaunching the prompt
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User") 

# checking if current working directory is under Sia-Root or Sia-EventUI
# moving up one level above to go back to repos directory if needed
if ((Get-Location).Path -match "Sia-Root" -or (Get-Location).Path -match "Sia-EventUI") {
    $reposDir = Split-Path (Get-Location).Path
    Write-Output "Moving up one directory level to $reposDir"
    Push-Location $reposDir
}

if (Test-Path Sia-EventUI) {
    $removeMsg = "Sia-EventUI folder already existed. `nEnter [Y] Yes to remove and then re-clone the folder. Or enter any other key to skip this step."
    $removeResponse = Read-Host -Prompt $removeMsg
    if (($removeResponse -eq "y") -or ($removeResponse -eq "yes")) {
        Write-Output "Deleting existing Sia-EventUI folder..."
        Remove-Item Sia-EventUI -Recurse -Force -ErrorAction Ignore
        Write-Output "Cloning Sia-EventUI source code from GitHub again..."
        git clone https://github.com/Azure/Sia-EventUI.git
    }
}
else {
    Write-Output "Sia-EventUI is not there. Cloning the source code from GitHub..."
    git clone https://github.com/Azure/Sia-EventUI.git
}

Push-Location Sia-EventUI

#npm install @aspnet/signalr-client
npm install

# creating localhost.const from constExample.js as part of the requirements
Copy-Item cfg\constExample.js cfg\localhost.const.js -Recurse -Force

Write-Output "`nSIA-EventUI is now installed successfully with the prerequisites and source files."
Write-Output "You may now start the UI with 'npm start', and then open http://localhost:3000 in your browser.`n"