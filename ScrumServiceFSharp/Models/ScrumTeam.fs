namespace ScrumServiceFSharp.Models

open System
open System.Collections.Generic
open Amazon.DynamoDBv2.Model
open ScrumServiceFSharp.HelperTypes.String100

type ScrumTeam =
    {
        Id: Guid
        Name: String100
        OrganizationId: Guid
    }
    
type ScrumTeamList = ScrumTeam list

module ScrumTeam =
    let create (dict: Dictionary<string, AttributeValue>) =
        let (idBool, idGuid) = Guid.TryParse(dict["Id"].S)
        let name = createString100 dict["Name"].S
        let (orgIdBool, organizationId) = Guid.TryParse(dict["Id"].S)
        let scrumTeam : ScrumTeam = {Id = idGuid; Name = name; OrganizationId = organizationId }
        scrumTeam
        