
import * as React from "react";
import { Breadcrumb } from "../../controls";
import _ from "lodash";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import { Paginate } from "../../controls";
import { TakeAllDeliveryDriver , ApproveDelvery, RemoveDriver} from "../../api/store/storeService";
import { Link } from "react-router-dom";
import Swal from "sweetalert2";

export default function DeliveryPage() {
  const breadcrumbSources = [
    {
      name: "Tài xế",
      href: "#",
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
    pageSize: 20,
  });
  async function changeSearch(e) {
    setFilter({ ...filter, term: e.target.value });
    //    localStorage.term = e.target.value;
  }
  function onPageChange(e) {
    setFilter({
      ...filter,
      page: e.selected,
    });
  }
  async function onViewAppearing() {
    setLoading(true);
      let response = await TakeAllDeliveryDriver(filter);
      if (response.success) {
        setData(response.data);
        setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
      }
    setLoading(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.page, filter.pageSize, filter.term]);
  return (
    <>
      <Breadcrumb
        title={"Danh sách tài xế"}
        sources={breadcrumbSources}
      />
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
        <div className="row">
            <div className="col-lg-4">
              <div className="mb-3">
                <label className="d-flex fw-bold">Tìm kiếm</label>
                <div className="input-group">
                  <input
                    type="search"
                    className="form-control"
                    placeholder="Tên shiper..."
                    value={filter.term}
                    onChange={changeSearch}
                    onKeyDown={(e) => {
                      if (e.code == "Enter") {
                        onViewAppearing();
                        setFilter({ ...filter, page: 0 });
                        changeSearch(e);
                      }
                    }}
                    style={{ fontSize: "12px" }}
                  />
                  <button
                    className="btn btn-success"
                    type="button"
                    onClick={() => {
                      onViewAppearing();
                      setFilter({
                        ...filter,
                        page: 0,
                      });
                    }}
                  >
                    <i className="bx bx-search-alt-2"></i>
                  </button>
                </div>
              </div>
            </div>
            <div
              className="col d-flex justify-content-end align-items-right"
              style={{ marginTop: "27px" }}
            >
              <div className="mb-3">
                <Link to="/delivery/create">
                  <button
                    type="button"
                    className="btn btn-info"
                    onClick={() => {   }}
                    style={{ fontSize: "12px" }}
                  >
                    <i className="mdi mdi-plus me-1"></i> Thêm mới tài xế
                  </button>
                </Link>
              </div>
            </div>
          </div>
          <div className="flex items-center justify-between">
          </div> 
        <Table className="table table-striped">
            <Thead className="table-light">
              <Tr>
                <Th className="align-middle">STT</Th>
                <Th className="align-middle">Họ tên</Th>
                <Th className="align-middle">Tỉnh thành</Th>
                <Th className="align-middle">Số điện thoại</Th>
                <Th className="align-middle">Email</Th>
                <Th className="align-middle">Trạng thái</Th>
                <Th className="align-middle">Cập nhật</Th>
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
                        <Td>{filter.page * filter.pageSize + idxx + 1}</Td>
                        <Td>{i.CompleteName}</Td>
                        <Td>{i.ProvinceName}</Td>
                        <Td>{i.Phone}</Td>
                        <Td>{i.Email}</Td>
                        {i.Status === true ? (
                          <Td>
                            <div className="d-flex gap-3">
                              <div
                                className="text-success"
                                onClick={() => {
                                  Swal.fire({
                                    title: "Huỷ duyệt tài xế ?",
                                    text: "Bạn có chắc muốn huỷ duyệt !",
                                    icon: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#3085d6",
                                    cancelButtonColor: "#d33",
                                    confirmButtonText: "Xác nhận !",
                                  }).then((result) => {
                                    if (result.isConfirmed) {
                                      ApproveDelvery(i).then((response) => {
                                        if (response.success) {
                                          Swal.fire(
                                            "Hoàn thành!",
                                            "Bỏ duyệt tài xế thành công.",
                                            "success"
                                          );
                                          onViewAppearing();
                                        } else {
                                          Swal.fire({
                                            title: "Lỗi!",
                                            text: "Đã xảy ra lỗi hệ thống!",
                                            icon: "error",
                                            confirmButtonText: "OK",
                                          });
                                          return;
                                        }
                                      });
                                    }
                                  });
                                }}
                              >
                                <i className="fas fa-check-circle font-size-18"></i>
                              </div>
                            </div>
                          </Td>
                        ) : (
                          <Td>
                            <div className="d-flex gap-3">
                              <div
                                className="text-danger"
                                onClick={() => {
                                  Swal.fire({
                                    title: "Duyệt tài xế ?",
                                    text: "Bạn có chắc muốn duyệt !",
                                    icon: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#3085d6",
                                    cancelButtonColor: "#d33",
                                    confirmButtonText: "Xác nhận !",
                                  }).then((result) => {
                                    if (result.isConfirmed) {
                                      ApproveDelvery(i).then((response) => {
                                        if (response.success) {
                                          Swal.fire(
                                            "Hoàn thành!",
                                            "Duyệt tài xế thành công.",
                                            "success"
                                          );
                                          onViewAppearing();
                                        } else {
                                          Swal.fire({
                                            title: "Lỗi!",
                                            text: "Đã xảy ra lỗi hệ thống!",
                                            icon: "error",
                                            confirmButtonText: "OK",
                                          });
                                          return;
                                        }
                                      });
                                    }
                                  });
                                }}
                              >
                                <i className="fas fa-times-circle font-size-18"></i>
                              </div>
                            </div>
                          </Td>
                          
                        )}
                        <Td>
                          <Link to={`/delivery/edit/${i.DeliveryDriverId}`}>
                            <div className="d-flex gap-3">
                              <div className="text-success">
                                <i className="mdi mdi-pencil font-size-18"></i>
                              </div>
                            </div>
                          </Link>
                        </Td>
                        {/* Xóa tài xế */}
                        <Td>
                          <div className="d-flex gap-3">
                            <div
                              className="text-danger"
                              onClick={() => {
                                Swal.fire({
                                  title: "Xóa tài xế ?",
                                  text: "Bạn muốn xóa tài xế này ra khỏi danh sách !",
                                  icon: "warning",
                                  showCancelButton: true,
                                  confirmButtonColor: "#3085d6",
                                  cancelButtonColor: "#d33",
                                  confirmButtonText: "Xác nhận !",
                                }).then((result) => {
                                  if (result.isConfirmed) {
                                    RemoveDriver({
                                      DeliveryDriverId: i.DeliveryDriverId
                                    }).then((response) => {
                                      if (response.success) {
                                        Swal.fire(
                                          "Hoàn thành!",
                                          "Xóa dữ liệu thành công.",
                                          "success"
                                        );
                                        onViewAppearing();
                                      } else {
                                        Swal.fire(
                                          "Tài xế này đã có phát sinh vận chuyển! Vui lòng cập nhật trạng thái nếu muốn khóa tài khoản!",
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
          <Paginate
            onPageChange={onPageChange}
            pageRangeDisplayed={5}
            pageTotal={pageTotal || 1}
          />
        </div>
      </div>
    </>
  );
}
