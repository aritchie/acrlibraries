@echo off
del *.nupkg
nuget pack Acr.nuspec
nuget pack Acr.NetFx.nuspec
nuget pack Acr.Ef.nuspec
nuget pack Acr.Ef.NinjectExtensions.nuspec
nuget pack Acr.Nh.nuspec
nuget pack Acr.Nh.NinjectExtensions.nuspec
nuget pack Acr.Mail.nuspec
nuget pack Acr.Mail.NVelocityParser.nuspec
nuget pack Acr.Mail.RazorParser.nuspec
pause