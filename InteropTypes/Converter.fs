namespace InteropTypes.Converter

open System
open System.ComponentModel
open System.Globalization

type TemperatureConverter() =
    inherit TypeConverter()

    override this.CanConvertFrom(context, sourceType) =
        sourceType = typeof<string> || base.CanConvertFrom(context, sourceType)

    override this.ConvertFrom(context, culture, value) =
        match value with
        | :? string as str ->
            match Double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture) with
            | (true, result) -> float result
            | _ -> raise (InvalidOperationException("Invalid temperature format"))
        | _ -> base.ConvertFrom(context, culture, value)