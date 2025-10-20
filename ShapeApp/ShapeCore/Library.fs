namespace ShapeCore

module Models = 

    open System.Text.Json.Serialization

    [<JsonConverter(typeof<JsonStringEnumConverter>)>]
    type ShapeType = 
        | Circle = 0
        | Square = 1
        | Triangle = 2

    type Shape = {
        Id: int
        Name: string
        ShapeType: ShapeType
        Size: float
    }
