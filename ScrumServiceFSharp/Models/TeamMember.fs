namespace ScrumServiceFSharp.Models

open System
open System.Collections.Generic
open Amazon.DynamoDBv2.Model
open ScrumServiceFSharp.HelperTypes.String100

type TeamMember =
    {
        Id: Guid
        FirstName: String100
        LastName: String100
        Position: string
        Salary: float
        TeamId: Guid
        DepartmentId: Guid
    }
    
type TeamMemberList = TeamMember list
    
module TeamMember =
    let create (dict: Dictionary<string, AttributeValue>) =
        let (idBool, idGuid) = Guid.TryParse(dict["Id"].S)
        let firstName = createString100 dict["FirstName"].S
        let lastName = createString100 dict["LastName"].S
        let position = dict["Position"].S
        let salary = float dict["Position"].N
        let (teamIdBool, teamId) = Guid.TryParse(dict["TeamId"].S)
        let (departmentIdBool, departmentId) = Guid.TryParse(dict["DepartmentId"].S)
        let teamMember : TeamMember = {
            Id = idGuid
            FirstName = firstName
            LastName = lastName
            Position = position
            Salary = salary
            TeamId = teamId
            DepartmentId = departmentId 
        }
        teamMember
        
    let convertToDynamoDbDictionary (teamMember: TeamMember) =
        let dict = Dictionary<string, AttributeValue>()
        dict.Add("Id", AttributeValue(s = teamMember.Id.ToString()))
        dict.Add("FirstName", AttributeValue(s = teamMember.FirstName.ToString()))
        dict.Add("LastName", AttributeValue(s = teamMember.LastName.ToString()))
        dict.Add("Position", AttributeValue(s = teamMember.Position.ToString()))
        dict.Add("Salary", AttributeValue(N = teamMember.Salary.ToString()))
        dict.Add("TeamId", AttributeValue(s = teamMember.TeamId.ToString()))
        dict.Add("DepartmentId", AttributeValue(s = teamMember.DepartmentId.ToString()))
        dict