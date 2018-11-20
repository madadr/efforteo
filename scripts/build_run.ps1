Function Msg()
{
	Write-Host "========================================================" -f red;
	Write-Host ">>> " -f green -nonewline; Write-Host $args[0] -f red; 
	Write-Host "========================================================" -f red;
}

Msg "Rebuilding source code"
./dotnet-publish.ps1

Msg "Stopping all containers"
docker stop $(docker ps -a -q)

Msg "Removing all containers"
docker rm $(docker ps -a -q)
docker-compose rm -f

Msg "Rebuilding containers"
docker-compose build --no-cache --force-rm

Msg "Starting containers"
docker-compose up
