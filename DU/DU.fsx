open System.Threading.Tasks
open System
open System.Collections.Generic

type Response<'TSuccess, 'TError> = 
    | Ok of 'TSuccess
    | Error of 'TError

type Sentiment =
    | Poor = 1
    | Bad = 2
    | Ok = 3
    | Good = 4
    | Great = 5

type Limit<'T> = 
    | Upper of 'T
    | Lower of 'T
    with 
    member this.Value = 
        match this with 
        | Upper x -> x
        | Lower y -> y

type NumericHeuristic = {
    Name : string
    Value : double
    Description : string
    UpperLimit : Limit<double>
    LowerLimit : Limit<double>
} with
    member this.Sentiment : Sentiment = 
        if this.Value > this.UpperLimit.Value || this.Value < this.LowerLimit.Value then Sentiment.Poor else Sentiment.Good

type Performance = {
    Heuristics : Map<string, float>
    OverallSentiment : Sentiment
}

type KPIRecord = {
    Name : string
    Description : Option<string>
    Value : double
}

type Plugin<'TSuccess, 'TFailure> = 
    | Zone of 'TSuccess
    | Prediction of 'TSuccess
    | Safety of Response<'TSuccess, 'TFailure>
    | Quality of Response<'TSuccess, 'TFailure>
    | Reliability of Performance
    | Simulation of List<'TSuccess>
    | KPI of KPIRecord
    | DataRetrieval of List<'TSuccess>

type PluginKind =
    | Zone
    | Prediction
    | Safety
    | Quality
    | Reliability
    | Simulation
    | KPI
    | DataRetrieval

type IModelPlugin<'TSuccess, 'TFailure> =
    abstract member Name : string
    abstract member Id : System.Guid
    abstract member Version : double
    abstract member ModelType : PluginKind
    abstract member RequiredInputs : seq<string>
    abstract member RequiredPlugins : seq<string>
    abstract member InitializeAsync : IServiceProvider -> Task
    abstract member ExecuteAsync : IDictionary<string, obj> -> Task<Plugin<'TSuccess, 'TFailure>>

module PluginHost =
    let runPlugin (plugin: IModelPlugin<'TSuccess, 'TFailure>) (inputs: IDictionary<string, obj>) =
        task {
            do! plugin.InitializeAsync(null)
            let! result = plugin.ExecuteAsync(inputs)
            return result
        }

type SafetyPlugin() =
    interface IModelPlugin<NumericHeuristic, string> with
        member _.Name = "Safety Monitor"
        member _.Id = Guid.NewGuid()
        member _.Version = 1.0
        member _.ModelType = PluginKind.Safety
        member _.RequiredInputs = seq ["pressure"; "temperature"]
        member _.RequiredPlugins = Seq.empty

        member _.InitializeAsync(_services: IServiceProvider) =
            printfn "Initializing Safety Plugin..."
            Task.CompletedTask

        member _.ExecuteAsync(inputs: IDictionary<string, obj>) =
            task {
                // Retrieve required inputs
                let pressure = inputs.["pressure"] :?> float
                let temp = inputs.["temperature"] :?> float

                // Simple numeric heuristic
                let heuristic = {
                    Name = "Safety Margin"
                    Value = pressure / temp
                    Description = "Pressure-to-temperature ratio"
                    UpperLimit = Upper 2.5
                    LowerLimit = Lower 0.5
                }

                // Return Plugin result
                let result =
                    if heuristic.Sentiment = Sentiment.Poor then
                        Plugin.Safety (Error "Unsafe condition detected!")
                    else
                        Plugin.Safety (Ok heuristic)

                return result
            }

type ZonePlugin() =
    interface IModelPlugin<string, string> with
        member _.Name = "Zone Calculations"
        member _.Id = Guid.NewGuid()
        member _.Version = 1.0
        member _.ModelType = PluginKind.Zone
        member _.RequiredInputs = seq ["temperature"]
        member _.RequiredPlugins = Seq.empty

        member _.InitializeAsync(_services: IServiceProvider) =
            printfn "Initializing Zone Plugin..."
            Task.CompletedTask

        member _.ExecuteAsync(inputs: IDictionary<string, obj>) =
            task {
                let temp = inputs.["temperature"] :?> float

                let zone =
                    if temp < 50.0 then "Cold Zone"
                    elif temp < 80.0 then "Normal Zone"
                    else "Overheat Zone"

                return Plugin.Zone zone
            }

type ReliabilityPlugin() =
    interface IModelPlugin<Performance, string> with
        member _.Name = "Reliability Estimator"
        member _.Id = Guid.NewGuid()
        member _.Version = 1.0
        member _.ModelType = PluginKind.Reliability
        member _.RequiredInputs = seq ["uptime"; "downtime"; "errors"]
        member _.RequiredPlugins = Seq.empty

        member _.InitializeAsync(_services: IServiceProvider) =
            printfn "Initializing Reliability Plugin..."
            Task.CompletedTask

        member _.ExecuteAsync(inputs: IDictionary<string, obj>) =
            task {
                let uptime = inputs.["uptime"] :?> float
                let downtime = inputs.["downtime"] :?> float
                let errors = inputs.["errors"] :?> int

                let reliability = uptime / (uptime + downtime)
                let errorRate = float errors / (uptime + downtime)

                let heuristics =
                    [
                        "Reliability", reliability
                        "ErrorRate", errorRate
                    ]
                    |> Map.ofList

                let sentiment =
                    if reliability > 0.95 && errorRate < 0.01 then Sentiment.Great
                    elif reliability > 0.9 then Sentiment.Good
                    elif reliability > 0.75 then Sentiment.Ok
                    elif reliability > 0.5 then Sentiment.Bad
                    else Sentiment.Poor

                let performance = { Heuristics = heuristics; OverallSentiment = sentiment }

                return Plugin.Reliability performance
            }

type KPIPlugin() =
    interface IModelPlugin<KPIRecord, string> with
        member _.Name = "System Health KPI"
        member _.Id = Guid.NewGuid()
        member _.Version = 1.0
        member _.ModelType = PluginKind.KPI
        member _.RequiredInputs = seq ["pressure"; "temperature"; "uptime"; "downtime"; "errors"]
        member _.RequiredPlugins = seq ["Safety Margin"; "Reliability Estimator"]

        member _.InitializeAsync(_services) =
            printfn "Initializing KPI Plugin..."
            Task.CompletedTask

        member _.ExecuteAsync(inputs) =
            task {
                // Create instances of required plugins
                let safety = SafetyPlugin() :> IModelPlugin<NumericHeuristic, string>
                let reliability = ReliabilityPlugin() :> IModelPlugin<Performance, string>

                let safetyResult = safety.ExecuteAsync inputs |> fun t -> t.GetAwaiter().GetResult()
                let reliabilityResult = reliability.ExecuteAsync inputs |> fun t -> t.GetAwaiter().GetResult()

                // Extract sentiments
                let safetySentiment =
                    match safetyResult with
                    | Plugin.Safety (Ok heuristic) -> heuristic.Sentiment
                    | Plugin.Safety (Error _) -> Sentiment.Poor
                    | _ -> Sentiment.Ok

                let reliabilitySentiment =
                    match reliabilityResult with
                    | Plugin.Reliability perf -> perf.OverallSentiment
                    | _ -> Sentiment.Ok

                // Combine using rule system
                let avgScore =
                    let s1 = int safetySentiment
                    let s2 = int reliabilitySentiment
                    let baseScore = float (s1 + s2) / 2.0
                    baseScore

                let kpiValue = avgScore

                let kpi = {
                    Name = "Overall System Health"
                    Description = Some "Aggregated KPI from Safety and Reliability"
                    Value = kpiValue
                }

                return Plugin.KPI kpi
            }

let runPlugins () = 
    task {
        let zonePlugin = ZonePlugin() :> IModelPlugin<string, string>
        let zoneInputs = dict ["temperature", box 72.0]
        let! zoneResult = PluginHost.runPlugin zonePlugin zoneInputs

        match zoneResult with
        | Plugin.Zone z -> printfn $"Zone: {z}"
        | _ -> printfn "Unexpected Zone result"

        // ---- Reliability Plugin ----
        let reliabilityPlugin = ReliabilityPlugin() :> IModelPlugin<Performance, string>
        let reliabilityInputs =
            dict [
                "uptime", box 870.0
                "downtime", box 30.0
                "errors", box 5
            ]

        let! reliabilityResult = PluginHost.runPlugin reliabilityPlugin reliabilityInputs

        let reliability = "Reliability"
        let errorRate = "ErrorRate"

        match reliabilityResult with
        | Plugin.Reliability perf ->
            printfn $"Reliability: {(perf.Heuristics.Item reliability) * 100.0:F1}%%"
            printfn $"Error Rate: {(perf.Heuristics.Item errorRate) * 100.0:F2}%%"
            printfn $"Overall Sentiment: {perf.OverallSentiment}"
        | _ -> printfn "Unexpected reliability result."

        // ---- Safety Plugin ----
        let safetyPlugin = SafetyPlugin() :> IModelPlugin<NumericHeuristic, string>
        let safetyInputs =
            dict [
                "pressure", box 100.0
                "temperature", box 50.0
            ]
        let! safetyResult = PluginHost.runPlugin safetyPlugin safetyInputs
        // https://www.w3schools.com/charsets/ref_utf_dingbats.asp
        match safetyResult with
            | Plugin.Safety (Ok heuristic) ->
                printfn $"✅ Safety OK: {heuristic.Name} -> Value = {heuristic.Value}, Sentiment = {heuristic.Sentiment}"
            | Plugin.Safety (Error msg) ->
                printfn $"❌ Safety Error: {msg}"
            | _ ->
                printfn "Unexpected plugin result."

        // ---- KPI Plugin ----
        let kpiPlugin = KPIPlugin() :> IModelPlugin<KPIRecord, string>
        let kpiInputs =
            dict [
                "uptime", box 870.0
                "downtime", box 30.0
                "errors", box 5
                "pressure", box 100.0
                "temperature", box 50.0
            ]

        let! kpiResult = PluginHost.runPlugin kpiPlugin kpiInputs

        let defaultValue = "";
        match kpiResult with
        | Plugin.KPI k ->
            printfn $"KPI: {k.Name}"
            printfn $"Description: {k.Description |> Option.defaultValue defaultValue}"
            printfn $"Value: {k.Value:F1}"
        | _ -> printfn "Unexpected plugin result."
    } |> ignore