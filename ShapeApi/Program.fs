namespace ShapeApi

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open ShapeCore.Models
open ShapeApi.Controllers
open ShapeApi.Handlers
open System.Text.Json
open FSharp.SystemTextJson
open System.Text.Json
open Giraffe.GoodRead

module Program = 
    [<EntryPoint>]
    let main _ = 
        let builder = WebApplication.CreateBuilder()
        // Shape Dependencies
        builder.Services.AddSingleton<ShapeDependencies>({
            GetAll = Store.getAll
            GetShapeById = Store.getShapeById
            UpdateList = Store.updateList
            DeleteList = Store.deleteList
            AddList = Store.addList
        }) |> ignore

        // Giraffe
        builder.Services.AddGiraffe() |> ignore

        //CORS
        builder.Services.AddCors(fun options ->
            options.AddPolicy("AllowAll", fun policy ->
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                |> ignore
            )
        ) |> ignore

        // Standard Controller
        builder.Services.AddControllers() |> ignore
        builder.Services.AddEndpointsApiExplorer() |> ignore
        builder.Services.AddOpenApiDocument() |> ignore;

        // Build
        let app = builder.Build()

        // Enable Giraffe Endpoints
        let webApp = 
            choose [
                GET >=> choose [
                    route "/shapes" >=> Require.services<ShapeDependencies>(getShapes)
                    routef "/shapeById/%i" (fun id -> 
                        Require.services<ShapeDependencies>(fun deps -> 
                            getShapeById deps id)
                    )
                ]
                POST >=> route "/addShape" >=> Require.services<ShapeDependencies>(postShapes)
            ]
        
        // Enable Controllers
        app.UseRouting() |> ignore
        app.UseEndpoints(fun endpoints ->
            endpoints.MapControllers() |> ignore
        ) |> ignore
        
        app.UseCors("AllowAll") |> ignore

        app.UseGiraffe webApp
        app.UseOpenApi() |> ignore;
        app.UseSwaggerUi() |> ignore;
        app.Run()
        0