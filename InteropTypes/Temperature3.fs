namespace InteropTypes.Temperature3

open System
open System.ComponentModel
open System.Globalization

type Temperature3(fahrenheit: float) = 
    member val Fahrenheit = fahrenheit with get, set
    with 
        member this.Celcius = 
            (5.0 / 9.0) * (this.Fahrenheit - 32.0)
        override this.ToString() = 
            this.Fahrenheit.ToString(CultureInfo.InvariantCulture)