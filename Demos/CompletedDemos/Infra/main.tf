data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "random_content" {
  name     = "rg-RandomContent-use2"
  location = "eastus2"
}

resource "azurerm_user_assigned_identity" "random_content" {
  name = "id-RandomContent-use2"
  location = azurerm_resource_group.random_content.location
  resource_group_name = azurerm_resource_group.random_content.name
}

resource "azurerm_log_analytics_workspace" "random_content" {
  name                = "log-RandomContent-use2"
  location            = azurerm_resource_group.random_content.location
  resource_group_name = azurerm_resource_group.random_content.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "random_content" {
  name                = "appi-RandomContent-use2"
  location            = azurerm_resource_group.random_content.location
  resource_group_name = azurerm_resource_group.random_content.name
  workspace_id        = azurerm_log_analytics_workspace.random_content.id
  application_type    = "web"
}

resource "azurerm_mssql_server" "random_content" {
  name                         = "sql-randomcontent-use2"
  resource_group_name          = azurerm_resource_group.random_content.name
  location                     = azurerm_resource_group.random_content.location
  version                      = "12.0"
  administrator_login          = "admin"
  administrator_login_password = var.sql_server_admin_password
}

resource "azurerm_mssql_server_microsoft_support_auditing_policy" "random_content" {
  server_id              = azurerm_mssql_server.random_content.id
  enabled                = false
  log_monitoring_enabled = false
  depends_on = [
    azurerm_mssql_server.random_content
  ]
}

resource "azurerm_mssql_server_extended_auditing_policy" "random_content" {
  server_id              = azurerm_mssql_server.random_content.id
  enabled                = true
  log_monitoring_enabled = true
}

resource "azurerm_mssql_database" "random_content" {
  name                = "sqldb-RandomContent-use2"
  server_id           = azurerm_mssql_server.random_content.id
  auto_pause_delay_in_minutes = 60
  max_size_gb                 = 2
  min_capacity                = 0.5
  sku_name                    = "GP_S_Gen5_4"
  zone_redundant              = false
}

resource "azurerm_container_app_environment" "random_content" {
  name                       = "cae-RandomContent-use2"
  resource_group_name        = azurerm_resource_group.random_content.name
  location                   = azurerm_resource_group.random_content.location
  log_analytics_workspace_id = azurerm_log_analytics_workspace.random_content.id
  infrastructure_resource_group_name = "rg-RandomContentCAEInfrastructure-use2"
  workload_profile {
    name                  = "consumption"
    workload_profile_type = "Consumption"
    maximum_count         = 2
    minimum_count         = 0
  }
}