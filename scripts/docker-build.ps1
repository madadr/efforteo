cd ../src
#docker build --no-cache -f ./Efforteo.Api/Dockerfile -t efforteo.api ./Efforteo.Api
docker build --no-cache -f ./Efforteo.Services.Activities/Dockerfile -t efforteo.services.activities ./Efforteo.Services.Activities
docker build --no-cache -f ./Efforteo.Services.Accounts/Dockerfile -t efforteo.services.accounts ./Efforteo.Services.Accounts
docker build --no-cache -f ./Efforteo.Services.Authentication/Dockerfile -t efforteo.services.authentication ./Efforteo.Services.Authentication