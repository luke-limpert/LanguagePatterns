namespace InteropTypes

type Shape =
    {
        Edges : int
    }

type ShapeWithExtraConstructor() = 
    member val Edges = 0 with get, set
    new(edges: int) as this = 
        ShapeWithExtraConstructor()
        then this.Edges <- edges
    new(shape: Shape) as this = 
        ShapeWithExtraConstructor()
        then this.Edges <- shape.Edges

type ExpandedShape = 
    {
        Edges: int
        Area: double
        Name: string
    }

module RightTriangle = 
    let private area(length: double, height: double) : double = 0.5 * length * height
    let convertToShape(length: double, height: double) = 
        {
            Edges = 3
            Area = area(length, height)
            Name = "Triangle"
        }