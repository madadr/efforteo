cd ../src
docker build --no-cache -f ./Efforteo.Services.Api/Dockerfile -t efforteo.services.api ./Efforteo.Services.Api
docker build --no-cache -f ./Efforteo.Services.Activities/Dockerfile -t efforteo.services.activities ./Efforteo.Services.Activities
docker build --no-cache -f ./Efforteo.Services.Accounts/Dockerfile -t efforteo.services.accounts ./Efforteo.Services.Accounts
docker build --no-cache -f ./Efforteo.Services.Authentication/Dockerfile -t efforteo.services.authentication ./Efforteo.Services.Authentication