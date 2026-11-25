$ErrorActionPreference='Stop'
Set-Location $PSScriptRoot
$root = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
$data = Join-Path $root 'mysql-local'
if (!$env:OOAD_PASSWORD -and (Test-Path (Join-Path $data 'demo-password'))) {
    $env:OOAD_PASSWORD=(Get-Content (Join-Path $data 'demo-password') -Raw).Trim()
    $env:OOAD_PORT='33316'
    if (!(Get-NetTCPConnection -LocalPort 33316 -State Listen -ErrorAction SilentlyContinue)) {
        Start-Process 'C:\Program Files\MySQL\MySQL Server 8.4\bin\mysqld.exe' -WindowStyle Hidden -ArgumentList @('--no-defaults',('--datadir="{0}"' -f $data),'--port=33316','--bind-address=127.0.0.1','--mysqlx=OFF')
        for($i=0;$i -lt 40;$i++){ if(Get-NetTCPConnection -LocalPort 33316 -State Listen -ErrorAction SilentlyContinue){break};Start-Sleep -Milliseconds 500 }
    }
} elseif (!$env:OOAD_PASSWORD) {
    if (!$env:OOAD_PORT) { $env:OOAD_PORT='3306' }
    $env:OOAD_USER=Read-Host 'Tai khoan MySQL'
    $pw=Read-Host 'Mat khau MySQL' -AsSecureString
    $env:OOAD_PASSWORD=[Net.NetworkCredential]::new('',$pw).Password
}
if (!$env:OOAD_EMAIL_DIR) { $env:OOAD_EMAIL_DIR=Join-Path $data 'ThuXacNhan' }
$env:ASPNETCORE_URLS='http://127.0.0.1:5086'
Write-Host 'Mo trinh duyet tai http://127.0.0.1:5086'
dotnet run --project KhachSan.csproj
exit $LASTEXITCODE
