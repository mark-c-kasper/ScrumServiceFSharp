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
            let! scanResponse = scanDynamoDbTable scrumOrganizationsTableName
            let (scrumOrganizationList: ScrumOrganizationList) =
                [
                 for item in scanResponse.Items do
                    ScrumOrganization.create item
                ]
            
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
    member this.CreateScrumOrganizationAsync([<FromBody>] newScrumOrganization: ScrumOrganization) =
        async {
            let dict = Dictionary<string, AttributeValue>()
            dict.Add("Id", AttributeValue(s = newScrumOrganization.Id.ToString()))
            dict.Add("Name", AttributeValue(s = newScrumOrganization.Name.ToString()))
            
            let putItemRequest = PutItemRequest(scrumOrganizationsTableName, dict)
            do! putItemInDynamoDbAsync putItemRequest
            
            return NoContentResult() :> IActionResult
        }
        
    [<HttpPut(Name="UpdateScrumOrganization")>]
    member this.UpdateScrumOrganizationAsync([<FromBody>] updatingScrumOrganization: ScrumOrganization) =
        async {
            let dict = Dictionary<string, AttributeValue>()
            dict.Add("Id", AttributeValue(updatingScrumOrganization.Id.ToString()))
            dict.Add("Name", AttributeValue(updatingScrumOrganization.Name.ToString()))
            
            let putItemRequest = PutItemRequest(scrumOrganizationsTableName, dict)
            do! putItemInDynamoDbAsync putItemRequest
            
            return NoContentResult() :> IActionResult
        }