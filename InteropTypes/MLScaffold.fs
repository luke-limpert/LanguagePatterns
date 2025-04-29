namespace InteropTypes.MLScaffold
open System.IO
open Microsoft.ML
open MLModel;

type DataInput = 
    private 
    | ShapePredictor of ShapePredictor.ModelInput
    static member CreateShapePredictorInput(shapePredictor : ShapePredictor.ModelInput) : DataInput = 
        ShapePredictor shapePredictor

[<NoEquality; NoComparison>]
type Model(schema: DataViewSchema, model: ITransformer, engine: obj) = 
    new() = Model(null,null,null)
    member this.Context = MLContext()
    member val PredictionEngine = engine with get, set
    member val Model : ITransformer = model with get, set
    member val Schema : DataViewSchema = schema with get, set
    member this.LoadModel(filepath) = 
        use MLNetModelPath : FileStream = new FileStream(Path.GetFullPath(filepath), FileMode.Open, FileAccess.Read, FileShare.Read)
        let mutable schema : DataViewSchema = null
        this.Model <- this.Context.Model.Load(MLNetModelPath, &schema)
        this.Schema <- schema
    member private this.GeneratePredictionEngine(input: DataInput) =
        match input with
        | ShapePredictor _ -> 
            this.PredictionEngine <- this.Context.Model.CreatePredictionEngine<ShapePredictor.ModelInput, ShapePredictor.ModelOutput>(this.Model) :> obj;
    member this.CanPredict(input : DataInput) = 
        match this.PredictionEngine with 
        | null -> 
            printfn "Cannot Predict. Generating Engine... "
            this.GeneratePredictionEngine(input)
            printfn "Proceed with Prediction."
            true
        | _ -> 
            printfn "Proceed with Prediction."
            true
    member this.Predict(dataInput : DataInput) = 
        if this.PredictionEngine = null
        then this.GeneratePredictionEngine(dataInput)
        else()
        match dataInput with 
        | ShapePredictor s -> 
            let SPEngine = this.PredictionEngine :?> PredictionEngine<ShapePredictor.ModelInput, ShapePredictor.ModelOutput>
            SPEngine.Predict(s)

        


