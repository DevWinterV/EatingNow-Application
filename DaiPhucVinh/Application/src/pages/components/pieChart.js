import React from "react";
import _ from "lodash";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  ArcElement,
  Tooltip,
  Legend,
} from "chart.js";
import { Pie } from "react-chartjs-2";
ChartJS.register(CategoryScale, LinearScale, ArcElement, Tooltip, Legend);

export default function PieChart({ sources }) {
  const [chartData, setChartData] = React.useState({
    labels: [],
    datasets: [],
  });

  async function onViewAppearing() {
    var chinhanh = _.uniq([...sources.map((e) => e.LocationCode)]);
    var tongtien = [];
    chinhanh.forEach((e) => {
      var HD = _.filter(sources, { LocationCode: e });
      var amtHD = _.sumBy(HD, "TongTien");
      var amtHD_2 = amtHD / 1000000;
      tongtien = [...tongtien, amtHD_2];
    });
    setChartData({
      labels: _.uniq([...sources.map((e) => e.LocationName)]),
      datasets: [
        {
          label: "Doanh thu",
          data: [...tongtien],
          backgroundColor: [
            "#B22222",
            "#3CB371",
            "#FF6347",
            "#4682B4",
            "#FFA500",
            "#C0C0C0",
            "#CD853F",
            "#308B88",
          ],
          hoverBackgroundColor: [
            "#552117",
            "#104410",
            "#552117",
            "#172B3C",
            "#553700",
            "#404040",
            "#442C15",
            "#308B88",
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
        <Pie
          height={300}
          data={chartData}
          options={{
            maintainAspectRatio: false,
            legend: {
              display: true,
              position: "right",
            },
          }}
        />
      </div>
    </>
  );
}
