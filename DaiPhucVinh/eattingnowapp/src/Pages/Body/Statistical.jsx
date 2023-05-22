import React, { useEffect } from "react";
import { AiOutlineAppstoreAdd } from "react-icons/ai";
import { BsSearch } from "react-icons/bs";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Tooltip,
  Legend,
} from "chart.js";
import { Bar, Doughnut } from "react-chartjs-2";
import { useStateValue } from "../../context/StateProvider";
import { TakeStatisticalByStoreId } from "../../api/store/storeService";
ChartJS.register(CategoryScale, LinearScale, BarElement, Tooltip, Legend);

const Statistical = () => {
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
  const [dataTop10TaiSan, setTop10TaiSan] = React.useState({
    labels: ["Tháng 1", "Tháng 2", "Tháng 3"],
    datasets: [
      {
        data: [100, 250, 300],
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
  async function onViewAppearing() {
    if (user) {
      let response = await TakeStatisticalByStoreId({...data, storeId: user?.UserId});
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
            label: "Tổng số lượng được tạo trong ngày ",
            data: dataChartSoLuongKhachHangTheoNgay,
            backgroundColor: ['#29e916'],
          },
        ],
      });
      setData(response.item);
    }
  }
  useEffect(() => {
    onViewAppearing();
  }, []);
  console.log("data", data)
  return (
    <div className="bg-bodyBg h-[100%] basis-80 p-8 overflow-x-scroll scrollbar-none">
      <div className="flex items-center justify-between">
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
      </div>

      {/* Title Div */}
      <div className="flex items-center justify-between mt-8">
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
      <div className="w-full">
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
      </div>
    </div>
  );
};

export default Statistical;
