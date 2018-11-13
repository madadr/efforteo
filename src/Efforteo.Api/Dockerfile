FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /dotnetapp
COPY ./bin/Docker .
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000/tcp
EXPOSE 5001/tcp
ENV ASPNETCORE_ENVIRONMENT Release
ENTRYPOINT dotnet Efforteo.Api.dll