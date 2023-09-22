namespace ScrumServiceFSharp.Controllers

open ScrumServiceFSharp.Models
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open ScrumServiceFSharp.Services.DepartmentService 


[<ApiController>]
[<Route("api/[controller]/[action]")>]
type DepartmentsController (logger : ILogger<DepartmentsController>) =
    inherit ControllerBase()
    
    [<HttpGet(Name="GetAllDepartments")>]
    member this.GetAllDepartmentsAsync() =
        async {
            let mutable (departments: departmentList) = []
            let! scanResponse = scanDynamoDbTable "Departments"
            for item in scanResponse.Items do
                let department = [{Id = item["Id"].S; Name = item["Name"].S}]
                departments <- department @ departments
            
            match departments with
            | [] -> return NoContentResult() :> IActionResult
            | _ -> return ObjectResult(departments) :> IActionResult
        }
    
    // https://localhost:7061/api/departments/GetDepartmentById?id=<id>
    [<HttpGet(Name="GetDepartmentById")>]
    member this.GetDepartmentByIdAsync(id: string) =
        async {
            let! getItemResponse = queryDynamoDbForId "Departments" id
            let (department: Department) = {Id = getItemResponse.Item["Id"].S; Name = getItemResponse.Item["Name"].S}
            return ObjectResult(department) :> IActionResult
        }

