// Counting DNA Nucleotides

let dnaString = "AGCTTTTCATTCTGACTGCAACGGGCAATATGTCTCTGTGTGGATTAAAAAAAGAGTGTCTGATAGCAGC"

let dnacharacters = dnaString.ToCharArray()

dnacharacters
|> Array.countBy id
|> Array.map snd

// The second nucleic acid

let dnaString2 = "GATGGAACTTGACTACGTAAATT"

let convertToRNA (dna : string) : string = dnaString2.Replace("T", "U")

dnaString2
|> convertToRNA

// the secondary and tertiary structures of dna

let dnaString3 = "AAAACCCGGT"

let reverseComplement (character : char) = 
    match character with
    | 'T' -> "A"
    | 'A' -> "T"
    | 'C' -> "G"
    | 'G' -> "C"
    | _ -> ""

let dnaComplement = 
    let generateComplement = 
        dnaString3.ToCharArray()
            |> Array.map(fun character -> reverseComplement character)
            |> Array.rev 
    String.concat "" generateComplement 

// Wascally Wabbits

let rec estimateRabbits (months : int) (litters : int) : int64 = 
    if (months = 1) 
    then 1
    else 
        if (months = 2) 
        then litters
        else 
            if (months <= 4)
            then (estimateRabbits (months-1) litters) + (estimateRabbits (months-2) litters)
            else (estimateRabbits (months-1) litters) + ((estimateRabbits (months-2) litters) * int64 litters)

estimateRabbits 33 2 

// Computing GC Content

let sampleDataset = 
    ">Rosalind_6404
    CCTGCGGAAGATCGGCACTAGAATAGCCAGAACCGTTTCTCTGAGGCTTCCGGCCTTCCC
    TCCCACTAATAATTCTGAGG
    >Rosalind_5959
    CCATCGGTAGCGCATCCTTAGTCCAATTAAGTCCCTATCCAGGCGCTCCGCCGAAGGTCT
    ATATCCATTTGTCAGCAGACACGC
    >Rosalind_0808
    CCACCCTCGTGGTATGGCTAGGCATTCAGGAACCGGAGAACGCTTCAGACCAGCCCGGAC
    TGGGAACCTGCGGGCAGTAGGTGGAAT"

let retrieveIDandDnaString (fasta : string) = 
    fasta.[0..3], fasta.Substring 4

let strandArray = [|'C'; 'G'; 'A'; 'T';|]

let computeGCContent (s : string) = 
    let denominator = 
        s.ToCharArray()
        |> Array.countBy id
        |> Array.filter(fun (c, _) -> strandArray |> Array.contains c )
        |> Array.sumBy snd
    let numerator = 
        s.ToCharArray()
        |> Array.filter(fun strand -> strand.Equals 'G' || strand.Equals 'C')
        |> Array.length
    float numerator / float denominator * 100.0

let returnHighestGCContent (dataset : string) = 
    dataset.Split(">Rosalind_")
    |> Array.removeAt 0
    |> Array.map retrieveIDandDnaString
    |> Array.map(fun (id, strand) -> 
        "Rosalind_" +  id, computeGCContent strand
        )
    |> Array.maxBy snd

open System.IO
let testDataset = 
    let dataset = File.ReadAllLines "./RosalindProblems/Datasets/rosalind_gc.txt"
    String.concat "" dataset

returnHighestGCContent testDataset

type GCContentTest = {
    id : string
    GCContent : float
}

let returnHighestGCContentRecord (dataset : string) : GCContentTest = 
    dataset.Split(">Rosalind_")
    |> Array.removeAt 0
    |> Array.map retrieveIDandDnaString
    |> Array.map(fun (id, strand) -> 
        "Rosalind_" +  id, computeGCContent strand
        )
    |> Array.map(fun (s, f) -> {
        id = s
        GCContent = f
    })
    |> Array.maxBy(fun gctest -> gctest.GCContent)

returnHighestGCContentRecord testDataset

// Counting point mutations
let calculateHammingDistance (string1 : string) (string2 : string) = 
    let array1 = string1.ToCharArray()
    let array2 = string2.ToCharArray()
    array1
    |> Array.zip array2
    |> Array.filter(fun (f, s) -> f <> s)
    |> Array.length

let getDataset filepath = 
    File.ReadAllLines filepath

let info = getDataset "./RosalindProblems/Datasets/rosalind_hamm.txt" 

calculateHammingDistance info.[0] info.[1]