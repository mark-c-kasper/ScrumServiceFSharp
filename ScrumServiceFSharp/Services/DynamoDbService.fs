module ScrumServiceFSharp.Services

open System.Collections.Generic
open Amazon.DynamoDBv2
open Amazon.DynamoDBv2.Model

module DynamoDbService =

    let public scanDynamoDbTable tableName =
         async {
            let dynamoDbQuery = ScanRequest(tableName)
            use amazonClient = new AmazonDynamoDBClient()
            let dynamoDbResponse = amazonClient.ScanAsync(dynamoDbQuery)
            return dynamoDbResponse.Result
        }
         
    let public queryDynamoDbForId tableName id =
        async {
            let idAttributeValue: AttributeValue = AttributeValue(s=id)
            let key = Dictionary<string, AttributeValue>()
            key.Add("Id", idAttributeValue)
            let dynamoDbQuery = GetItemRequest(tableName, key)
            use amazonClient = new AmazonDynamoDBClient()
            
            let dynamoDbResponse = amazonClient.GetItemAsync(dynamoDbQuery)
            return dynamoDbResponse.Result
        }

    let public putItemInDynamoDbAsync putItemRequest =
        async {
            use amazonClient = new AmazonDynamoDBClient()
            do amazonClient.PutItemAsync(putItemRequest) |> ignore
            return ()
        }
