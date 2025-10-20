namespace ShapeApi

open Giraffe
open Microsoft.AspNetCore.Http
open ShapeCore.Models
open ShapeApi.Store

type ShapeDependencies = {
    GetAll: unit -> Shape list
    GetShapeById: int -> Shape
    AddList : Shape list -> unit
    UpdateList : Shape list -> unit
    DeleteList : int list -> unit
}


module Handlers = 

    let getShapes { GetAll = getAll } : HttpHandler = 
        fun next ctx -> 
            let shapes = getAll()
            json shapes next ctx

    let getShapeById { GetShapeById = getShapeById } (id : int) : HttpHandler = 
        fun next ctx -> 
            try 
                let shapeFind = getShapeById id
                json shapeFind next ctx
            with ex -> 
                setStatusCode 404 >=> text ex.Message <| next <| ctx
    
    let postShapes { AddList = addList } : HttpHandler = 
        fun next ctx -> task {
            let q = ctx.Request.Query

            match q.TryGetValue("id"), q.TryGetValue("name"), q.TryGetValue("shapeType"), q.TryGetValue("size") with
            | (true, idVals), (true, nameVals), (true, shapeTypeVals), (true, sizeVals) -> 
                let id = int idVals.[0]
                let name = string nameVals.[0]
                let shapeType = 
                    match System.Enum.TryParse<ShapeType>(shapeTypeVals.[0], true ) with
                    | true, v -> v
                    | _ -> failwithf "Invalid Shape type: %s" shapeTypeVals.[0]
                let size = float sizeVals.[0]
                let newShape = { Id = id; Name = name; ShapeType = shapeType; Size = size }
                addList [ newShape ]
                return! json newShape next ctx
            | _ -> return! (setStatusCode 400 >=> json {| error = "Missing one or more required query parameters!"|}) next ctx
        }

    let putShapes { UpdateList = updateList } : HttpHandler = 
        fun next ctx -> task {
            let! updatedShapes = ctx.BindJsonAsync<Shape List>()
            updateList updatedShapes
            return! json updatedShapes next ctx
        }
    
    let deleteShapes { DeleteList = deleteList } : HttpHandler = 
        fun next ctx -> task {
            let! ids = ctx.BindJsonAsync<int list>()
            deleteList ids
            return! json ids next ctx
        }