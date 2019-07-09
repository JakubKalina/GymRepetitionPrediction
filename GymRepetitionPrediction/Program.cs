using Microsoft.ML;
using System;
using System.IO;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymRepetitionPrediction
{
    class Program
    {
        /// <summary>
        /// Path to the file containing training data
        /// </summary>
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "Zeszyt1.csv");

        /// <summary>
        /// Firebase configuration object
        /// </summary>
        static IFirebaseConfig config;

        /// <summary>
        /// Firebase database client
        /// </summary>
        static IFirebaseClient client;

        /// <summary>
        /// List of training data retrieved from database
        /// </summary>
        static List<GymRepetition> trainingDataList;

        static void Main(string[] args)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Add new training data");
            Console.WriteLine("2. Predict exercise");
            switch(Int32.Parse(Console.ReadLine()))
            {
                case 1:
                    var newModel = GetUserInput();
                    var addingTask = AddDataToFirebase(newModel);
                    addingTask.Wait();
                    break;
                case 2:
                    // Configure and connect to Firebase
                    ConfigureFirebase();
                    ConnectClientFirebase();
                    var task = RetrieveDataFromFirebaseAsync();
                    task.Wait();

                    // New ML context
                    MLContext mlContext = new MLContext(seed: 0);

                    // Return trained model
                    var model = Train(mlContext, _trainDataPath);

                    // User input data to build prediction around
                    var predictDataModel = GetUserInputData();

                    // Predict 
                    Predict(mlContext, model, predictDataModel);
                    break;
            }
        }

        /// <summary>
        /// Configure firebase connection
        /// </summary>
        public static void ConfigureFirebase()
        {
            config = new FirebaseConfig()
            {
                AuthSecret = "X2ALVqwVJ9t7M3xMptU4o2gbDhAMSUL0YXYFnD2G",
                BasePath = "https://gymrepetitionprediction.firebaseio.com/"
            };
        }

        /// <summary>
        /// Establish firebase connection
        /// </summary>
        public static void ConnectClientFirebase()
        {
            client = new FireSharp.FirebaseClient(config);
        }

        /// <summary>
        /// Retrieve data from Firebase database
        /// </summary>
        public static async System.Threading.Tasks.Task RetrieveDataFromFirebaseAsync()
        {
            FirebaseResponse response = await client.GetTaskAsync("");
            var result = JsonConvert.DeserializeObject<List<GymRepetition>>(response.Body);
            //trainingDataList = result;

            // Test input training data
            trainingDataList = new List<GymRepetition>()
            {
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 20,
                    Weight = 20
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 20,
                    Weight = 20
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 20,
                    Weight = 20
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 18,
                    Weight = 25
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 18,
                    Weight = 25
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 15,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 15,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 15,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 15,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 10,
                    Weight = 40
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 10,
                    Weight = 40
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 10,
                    Weight = 40
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 10,
                    Weight = 50
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 5,
                    Weight = 60
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 5,
                    Weight = 60
                },
                new GymRepetition()
                {
                    Exercise = "Squat",
                    Repetitions = 5,
                    Weight = 60
                },
                new GymRepetition()
                {
                    Exercise = "Pull Up",
                    Repetitions = 7,
                    Weight = 0
                },
                new GymRepetition()
                {
                    Exercise = "Pull Up",
                    Repetitions = 7,
                    Weight = 0
                },
                new GymRepetition()
                {
                    Exercise = "Pull Up",
                    Repetitions = 5,
                    Weight = 0
                },
                new GymRepetition()
                {
                    Exercise = "Pull Up",
                    Repetitions = 6,
                    Weight = 0
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 10,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 10,
                    Weight = 20
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 10,
                    Weight = 25
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 10,
                    Weight = 25
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 10,
                    Weight = 25
                },
                new GymRepetition()
                {
                    Exercise = "Biceps Curl",
                    Repetitions = 10,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Biceps Curl",
                    Repetitions = 10,
                    Weight = 12.5f
                },
                new GymRepetition()
                {
                    Exercise = "Biceps Curl",
                    Repetitions = 10,
                    Weight = 12.5f
                },
                new GymRepetition()
                {
                    Exercise = "Biceps Curl",
                    Repetitions = 5,
                    Weight = 15
                },
                new GymRepetition()
                {
                    Exercise = "Lat Pulldown",
                    Repetitions = 10,
                    Weight = 55
                },
                new GymRepetition()
                {
                    Exercise = "Lat Pulldown",
                    Repetitions = 10,
                    Weight = 55
                },
                new GymRepetition()
                {
                    Exercise = "Lat Pulldown",
                    Repetitions = 10,
                    Weight = 55
                },
                new GymRepetition()
                {
                    Exercise = "Lat Pulldown",
                    Repetitions = 10,
                    Weight = 55
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 20,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 20,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 20,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 20,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 20,
                    Weight = 5
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 20,
                    Weight = 5
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 20,
                    Weight = 5
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 15,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 15,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 15,
                    Weight = 10
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 5,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 5,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 5,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 5,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 5,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 5,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 5,
                    Weight = 30
                },
                new GymRepetition()
                {
                    Exercise = "Rack Pull",
                    Repetitions = 5,
                    Weight = 30
                }


            };
        }


        /// <summary>
        /// Add new data to Firebase database
        /// </summary>
        public static async System.Threading.Tasks.Task AddDataToFirebase(GymRepetition newModel)
        {
            SetResponse response = await client.SetTaskAsync("gymrepetitionprediction/15", newModel);
            GymRepetition result = response.ResultAs<GymRepetition>();

            Console.WriteLine(result);
        }

        /// <summary>
        /// Returns object that consist of data inserted by user
        /// </summary>
        /// <returns></returns>
        public static GymRepetition GetUserInput()
        {
            var result = new GymRepetition();
            Console.WriteLine("Exercise name: ");
            result.Exercise = Console.ReadLine();
            Console.WriteLine("Number of repetitions: ");
            result.Repetitions = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Used weight: ");
            result.Weight = Int32.Parse(Console.ReadLine());
            return result;
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
            IDataView dataView = mlContext.Data.LoadFromEnumerable<GymRepetition>(trainingDataList);

            // Read data from external file
           // IDataView dataView = mlContext.Data.LoadFromTextFile<GymRepetition>(dataPath, hasHeader: true, separatorChar: ';');

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
