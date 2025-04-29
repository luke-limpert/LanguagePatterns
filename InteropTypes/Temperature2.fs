module InteropTypes.Temperature2
    type Temperature2 = 
        {
            Fahrenheit : double
        }
        with 
            member this.Celcius = 
                (5.0 / 9.0) * (this.Fahrenheit - 32.0)