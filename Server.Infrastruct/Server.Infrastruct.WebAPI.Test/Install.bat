set serviceName=Server.Infrastruct.WebAPI.Test
set serviceFilePath=%~dp0Server.Infrastruct.WebAPI.Test.exe
set serviceDescription=SystemWebAPIService

sc create %serviceName%  BinPath=%serviceFilePath%
sc config %serviceName%    start=auto  
sc description %serviceName%  %serviceDescription%
sc start  %serviceName%
pause

