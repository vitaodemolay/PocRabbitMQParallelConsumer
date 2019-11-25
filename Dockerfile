FROM microsoft/dotnet:3.0-sdk

WORKDIR /app

COPY CosumerSystemPoc ./
COPY MessagingLib ./
RUN dotnet restore

# COPY . ./
RUN dotnet publish -c Release -o out CosumerSystemPoc/CosumerSystemPoc.csproj
ENTRYPOINT ["dotnet", "out/CosumerSystemPoc.dll"]

 