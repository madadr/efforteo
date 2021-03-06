Function Msg()
{
	Write-Host "========================================================" -f red;
	Write-Host ">>> " -f green -nonewline; Write-Host $args[0] -f red; 
	Write-Host "========================================================" -f red;
}

Msg "Stopping all containers"
docker stop $(docker ps -a -q)

Msg "Removing all containers"
docker rm $(docker ps -a -q)
docker-compose rm -f

Msg "Starting mongo & rabbit"
docker-compose up -d auth-db
docker-compose up -d accounts-db
docker-compose up -d activities-db
docker-compose up -d stats-db
docker-compose up -d rabbitmq

Msg "Rebuilding source code"
./dotnet-publish.ps1

Msg "Rebuilding containers"
docker-compose build --no-cache --force-rm

Msg "Starting containers"
docker-compose up
