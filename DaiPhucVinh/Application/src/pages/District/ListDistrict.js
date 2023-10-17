import * as React from "react";
import { useNavigate } from "react-router-dom";
import { Breadcrumb } from "../../controls";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import { Paginate } from "../../controls";
import { Link } from "react-router-dom";
import Select from "react-select";
import Swal from "sweetalert2";
import {
  TakeAllDistrict,
  DeleteDistrict,
} from "../../api/district/districtService";
import { TakeAllProvince } from "../../api/province/provinceService";
export default function ListProvince() {
  const history = useNavigate();
  const breadcrumbSources = [
    {
      name: "Danh sách huyện",
      href: "#",
    },
    {
      name: "Danh sách",
      active: true,
    },
  ];

  const [loading, setLoading] = React.useState(false);
  const [data, setData] = React.useState([]);
  const [pageTotal, setPageTotal] = React.useState(1);
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 20,
    term: "",
    ItemProvinceCode: "",
  });

  async function onViewAppearing() {
    setLoading(true);
    let response = await TakeAllDistrict(filter);
    if (response.success) {
      setData(response.data);
      setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
    }
    await onFillItemProvince();
    setLoading(false);
  }

  const [itemProvince, setItemProvince] = React.useState([]);
  const [defaultItemProvince, setDefaultItemProvince] = React.useState({
    value: "",
    label: "Chọn tỉnh",
  });
  async function onFillItemProvince() {
    let itemProvinceResponse = await TakeAllProvince();
    if (itemProvinceResponse.success) {
      setItemProvince([
        {
          value: "",
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

  function onPageChange(e) {
    setFilter({
      ...filter,
      page: e.selected,
    });
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.page, filter.pageSize, filter.ProvinceId]);

  React.useEffect(() => {
    onFillItemProvince();
  }, []);
  return (
    <>
      <Breadcrumb title="Danh sách huyện" sources={breadcrumbSources} />
      <div className="row" style={{ fontSize: "12px" }}>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col-lg-3">
                <div>
                  <label className="form-label fw-bold">Tìm kiếm</label>
                </div>
                <div className="mb-3 input-group">
                  <input
                    type="search"
                    className="form-control"
                    placeholder="Tên huyện..."
                    value={filter.term}
                    onChange={(e) => {
                      setFilter({ ...filter, term: e.target.value });
                    }}
                    onKeyDown={(e) => {
                      if (e.code == "Enter") {
                        onViewAppearing();
                        setFilter({
                          ...filter,
                          page: 0,
                        });
                      }
                    }}
                    style={{ fontSize: "12px" }}
                  />
                  <button
                    className="btn btn-success "
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
              <div className="col-lg-4">
                <div className="mb-3">
                  <label className="form-label fw-bold">Tỉnh thành</label>
                  <Select
                    options={itemProvince}
                    value={defaultItemProvince}
                    onChange={(e) => {
                      setDefaultItemProvince({
                        value: e.value,
                        label: e.label,
                      });
                      setFilter({
                        ...filter,
                        page: 0,
                        ProvinceId: e.value,
                      });
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
                  <Link to="./create">
                    <button
                      type="button"
                      className="btn btn-info"
                      onClick={() => {}}
                      style={{ fontSize: "12px" }}
                    >
                      <i className="mdi mdi-plus me-1"></i> Thêm huyện
                    </button>
                  </Link>
                </div>
              </div>
            </div>
            <Table className="table table-striped">
              <Thead className="table-light">
                <Tr>
                  <Th className="align-middle">STT</Th>
                  <Th className="align-middle">Tên huyện</Th>
                  <Th className="align-middle"></Th>
                  <Th className="align-middle"></Th>
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
                        <Tr key={"row_" + idx} onDoubleClick={() => {}}>
                          <Td>{filter.page * filter.pageSize + idx + 1}</Td>
                          <Td>{i.Name}</Td>
                          <Td>
                            <div className="d-flex gap-3">
                              <div
                                className="text-success"
                                onClick={() => {
                                  history("/district/edit/" + i.DistrictId, {
                                    state: { data: i },
                                  });
                                }}
                              >
                                <i className="mdi mdi-pencil font-size-18"></i>
                              </div>
                            </div>
                          </Td>
                          <Td>
                            <div className="d-flex gap-3">
                              <div
                                className="text-danger"
                                onClick={() => {
                                  Swal.fire({
                                    title: "Xóa nhóm sản phẩm ?",
                                    text: "Bạn muốn xóa nhóm sản phẩm này ra khỏi danh sách !",
                                    icon: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#3085d6",
                                    cancelButtonColor: "#d33",
                                    confirmButtonText: "Xác nhận !",
                                  }).then((result) => {
                                    if (result.isConfirmed) {
                                      DeleteDistrict(i.DistrictId).then(
                                        (response) => {
                                          if (response.success) {
                                            Swal.fire(
                                              "Hoàn thành!",
                                              "Xóa dữ liệu thành công.",
                                              "success"
                                            );
                                            onViewAppearing();
                                          } else {
                                            Swal.fire({
                                              title: "Lỗi!",
                                              text: "Không thể xóa. Nhóm sản phẩm này đã tồn tại sản phẩm!",
                                              icon: "error",
                                              confirmButtonText: "OK",
                                            });
                                            return;
                                          }
                                        }
                                      );
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
                        <Td colSpan={6}>Không có dữ liệu</Td>
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
      </div>
    </>
  );
}
