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

            Predict(mlContext, model);
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
        public static void Predict(MLContext mlContext, ITransformer model)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<GymRepetition, GymRepetitionPrediction>(model);
            // Sample data
            var gymRepetition = new GymRepetition()
            {
                Exercise = "Squat",
                Repetitions = 10,
                Weight = 0
            };

            var prediction = predictionFunction.Predict(gymRepetition);

            Console.WriteLine("Predicted data");
            Console.WriteLine($"Weight: {prediction.Weight:0.####}");          
            Console.ReadLine();
        }

    }
}
