let array1 = [|1; 2; 3|]
let array2 = [|4; 5; 6|]

let allpairs = Array.allPairs array1 array2
let append = 
    array2
    |> Array.append array1

let average = 
    array1
    |> Array.map float
    |> Array.average

[<Measure>]
type WidgetsPerMinute

type Operator = 
    {
        Name : string
        Throughput : float<WidgetsPerMinute>
    }

let operatorArray = 
    [|
        { Name = "Mark"; Throughput = 5.0<WidgetsPerMinute>}
        { Name = "Luke"; Throughput = 6.0<WidgetsPerMinute>}
        { Name = "Jim"; Throughput = 7.0<WidgetsPerMinute>}
    |]

operatorArray
|> Array.averageBy(fun operator -> operator.Throughput)