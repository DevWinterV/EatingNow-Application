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
  TotalRevenue_Chart,
  TotalPriceQuote,
} from "../../api/dashboard/dashboardService";
import { decrypt } from "../../framework/encrypt";

export default function HomePage() {
  const history = useNavigate();
  const [dataChart, setDataChart] = React.useState([]);
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
  const [loading, setLoading] = React.useState(false);
  const [defaultSelect, setDefaultSelect] = React.useState(select[0]);
  const [isMonth, setIsMonth] = React.useState(false);
  const [request, setRequest] = React.useState({
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
  async function onViewAppearing() {
    setLoading(true);
    let response_Revenue = await TotalRevenue_Chart(request);
    if (response_Revenue.success) {
      var amtHD = _.sumBy(response_Revenue.data, "TongTien");
      var Item = [];
      response_Revenue.data.forEach((e) => {
        Item = [...Item, ...e.Item];
      });
      var SL_SP = _.sumBy(Item, "SoLuong");
      let response_PriceQuote = await TotalPriceQuote(request);
      var amtBG = _.sumBy(response_PriceQuote.data, "Amt");
      setData({
        ...data,
        SL_HopDong: response_Revenue.data.length,
        TongTien_DoanhThu: amtHD,
        SL_BaoGia: response_PriceQuote.data.length,
        TongTien_BaoGia: amtBG,
        SL_SanPham: SL_SP,
      });
      setDataChart(response_Revenue.data);
    }
    setLoading(false);
  }
  React.useEffect(() => {
    isUserComback();
    onViewAppearing();
    return () => {
      setData({
        SL_HopDong: 0,
        SL_BaoGia: 0,
        SL_SanPham: 0,
        TongTien_BaoGia: 0,
        TongTien_DoanhThu: 0,
      });
    };
  }, [request.FromDt, request.ToDt]);

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
              <h4 className="mb-sm-0 font-size-18">Tổng quan</h4>
            </div>
          </div>
          <div className="col-lg-8">
            <div className="row" style={{ marginRight: "-25px" }}>
              <div className="col-lg-4">
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
              <div className="col-lg-4">
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
              <div className="col-lg-4">
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
                  <span className="sr-only">Loading...</span>
                </div>
              ) : (
                <h6 className="mb-2 fw-bold" style={{ color: "red" }}>
                  {Intl.NumberFormat("vi-VN", {
                    style: "currency",
                    currency: "VND",
                  }).format(data.TongTien_DoanhThu)}
                </h6>
              )}

              <p className="text-muted fw-medium fw-bold mb-0">Doanh thu</p>
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
                  <span className="sr-only">Loading...</span>
                </div>
              ) : (
                <h6 className="mb-2 fw-bold" style={{ color: "red" }}>
                  {Intl.NumberFormat("vi-VN", {
                    style: "currency",
                    currency: "VND",
                  }).format(data.TongTien_BaoGia)}
                </h6>
              )}

              <p className="text-muted fw-medium fw-bold mb-0">Tiền báo giá</p>
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
                  <span className="sr-only">Loading...</span>
                </div>
              ) : (
                <h4 className="mb-2 fw-bold" style={{ color: "green" }}>
                  {Intl.NumberFormat("vi-VN").format(data.SL_SanPham)}
                </h4>
              )}

              <p className="text-muted fw-medium fw-bold mb-0">
                Sản phẩm đã bán
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
                  <span className="sr-only">Loading...</span>
                </div>
              ) : (
                <h4 className="mb-2 fw-bold" style={{ color: "green" }}>
                  {Intl.NumberFormat("vi-VN").format(data.SL_HopDong)}
                </h4>
              )}
              <p className="text-muted fw-medium fw-bold mb-0">SL hợp đồng</p>
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
                  <span className="sr-only">Loading...</span>
                </div>
              ) : (
                <h4 className="mb-2 fw-bold" style={{ color: "green" }}>
                  {Intl.NumberFormat("vi-VN").format(data.SL_BaoGia)}
                </h4>
              )}

              <p className="text-muted fw-medium fw-bold mb-0">SL báo giá</p>
            </div>
          </div>
          <div className="col-lg-10">
            <div className="row">
              <div className="col-lg-6">
                <div className="card" style={{ width: "103%" }}>
                  <div
                    className="d-sm-flex flex-wrap alert alert-info"
                    style={{
                      height: "30px",
                      marginBottom: "0px",
                      backgroundColor: "#2b78e4",
                      borderRadius: "10px 10px 0px 0px",
                      borderColor: "#2b78e4",
                    }}
                  >
                    <h6 style={{ color: "white", marginTop: "-5px" }}>
                      Tổng doanh thu (Triệu đồng)
                    </h6>
                  </div>
                  <div
                    className="card-body bg-white alert alert-info mb-0"
                    style={{
                      zIndex: "1",
                      borderColor: "#2b78e4",
                      borderRadius: "0px 0px 10px 10px",
                    }}
                  >
                    {loading ? (
                      <div
                        className="spinner-border text-info"
                        role="status"
                        style={{
                          position: "relative",
                          top: "50%",
                          left: "50%",
                        }}
                      >
                        <span className="sr-only"></span>
                      </div>
                    ) : dataChart && dataChart.length > 0 ? (
                      <BarChart sources={[...dataChart, isMonth]} />
                    ) : (
                      <h6 style={{ marginTop: "-5px", textAlign: "center" }}>
                        Không có doanh thu
                      </h6>
                    )}
                  </div>
                </div>
              </div>
              <div className="col-lg-6">
                <div className="card" style={{ width: "103%" }}>
                  <div
                    className="d-sm-flex flex-wrap alert alert-info"
                    style={{
                      height: "30px",
                      marginBottom: "0px",
                      backgroundColor: "#2b78e4",
                      borderRadius: "10px 10px 0px 0px",
                      borderColor: "#2b78e4",
                    }}
                  >
                    <h6 style={{ color: "white", marginTop: "-5px" }}>
                      Doanh thu tỉnh thành (Triệu đồng)
                    </h6>
                  </div>
                  <div
                    className="card-body bg-white alert alert-info mb-0"
                    style={{
                      zIndex: "1",
                      borderColor: "#2b78e4",
                      borderRadius: "0px 0px 10px 10px",
                    }}
                  >
                    {loading ? (
                      <div
                        className="spinner-border text-info"
                        role="status"
                        style={{
                          position: "relative",
                          top: "50%",
                          left: "50%",
                        }}
                      >
                        <span className="sr-only"></span>
                      </div>
                    ) : dataChart && dataChart.length > 0 ? (
                      <BarProvinceChart sources={dataChart} />
                    ) : (
                      <h6 style={{ marginTop: "-5px", textAlign: "center" }}>
                        Không có doanh thu
                      </h6>
                    )}
                  </div>
                </div>
              </div>
            </div>
            <div className="row" style={{ marginTop: "-15px" }}>
              <div className="col-lg-6">
                <div className="card" style={{ width: "103%" }}>
                  <div
                    className="d-sm-flex flex-wrap alert alert-info"
                    style={{
                      height: "30px",
                      marginBottom: "0px",
                      backgroundColor: "#2b78e4",
                      borderRadius: "10px 10px 0px 0px",
                      borderColor: "#2b78e4",
                    }}
                  >
                    <h6 style={{ color: "white", marginTop: "-5px" }}>
                      Doanh thu chi nhánh (Triệu đồng)
                    </h6>
                  </div>
                  <div
                    className="card-body bg-white alert alert-info mb-0"
                    style={{
                      zIndex: "1",
                      borderRadius: "0px 0px 10px 10px",
                      borderColor: "#2b78e4",
                    }}
                  >
                    {loading ? (
                      <div
                        className="spinner-border text-info"
                        role="status"
                        style={{
                          position: "relative",
                          top: "50%",
                          left: "50%",
                        }}
                      >
                        <span className="sr-only"></span>
                      </div>
                    ) : dataChart && dataChart.length > 0 ? (
                      <DoughnutLocationChart sources={dataChart} />
                    ) : (
                      <h6 style={{ marginTop: "-5px", textAlign: "center" }}>
                        Không có doanh thu
                      </h6>
                    )}
                  </div>
                </div>
              </div>
              <div className="col-lg-6">
                <div className="card" style={{ width: "103%" }}>
                  <div
                    className="d-sm-flex flex-wrap alert alert-info"
                    style={{
                      height: "30px",
                      marginBottom: "0px",
                      backgroundColor: "#2b78e4",
                      borderRadius: "10px 10px 0px 0px",
                      borderColor: "#2b78e4",
                    }}
                  >
                    <h6 style={{ color: "white", marginTop: "-5px" }}>
                      Doanh thu nhân viên (Triệu đồng)
                    </h6>
                  </div>
                  <div
                    className="card-body bg-white alert alert-info mb-0"
                    style={{
                      zIndex: "1",
                      borderColor: "#2b78e4",
                      borderRadius: "0px 0px 10px 10px",
                    }}
                  >
                    {loading ? (
                      <div
                        className="spinner-border text-info"
                        role="status"
                        style={{
                          position: "relative",
                          top: "50%",
                          left: "50%",
                        }}
                      >
                        <span className="sr-only"></span>
                      </div>
                    ) : dataChart && dataChart.length > 0 ? (
                      <HorizontalEmployeeChart sources={dataChart} />
                    ) : (
                      <h6 style={{ marginTop: "-5px", textAlign: "center" }}>
                        Không có doanh thu
                      </h6>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
