namespace ScrumServiceFSharp.Controllers

open System.Collections.Generic
open Amazon.DynamoDBv2.Model
open ScrumServiceFSharp.Models
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open ScrumServiceFSharp.Services.DynamoDbService

[<ApiController>]
[<Route("api/[controller]/[action]")>]
type TeamMemberController (logger : ILogger<TeamMemberController>) =
    inherit ControllerBase()
    
    let teamMemberTableName = "TeamMembers"
    
    [<HttpGet(Name="GetAllTeamMembers")>]
    member this.GetAllTeamMembersAsync() =
        async {
            let! scanResponse = scanDynamoDbTable teamMemberTableName
            let (teamMembers: TeamMemberList) =
                [
                    for item in scanResponse.Items do
                        let (teamMember : TeamMember) = TeamMember.create item
                        teamMember
                ]
            
            match teamMembers with
            | [] -> return NoContentResult() :> IActionResult
            | _ -> return ObjectResult(teamMembers) :> IActionResult
        }
        
    // https://localhost:7061/api/scrumorganization/GetScrumOrganizationById?id=<id>
    [<HttpGet(Name="GetTeamMembersById")>]
    member this.GetTeamMembersByIdAsync(id: string) =
        async {
            let! getItemResponse = queryDynamoDbForId teamMemberTableName id
            return ObjectResult (TeamMember.create getItemResponse.Item) :> IActionResult
        }
        
    [<HttpPost(Name="CreateTeamMember")>]
    member this.CreateTeamMemberAsync([<FromBody>] newTeamMember: TeamMember) =
        async {
            let dict = TeamMember.convertToDynamoDbDictionary newTeamMember
            
            let putItemRequest = PutItemRequest(teamMemberTableName, dict)
            do! putItemInDynamoDbAsync putItemRequest
            
            return NoContentResult() :> IActionResult
        }
        
    [<HttpPut(Name="UpdateTeamMember")>]
    member this.UpdateTeamMemberAsync([<FromBody>] updatingTeamMember: TeamMember) =
        async {
            let dict = TeamMember.convertToDynamoDbDictionary updatingTeamMember
            let putItemRequest = PutItemRequest(teamMemberTableName, dict)
            do! putItemInDynamoDbAsync putItemRequest
            
            return NoContentResult() :> IActionResult
        }
