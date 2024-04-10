import * as React from "react";
import _ from "lodash";
import moment from "moment";
import { useNavigate } from "react-router-dom";
import Select from "react-select";
import BarChart from "../components/barChart";
//import Horizontal from "../components/horizontalChart";
// import PieChart from "../components/pieChart";
//import LineChart from "../components/lineChart";
import DoughnutLocationChart from "../components/doughnutLocationChart";
import BarProvinceChart from "../components/barProvinceChart";
import HorizontalEmployeeChart from "../components/horizontalEmployeeChart";
import {
  TakeProductStatistics,
  TotalRevenueStatistics
} from "../../api/dashboard/dashboardService";
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
import {
  TakeAllProvince
} from "../../api/province/provinceService";

import { decrypt } from "../../framework/encrypt";

export default function HomePage() {
  const history = useNavigate();
  const [dataChart, setDataChart] = React.useState([]);
  const [selectedProvince, setselectedProvince] = React.useState({
    value: 0,
    label: "Tất cả",
  });
  const [DataTotalRevenueStatistics, setDataTotalRevenueStatistics] = React.useState([]);
  const backgroundColors = ["#FF5733", "#33FFC0", "#3366FF", "#FF33E9", "#33FF57", "#3391FF"];

  const select = [
    {
      value: 1,
      label: "Tháng này",
    },
    {
      value: 2,
      label: "Tháng trước",
    },
    {
      value: 3,
      label: "Quý này",
    },
    {
      value: 4,
      label: "Quý trước",
    },
    {
      value: 5,
      label: "Năm này",
    },
    {
      value: 6,
      label: "Năm trước",
    },
  ];
  const [thongKeDoanhThu, setthongKeDoanhThu] = React.useState({
    labels: [],
    datasets: [
      {
        data: [],
      },
    ],
  });

  const [thongkeSoLuongSanPham, setthongkeSoLuongSanPham] = React.useState({
    labels: [],
    datasets: [
      {
        data: [],
      },
    ],
  });


  async function onFillItemProvince() {
    let itemProvinceResponse = await TakeAllProvince();
    if (itemProvinceResponse.success) {
      setprovinceData([
        {
          value: 0,
          label: "Tất cả",
        },
        ...itemProvinceResponse.data.map((e) => {
          return {
            value: e.ProvinceId,
            label: e.Name,
          };
        }),
      ]);
    }
  }

  const [provinceData, setprovinceData] = React.useState([]);

  const [loading, setLoading] = React.useState(false);
  const [defaultSelect, setDefaultSelect] = React.useState(select[0]);
  const [isMonth, setIsMonth] = React.useState(false);

  const [request, setRequest] = React.useState({
    IdZone: 1,
    FromDt: moment().date(1),
    ToDt: moment().date(moment().daysInMonth()),
  });

  const [data, setData] = React.useState({
    SL_HopDong: 0,
    SL_BaoGia: 0,
    SL_SanPham: 0,
    TongTien_BaoGia: 0,
    TongTien_DoanhThu: 0,
  });

  async function onChangeDateFilter(e) {
    //tháng này
    if (e.value == 1) {
      setRequest({
        ...request,
        FromDt: moment().date(1),
        ToDt: moment().date(moment().daysInMonth()),
      });
    }
    //tháng trước
    if (e.value == 2) {
      setRequest({
        ...request,
        FromDt: moment().add(-1, "M").date(1),
        ToDt: moment().add(-1, "M").date(moment().daysInMonth()),
      });
    }
    //quý này
    if (e.value == 3) {
      //var currentMonth = moment().add(1, "M")._d.getMonth();
      var currentMonth = new Date().getMonth();
      var quarter = Math.ceil((currentMonth + 1) / 3);
      //quý 1
      if (quarter == 1) {
        setRequest({
          ...request,
          FromDt: `01/01/${moment().year()}`,
          ToDt: `03/31/${moment().year()}`,
        });
      }
      if (quarter == 2) {
        setRequest({
          ...request,
          FromDt: `04/01/${moment().year()}`,
          ToDt: `06/30/${moment().year()}`,
        });
      }
      if (quarter == 3) {
        setRequest({
          ...request,
          FromDt: `07/01/${moment().year()}`,
          ToDt: `09/30/${moment().year()}`,
        });
      }
      if (quarter == 4) {
        setRequest({
          ...request,
          FromDt: `10/01/${moment().year()}`,
          ToDt: `12/31/${moment().year()}`,
        });
      }
    }
    //quý trước
    if (e.value == 4) {
      var curMonth = new Date().getMonth();
      // hiện tại là quý 1
      if (curMonth == 0 || curMonth == 1 || curMonth == 2) {
        //quý 4
        let lastYear = new Date().getFullYear() - 1;
        setRequest({
          ...request,
          FromDt: `10/01/${lastYear}`,
          ToDt: `12/31/${lastYear}`,
        });
      } else {
        let lastMonth = curMonth - 1;
        let lastQuarter = Math.ceil((lastMonth + 1) / 3) - 1;
        //quý 1
        if (lastQuarter == 1) {
          setRequest({
            ...request,
            FromDt: `01/01/${moment().year()}`,
            ToDt: `03/31/${moment().year()}`,
          });
        }
        if (lastQuarter == 2) {
          setRequest({
            ...request,
            FromDt: `04/01/${moment().year()}`,
            ToDt: `06/30/${moment().year()}`,
          });
        }
        if (lastQuarter == 3) {
          setRequest({
            ...request,
            FromDt: `07/01/${moment().year()}`,
            ToDt: `09/30/${moment().year()}`,
          });
        }
        if (lastQuarter == 4) {
          setRequest({
            ...request,
            FromDt: `10/01/${moment().year()}`,
            ToDt: `12/31/${moment().year()}`,
          });
        }
      }
    }
    //năm này
    if (e.value == 5) {
      setRequest({
        ...request,
        FromDt: `01/01/${moment().year()}`,
        ToDt: `12/31/${moment().year()}`,
      });
    }
    //năm trước
    if (e.value == 6) {
      setRequest({
        ...request,
        FromDt: `01/01/${moment().year() - 1}`,
        ToDt: `12/31/${moment().year() - 1}`,
      });
    }
    setIsMonth(e.value > 2);
  }

  function isUserComback() {
    let authCache = localStorage.getItem("@auth");
    if (authCache) {
      const data = decrypt(authCache);
      if (data && moment(data.ExpiredAt) < moment()) {
        localStorage.removeItem("@auth");
        localStorage.removeItem("@permissions");
        history("/login");
      }
    } else {
      history("/login");
    }
  }


  async function onViewAppearingTakeProductStatistics() {
    setLoading(true);
    let responseTakeProductStatistics = await TakeProductStatistics(request);
    if (responseTakeProductStatistics.success) {
      console.table(responseTakeProductStatistics.item);
        const chartLabelsFoodName =  responseTakeProductStatistics.item.map(
            (item) => (`${item.FoodName}`)
          );
        const dataChartSoluongDaBan = responseTakeProductStatistics.item.map(
            (item) => item.CountBuy
          );
        setthongkeSoLuongSanPham({
        labels: chartLabelsFoodName,
        datasets: [
            {
              label: `Sản phẩm`,
              data: dataChartSoluongDaBan,
              backgroundColor: backgroundColors,
            },
        ],
      });
    }
    setLoading(false);
  }
  async function onViewAppearing() {
    setLoading(true);
    let responseTotalRevenueStatistics = await TotalRevenueStatistics(request);
    if (responseTotalRevenueStatistics.success) {
      setDataTotalRevenueStatistics(responseTotalRevenueStatistics.item);
      const chartLabelsNameStore =  responseTotalRevenueStatistics.item.ListItemStatisticsResponse.map(
          (item) => (`${item.FullName} - ${item.Percentagerevenue}%`)
        );
      const dataChartDoanhthu = responseTotalRevenueStatistics.item.ListItemStatisticsResponse.map(
          (item) => item.Total
        );
      setthongKeDoanhThu({
      labels: chartLabelsNameStore,
      datasets: [
          {
            label: `Doanh thu`,
            data: dataChartDoanhthu,
            backgroundColor: backgroundColors,
          },
      ],
    });
    }
    setLoading(false);
  }

  React.useEffect(() =>{
    onFillItemProvince();
  },[])

  React.useEffect(() => {
    isUserComback();
    onViewAppearing();
    onViewAppearingTakeProductStatistics();
  }, [request.FromDt, request.ToDt, request.IdZone]);

  return (
    <>
      <div
        className="container-fluid"
        style={{ margin: "-15px", width: "102%" }}
      >
        <div className="row" style={{ paddingTop: "10px" }}>
          <div className="col-lg-4">
            <div
              className="page-title-box d-sm-flex align-items-center justify-content-between"
              style={{ marginTop: "8px" }}
            >
              <h4 className="mb-sm-0 font-size-18">Tổng quan hệ thống</h4>
            </div>
          </div>

          <div className="col-lg-8">
            <div className="row" style={{ marginRight: "-25px" }}>
              <div className="col-lg-3">
                <div
                  className="mb-3"
                  style={{
                    zIndex: "999",
                    position: "inherit",
                  }}
                >
                  <Select
                    options={provinceData}
                    value={selectedProvince}
                    onChange={(e) => {
                      setselectedProvince({
                        value: e.value,
                        label: e.label,
                      });
                      setRequest({
                        ...request,
                        IdZone: e.value
                      })
                    }}
                    isDisabled={loading}
                  />
                </div>
              </div>

              <div className="col-lg-3">
                <div
                  className="mb-3"
                  style={{
                    zIndex: "999",
                    position: "inherit",
                  }}
                >
                  <Select
                    options={select}
                    value={defaultSelect}
                    onChange={(e) => {
                      setDefaultSelect(e);
                      onChangeDateFilter(e);
                    }}
                    isDisabled={loading}
                  />
                </div>
              </div>

              <div className="col-lg-3">
                <div className="mb-3">
                  <input
                    className="form-control"
                    style={{ height: "37px" }}
                    type="date"
                    value={moment(request.FromDt).format("yyyy-MM-DD")}
                    onChange={(e) => {
                      setRequest({
                        ...request,
                        FromDt: moment(e.target.value),
                      });
                    }}
                    disabled={loading}
                  />
                </div>
              </div>

              <div className="col-lg-3">
                <div className="mb-3">
                  <input
                    className="form-control"
                    style={{ height: "37px" }}
                    type="date"
                    value={moment(request.ToDt).format("yyyy-MM-DD")}
                    onChange={(e) => {
                      setRequest({
                        ...request,
                        ToDt: moment(e.target.value),
                      });
                    }}
                    disabled={loading}
                  />
                </div>
              </div>
            </div>
          </div>


        </div>
        <div className="row" style={{ marginTop: "-10px" }}>
          <div className="col-lg-2">
            <div
              className="card-body bg-white alert alert-info  mb-3 flex-grow-1"
              style={{
                borderRadius: "10px",
                borderColor: "#2b78e4",
                textAlign: "center",
                height: "80px",
              }}
            >
              {loading ? (
                <div
                  className="spinner-border text-info m-1 mx-auto  text-center"
                  role="status"
                  style={{
                    alignSelf: "center",
                    display: "block",
                  }}
                >
                  <span className="sr-only">Đang tải ...</span>
                </div>
              ) : (
                <h6 className="mb-2 fw-bold" style={{ color: "red", fontSize: "20px"}}>
                  {Intl.NumberFormat("vi-VN", {
                    style: "currency",
                    currency: "VND",
                  }).format(DataTotalRevenueStatistics.Totalsystemrevenue)}
                </h6>
              )}

              <p className="text-muted fw-medium fw-bold mb-0">Tổng doanh thu</p>
            </div>
            <div
              className="card-body bg-white alert alert-info mb-3 flex-grow-1"
              style={{
                borderRadius: "10px",
                borderColor: "#2b78e4",
                textAlign: "center",
                height: "80px",
              }}
            >
              {loading ? (
                <div
                  className="spinner-border text-info m-1 mx-auto  text-center"
                  role="status"
                  style={{
                    alignSelf: "center",
                    display: "block",
                  }}
                >
                  <span className="sr-only">Đang tải ...</span>
                </div>
              ) : (
                <h4 className="mb-2 fw-bold" style={{ color: "green" }}>
                  {Intl.NumberFormat("vi-VN").format(DataTotalRevenueStatistics.TotalCountCustomersystem)}
                </h4>
              )}

              <p className="text-muted fw-medium fw-bold mb-0">
                Số lượng khách hàng hệ thống
              </p>
            </div>
            <div
              className="card-body bg-white alert alert-info mb-3 flex-grow-1"
              style={{
                borderRadius: "10px",
                borderColor: "#2b78e4",
                textAlign: "center",
                height: "80px",
              }}
            >
              {loading ? (
                <div
                  className="spinner-border text-info m-1 mx-auto  text-center"
                  role="status"
                  style={{
                    alignSelf: "center",
                    display: "block",
                  }}
                >
                  <span className="sr-only">Đang tải ...</span>
                </div>
              ) : (
                <h4 className="mb-2 fw-bold" style={{ color: "green" }}>
                  {Intl.NumberFormat("vi-VN").format(DataTotalRevenueStatistics.TotalCountStoresystem)}
                </h4>
              )}
              <p className="text-muted fw-medium fw-bold mb-0">Số lượng cửa hàng khu vực {selectedProvince.value > 0 ? selectedProvince.label : ""}</p>
            </div>
            {/* 
            <div
              className="card-body bg-white alert alert-info mb-3 flex-grow-1"
              style={{
                borderRadius: "10px",
                borderColor: "#2b78e4",
                textAlign: "center",
                height: "80px",
              }}
            >
              {loading ? (
                <div
                  className="spinner-border text-info m-1 mx-auto  text-center"
                  role="status"
                  style={{
                    alignSelf: "center",
                    display: "block",
                  }}
                >
                  <span className="sr-only">Loading...</span>
                </div>
              ) : (
                <h4 className="mb-2 fw-bold" style={{ color: "green" }}>
                  {Intl.NumberFormat("vi-VN").format(data.SL_BaoGia)}
                </h4>
              )}

              <p className="text-muted fw-medium fw-bold mb-0">SL báo giá</p>
            </div> */}
          </div>
          <div className="col-lg-10">
              <div className="row">
                <p className="text-muted fw-medium fw-bold mb-0">Thống kê doanh thu theo khu vực {selectedProvince.value > 0 ? selectedProvince.label : ""}</p>
              </div>
              <div className="row">
                <div className="w-full">
                  <React.Fragment>
                    <ResponsiveContainer width="100%" aspect={4} >
                      <Bar
                        height="auto"
                        data={thongKeDoanhThu}
                        options={{
                          indexAxis: "x",
                          plugins: {
                            title: {
                              display: true,
                              text: `Danh thu khu vực ${selectedProvince.label}`,
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
              </div>
          </div>

          <div className="col-lg-12">
              <div className="row">
                <p className="text-muted fw-medium fw-bold mb-0">Thống kê số lượng sản phẩm bán theo khu vực  {selectedProvince.value > 0 ? selectedProvince.label : ""}</p>
              </div>

              <div className="row">
                <div className="w-full">
                  <React.Fragment>
                    <ResponsiveContainer width="100%" aspect={4} >
                      <Bar
                        height="auto"
                        data={thongkeSoLuongSanPham}
                        options={{
                          indexAxis: "x",
                          plugins: {
                            title: {
                              display: true,
                              text: `Thống kê sản phẩm khu vực ${selectedProvince.label}`,
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
              </div>
          </div>
        </div>
      </div>
    </>
  );
}
