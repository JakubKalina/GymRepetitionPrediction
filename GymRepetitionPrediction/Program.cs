using Microsoft.ML;
using System;
using System.IO;

namespace GymRepetitionPrediction
{
    class Program
    {
        /// <summary>
        /// Path to the file containing training data
        /// </summary>
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "Zeszyt1.csv");

        static void Main(string[] args)
        {
            // New ML context
            MLContext mlContext = new MLContext(seed: 0);

            // Return trained model
            var model = Train(mlContext, _trainDataPath);

            // User input data to build prediction around
            var predictDataModel = GetUserInputData();

            // Predict 
            Predict(mlContext, model, predictDataModel);
        }

        /// <summary>
        /// Returns object with data inserted by user
        /// </summary>
        /// <returns></returns>
        public static GymRepetition GetUserInputData()
        {
            var result = new GymRepetition();

            Console.WriteLine("Insert exercise name: ");
            result.Exercise = Console.ReadLine();

            Console.WriteLine("Insert number of repetitions: ");
            result.Repetitions = float.Parse(Console.ReadLine());

            result.Weight = 0;

            return result;
        }

        /// <summary>
        /// Train model
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            // Read data from external file
            IDataView dataView = mlContext.Data.LoadFromTextFile<GymRepetition>(dataPath, hasHeader: true, separatorChar: ';');

            // Create pipeline
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Weight").Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "ExerciseEncoded", inputColumnName: "Exercise"))
                                                                                                                 .Append(mlContext.Transforms.Concatenate("Features", "ExerciseEncoded", "Repetitions"))
                                                                                                                 .Append(mlContext.Regression.Trainers.FastForest());

            // Create model
            var model = pipeline.Fit(dataView);

            // Return model
            return model;
        }

        /// <summary>
        /// Predict value
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="model"></param>
        public static void Predict(MLContext mlContext, ITransformer model, GymRepetition predictionData)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<GymRepetition, GymRepetitionPrediction>(model);

            var prediction = predictionFunction.Predict(predictionData);

            Console.WriteLine("Predicted data");
            Console.WriteLine($"Weight: {prediction.Weight:0.####}");          
            Console.ReadLine();
        }

    }
}
