module Tests

open NUnit.Framework
open AlgoHelper.StringAlgos

[<TestFixture>]
type TestStringAlgorithms() = 

    [<Test>]
    member this.TestPalindromePassing () =
        Assert.That(AlgoHelper.StringAlgos.checkPalindrome "trap a for of apart", Is.True)
    [<Test>]
    member this.TestPalindromeFails () =
        Assert.That(AlgoHelper.StringAlgos.checkPalindrome " Spectre of the Spire ", Is.False)
    [<Test>]
    member this.TestPalindromeOddInputPasses () =
        Assert.Multiple(fun () -> 
            Assert.That(checkPalindrome "traP    a For of Apart; ", Is.True)
            Assert.That(checkPalindrome "t#raPa   For !of8 Ap0art; ", Is.True)
            Assert.That(checkPalindrome " ???   121212  traP    a For of Apart;... ", Is.True)
        )
    