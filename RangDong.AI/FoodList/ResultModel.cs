using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FoodList
{
    public class ResultModel :InputData
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
