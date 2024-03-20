$RESOURCE_GROUP="rg-RandomContent-use2"
$LOCATION="eastus2"
$ENVIRONMENT="cae-RandomContent-use2"
$API_NAME="ca-RandomContentQuotes-use2"
az containerapp up --name $API_NAME --resource-group $RESOURCE_GROUP --location $LOCATION --environment $ENVIRONMENT --ingress external --target-port 8080 --source .