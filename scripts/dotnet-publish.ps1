cd ../src
dotnet publish --force ./Efforteo.Services.Api -c Debug -o ./bin/Docker
dotnet publish --force ./Efforteo.Services.Activities -c Debug -o ./bin/Docker
dotnet publish --force ./Efforteo.Services.Authentication -c Debug -o ./bin/Docker
dotnet publish --force ./Efforteo.Services.Accounts -c Debug -o ./bin/Docker
cd ../scripts