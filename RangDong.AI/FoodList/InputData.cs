using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FoodList
{
    public class InputData
    {
        [LoadColumn(0)]
        public string CustomerId { get; set; }
        [LoadColumn(1)]
        public int FoodListId { get; set; }
        [LoadColumn(2)]
        public float  Rating { get; set;}

    }
}
