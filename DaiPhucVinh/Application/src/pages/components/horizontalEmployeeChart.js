import React from "react";
import _ from "lodash";
// import {
//   Chart as ChartJS,
//   CategoryScale,
//   LinearScale,
//   BarElement,
//   Tooltip,
//   Legend,
// } from "chart.js";
// import { Bar } from "react-chartjs-2";
// ChartJS.register(CategoryScale, LinearScale, BarElement, Tooltip, Legend);
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  ArcElement,
  Tooltip,
  Legend,
} from "chart.js";
import { Doughnut } from "react-chartjs-2";
ChartJS.register(CategoryScale, LinearScale, ArcElement, Tooltip, Legend);

export default function HorizontalEmployeeChart({ sources }) {
  const [chartData, setChartData] = React.useState({
    labels: [],
    datasets: [],
  });

  async function onViewAppearing() {
    var nhanvien = _.uniq([...sources.map((e) => e.EmployeeCode)]);
    var tongtien = [];
    nhanvien.forEach((e) => {
      var HD = _.filter(sources, { EmployeeCode: e });
      var amtHD = _.sumBy(HD, "TongTien");
      var amtHD_2 = amtHD / 1000000;
      tongtien = [...tongtien, amtHD_2];
    });
    setChartData({
      labels: _.uniq([
        ...sources.map((e) => {
          if (e.EmployeeName == null || e.EmployeeName == undefined) {
            return "Không có nhân viên";
          } else {
            return e.EmployeeName;
          }
        }),
      ]),
      datasets: [
        {
          label: "Doanh thu",
          data: [...tongtien],
          backgroundColor: [
            "#cf4804",
            "#007500",
            "#e5e700",
            "#0000fe",
            "#fe0000",
            "#4fd1c5",
            "#f56565",
            "#4299e1",
            "#e7b601",
            "#38a169",
            "#a0aec0",
          ],
          hoverBackgroundColor: [
            "#451801",
            "#002700",
            "#4C4D00",
            "#000054",
            "#540000",
            "#1A4541",
            "#512121",
            "#16334B",
            "#4D3C00",
            "#123523",
            "#353A40",
          ],
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
    <>
      <div>
        <Doughnut
          height={220}
          data={chartData}
          options={{
            maintainAspectRatio: false,
            plugins: {
              legend: {
                position: "right",
                labels: {
                  usePointStyle: true, //for style circle
                },
              },
            },
          }}
        />
      </div>
    </>
  );
}
