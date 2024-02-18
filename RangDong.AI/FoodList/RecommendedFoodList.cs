using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ML.DataOperationsCatalog;
using Microsoft.ML.Trainers;

namespace AI.FoodList
{
     public class RecommendedFoodList
    {
        // tạo đối tượng context từ MLContext
        private static MLContext context = new MLContext();
        // IDataView là một interface đại diện cho tập dữ liệu dạng không thay đổi được đọc từ nguồn dữ liệu.
        private static IDataView dataView;
        // ITransformer đại diện cho một mô hình machine learning đã được huấn luyện.
        private static ITransformer model;
        // TrainTestData chứa dữ liệu được chia thành tập huấn luyện và tập kiểm thử.
        private static TrainTestData splitData;
        // EstimatorChain là một chuỗi các bước biến đổi dữ liệu và huấn luyện mô hình.
        private static EstimatorChain<ValueToKeyMappingTransformer> estimator;

        public RecommendedFoodList()
        {
            LoadData();// Load dữ liệu đầu vào từ tệp tin hoặc CSDL.
            PreProgressData();// Tiền xử lý dữ liệu, chẳng hạn như ánh xạ các giá trị sang các khóa số.
            CreateModel();// Xây dựng mô hình machine learning sử dụng thuật toán Matrix Factorization.
            EvaluateModel();  // Đánh giá hiệu suất của mô hình.

        }
        // Thực thi 
        public static void Execute()
        {
            LoadData();
            PreProgressData();
            CreateModel();
            EvaluateModel();
            Predicvalue1();
        }

        // Dự đoán kết quả dựa trên danh sách đầu vào và trả về danh sách kết quả.
        public List<ResultModel> Predicvalue(List<InputData> inputdata)
        {
            // danh sách chứ kết quả dự đoán
            List<ResultModel> results = new List<ResultModel>();
            //Tạo một đối tượng PredictionEngine từ mô hình đã được huấn luyện (model)
            //. Đối tượng này sẽ được sử dụng để thực hiện dự đoán cho các đối tượng InputData.
            var predictEngine = context.Model.CreatePredictionEngine<InputData, ResultModel>(model);
            // duyệt qua danh sách Inputdata
            foreach (var data in inputdata)
            {
                //Sử dụng predictEngine để dự đoán kết quả cho mỗi đối tượng InputData trong danh sách. Kết quả được lưu vào biến resultmodel.
                var resultmodel = predictEngine.Predict(new InputData
                {
                    CustomerId = data.CustomerId,
                    FoodListId = data.FoodListId
                });
                //Kiểm tra xem điểm số (Score) từ kết quả dự đoán có lớn hơn hoặc bằng 2.8 hay không. Nếu có, thì thêm đối tượng resultmodel vào danh sách kết quả.
                if (resultmodel.Score >= 2.8)
                {   
                    results.Add(resultmodel);
                }
            }

            // Sắp xếp danh sách kết quả theo thuộc tính Score giảm dần
            results = results.OrderByDescending(r => r.Score).ToList();
            return results;
        }

        private static void Predicvalue1()
        {
            var predictEngine = context.Model.CreatePredictionEngine<InputData, ResultModel>(model);
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "an3ciUNfaaOIYJp6ycWeSO4uZIU2",
                FoodListId = 2
            }
            ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "an3ciUNfaaOIYJp6ycWeSO4uZIU2",
                FoodListId = 4
            }
            ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "72wDxz0bG8T5loi1tZmBI0gM3yI3",
                FoodListId = 3
            }
            ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "72wDxz0bG8T5loi1tZmBI0gM3yI3",
                FoodListId = 1
            }
            ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "72wDxz0bG8T5loi1tZmBI0gM3yI3",
                FoodListId = 27
            }
          ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "72wDxz0bG8T5loi1tZmBI0gM3yI3",
                FoodListId = 21
            }
          )); 
            PrintResult(predictEngine.Predict(new InputData
          {
              CustomerId = "72wDxz0bG8T5loi1tZmBI0gM3yI3",
              FoodListId = 22
          }
          ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "72wDxz0bG8T5loi1tZmBI0gM3yI3",
                FoodListId = 27
            }
          ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "an3ciUNfaaOIYJp6ycWeSO4uZIU2",
                FoodListId = 4
            }
            ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "72wDxz0bG8T5loi1tZmBI0gM3yI3",
                FoodListId = 8
            }
          ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "72wDxz0bG8T5loi1tZmBI0gM3yI3",
                FoodListId = 17
            }
            ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "an3ciUNfaaOIYJp6ycWeSO4uZIU2",
                FoodListId = 11
            }
            ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "an3ciUNfaaOIYJp6ycWeSO4uZIU2",
                FoodListId = 20
            }
            ));
            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "kTFgvQgpmyYMW1fhyopWYQSQx3C2",
                FoodListId = 1
            }
         )); 
            PrintResult(predictEngine.Predict(new InputData
         {
             CustomerId = "kTFgvQgpmyYMW1fhyopWYQSQx3C2",
             FoodListId = 11
         }
        ));

            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "kTFgvQgpmyYMW1fhyopWYQSQx3C2",
                FoodListId = 11
            }));

            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "PioLfJtd66M8oUMnHFBH8tsM64J2",
                FoodListId = 20
            }));

            PrintResult(predictEngine.Predict(new InputData
            {
                CustomerId = "PioLfJtd66M8oUMnHFBH8tsM64J2",
                FoodListId = 9
            }));

        }
        //In kết quả gợi ý dựa trên Score
        private static void PrintResult(ResultModel result)
        {
            Console.WriteLine($"CustomerId: {result.CustomerId} | FoodId: {result.FoodListId} | Score: {result.Score} : Is Recommended: {result.Score > 2.6}");
        }

        // Đánh giá mô hình
        private static void EvaluateModel()
        {
            var predictions = model.Transform(splitData.TestSet);
            var metrics = context.Recommendation().Evaluate(predictions, labelColumnName: nameof(InputData.Rating));
            Console.WriteLine(metrics.ToString());
        }

        // Xây dựng mô hình. Thuật toán Matrix Factorization. Khoảng đánh giá món ăn từ 1 đến 5 sao
        public static void CreateModel()
        {
            var options = new MatrixFactorizationTrainer.Options
            {
                LabelColumnName = nameof(InputData.Rating),// Đây là tên cột trong dữ liệu đầu vào (InputData) chứa điểm đánh giá. Tùy chọn này xác định cột dùng để đo lường sự tương quan giữa các yếu tố trong ma trận đánh giá.
                MatrixColumnIndexColumnName = "Encoded_CustomerId",//chứa chỉ số dùng để tham chiếu đến các cột và hàng trong ma trận đánh giá
                MatrixRowIndexColumnName = "Encoded_FoodListId",//chứa chỉ số dùng để tham chiếu đến các cột và hàng trong ma trận đánh giá
                NumberOfIterations = 300,//Số lần lặp trong quá trình đào tạo.
                ApproximationRank = 100,//ước tính về độ phức tạp của ma trận đánh giá được phân tách.
            };
            var trainer = context.Recommendation().Trainers.MatrixFactorization(options);
            var pipeline = estimator.Append(trainer);
            model = pipeline.Fit(splitData.TrainSet);
        }


        // Dữ liệu trước tiến trình
        public static void PreProgressData()
        {
            estimator = context.Transforms.Conversion.MapValueToKey(
                outputColumnName: "Encoded_CustomerId", inputColumnName: "CustomerId")
                .Append(context.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "Encoded_FoodListId", inputColumnName: "FoodListId"));

            var PreProgressData = estimator.Fit(dataView).Transform(dataView);

            splitData = context.Data.TrainTestSplit(PreProgressData, 0.05);
        }
        // Load dữ liệu đầu vào 
        public static void LoadData()
        {
            /*
            var dbLoader = context.Data.CreateDatabaseLoader<InputData>();
            // Chuỗi kết nối đến CSDL SQL SV
            string con = @"Data Source=rangdong\dongchau;Initial Catalog=EattingNowApp;Integrated Security=True";
            // Câu lệnh trruy vấn lịch sử mua hàng khách hàng
            // string query = "SELECT CTM.CustomerId, ODL.FoodListId, FLOOR(RAND()*(ODL.FoodListId)+ ODL.FoodListId) AS FoodListRating  FROM Customer CTM  INNER JOIN OrderHeader OD ON CTM.CustomerId = OD.CustomerId INNER JOIN OrderLine ODL ON OD.OrderHeaderId = ODL.OrderHeaderId";
            string query = "SELECT CTM.CustomerId, ODL.FoodListId, FR.Rating\r\nFROM Customer CTM\r\nINNER JOIN OrderHeader OD ON CTM.CustomerId = OD.CustomerId\r\nINNER JOIN OrderLine ODL ON OD.OrderHeaderId = ODL.OrderHeaderId\r\nLEFT JOIN FoodRating FR ON ODL.FoodListId = FR.FoodId\r\nWHERE FR.Rating IS NOT NULL;\r\n";
            var databaseSource = new DatabaseSource(SqlClientFactory.Instance, con, query);
            // Traning Data
            dataView = dbLoader.Load(databaseSource);
            var preview = dataView.Preview();
            */
            dataView = context.Data.LoadFromTextFile<InputData>("D:\\EATINGNOW\\RangDong.AI\\Data\\dataset_train.csv", ',', true);
        }
    }
}

