echo ">>> Rebuild source code"
./dotnet-publish.ps1

echo ">>> Stopping all containers"
docker stop $(docker ps -a -q)

echo ">>> Removing all containers "
docker rm $(docker ps -a -q)
docker-compose rm -f

echo ">>> Rebuilding containers"
docker-compose build --no-cache --force-rm

echo ">>> Starting containers"
docker-compose up
