cd ../src
docker build -f ./Efforteo.Api/Dockerfile -t efforteo.api ./Efforteo.Api
docker build -f ./Efforteo.Services.Activities/Dockerfile -t efforteo.services.activities ./Efforteo.Services.Activities
docker build -f ./Efforteo.Services.Identity/Dockerfile -t efforteo.services.identity ./Efforteo.Services.Identity