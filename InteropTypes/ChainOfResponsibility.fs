namespace SadCoR

[<Measure>]
type inch

type Record = {
    Name : string;
    Age : int
    Weight: float
    Height: float<inch>
}

module RecordManager = 
// Chain of responsibility pattern
    // function to check that the age is between 18 and 65
    let private validAge record =
        record.Age < 65 && record.Age > 18

    // function to check that the weight is less than 200
    let private validWeight record =
        record.Weight < 200.

    // function to check that the height is greater than 120
    let private validHeight record =
        record.Height > 60.<inch>

    // function to perform the check according to parameter f
    let private check f (record, result) =
        if not result then record, false
        else record, f(record)

    // create chain function
    let private chain = check validAge >> check validWeight >> check validHeight
    let ChainOfResponsibility record = chain (record, true)
    let CompareHeights (record: Record) = 
        record.Height = 60.<inch>
