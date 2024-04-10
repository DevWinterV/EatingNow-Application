import * as React from "react";
import { Breadcrumb } from "../../controls";
import moment from "moment";
import _ from "lodash";
import { useLocation, useNavigate , useParams} from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import { Paginate } from "../../controls";
import Swal from "sweetalert2";
import { GetListOrderLineDetails } from "../../api/store/storeService";
import { RemoveOrderLine } from "../../api/customer/customerService";
export default function DetailOrderPage() {
  const navigate = useNavigate();
  const { state } = useLocation();
  const { id } = useParams();
  const breadcrumbSources = [
    {
      name: "Chi tiết đơn hàng",
      href: "/order",
    },
    {
      name: "Danh sách",
      active: true,
    },
  ];
  const [Loading, setLoading] = React.useState(false);
  const [pageTotal, setPageTotal] = React.useState(1);
  const [data, setData] = React.useState([]);

  async function onViewAppearing() {
    setLoading(true);
    if (state?.data) {
      if(id != null){
        let responseOrder = await GetListOrderLineDetails(id)
        console.log(responseOrder);
          setData(responseOrder.data);
          //setPageTotal(Math.ceil(responseOrder.dataCount / filter.pageSize));
      }
    }
    setLoading(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, []);
  function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = String(date.getFullYear());
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    return `${day}/${month}/${year} ${hours}:${minutes}`;
  }
  return (
    <>
      <Breadcrumb
        title={"Chi tiết đơn hàng"}
        sources={breadcrumbSources}
      />
      {/* <div className="row">
        <div className="col-sm-4">
          <span style={{ fontSize: "10px" }} className="fw-bold">
            Thông tin bảng báo giá
          </span>
        </div>
      </div> 
      
         <div className="row">
        <div className="col-sm-4">
          <p className="fw-bold" style={{ fontSize: "12px" }}>
            Thông tin sản phẩm khách hàng đã mua
          </p>
        </div>
      </div>
      */}
      <div className="row">
          <h6>
            Mã đơn hàng: {state?.data.OrderHeaderId ?? ""}
          </h6>
          <h6>
            Đặt lúc: {formatDate(state?.data.CreationDate ?? "")}
          </h6>
          <div className="row">
            <div className="col-md-3">
              <h6>
                Cửa hàng kinh doanh: {state?.data.StoreName ?? ""} 
              </h6>
            </div>
          </div>
          <div className="row">
            <div className="col-md-3">
              <h6>
                Khách hàng: {state?.data.RecipientName ?? ""} 
              </h6>
            </div>
            <div className="col-md-3">
              <h6>
                Số điện thoại: {state?.data.RecipientPhone ?? ""} 
              </h6>
            </div>
            <div className="col-md-6">
              <h6>
                Địa chỉ nhận hàng: {state?.data.FormatAddress ?? ""}
              </h6>
            </div>
          </div>
          <div className="row">
            <div className="col-md-3">
              <h6>
                Trạng thái: {state?.data.Status == true ? "Đã xác nhận" : "Chưa xác nhận"}
              </h6>
            </div>
            <div className="col-md-3">
              <h6>
                Trạng thái giao hàng: {state?.data.ShippingStatus == 1 ? "Đang chuẩn bị"  : state?.data.ShippingStatus == 2 ? "Đang giao hàng" : "Đã giao thành công"}
              </h6>
            </div>
          </div>

      </div>
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <h6>
            Danh sách sản phẩm
          </h6>
          <Table className="table table-striped">
            <Thead className="table-light">
              <Tr>
                <Th className="align-middle">STT</Th>
                <Th className="align-middle">Tên món ăn</Th>
                <Th className="align-middle">Số lượng</Th>
                <Th className="align-middle">Giá</Th>
                <Th className="align-middle">Mô tả</Th>
                <Th className="align-middle">Xóa</Th>
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
                        <Td>{idxx + 1}</Td>
                        <Td>{i.FoodName}</Td>
                        <Td>{i.qty}</Td>
                        <Td>{i.Price}</Td>
                        {i.Description != null ? 
                        (
                          <Td>{i.Description }</Td>
                        ):
                        (
                          <Td>Không có</Td>
                        )}
                        <Td>
                          <div className="d-flex gap-3">
                            <div
                              className="text-danger"
                              onClick={() => {
                                Swal.fire({
                                  title: "Xóa sản phẩm ?",
                                  text: "Bạn muốn xóa sản phẩm này ra khỏi chi tiết đơn hàng không ?",
                                  icon: "warning",
                                  showCancelButton: true,
                                  confirmButtonColor: "#3085d6",
                                  cancelButtonColor: "#d33",
                                  confirmButtonText: "Xác nhận !",
                                }).then((result) => {
                                  if (result.isConfirmed) {
                                    RemoveOrderLine(i).then((response) => {
                                      if (response.success) {
                                        Swal.fire(
                                          "Hoàn thành!",
                                          "Xóa dữ liệu thành công.",
                                          "success"
                                        );
                                        onViewAppearing();
                                      } else {
                                        Swal.fire(
                                          "Không thể xóa sản phẩm!",
                                          "Lỗi trong khi xóa sản phẩm, vui lòng thử lại!",
                                          "warning"
                                        );
                                      }
                                    });
                                  }
                                });
                              }}
                            >
                              <i className="mdi mdi-delete font-size-18"></i>
                            </div>
                          </div>
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
          {
              /*
                 <Paginate
            onPageChange={onPageChange}
            pageRangeDisplayed={5}
            pageTotal={pageTotal || 1}
          />
              */
          }
       
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
