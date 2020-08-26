#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PaymentApi.API/PaymentApi.API.csproj", "PaymentApi.API/"]
COPY ["PaymentApi.AcquiringBankMock/PaymentApi.AcquiringBankMock.csproj", "PaymentApi.AcquiringBankMock/"]
COPY ["PaymentApi.Application/PaymentApi.Application.csproj", "PaymentApi.Application/"]
COPY ["PaymentApi.Domain/PaymentApi.Domain.csproj", "PaymentApi.Domain/"]
COPY ["PaymentApi.CrossCutting/PaymentApi.CrossCutting.csproj", "PaymentApi.CrossCutting/"]
COPY ["PaymentApi.Gateway/PaymentApi.Gateway.csproj", "PaymentApi.Gateway/"]
RUN dotnet restore "PaymentApi.API/PaymentApi.API.csproj"
COPY . .
WORKDIR "/src/PaymentApi.API"
RUN dotnet build "PaymentApi.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentApi.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentApi.API.dll"]