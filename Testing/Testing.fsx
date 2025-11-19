open System

// take a number (4 digits) sum the individual digits (1111) 
// number is 1111 for testing
let sumOfDigits (fourDigitNumber : int) = 
    fourDigitNumber.ToString()
    |> Seq.map(fun x -> Int32.Parse (string x) )
    |> Seq.sum

sumOfDigits 1111

