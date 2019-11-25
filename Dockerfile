FROM microsoft/dotnet:3.0-sdk

WORKDIR /app

ADD CosumerSystemPoc ./
ADD MessagingLib ./
# COPY . ./
RUN dotnet restore CosumerSystemPoc/CosumerSystemPoc.csproj



RUN dotnet publish -c Release -o out CosumerSystemPoc/CosumerSystemPoc.csproj
ENTRYPOINT ["dotnet", "out/CosumerSystemPoc.dll"]

 