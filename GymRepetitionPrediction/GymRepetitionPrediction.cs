using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymRepetitionPrediction
{
    public class GymRepetition
    {
        /// <summary>
        /// Exercise name
        /// </summary>
        [LoadColumn(0)]
        public string Exercise;
        /// <summary>
        /// Performed repetitions
        /// </summary>
        [LoadColumn(1)]
        public float Repetitions;
        /// <summary>
        /// Weight
        /// </summary>
        [LoadColumn(2)]
        public float Weight;
    }

    public class GymRepetitionPrediction
    {
        /// <summary>
        /// Weight
        /// </summary>
        [ColumnName("Score")]
        public float Weight;
    }
}
