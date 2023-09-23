namespace ScrumServiceFSharp.Controllers


open System.Collections.Generic
open Amazon.DynamoDBv2.Model
open ScrumServiceFSharp.Models
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open ScrumServiceFSharp.Services.DynamoDbService


[<ApiController>]
[<Route("api/[controller]/[action]")>]
type ScrumOrganizationController (logger : ILogger<ScrumOrganizationController>) =
    inherit ControllerBase()
    
    let scrumOrganizationsTableName = "ScrumOrganizations"
    
    [<HttpGet(Name="GetAllScrumOrganizations")>]
    member this.GetAllScrumOrganizationsAsync() =
        async {
            let mutable (scrumOrganizationList: ScrumOrganizationList) = []
            let! scanResponse = scanDynamoDbTable scrumOrganizationsTableName
            for item in scanResponse.Items do
                let scrumOrganization: ScrumOrganizationList = [ScrumOrganization.create item]
                scrumOrganizationList <- scrumOrganization @ scrumOrganizationList
            
            match scrumOrganizationList with
            | [] -> return NoContentResult() :> IActionResult
            | _ -> return ObjectResult(scrumOrganizationList) :> IActionResult
        }
    
    // https://localhost:7061/api/scrumorganization/GetScrumOrganizationById?id=<id>
    [<HttpGet(Name="GetScrumOrganizationById")>]
    member this.GetScrumOrganizationByIdAsync(id: string) =
        async {
            let! getItemResponse = queryDynamoDbForId scrumOrganizationsTableName id
            let (scrumOrganization: ScrumOrganization) = ScrumOrganization.create getItemResponse.Item
            return ObjectResult(scrumOrganization) :> IActionResult
        }

    [<HttpPost(Name="CreateScrumOrganization")>]
    member this.CreateScrumOrganizationAsync([<FromBody>] newScrumOrganization: ScrumOrganizationParam) =
        async {
            let scrumOrganization = ScrumOrganization.convertParam newScrumOrganization
            
            let dict = Dictionary<string, AttributeValue>()
            dict.Add("Id", AttributeValue(s = scrumOrganization.Id.ToString()))
            dict.Add("Name", AttributeValue(s = scrumOrganization.Name.ToString()))
            
            let putItemRequest = PutItemRequest(scrumOrganizationsTableName, dict)
            let! putItemResponse = putItemInDynamoDb putItemRequest
            
            return NoContentResult() :> IActionResult
        }
        
    [<HttpPut(Name="UpdateScrumOrganization")>]
    member this.UpdateScrumOrganizationAsync([<FromBody>] updatingScrumOrganization: ScrumOrganizationParam) =
        async {
            let scrumOrganization = ScrumOrganization.convertParam updatingScrumOrganization
            
            let dict = Dictionary<string, AttributeValue>()
            dict.Add("Id", AttributeValue(scrumOrganization.Id.ToString()))
            dict.Add("Name", AttributeValue(scrumOrganization.Name.ToString()))
            
            let putItemRequest = PutItemRequest(scrumOrganizationsTableName, dict)
            let! putItemResponse = putItemInDynamoDb putItemRequest
            
            return NoContentResult() :> IActionResult
        }