import * as React from "react";
import { Breadcrumb } from "../../controls";
import moment from "moment";
import _ from "lodash";
import { useLocation, useNavigate } from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import { Paginate } from "../../controls";
import { TakeContractByCustomerCode } from "../../api/customer/customerService";
export default function DetailCustomerPage() {
  const navigate = useNavigate();
  const { state } = useLocation();
  const breadcrumbSources = [
    {
      name: "Danh sách khách hàng",
      href: "/customer",
    },
    {
      name: "Danh sách",
      active: true,
    },
  ];
  const [Loading, setLoading] = React.useState(false);
  const [pageTotal, setPageTotal] = React.useState(1);
  const [data, setData] = React.useState([]);
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 10,
  });
  function onPageChange(e) {
    setFilter({
      ...filter,
      page: e.selected,
    });
  }
  async function onViewAppearing() {
    setLoading(true);
    if (state?.data) {
      let response = await TakeContractByCustomerCode(filter, state?.data.Code);
      if (response.success) {
        setData(response.data);
        setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
      }
    }
    setLoading(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.page, filter.pageSize]);
  return (
    <>
      <Breadcrumb
        title={"Danh sách sản phẩm của khách hàng"}
        sources={breadcrumbSources}
      />
      {/* <div className="row">
        <div className="col-sm-4">
          <span style={{ fontSize: "10px" }} className="fw-bold">
            Thông tin bảng báo giá
          </span>
        </div>
      </div> */}
      <div className="row">
        <div className="col-sm-4">
          <p className="fw-bold" style={{ fontSize: "12px" }}>
            Thông tin sản phẩm của khách hàng
          </p>
        </div>
      </div>
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <Table className="table table-striped">
            <Thead className="table-light">
              <Tr>
                <Th className="align-middle">STT</Th>
                <Th className="align-middle">Số hợp đồng</Th>
                <Th className="align-middle">Ngày tạo hợp đồng</Th>
                <Th className="align-middle">Tên máy</Th>
                <Th className="align-middle">Model</Th>
                <Th className="align-middle">Số lượng</Th>
                <Th className="align-middle">Ngày đã giao hàng</Th>
              </Tr>
            </Thead>
            <Tbody style={{ position: "relative" }}>
              {Loading ? (
                <>
                  <Tr>
                    <Td colSpan={8} style={{ height: "70px" }}>
                      <div
                        className="spinner-border text-info"
                        role="status"
                        style={{
                          position: "absolute",
                          top: "50%",
                          left: "50%",
                        }}
                      >
                        <span className="sr-only"></span>
                      </div>
                    </Td>
                  </Tr>
                </>
              ) : (
                <>
                  {data.length > 0 ? (
                    data.map((i, idxx) => (
                      <Tr key={"row_" + idxx}>
                        <Td>{filter.page * filter.pageSize + idxx + 1}</Td>
                        <Td>{i.SoHopDong}</Td>
                        <Td>
                          {i.NgayTaoHopDong === null
                            ? ""
                            : moment(i.NgayTaoHopDong).format("DD/MM/YYYY")}
                        </Td>
                        <Td>{i.Name}</Td>
                        <Td>{i.Model}</Td>
                        <Td>{i.qty}</Td>
                        <Td>
                          {i.NgayDaGiaoHang === null
                            ? ""
                            : moment(i.NgayDaGiaoHang).format("DD/MM/YYYY")}
                        </Td>
                      </Tr>
                    ))
                  ) : (
                    <Tr>
                      <Td colSpan={7}>Không có dữ liệu</Td>
                    </Tr>
                  )}
                </>
              )}
            </Tbody>
          </Table>
          <Paginate
            onPageChange={onPageChange}
            pageRangeDisplayed={5}
            pageTotal={pageTotal || 1}
          />
          <div className="col d-flex justify-content-end align-items-right">
            <div className="text-sm-end">
              <div className="col me-2 d-flex">
                <button
                  className="btn btn-warning me-2"
                  onClick={() => navigate(-1)}
                  style={{ fontSize: "12px" }}
                >
                  Trở lại
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
