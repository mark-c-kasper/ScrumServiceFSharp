namespace ScrumServiceFSharp.Models

open System
open System.Collections.Generic
open Amazon.DynamoDBv2.Model
open ScrumServiceFSharp.HelperTypes.String100

type ScrumOrganization =
    {
        Id: Guid
        Name: String100
    }
    
type ScrumOrganizationList = ScrumOrganization list

module ScrumOrganization =
    let create (dict: Dictionary<string, AttributeValue>) =
        let (idBool, idGuid) = Guid.TryParse(dict["Id"].S)
        let name = createString100 dict["Name"].S
        let scrumOrganization : ScrumOrganization = {Id = idGuid; Name = name }
        scrumOrganization
        