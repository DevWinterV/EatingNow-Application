import * as React from "react";
import { SendNotification } from "../../api/fcm/fcmService";
import { Paginate } from "../../controls";
import { Breadcrumb } from "../../controls";
import { useNavigate } from "react-router-dom";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import { TakeAllOrder, ApproveOrder , GetListOrderLineDetails} from "../../api/order/orderService";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import Swal from "sweetalert2";
import { RemoveOrderHeader } from "../../api/customer/customerService";

import $ from 'jquery'; // Ensure you have jQuery installed
window.jQuery = $;
require('signalr'); // Ensure you have the SignalR library installed
//Kết nối đến SignalR Ordernotication
let connection = $.hubConnection('http://localhost:3002/signalr/hubs');
let proxy = connection.createHubProxy('OrderNotificationHub');


export default function OrderPage() {
  const [isShown, setIsShown] = React.useState(false);
  const [showOrderDetailDialog, setShowOrderDetailDialog] = React.useState(false);
  const history = useNavigate();

  const breadcrumbSources = [
    {
      name: "Đơn hàng",
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
  });
  const [data, setData] = React.useState([]);
  const [datadetail, setDatadetail ] = React.useState([]);

  const [notification, setNotification] = React.useState(
    {
      to: "",
      data: 
      {
        body: "Đơn hàng đã được xác nhận",
        title: "Thông báo",
        icon: "https://img.icons8.com/?size=1x&id=25175&format=png",
        image: "https://image.shutterstock.com/image-vector/chat-notification-260nw-660974722.jpg",
        action_link: "localhost:3001/account",
      },
      notification: 
      {
        body: "Đơn hàng đã được xác nhận",
        title: "Thông báo",
        icon: "https://img.icons8.com/?size=1x&id=25175&format=png",
        image: "https://image.shutterstock.com/image-vector/chat-notification-260nw-660974722.jpg",
      },
    });
    

  const [notificationCancle, setNotificationCancle] = React.useState(
      {
        to: "",
        data: 
        {
          body: "Đơn hàng của bạn đã bị hủy",
          title: "Thông báo",
          icon: "https://img.icons8.com/?size=1x&id=25175&format=png",
          image: "https://image.shutterstock.com/image-vector/chat-notification-260nw-660974722.jpg",
          action_link: "localhost:3001/account",
        },
        notification: 
        {
          body: "Đơn hàng của bạn đã bị hủy",
          title: "Thông báo",
          icon: "https://img.icons8.com/?size=1x&id=25175&format=png",
          image: "https://image.shutterstock.com/image-vector/chat-notification-260nw-660974722.jpg",
        },
    });
  const [loading, setLoading] = React.useState(false);
  const [pageTotal, setPageTotal] = React.useState(1);
  async function changeSearch(e) {
    console.log(e.target.value);
    setFilter({ ...filter,
       term: e.target.value });
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
    let response = await TakeAllOrder(filter);
    if (response.success) {
      setData(response.data);
      setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
    }
    setLoading(false);
  }
  async function LoadDataDetails(orderid) {
    setIsShown(true);
    let response = await GetListOrderLineDetails(orderid);
    if (response.success) {
      setDatadetail(response.data);
    }
  }


  React.useEffect(() => {
    // Attempt connection and handle connection and error events
    connection.start()
      .done(() => {
        console.log('Kết nối thành công SignalR');
      })
      .fail((error) => {
        console.error('Could not connect:', error);
      });
  
    // Log the connection status
    connection.stateChanged((change) => {
      console.log('Connection state changed:', change.newState);
    });
  }, []);

  // Sử dụng useEffect để theo dõi sự thay đổi trong notification và gửi thông báo khi nó thay đổi
  React.useEffect(() => {
    onViewAppearing();
    SendNotification(notificationCancle).then(
      (response) => {
        if (response.success) {
          Swal.fire(
            "Thành công!",
            "Đã gửi thông báo hủy đơn hàng thành công.",
            "success"
          );
          setNotificationCancle({...notificationCancle, to:""})
          onViewAppearing();
        } else {
          Swal.fire({
            title: "Lỗi!",
            text: "Không thể gửi thông báo hủy đơn hàng.",
            icon: "error",
            confirmButtonText: "OK",
          });
          return;
        }
      }
    );
    SendNotification(notification).then(
      (response) => {
        if (response.success) {
          Swal.fire(
            "Thành công!",
            "Đã gửi thông báo xác nhận thành công.",
            "success"
          );
          setNotification({...notification, to:""})
          onViewAppearing();
        } else {
          Swal.fire({
            title: "Lỗi!",
            text: "Không thể gửi thông báo xác nhận đơn hàng!",
            icon: "error",
            confirmButtonText: "OK",
          });
          return;
        }
      }
    );
  }, [notification, notificationCancle]);


  React.useEffect(() => {
    onViewAppearing(); 
  }, [filter.page, filter.pageSize, filter.term]);
  return (
    <>
    
      <Breadcrumb title="Danh sách đơn hàng" sources={breadcrumbSources} />
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
                    placeholder="Mã đơn hàng, khách hàng, ..."
                    value={filter.term}
                    onChange={changeSearch}
                    onKeyDown={(e) => {
                      if (e.code == "Enter") {
                        onViewAppearing();
                        setFilter({ ...filter,
                          term: e.target.value,
                           page: 0 });
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
            {/*         Thêm mới đơn hàng    
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
                    <i className="mdi mdi-plus me-1"></i> Thêm đơn hàng
                  </button>
                </Link>
              </div>
            </div> 
            */}


          </div>
          <div className="flex items-center justify-between">
          </div>  
          <Table className="table table-striped">
            <Thead className="table-light">
              <Tr style={{ borderColor: "#d9d9d9" }}>
                <Th className="align-middle">STT</Th>
                <Th className="align-middle">Mã đơn hàng</Th>
                <Th className="align-middle">Ngày tạo</Th>
                <Th className="align-middle">Khách hàng đặt</Th>
                <Th className="align-middle">Địa chỉ nhận hàng</Th>
                <Th className="align-middle">Tổng tiền sản phẩm</Th>
                <Th className="align-middle">Phí ship</Th>
                <Th className="align-middle">Tổng thanh toán</Th>
                <Th className="align-middle">Trạng thái</Th>
                <Th className="align-middle">Chi tiết đơn hàng</Th>
                <Th className="align-middle">Xóa</Th>
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
                        <Td>{i.OrderHeaderId}</Td>
                        <Td>{i.CreationDate}</Td>
                        <Td>{i.CustomerName}</Td>
                        <Td>{i.FormatAddress}</Td>
                        <Td>{parseFloat(i.TotalAmt).toLocaleString("vi-VN", { style: "currency", currency: "VND" })}</Td>
                        <Td>{parseFloat(i.TransportFee).toLocaleString("vi-VN", { style: "currency", currency: "VND" })}</Td>
                        <Td>{parseFloat(i.IntoMoney).toLocaleString("vi-VN", { style: "currency", currency: "VND" })}</Td>
                        {i.Status === true ? (
                          <Td>
                            <div className="d-flex gap-3">
                              <div
                                className="text-success"
                                onClick={() => {
                                  Swal.fire({
                                    title: "Huỷ duyệt đơn hàng ?",
                                    text: "Bạn có chắc muốn huỷ duyệt !",
                                    icon: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#3085d6",
                                    cancelButtonColor: "#d33",
                                    confirmButtonText: "Xác nhận !",
                                  }).then((result) => {
                                    if (result.isConfirmed) {
                                      ApproveOrder(i).then((response) => {
                                        if (response.success) {
                                          setNotificationCancle({ ...notification, 
                                            to: i.TokenApp,
                                            data: { ...notification.data,            
                                                    body:`Đơn hàng ${i.OrderHeaderId} đã bị hủy.`,
                                                    action_link:`http://localhost:3001/order/${i.OrderHeaderId}`
                                                  }, 
                                                notification: { ...notification.notification,            
                                                  body:`Đơn hàng ${i.OrderHeaderId} đã bị hủy.`,
                                                  action_link:`http://localhost:3001/order/${i.OrderHeaderId}`
                                                } 
                                              });
                                          setNotificationCancle({ ...notification, 
                                            to: i.TokenWeb,
                                            data: { ...notification.data,            
                                                    body:`Đơn hàng ${i.OrderHeaderId} đã bị hủy.`,
                                                    action_link:`http://localhost:3001/order/${i.OrderHeaderId}`
                                                  }, 
                                                notification: { ...notification.notification,            
                                                  body:`Đơn hàng ${i.OrderHeaderId} đã bị hủy.`,
                                                  action_link:`http://localhost:3001/order/${i.OrderHeaderId}`
                                                } 
                                              });


                                          Swal.fire(
                                            "Hoàn thành!",
                                            "Bỏ duyệt đơn hàng thành công.",
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
                                    title: "Duyệt đơn hàng ?",
                                    text: "Bạn có chắc muốn duyệt !",
                                    icon: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#3085d6",
                                    cancelButtonColor: "#d33",
                                    confirmButtonText: "Xác nhận !",
                                  }).then((result) => {
                                    if (result.isConfirmed) {
                                      ApproveOrder(i).then((response) => {
                                        if (response.success) {
                                          setNotification({ ...notification, 
                                            to: i.TokenApp,
                                            data: { ...notification.data,            
                                                    body:`Đơn hàng ${i.OrderHeaderId} đã được xác nhận.`,
                                                    action_link:`http://localhost:3001/order/${i.OrderHeaderId}`
                                                  }, 
                                                notification: { ...notification.notification,            
                                                  body:`Đơn hàng ${i.OrderHeaderId} đã được xác nhận.`,
                                                  action_link:`http://localhost:3001/order/${i.OrderHeaderId}`
                                              } 
                                          });
                                          setNotification({ ...notification, 
                                            to: i.TokenWeb,
                                            data: { ...notification.data,            
                                                    body:`Đơn hàng ${i.OrderHeaderId} đã được xác nhận.`,
                                                    action_link:`http://localhost:3001/order/${i.OrderHeaderId}`
                                                  }, 
                                                notification: { ...notification.notification,            
                                                  body:`Đơn hàng ${i.OrderHeaderId} đã được xác nhận.`,
                                                  action_link:`http://localhost:3001/order/${i.OrderHeaderId}`
                                              } 
                                              
                                          });
                                          Swal.fire(
                                            "Hoàn thành!",
                                            "Duyệt đơn hàng thành công.",
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
                      {
                          /*  <Td>
                        <div className="text-success">
                          <i
                            style={{ cursor: "pointer" }}
                            className="mdi mdi-pencil font-size-18"
                            onClick={() => {
                              LoadDataDetails(i.OrderHeaderId);
                              setShowOrderDetailDialog(true); // Mở dialog khi người dùng click
                            }}
                          ></i>
                        </div>
                      </Td>*/
                      }
                      {/* Xem chi tiết đơn hàng*/}
                      <Td>
                          <div className="d-flex gap-3">
                            <div
                              className="text-info"
                              onClick={() => {
                                history("/order/detail/" + i.OrderHeaderId, {
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
                                  title: "Xóa đơn hàng ?",
                                  text: "Bạn có chắc chắn muốn xóa đơn hàng này không?",
                                  icon: "warning",
                                  showCancelButton: true,
                                  confirmButtonColor: "#3085d6",
                                  cancelButtonColor: "#d33",
                                  confirmButtonText: "Xác nhận !",
                                }).then((result) => {
                                  if (result.isConfirmed) {
                                    RemoveOrderHeader(i).then((response) => {
                                      if (response.success) {
                                        Swal.fire(
                                          "Hoàn thành!",
                                          "Xóa dữ liệu thành công.",
                                          "success"
                                        );
                                        onViewAppearing();
                                      } else {
                                        Swal.fire(
                                          "Không thể xóa đơn hàng!",
                                          "Lỗi trong khi xóa đơn hàng, vui lòng thử lại!",
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



