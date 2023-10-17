import * as React from "react";
import Select from "react-select";
import { Paginate } from "../../controls";
import { Breadcrumb } from "../../controls";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import { TakeAllCuisine } from "../../api/categoryItem/categoryItemService";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import { ApproveStore, TakeAllStore, SearchStore } from "../../api/store/storeService";
import { Link } from "react-router-dom";
import Swal from "sweetalert2";

export default function ListStorePage() {
  const breadcrumbSources = [
    {
      name: "Cửa hàng",
      href: "#",
    },
    {
      name: "Danh sách",
      active: true,
    },
  ];
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 20,
    term: "",
    ItemGroup_Code: "",
    ItemGroup_Name: "",
    ItemCategoryCode: 0,
    ItemCategoryName: "",
  });
  const [data, setData] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const [pageTotal, setPageTotal] = React.useState(1);
  const [itemCuisine, setItemCuisine] = React.useState([]);

  const [defaultItemCategory, setDefaultItemCategory] = React.useState({
    value: 0,
    label: "Tất cả",
  });

  async function changeSearch(e) {
    // Wait for 2 seconds before updating the term property
      setFilter({ ...filter, term: e.target.value });
    console.log(filter)
  }
  
  async function onFillItemCategory() {
    let itemCategoryResponse = await TakeAllCuisine();
    if (itemCategoryResponse.success) {
      setItemCuisine([
        {
          value: 0,
          label: "Tất cả",
        },
        ...itemCategoryResponse.data.map((e) => {
          return {
            value: e.CuisineId,
            label: e.Name,
          };
        }),
      ]);
    }
  }
  function handleInputChange(e) {
    setFilter({ ...filter, term: e.target.value });
  }



  function onPageChange(e) {
    setFilter({
      ...filter,
      page: e.selected,
    });
  }
  async function onViewAppearing() {
    setLoading(true);
    if(filter.term != ""){
      let response = await SearchStore(filter);
      if (response.success) {
        setData(response.data);
        setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
      }
    }else
    {
      let response = await TakeAllStore(filter);
      if (response.success) {
        setData(response.data);
        setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
      }
    }

    setLoading(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [filter]);

  React.useEffect(() => {
    onFillItemCategory();
  }, []);

  return (
    <>
      <Breadcrumb title="Danh sách cửa hàng" sources={breadcrumbSources} />
      <div className="card" style={{  fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
            <div className="col-lg-4">
              <div className="mb-3">
                <label className="d-flex fw-bold">Tìm kiếm</label>
                <div className="input-group">
                <input
                  type="search"
                  className="form-control"
                  placeholder="Tên cửa hàng..."
                  value={filter.term}
                  onChange={handleInputChange}
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
            <div className="col-lg-4">
              <div className="mb-3">
                <label className="form-label fw-bold">Loại hình món ăn</label>
                <Select
                  options={itemCuisine}
                  value={defaultItemCategory}
                  onChange={(e) => {
                    setDefaultItemCategory({
                      value: e.value,
                      label: e.label,
                    });
                    setFilter({
                      ...filter,
                      page: 0,
                      ItemCategoryCode: e.value,
                    });
                    console.log(filter);
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div
              className="col d-flex justify-content-end align-items-right"
              style={{ marginTop: "27px" }}
            >
              <div className="mb-3">
                <Link to="/store/create">
                  <button
                    type="button"
                    className="btn btn-info"
                    onClick={() => {}}
                    style={{ fontSize: "12px" }}
                  >
                    <i className="mdi mdi-plus me-1"></i> Thêm cửa hàng
                  </button>
                </Link>
              </div>
            </div>
          </div>
          <Table className="table table-striped">
            <Thead className="table-light">
              <Tr style={{ borderColor: "#d9d9d9" }}>
                <Th className="align-middle">STT</Th>
                <Th className="align-middle">Ảnh đại diện</Th>
                <Th className="align-middle">Tên cửa hàng</Th>
                <Th className="align-middle">Tỉnh</Th>
                <Th className="align-middle">Hình thức bán hàng</Th>
                <Th className="align-middle">Email</Th>
                <Th className="align-middle">Địa chỉ</Th>
                <Th className="align-middle">Trạng thái</Th>
                <Th className="align-middle">Cập nhật</Th>
              </Tr>
            </Thead>

            <Tbody style={{ position: "relative" }}>
              {loading ? (
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
                    data.map((i, idx) => (
                      <Tr key={"row_" + idx}>
                        <Td>{filter.page * filter.pageSize + idx + 1}</Td>
                        {i.AbsoluteImage == "" || i.AbsoluteImage == null ? (
                          <Td>Chưa có hình ảnh</Td>
                        ) : (
                          <Td>
                            <img
                              style={{ width: "100px", height: "50px" }}
                              src={i.AbsoluteImage}
                            />
                          </Td>
                        )}
                        <Td>{i.FullName}</Td>
                        <Td>{i.Province}</Td>
                        <Td>{i.Cuisine}</Td>
                        <Td>{i.Email}</Td>
                        <Td>{i.Address}</Td>
                        {i.Status == "True" ? (
                          <Td>
                            <div className="d-flex gap-3">
                              <div
                                className="text-success"
                                onClick={() => {
                                  Swal.fire({
                                    title: "Huỷ duyệt cửa hàng ?",
                                    text: "Bạn có chắc muốn huỷ duyệt !",
                                    icon: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#3085d6",
                                    cancelButtonColor: "#d33",
                                    confirmButtonText: "Xác nhận !",
                                  }).then((result) => {
                                    if (result.isConfirmed) {
                                      ApproveStore(i).then((response) => {
                                        if (response.success) {
                                          Swal.fire(
                                            "Hoàn thành!",
                                            "Bỏ duyệt cửa hàng thành công.",
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
                                    title: "Duyệt cửa hàng ?",
                                    text: "Bạn có chắc muốn duyệt !",
                                    icon: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#3085d6",
                                    cancelButtonColor: "#d33",
                                    confirmButtonText: "Xác nhận !",
                                  }).then((result) => {
                                    if (result.isConfirmed) {
                                      ApproveStore(i).then((response) => {
                                        if (response.success) {
                                          Swal.fire(
                                            "Hoàn thành!",
                                            "Duyệt cửa hàng thành công.",
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
                          <div className="d-flex gap-3">
                            <div
                              className="text-success"
                              onClick={() => {
                                window.open("/store/detail/" + i.UserId);
                                localStorage.Code = i.Code;
                              }}
                            >
                              <i
                                style={{ cursor: "pointer" }}
                                className="mdi mdi-pencil font-size-18"
                              ></i>
                            </div>
                          </div>
                        </Td>
                      </Tr>
                    ))
                  ) : (
                    <Tr>
                      <Td colSpan={8}>Không có dữ liệu</Td>
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
