namespace ScrumServiceFSharp.Models

open System
open System.Collections.Generic
open Amazon.DynamoDBv2.Model
open ScrumServiceFSharp.HelperTypes.String100

type ScrumOrganizationParam =
    {
        Id: string
        Name: string
    }

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
        {Id = idGuid; Name = name }
        
    let convertParam (scrumOrganizationParam: ScrumOrganizationParam) =
        let (idBool, idGuid) = Guid.TryParse(scrumOrganizationParam.Id)
        if not idBool then invalidArg (nameof scrumOrganizationParam.Id) (sprintf $"Id value of %s{scrumOrganizationParam.Id} was unable to be parsed to Guid.")
        let name = createString100 scrumOrganizationParam.Name
        {Id = idGuid; Name = name }
        