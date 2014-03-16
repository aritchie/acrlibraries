@echo off
del *.nupkg
nuget pack Acr.nuspec
nuget pack Acr.NetFx.nuspec
nuget pack Acr.Ef.nuspec
nuget pack Acr.Nh.nuspecs
nuget pack Acr.Mail.nuspec
nuget pack Acr.Mail.NVelocityParser.nuspec
nuget pack Acr.Mail.RazorParser.nuspec
pause