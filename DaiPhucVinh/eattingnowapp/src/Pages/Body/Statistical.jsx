import React, { useEffect } from "react";
import { FilterListIcon } from "evergreen-ui";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Legend,
} from "chart.js";
import { ResponsiveContainer, Tooltip, PieChart, Pie, LineChart, Line, XAxis, YAxis} from "recharts";
import { Label } from "recharts";
import { Bar, Doughnut } from "react-chartjs-2";
import { ToastContainer } from "react-toastify";
import { useStateValue } from "../../context/StateProvider";
import { TakeStatisticalByStoreId, TakeLitsFoodSold } from "../../api/store/storeService";
import { useState } from "react";
import { sampleSize } from "lodash";
import { toast } from "react-toastify";
ChartJS.register(CategoryScale, LinearScale, BarElement,  Legend);
const Statistical = () => {

  const [selectedData, setSelectedData] = useState(null);

  const handlePieClick = (data, index) => {
      setSelectedData(datachart[index]);
  };
  const [datachart, setDatachart ]= useState([]);
  const [ListFoodSold, setListFoodSold]= useState([]);

  const [thongKeThang, setThongKeThang] = React.useState({
    labels: [],
    datasets: [
      {
        data: [],
      },
    ],
  });
  const [dataNhomTaiSan, setDataNhomTaiSan] = React.useState({
    labels: [],
    datasets: [
      {
        data: [],
      },
    ],
  });


  const vietnameseCurrencyFormat = {
    style: 'currency',
    currency: 'VND',
  };
  const [data, setData] = React.useState({
    revenueDate: 0,
    revenueMonth: 0,
    revenueWeek: 0,
    revenueYear: 0,
    listChart: {}
  });

  const [{ user }] = useStateValue();
  async function ViewAllStatistical() {
    if (user) {
      let response = await TakeStatisticalByStoreId({...data, storeId: user?.UserId, startDate: null, endDate: null});
      const chartLabelsMonth =
      response.item.listChart.map(
          (item) => item.nameMonth
        );
      const dataChartSoLuongKhachHangTheoNgay =
      response.item.listChart.map(
          (item) => item.revenueMonth
        );
        setThongKeThang({
        labels: chartLabelsMonth,
        datasets: [
          {
            label: "Doanh thu",
            data: dataChartSoLuongKhachHangTheoNgay,
            backgroundColor: ['#29e916'],
          },
        ],
      });
      setData(response.item);
      let responseListSold = await TakeLitsFoodSold(user?.UserId)
      if(responseListSold.success){
        setListFoodSold(responseListSold.data);
         // Sử dụng map để tạo datachart từ responseListSold.data
         setDatachart( responseListSold.data.map(item => ({
          name: item.FoodName,
          count: item.FoodCount
        })));
      }
      else{
        console.log(responseListSold.message);
      }
    }
  }

  async function onViewAppearing() {
    if (user) {
      var endDateObj = new Date(endDate);
      var startDateObj = new Date(startDate);

      var countYear = endDateObj.getFullYear() - startDateObj.getFullYear();

      console.log(countYear);

      if (countYear > 1) {
        toast.error('Khoảng cách không thể quá 2 năm', { autoClose: 3000 });
        return;
      }
      let response = await TakeStatisticalByStoreId({...data, storeId: user?.UserId, startDate: startDate, endDate: endDate});
      const chartLabelsMonth =
      response.item.listChart.map(
          (item) => item.nameMonth
        );
      const dataChartSoLuongKhachHangTheoNgay =
      response.item.listChart.map(
          (item) => item.revenueMonth
        );
        setThongKeThang({
        labels: chartLabelsMonth,
        datasets: [
          {
            label: "Doanh thu",
            data: dataChartSoLuongKhachHangTheoNgay,
            backgroundColor: ['#29e916'],
          },
        ],
      });
      setData(response.item);
      let responseListSold = await TakeLitsFoodSold(user?.UserId)
      if(responseListSold.success){
        setListFoodSold(responseListSold.data);
         // Sử dụng map để tạo datachart từ responseListSold.data
         setDatachart( responseListSold.data.map(item => ({
          name: item.FoodName,
          count: item.FoodCount
        })));
      }
      else{
        console.log(responseListSold.message);
      }
    }
  }
  const getStartDate = () => {
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = "01"; //String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = "01";//String(currentDate.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  };
  const getEndDate = () => {
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = "12"; //String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = "31";//String(currentDate.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  };

  const [startDate, setstartDate] = useState(getStartDate());
  const [endDate, setendDate] = useState(getEndDate());
  const handleStartDateChange = (e) => {
    setstartDate(e.target.value);
  };
  const handleendDateChange = (e) => {
    setendDate(e.target.value);
  };
  useEffect(() => {
    onViewAppearing();
  }, []);
  return (
    <div className="bg-bodyBg h-[100%] basis-80 p-8 overflow-x-scroll scrollbar-none">
      {/*Hiển thị thông báo */}
     <ToastContainer />
      {/* <div className="flex items-center justify-between">
        <div className="flex items-center border-b-2 pb-2 basis-1/2 gap-2">
          <BsSearch className="text-hoverColor text-[20px] cursor-pointer" />
          <input
            type="text"
            placeholder="Tìm nhóm món ăn..."
            className="border-none outline-none placeholder:text-sm focus:outline-none"
          />
        </div>

        <div className="flex gap-4 items-center">
          <AiOutlineAppstoreAdd className="text-hoverColor cursor-pointer text-[25px] hover:text-[20px] transition-all" />
          <button className="bg-sideMenuBg cursor-pointer text-bodyBg font-semibold py-1 px-4 rounded-[5px] hover:bg-[#55545e] transition-all">
            Quản lý
          </button>
        </div>
      </div> */}

      {/* Title Div */}
      <div className="flex items-center justify-between mt-2">
        <div className="title">
          <h1 className="text-[25px] text-titleColor tracking-[1px] font-black">
            Thống kê doanh số
          </h1>
        </div>
      </div>

      <div className="w-full h-40 flex items-center gap-5 scroll-smooth overflow-hidden justify-center mb-12 backdrop-blur">
        <div className="border-4 border-orange-100 w-275 h-full min-w-[275px] md:w-200 md:min-w-[200px] rounded-3xl py-1 px-2 my-2 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative">
          <h6 className="text-[18px] text-orange-900 font-bold flex items-center justify-center cursor-pointer">Doanh thu hôm nay</h6>
          <div className="text-[16px] text-orange-900 font-bold flex items-center justify-center cursor-pointer">{data?.revenueDate?.toLocaleString('vi-VN', vietnameseCurrencyFormat)}</div>
        </div>
        <div className="border-4 border-orange-100 w-275 h-full min-w-[275px] md:w-200 md:min-w-[200px] rounded-3xl py-1 px-2 my-2 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative">
          <h6 className="text-[18px] text-orange-900 font-bold flex items-center justify-center cursor-pointer">Doanh thu tuần này</h6>
          <div className="text-[16px] text-orange-900 font-bold flex items-center justify-center cursor-pointer">{data?.revenueWeek?.toLocaleString('vi-VN', vietnameseCurrencyFormat)}</div>
        </div>
        <div className="border-4 border-orange-100 w-275 h-full min-w-[275px] md:w-200 md:min-w-[200px] rounded-3xl py-1 px-2 my-2 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative">
          <h6 className="text-[18px] text-orange-900 font-bold flex items-center justify-center cursor-pointer">Doanh thu tháng này</h6>
          <div className="text-[16px] text-orange-900 font-bold flex items-center justify-center cursor-pointer">{data?.revenueMonth?.toLocaleString('vi-VN', vietnameseCurrencyFormat)}</div>
        </div>
        <div className="border-4 border-orange-100 w-275 h-full min-w-[275px] md:w-200 md:min-w-[200px] rounded-3xl py-1 px-2 my-2 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative">
          <h6 className="text-[18px] text-orange-900 font-bold flex items-center justify-center cursor-pointer">Doanh thu năm nay</h6>
          <div className="text-[16px] text-orange-900 font-bold flex items-center justify-center cursor-pointer">{data?.revenueYear?.toLocaleString('vi-VN', vietnameseCurrencyFormat)}</div>
        </div>
      </div>
      <div className="flex items-center border-b-2 pb-2 basis-2/2 gap-2">
          <label htmlFor="">Từ ngày: </label>
          <input 
            type="date" 
            value={startDate} 
            onChange={handleStartDateChange}/>
          <label htmlFor="">Đến ngày: </label>
          <input 
            type="date" 
            value={endDate} 
            onChange={handleendDateChange}/>
          <button 
            onClick={onViewAppearing}
            className="custom-button ml-2"
          >
            <div className="flex">
              <FilterListIcon className="icon" />
              Lọc
            </div>
          </button>
          <button 
            onClick={ViewAllStatistical}
            className="custom-button ml-2"
          >
            <div className="flex">
              <FilterListIcon className="icon" />
               Xem tổng doanh thu tháng tổng hợp
            </div>
          </button>
        </div>
      <div className="w-full">
        <React.Fragment>
          <ResponsiveContainer width="100%" aspect={2} >
            <Bar
              height="auto"
              data={thongKeThang}
              options={{
                indexAxis: "x",
                plugins: {
                  title: {
                    display: true,
                    text: "Doanh thu theo tháng",
                    font: {
                      size: 15,
                    },
                  },
                  legend: {
                    display: true,
                    position: "bottom",
                    labels: {
                      usePointStyle: true, //for style circle
                    },
                  },
                },
              }}
            />
          </ResponsiveContainer>
        </React.Fragment>   
      </div>
      <h1 className="text-[15px] text-titleColor tracking-[1px] font-black">
        Biểu đồ 10 sản phẩm bán chạy của cửa hàng
      </h1>
      <div className="flex">
            <div>
              {datachart != null ? (
                <React.Fragment>
                      <ResponsiveContainer width="100%" height={600} aspect={2}>
                    <PieChart>
                      <Tooltip formatter={(value) => `${value} số lượng`} />
                      <Pie
                        data={datachart}
                        dataKey="count"
                        cx="50%"
                        cy="50%"
                        outerRadius={80}
                        fill="#8884d8"
                        label 
                        onClick={handlePieClick}
                      >
                        <Label valueKey="name" position="insideBottom" offset={-10} fill="#fff" />
                      </Pie>
                      <Legend align="center" verticalAlign="bottom" height={36} />
                    </PieChart>
                  </ResponsiveContainer>
                </React.Fragment>
              ) : null}
            </div>
            <div>
              {selectedData && (
                <div className="details-container">
                  <h2 className="text-italic">Chi tiết số lượng</h2>
                  <div className="detail-item">
                    <span className="detail-label">Tên mặt hàng:</span>
                    <span className="detail-value">{selectedData.name}</span>
                  </div>
                  <div className="detail-item">
                    <span className="detail-label">Số lượng đã bán:</span>
                    <span className="detail-value">{`${selectedData.count}`}</span>
                  </div>
                  {/* Add more details if needed */}
                </div>
              )}
            </div>
      </div>
      <h1 className="text-[15px] text-titleColor tracking-[1px] font-black">
        Bảng 10 sản phẩm bán chạy của cửa hàng
      </h1>
      <div>
          <table className="styled-table">
           <thead>
                  <tr>
                    <th>ID</th>
                    <th>Tên mặt hàng</th>
                    <th>Số lượng đã bán</th>
                  </tr>
           </thead>
           <tbody>
                {ListFoodSold.sort((a, b) => b.FoodCount - a.FoodCount).map((item, index) => (
                    <tr key={index}>
                      <td>{item.FoodListId}</td>
                      <td>{item.FoodName}</td>
                      <td>{item.FoodCount}</td>
                    </tr>
                  ))}
           </tbody>
         </table>
      </div>
    </div>
  );
};

export default Statistical;
