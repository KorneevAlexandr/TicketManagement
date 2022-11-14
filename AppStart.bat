dotnet build src\TicketManagement.BusinessLogic\TicketManagement.BusinessLogic.csproj
dotnet build src\TicketManagement.DataAccess\TicketManagement.DataAccess.csproj
dotnet build src\TicketManagement.ClientModels\TicketManagement.ClientModels.csproj

start /d src\TicketManagement.EventAPI\ dotnet run
start /d src\TicketManagement.UserAPI\ dotnet run
start /d  src\TicketManagement.PurchaseAPI\ dotnet run
start /d  src\TicketManagement.VenueAPI\ dotnet run
start /d  src\TicketManagement.WebApplication\ dotnet run
start /d src\TicketManagement.ReactUI\ dotnet run

FOR /F "tokens=2" %%x IN (src\TicketManagement.WebApplication\hostsettings.json) DO start "" %%x