namespace ShapeApi

module Store = 
    open ShapeCore.Models
    let mutable private shapes: Shape list = [
        { Id = 1; Name = "Circle A"; ShapeType = ShapeType.Circle; Size = 10.0 }
        { Id = 2; Name = "Circle B"; ShapeType = ShapeType.Circle; Size = 5.0 }
        { Id = 3; Name = "Circle C"; ShapeType = ShapeType.Circle; Size = 8.0 }
        { Id = 4; Name = "Circle D"; ShapeType = ShapeType.Circle; Size = 3.0 }
    ]

    let getAll() = shapes
    let getShapeById (id : int) = 
        match shapes |> List.tryFind(fun shape -> shape.Id = id) with
        | Some s -> s
        | None -> failwithf "Shape with Id %d not found" id
    let addList (newShape : Shape list) = 
        shapes <- shapes @ newShape
    let updateList updateShapes = 
        shapes <-
            shapes
            |> List.map (fun s -> 
                updateShapes |> List.tryFind (fun us -> us.Id = s.Id)
                |> Option.defaultValue s)
    let deleteList ids = 
        shapes <- List.Empty