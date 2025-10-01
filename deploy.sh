#!/bin/bash

# =========================
# CONFIGURAÇÕES
# =========================
RG=rgqmove
ACR_NAME=acrqmove
APP_NAME=aci-qmove-api
DB_NAME=aci-qmove-db
LOCATION=eastus
IMAGE_NAME=qmoveapi:v1

MYSQL_ROOT_PASSWORD=root123
DB_DATABASE=qmove

# =========================
# 1️⃣ Criar Resource Group
# =========================
echo "🔹 Criando Resource Group..."
az group create --name $RG --location $LOCATION &> /dev/null || echo "RG já existe"

# =========================
# 2️⃣ Garantir que o ACR existe e pegar credenciais
# =========================
echo "🔹 Verificando ACR..."
az acr show -n $ACR_NAME -g $RG &> /dev/null || az acr create -n $ACR_NAME -g $RG --sku Basic --admin-enabled true

ACR_USER=$(az acr credential show -n $ACR_NAME --query "username" -o tsv)
ACR_PASS=$(az acr credential show -n $ACR_NAME --query "passwords[0].value" -o tsv)

# =========================
# 3️⃣ Pular criação do MySQL (já existe) e pegar FQDN
# =========================
echo "🔹 Obtendo FQDN do MySQL existente..."
DB_FQDN=$(az container show --resource-group $RG --name $DB_NAME --query "ipAddress.fqdn" -o tsv)

if [ -z "$DB_FQDN" ]; then
    echo "❌ Banco MySQL não encontrado. Rode o container MySQL primeiro."
    exit 1
fi

echo "✅ Banco MySQL já rodando em: $DB_FQDN"

# =========================
# 4️⃣ Criar container da aplicação
# =========================
echo "🔹 Criando container da aplicação..."
az container create \
  --resource-group $RG \
  --name $APP_NAME \
  --image $ACR_NAME.azurecr.io/$IMAGE_NAME \
  --registry-login-server $ACR_NAME.azurecr.io \
  --registry-username $ACR_USER \
  --registry-password $ACR_PASS \
  --cpu 1 --memory 1 \
  --os-type Linux \
  --ports 8080 \
  --environment-variables \
      DB_HOST=$DB_FQDN \
      DB_PORT=3306 \
      DB_NAME=$DB_DATABASE \
      DB_USER=root \
      DB_PASS=$MYSQL_ROOT_PASSWORD \
      PORT=8080 \
  --dns-name-label $APP_NAME \
  --restart-policy Always

echo "✅ Deploy concluído!"
echo "🌍 API disponível em: http://$APP_NAME.$LOCATION.azurecontainer.io"
