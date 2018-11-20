cd ../src
docker build --no-cache -f ./Efforteo.Api/Dockerfile -t efforteo.api ./Efforteo.Api
docker build --no-cache -f ./Efforteo.Services.Activities/Dockerfile -t efforteo.services.activities ./Efforteo.Services.Activities
docker build --no-cache -f ./Efforteo.Services.Accoutns/Dockerfile -t efforteo.services.accounts ./Efforteo.Services.Accounts