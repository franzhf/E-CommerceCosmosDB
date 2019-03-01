// Run cosmosdb emulator
1. cd C:\Program Files\Azure Cosmos DB Emulator
2. CosmosDB.Emulator.exe /NoFirewall

{"id": "100", "name":"iPad Pro 2", "Price": 300, "category": "Electronic"}

{{"query":"SELECT p.id, p.name, p.price, udf.applyTaxes(p.price) + p.price as price_with_taxes FROM products p WHERE p.id = '2000'"}}