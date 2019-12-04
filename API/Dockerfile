FROM microsoft/dotnet:3.0-sdk as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY Tweetbook.csproj /build/

RUN dotnet restore ./build/API.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./API.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM microsoft/dotnet:3.0-aspnetcore-runtime
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "API.dll"] 