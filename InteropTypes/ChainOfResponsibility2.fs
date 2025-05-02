namespace HappyCoR

[<Measure>]
type inch

type Record2 = 
    val private name: string
    val private age: int
    val private weight: float
    val private height: float<inch>
    member this.Name = this.name
    member this.Age = this.age
    member this.Weight = this.weight
    member this.Height = this.height
    new(name: string, age: int, weight: float, height: float) = 
        {name = name; age = age; weight = weight; height = height * 1.0<inch>;}

module RecordManager2 = 
// Chain of responsibility pattern
    // function to check that the age is between 18 and 65
    let private validAge2 (record : Record2) =
        record.Age < 65 && record.Age > 18

    // function to check that the weight is less than 200
    let private validWeight2 (record : Record2) =
        record.Weight < 200.

    // function to check that the height is greater than 120
    let private validHeight2 (record : Record2) =
        record.Height > 60.<inch>

    // function to perform the check according to parameter f
    let private check f (record : Record2, result) =
        if not result then record, false
        else record, f(record)

    // create chain function
    let private chain = check validAge2 >> check validWeight2 >> check validHeight2
    let ChainOfResponsibility (record : Record2)  = chain (record, true)
    