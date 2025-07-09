namespace MyApp.Services

type IShape = 
    abstract member Area: double * double -> double
    abstract member TriangleArea: double * double -> double

type Shape() = 
    interface IShape with 
        member _.Area(a, b) = a * b
        member _.TriangleArea (length: double, height: double): double = 0.5 * length * height

