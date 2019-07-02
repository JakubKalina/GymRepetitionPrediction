using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymRepetitionPrediction
{
    public class GymRepetition
    {
        /// <summary>
        /// Nazwa ćwiczenia
        /// </summary>
        [LoadColumn(0)]
        public string Exercise;
        /// <summary>
        /// Ilość powtórzeń
        /// </summary>
        [LoadColumn(1)]
        public int Repetitions;
        /// <summary>
        /// Obiążenie
        /// </summary>
        [LoadColumn(2)]
        public float Weight;
    }

    public class GymRepetitionPrediction
    {
        /// <summary>
        /// Przewidywane obciązenies
        /// </summary>
        [ColumnName("Score")]
        public float Weight;
    }
}
