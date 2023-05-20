import React from "react";
import _ from "lodash";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Tooltip,
  Legend,
} from "chart.js";
import { Bar } from "react-chartjs-2";
ChartJS.register(CategoryScale, LinearScale, BarElement, Tooltip, Legend);
ChartJS.defaults.font.size = 9;
export default function BarChart({ sources }) {
  const [chartData, setChartData] = React.useState({
    labels: [],
    datasets: [],
  });
  async function onViewAppearing() {
    let tongtien = [];
    if (sources[sources.length - 1] == false) {
      var ngay = _.uniq([...sources.map((e) => e.NgayKy)]);
      ngay.forEach((e) => {
        var HD = _.filter(sources, { NgayKy: e });
        var amtHD = _.sumBy(HD, "TongTien");
        var amtHD_2 = amtHD / 1000000;
        tongtien = [...tongtien, amtHD_2];
      });
      setChartData({
        labels: _.uniq([
          ...sources.slice(0, sources.length - 1).map((e) => e.NgayKy),
        ]),
        datasets: [
          {
            label: "Doanh thu ngày",
            data: [...tongtien],
            backgroundColor: "rgba(0,255,0,0.7)",
          },
        ],
      });
    } else {
      var thang = _.uniq([...sources.map((e) => e.Thang)]);
      thang.forEach((e) => {
        var HD = _.filter(sources, { Thang: e });
        var amtHD = _.sumBy(HD, "TongTien");
        var amtHD_2 = amtHD / 1000000;
        tongtien = [...tongtien, amtHD_2];
      });
      setChartData({
        labels: _.uniq([
          ...sources.slice(0, sources.length - 1).map((e) => `${e.Thang}`),
        ]),
        datasets: [
          {
            label: "Doanh thu tháng",
            data: [...tongtien],
            backgroundColor: "rgba(0,255,0,0.7)",
          },
        ],
      });
    }
  }
  React.useEffect(() => {
    onViewAppearing();
    return () => {
      setChartData({
        labels: [],
        datasets: [],
      });
    };
  }, [sources]);
  return (
    <Bar
      height={200}
      data={chartData}
      options={{
        //indexAxis: "y",
        plugins: {
          legend: {
            display: true,
            position: "bottom",
            labels: {
              usePointStyle: true,
              font: {
                size: 9,
              },
            },
          },
        },
      }}
    />
  );
}
