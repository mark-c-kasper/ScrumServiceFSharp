module ScrumServiceFSharp.Services.DepartmentService

open System.Collections.Generic
open Amazon.DynamoDBv2
open Amazon.DynamoDBv2.Model

let public scanDynamoDbTable table_name =
     async {
        let dynamoDbQuery = ScanRequest(table_name)
        let amazonClient = new AmazonDynamoDBClient()
        
        let dynamoDbResponse = amazonClient.ScanAsync(dynamoDbQuery)
        return dynamoDbResponse.Result
    }
     
let public queryDynamoDbForId table_name id =
    async {
        let mutable idAttributeValue: AttributeValue = AttributeValue()
        idAttributeValue.S <- id
        let key = Dictionary<string, AttributeValue>()
        key.Add("Id", idAttributeValue)
        let dynamoDbQuery = GetItemRequest(table_name, key)
        let amazonClient = new AmazonDynamoDBClient()
        
        let dynamoDbResponse = amazonClient.GetItemAsync(dynamoDbQuery)
        return dynamoDbResponse.Result
    }
