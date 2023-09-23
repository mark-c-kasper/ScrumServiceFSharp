namespace ScrumServiceFSharp.Controllers

open System.Collections.Generic
open Amazon.DynamoDBv2.Model
open ScrumServiceFSharp.Models
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open ScrumServiceFSharp.Services.DynamoDbService 


[<ApiController>]
[<Route("api/[controller]/[action]")>]
type DepartmentsController (logger : ILogger<DepartmentsController>) =
    inherit ControllerBase()
    
    let departmentsTableName = "Departments"
    
    [<HttpGet(Name="GetAllDepartments")>]
    member this.GetAllDepartmentsAsync() =
        async {
            let mutable (departments: DepartmentList) = []
            let! scanResponse = scanDynamoDbTable departmentsTableName
            for item in scanResponse.Items do
                let department: DepartmentList = [{Id = item["Id"].S; Name = item["Name"].S}]
                departments <- department @ departments
            
            match departments with
            | [] -> return NoContentResult() :> IActionResult
            | _ -> return ObjectResult(departments) :> IActionResult
        }
    
    // https://localhost:7061/api/departments/GetDepartmentById?id=<id>
    [<HttpGet(Name="GetDepartmentById")>]
    member this.GetDepartmentByIdAsync(id: string) =
        async {
            let! getItemResponse = queryDynamoDbForId departmentsTableName id
            let (department: Department) = {Id = getItemResponse.Item["Id"].S; Name = getItemResponse.Item["Name"].S}
            return ObjectResult(department) :> IActionResult
        }

    [<HttpPost(Name="Create")>]
    member this.CreateAsync([<FromBody>] newDepartment: Department) =
        async {
            let dict = Dictionary<string, AttributeValue>()
            dict.Add("Id", AttributeValue(newDepartment.Id))
            dict.Add("Name", AttributeValue(newDepartment.Name))
            
            let putItemRequest = PutItemRequest(departmentsTableName, dict)
            let! putItemResponse = putItemInDynamoDb putItemRequest
            
            return NoContentResult() :> IActionResult
        }
        
    [<HttpPut(Name="Update")>]
    member this.UpdateAsync([<FromBody>] newDepartment: Department) =
        async {
            let dict = Dictionary<string, AttributeValue>()
            dict.Add("Id", AttributeValue(newDepartment.Id))
            dict.Add("Name", AttributeValue(newDepartment.Name))
            
            let putItemRequest = PutItemRequest(departmentsTableName, dict)
            let! putItemResponse = putItemInDynamoDb putItemRequest
            
            return NoContentResult() :> IActionResult
        }