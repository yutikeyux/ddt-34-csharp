# HydroFilter Tüm IP Engellerini Kaldırma Script'i

# 1. Güvenlik Duvarı Kurallarını Sil
Write-Host "Güvenlik duvarı kuralları siliniyor..." -ForegroundColor Yellow
Get-NetFirewallRule | Where-Object {$_.DisplayName -like "HydroFilter - *"} | Remove-NetFirewallRule

# 2. IPBlockList.txt Dosyasını Temizle
$filePath = "C:\Yol\IPBlockList.txt" # Kendi dosya yolunuzla değiştirin
Write-Host "IPBlockList.txt temizleniyor..." -ForegroundColor Yellow
if (Test-Path $filePath) {
    Clear-Content -Path $filePath
    Write-Host "Dosya temizlendi: $filePath" -ForegroundColor Green
} else {
    Write-Host "Dosya bulunamadı: $filePath" -ForegroundColor Red
}

# 3. Programı Yeniden Başlat (Belleği Sıfırla)
$processName = "ProgramAdi" # Kendi process adınızla değiştirin
Write-Host "Program yeniden başlatılıyor..." -ForegroundColor Yellow
Stop-Process -Name $processName -Force -ErrorAction SilentlyContinue
Start-Process "C:\Yol\ProgramAdi.exe" # Kendi program yolunuzla değiştirin

Write-Host "Tüm IP engelleri kaldırıldı!" -ForegroundColor Green