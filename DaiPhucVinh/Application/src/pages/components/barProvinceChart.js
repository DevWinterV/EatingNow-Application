import React from "react";
import _ from "lodash";
// import {
//   Chart as ChartJS,
//   CategoryScale,
//   LinearScale,
//   LineElement,
//   PointElement,
//   Tooltip,
//   Legend,
// } from "chart.js";
// import { Line } from "react-chartjs-2";
// ChartJS.register(
//   CategoryScale,
//   LinearScale,
//   LineElement,
//   PointElement,
//   Tooltip,
//   Legend
// );
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

export default function BarProvinceChart({ sources }) {
  const [chartData, setChartData] = React.useState({
    labels: [],
    datasets: [],
  });

  async function onViewAppearing() {
    var tinh = _.uniq([...sources.map((e) => e.City_Id)]);
    var tongtien = [];
    tinh.forEach((e) => {
      var HD = _.filter(sources, { City_Id: e });
      var amtHD = _.sumBy(HD, "TongTien");
      var amtHD_2 = amtHD / 1000000;
      tongtien = [...tongtien, amtHD_2];
    });
    setChartData({
      labels: _.uniq([
        ...sources.map((e) => {
          if (e.City_Name == null || e.City_Name == undefined) {
            return "Trống";
          } else {
            return e.City_Name;
          }
        }),
      ]),
      datasets: [
        {
          label: "Doanh thu",
          data: [...tongtien],
          backgroundColor: "rgba(255, 113, 0, 1)",
        },
      ],
    });
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
        indexAxis: "y",
        plugins: {
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
  );
}
