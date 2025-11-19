open System
open System.Linq

let palindrome = "trap a for of apart"
module RecordManager = 
// Chain functions
    let noWhitespace (word: string) = 
        word.StartsWith(" ") || word.EndsWith(" ")

    let noSpaces (word: string) = 
        let checkForSpaces = 
            word.Split(" ")
            |> Array.length
        checkForSpaces = 1
    
    let noUpperCase (word: string) = 
        let upperCheck = word.ToCharArray().Any(fun x -> Char.IsUpper(x))
        not upperCheck

    let noOddCharacters (word: string) = 
        let characterCheck = 
            word
            |> Seq.forall Char.IsLetterOrDigit
        not characterCheck

    let noNumbers (word: string) = 
        let numberCheck = 
            word
            |> Seq.exists Char.IsDigit
        not numberCheck

    // Chain Initializer
    let private check f (record, result) =
        if not result then record, false
        else record, f(record)

    // Chain Executor
    let private chain = check noWhitespace >> check noSpaces >> check noUpperCase >> check noOddCharacters >> check noNumbers
    let ChainOfResponsibility record = chain (record, true)

open RecordManager
let cleanWord (word : string) = 
    word.Trim().Replace(" ", "").ToLower()
    |> Seq.filter Char.IsLetter
    |> String.Concat
let checkPalindrome (word : string) : bool = 
    if snd (ChainOfResponsibility word)
    then 
        let inverse : string = 
            word
            |> Seq.filter(fun c -> not (Char.IsWhiteSpace(c)))
            |> Seq.rev
            |> String.Concat
        inverse = word
    else 
        let inverse : string = 
            cleanWord word
            |> Seq.filter(fun c -> not (Char.IsWhiteSpace(c)))
            |> Seq.rev
            |> String.Concat
        inverse = cleanWord word

// Word Check
checkPalindrome palindrome
checkPalindrome " Spectre of the Spire "
checkPalindrome "traP    a For of Apart; "

