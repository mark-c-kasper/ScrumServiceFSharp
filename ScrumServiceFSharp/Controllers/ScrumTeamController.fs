namespace ScrumServiceFSharp.Controllers

open System.Collections.Generic
open Amazon.DynamoDBv2.Model
open ScrumServiceFSharp.Models
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open ScrumServiceFSharp.Services.DynamoDbService


[<ApiController>]
[<Route("api/[controller]/[action]")>]
type ScrumTeamController (logger : ILogger<ScrumTeamController>) =
    inherit ControllerBase()
    
    let scrumTeamsTableName = "ScrumTeams"
    
    [<HttpGet(Name="GetAllScrumTeams")>]
    member this.GetAllScrumTeamsAsync() =
        async {
            let! scanResponse = scanDynamoDbTable scrumTeamsTableName
            let (scrumTeams: ScrumTeamList) =
                [
                    for item in scanResponse.Items do
                        let (scrumTeam : ScrumTeam) = ScrumTeam.create item
                        scrumTeam
                ]
            
            match scrumTeams with
            | [] -> return NoContentResult() :> IActionResult
            | _ -> return ObjectResult(scrumTeams) :> IActionResult
        }
        
    // https://localhost:7061/api/scrumorganization/GetScrumOrganizationById?id=<id>
    [<HttpGet(Name="GetScrumTeamById")>]
    member this.GetScrumTeamByIdAsync(id: string) =
        async {
            let! getItemResponse = queryDynamoDbForId scrumTeamsTableName id
            let (scrumTeam: ScrumTeam) = ScrumTeam.create getItemResponse.Item
            return ObjectResult(scrumTeam) :> IActionResult
        }
        
    [<HttpPost(Name="CreateScrumTeam")>]
    member this.CreateScrumTeamAsync([<FromBody>] newScrumTeam: ScrumTeam) =
        async {
            let dict = Dictionary<string, AttributeValue>()
            dict.Add("Id", AttributeValue(s = newScrumTeam.Id.ToString()))
            dict.Add("Name", AttributeValue(s = newScrumTeam.Name.ToString()))
            dict.Add("OrganizationId", AttributeValue(s = newScrumTeam.OrganizationId.ToString()))
            
            let putItemRequest = PutItemRequest(scrumTeamsTableName, dict)
            do! putItemInDynamoDbAsync putItemRequest
            
            return NoContentResult() :> IActionResult
        }
        
    [<HttpPut(Name="UpdateScrumTeam")>]
    member this.UpdateScrumTeamAsync([<FromBody>] updatingScrumTeam: ScrumTeam) =
        async {
            let dict = Dictionary<string, AttributeValue>()
            dict.Add("Id", AttributeValue(updatingScrumTeam.Id.ToString()))
            dict.Add("Name", AttributeValue(updatingScrumTeam.Name.ToString()))
            dict.Add("OrganizationId", AttributeValue(s = updatingScrumTeam.OrganizationId.ToString()))
            
            let putItemRequest = PutItemRequest(scrumTeamsTableName, dict)
            do! putItemInDynamoDbAsync putItemRequest
            
            return NoContentResult() :> IActionResult
        }