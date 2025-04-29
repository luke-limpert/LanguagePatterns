module InteropTypes.Temperature

open InteropTypes.Converter
open System.ComponentModel
open System.Globalization

[<TypeConverter(typeof<TemperatureConverter>)>]
type Temperature(fahrenheit: float) = 
    member val Fahrenheit = fahrenheit with get, set
    with 
        member this.Celcius = 
            (5.0 / 9.0) * (this.Fahrenheit - 32.0)
        override this.ToString() = 
            this.Fahrenheit.ToString(CultureInfo.InvariantCulture)