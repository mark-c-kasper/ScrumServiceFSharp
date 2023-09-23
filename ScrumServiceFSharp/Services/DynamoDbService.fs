module ScrumServiceFSharp.Services.DynamoDbService

open System.Collections.Generic
open Amazon.DynamoDBv2
open Amazon.DynamoDBv2.Model

let public scanDynamoDbTable tableName =
     async {
        let dynamoDbQuery = ScanRequest(tableName)
        let amazonClient = new AmazonDynamoDBClient()
        
        let dynamoDbResponse = amazonClient.ScanAsync(dynamoDbQuery)
        return dynamoDbResponse.Result
    }
     
let public queryDynamoDbForId tableName id =
    async {
        let mutable idAttributeValue: AttributeValue = AttributeValue()
        idAttributeValue.S <- id
        let key = Dictionary<string, AttributeValue>()
        key.Add("Id", idAttributeValue)
        let dynamoDbQuery = GetItemRequest(tableName, key)
        let amazonClient = new AmazonDynamoDBClient()
        
        let dynamoDbResponse = amazonClient.GetItemAsync(dynamoDbQuery)
        return dynamoDbResponse.Result
    }

let public putItemInDynamoDb putItemRequest =
    async {
        let amazonClient = new AmazonDynamoDBClient()
        
        let putItemResponse = amazonClient.PutItemAsync(putItemRequest)
        return putItemResponse.Result
    }
