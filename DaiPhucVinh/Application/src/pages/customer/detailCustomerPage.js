import * as React from "react";
import { Breadcrumb } from "../../controls";
import moment from "moment";
import _ from "lodash";
import { useLocation, useNavigate , useParams} from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import { Paginate } from "../../controls";
import { TakeAllOrderLineByCustomerId } from "../../api/store/storeService";
export default function DetailCustomerPage() {
  const navigate = useNavigate();
  const { state } = useLocation();
  const { id } = useParams();
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
    CustomerId: id,
    page: 0,
    pageSize: 20,
    term: ""
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
      console.log(state.data);
      console.log(filter);
      let response = await TakeAllOrderLineByCustomerId(filter);
      console.log(response);
      if (response.success) {
        setData(response.data);
        setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
      }
    }
    setLoading(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.page, filter.pageSize, filter.term]);
  return (
    <>
      <Breadcrumb
        title={"Danh sách sản phẩm đã mua của khách hàng"}
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

   
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
              <div className="col-lg-3">
                <div className="mb-3">
                  <label className="d-flex fw-bold">Tìm kiếm</label>
                  <div className="input-group">
                    <input
                      type="search"
                      className="form-control"
                      placeholder="Mã đơn hàng, món ăn,..."
                      value={filter.term}
                      onChange={(e) =>
                        setFilter({ ...filter, term: e.target.value })
                      }
                      onKeyDown={(e) => {
                        if (e.code == "Enter") {
                          onViewAppearing();
                          setFilter({ ...filter,
                            term: e.target.value,
                            page: 0 });
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
              </div>
          </div>
          <Table className="table table-striped">
            <Thead className="table-light">
              <Tr>
                <Th className="align-middle">STT</Th>
                <Th className="align-middle">Mã hóa đơn</Th>
                <Th className="align-middle">Tên món ăn</Th>
                <Th className="align-middle">Số lượng</Th>
                <Th className="align-middle">Giá</Th>
                <Th className="align-middle">Mô tả</Th>
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
                        <Td>{i.OrderHeaderId}</Td>
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
