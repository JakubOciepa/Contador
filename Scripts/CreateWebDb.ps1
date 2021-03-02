param(
  [Parameter(Mandatory=$true)][string]$DatabaseName,
  [Parameter(Mandatory=$true)][string]$DatabaseUsername,
  [Parameter(Mandatory=$true)][string]$DatabasePwd
)

$SQLTables = Get-ChildItem "../src/Contador.Web/Database/dbo/Tables" -Filter "*.sql" -Recurse
$SQLProcedures = Get-ChildItem "../src/Contador.Web/Database/dbo/StoredProcedures" -Filter "*.sql" -Recurse
Write-Host "Creating or replacing database..."
mysql -u $DatabaseUsername --password=$DatabasePwd -e "CREATE or REPLACE DATABASE $DatabaseName"
foreach ($file in $SQLTables) {
  Write-Host "Updating database with file: $($file.Name)..."
  mysql -u $DatabaseUsername --password=$DatabasePwd $DatabaseName -e "source $file"
}

foreach ($file in $SQLProcedures) {
  Write-host "Updating database with file $($file.Name)..."
  mysql -u $DatabaseUsername --password=$DatabasePwd $DatabaseName -e "source $file"
}

