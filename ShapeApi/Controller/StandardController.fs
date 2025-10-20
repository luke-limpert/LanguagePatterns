namespace ShapeApi.Controllers

open ShapeApi
open ShapeApi.Handlers
open ShapeApi.Store
open ShapeCore.Models
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Configuration
open System.Threading.Tasks

[<Route("api/Shape")>]
[<ApiController>]
type ShapeController (deps : ShapeDependencies) =
    inherit ControllerBase()
    [<HttpGet("GetShapes")>]
    member this.GetShapes() : Task<IActionResult> =
        task {
            let shape : Shape list = deps.GetAll()
            return OkObjectResult(shape) :> IActionResult
        }

    [<HttpGet("GetShapeById/{shapeId}")>]
    member this.GetShapeById(shapeId : int) : Task<IActionResult> =
        task {
            let shape : Shape = deps.GetShapeById shapeId 
            return OkObjectResult(shape) :> IActionResult
            }
