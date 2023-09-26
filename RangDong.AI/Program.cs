using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.FoodList;
using Microsoft.ML;
using Microsoft.ML.Runtime;

namespace AI
{
    public class Program
    {
        static void Main(string[] args)
        {
            RecommendedFoodList.Execute();
            Console.ReadLine();
        }
    }
}
