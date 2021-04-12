# TO DO:
# 	- Move each steps to separate script file;
# 	- Add docs;
# 	- Cleanup this a little

$DbUserName = ""
$DbPasswd = ""
$DbName = ""
$ScriptPath = Split-Path $($MyInvocation.MyCommand.Path) -Parent

$SetupDb = Read-Host "Do you want to setup Contador.Server db? [Y/n]"

if ($SetupDb -eq "Y") {
	$DbName = Read-Host "Database name"
	$DbCredentials = Get-Credential -Message "Database server credentials"
	$DbPasswd = ConvertFrom-SecureString $DbCredentials.Password -AsPlainText
	$DbUserName = $DbCredentials.UserName
	./CreateWebDb.ps1 -DatabaseName $DbName -DatabaseUserName $DbUserName -DatabasePwd $DbPasswd;
}

$SetupSecrets = Read-Host "Do you want to setup dotnet secrets for db connection string? [Y/n]"
if ($SetupSecrets -eq "Y") {
	
	$projectPath = Get-ChildItem -Recurse -Path $($ScriptPath + "/../" ) -Filter Contador.Web.Server.csproj;
	
	dotnet user-secrets clear --project $projectPath
	dotnet user-secrets init --project $projectPath
	
	if ($SetupDb -eq "Y") {
		dotnet user-secrets set "DbContent" "user=$DbUserName;password=$DbPasswd;database=$DbName" -p "$($projectPath.FullName)";
	}

	else {
		$DbName = Read-Host "Database name"
		$DbCredentials = Get-Credential -Message "Database server credentials"
		$DbPasswd = ConvertFrom-SecureString $DbCredentials.Password -AsPlainText
		$DbUserName = $DbCredentials.UserName
		
		[void](dotnet user-secrets set "DbContent" "user=$DbUserName;password=$DbPasswd;database=$DbName" -p "$($projectPath.FullName)")
	}
}

$SetupWsl = Read-Host "Do you want to setup WSL? [Y/n]"
if ($SetupWsl -eq "Y") {

	$launchSettingsPath = Get-ChildItem -Recurse -Path $($ScriptPath + "/../src/*/Server/Properties/" ) -Filter launchSettings.json
	if ($launchSettingsPath.Length -ne 0) {
		$Ip = Read-Host "WSL IP address"

		$settings = Get-Content $launchSettingsPath.FullName | ConvertFrom-Json
		$settings.profiles | ForEach-Object { if ($null -ne $_."WSL 2") {
				$_."WSL 2".launchUrl = "https://$($Ip):5001/swagger"
				$_."WSL 2".environmentVariables.ASPNETCORE_URLS = "https://$($Ip):5001"
			}
			else {
				$WslContent = @"
				{
					"commandName": "WSL2",
					"launchBrowser": true,
					"launchUrl": "https://$($Ip):5001/swagger",
					"environmentVariables": "",
					"distributionName": ""
				}
"@

				$WslEnvVars = @"
{
	"ASPNETCORE_URLS": "https://$($Ip):5001",
	"ASPNETCORE_ENVIRONMENT": "Development"
}
"@
				$settings.profiles | Add-Member -Name "WSL 2" -Value (ConvertFrom-Json $WslContent) -MemberType NoteProperty
				$settings.profiles."WSL 2".environmentVariables = ConvertFrom-Json $WslEnvVars
			}
		}
		$settings | ConvertTo-Json -depth 32 | Set-Content $launchSettingsPath.FullName
	}

}

$SetupHooks = Read-Host "Do you want to setup git hooks? [Y/n]"
if ($SetupHooks -eq "Y") {
	Copy-Item -Path $($ScriptPath + "/pre-push.sh") -Destination $($ScriptPath + "/../.git/hooks/pre-push")
	Copy-Item -Path $($ScriptPath + "/prepare-commit-msg.sh") -Destination $($ScriptPath + "/../.git/hooks/prepare-commit-msg")
	Copy-Item -Path $($ScriptPath + "/getPrefixFromPath.py") -Destination $($ScriptPath + "/../.git/hooks/getPrefixFromPath.py")
}
