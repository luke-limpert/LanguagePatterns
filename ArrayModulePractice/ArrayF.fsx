let array1 = [| 0; 1; 2; 3; 4 |]

Array.fill array1 3 2 4

let getEvens (intArray : int array) : int array = 
    intArray
    |> Array.filter(fun x -> x % 2 = 0)

array1
|> getEvens

array1
|> Array.find(fun x -> x = 2)

array1
|> Array.findBack(fun x -> x = 4)

array1
|> Array.findIndex(fun x -> x = 4)

array1
|> Array.findIndex(fun x -> x = 4)

array1
|> Array.findIndexBack(fun x -> x = 4)

// input: [|0; 1; 2; 3; 4|]
// 0, 4, 10, 17, 25
let functionalAggregation (intArray: int array) :int = 
    (0, intArray)
    ||> Array.fold(fun acc input -> 
        acc + (input * 4) - (acc / 2)
        )

functionalAggregation array1

let functionalAggregationMultiArray (intArray1: int array, intArray2: int array) : int = 
    (0, intArray1, intArray2)
    |||> Array.fold2(fun acc input input2-> 
        let accAdjuster = acc / 2
        let input1Adjuster = input * 4
        let input2Adjuster = input * 2
        acc + input1Adjuster + input2Adjuster - accAdjuster
        )

let array2 = [| 5; 6; 7; 8; 9 |]

functionalAggregationMultiArray (array1, array2)

let functionalAggregationTailStart (intArray : int array) : int = 
    (intArray, 0)
    ||> Array.foldBack(fun input acc -> 
        acc + (input * 4) - (acc / 2)
        )

functionalAggregationTailStart array1

let functionalAggregationMultiArrayTailStart (intArray1: int array, intArray2: int array) : int = 
    (intArray2, intArray1, 0)
    |||> Array.foldBack2(fun input2 input acc-> 
        let accAdjuster = acc / 2
        let input1Adjuster = input * 4
        let input2Adjuster = input * 2
        acc + input1Adjuster + input2Adjuster - accAdjuster
        )

functionalAggregationMultiArrayTailStart (array1, array2)

let isEven (value: int) : bool = 
    value % 2 = 0

array1
|> Array.forall isEven

let testArrayEquivalence (testArray1, testArray2) = 
    (testArray1, testArray2)
    ||> Array.forall2 (=)

let array3 = [| 0; 1; 2; 4; 4 |]

testArrayEquivalence (array1, array3)