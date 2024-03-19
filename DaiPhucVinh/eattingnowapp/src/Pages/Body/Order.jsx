import React, { useEffect ,useState} from "react";
import { ToastContainer } from "react-toastify";
import { BsSearch } from "react-icons/bs";
import { TbAddressBook, TbCandle, TbCheck, TbCircleX, TbHomeCancel, TbHomeX, TbInfoCircle, TbListDetails, TbNotificationOff, TbPhoneCall, TbPhotoCancel } from "react-icons/tb";
import { useStateValue } from "../../context/StateProvider";
import {SendNotification} from "../../api/fcm/fcmService";
import { Loader } from "../../components";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  GetListOrderLineDetails,
  ApproveOrder,
  TakeOrderHeaderByStoreId,
} from "../../api/store/storeService";
import Swal from "sweetalert2";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import $ from 'jquery'; // Ensure you have jQuery installed
import { FilterListIcon } from "evergreen-ui";
import { toast } from "react-toastify";
window.jQuery = $;
require('signalr'); // Ensure you have the SignalR library installed
//Kết nối đến SignalR Ordernotication
let connection = $.hubConnection('http://localhost:3002/signalr/hubs');
let proxy = connection.createHubProxy('OrderNotificationHub');

const Order = () => {
  const getCurrentDate = () => {
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = String(currentDate.getDate()).padStart(2, '0');
    const hours = String(currentDate.getHours()).padStart(2, '0');
    const minutes = String(currentDate.getMinutes()).padStart(2, '0');
    const seconds = String(currentDate.getSeconds()).padStart(2, '0');
  
    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
  };
  const [isLoading, setIsLoading] = React.useState(false);
  const [status, setStatus] = useState('1');
  const [isButtonEnabled, setIsButtonEnabled] = useState(false);
  const [isDriverAvailabel, setisDriverAvailabel] = useState(false);

  const [data, setData] = React.useState([]);
  const [dataDetails, setDataDetails] = React.useState([]);
  const [{ user }] = useStateValue();


  const [startDate, setStartDate] = useState(getCurrentDate());
  const [endDate, setEndDate] = useState(getCurrentDate());

  useEffect(() => {
    // Set initial values when the component mounts
    setStartDate(getCurrentDate());
    setEndDate(getCurrentDate());
  }, []);

  const [requestFillter, setrequestFillter] =useState({
    Id: user?.UserId,
    OrderStatus: 2,
    startDate: "",
    endDate: "",
    keyword: ""
  });

  const [isShown, setIsShown] = React.useState(false);
  const [orderHeaderId, setOrderHeaderId] = React.useState("");

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
    
    const [customerDetail, setCustomerDetail] = React.useState(
      {
          customerName: "",
          customerPhone: "",
          customerAddress: "",
      });
  // Xử lý khi nút "Yêu cầu tài xế" được nhấn
    async function handleButtonClick() {
        setisDriverAvailabel(true);
    }

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

      const handleStatusChange = (event) => {
        const selectedStatus = event.target.value;
        setStatus(selectedStatus);
    
        if (selectedStatus === '2') {
          setIsButtonEnabled(true);
        } else {
          setIsButtonEnabled(false);
        }
      };

    const handleFilterClick = () => {
      if(requestFillter.startDate === "" || requestFillter.endDate ===""){
        toast.warning("Vui lòng chọn ngày bắt đầu và ngày kết thúc");
        return;
      }
      onViewAppearing();
    };


  async function onViewAppearing() {
    setIsLoading(true);
    if (user) {

      let response = await TakeOrderHeaderByStoreId(requestFillter);
      setData(response.data);
    }
    setIsLoading(false);
  }

  function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = String(date.getFullYear());
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    return `${year}-${month}-${day}`;
    return `${day}-${month}-${year} ${hours}:${minutes}`;
  }

  function formatMoney(amount) {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
      minimumFractionDigits: 0,
    }).format(amount);
  }


  async function GetOrderLineDetails() {
    setIsLoading(true);
    if (orderHeaderId != "") {
      setIsShown(true);
      let response = await GetListOrderLineDetails(orderHeaderId);
      setDataDetails(response.data);
    }
    setIsLoading(false);
  }

// Sử dụng useEffect để theo dõi sự thay đổi trong notification và gửi thông báo khi nó thay đổi
  useEffect(() => {
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



  useEffect(() => {
    GetOrderLineDetails();
  }, [orderHeaderId]);

  function sendOrderNotification(item) {
    proxy
      .invoke('SendOrderNotificationToUser', 'Thông báo mới', item.CustomerId)
      .done(() => {
        console.log('Gửi thông báo thành công');
      })
      .fail((error) => {
        console.error('Lỗi không gửi được thông báo:', error);
      });
  }
  
  useEffect(() => {
    // Set up a client method to receive order notifications
    proxy.on('ReceiveOrderNotification', (orderMessage) => {
      onViewAppearing();
    });
    // Set up a client method to receive user-specific order notifications
    proxy.on('ReceiveOrderNotificationOfUser', (orderMessage) => {
      onViewAppearing();
      Swal.fire({
        title: orderMessage,
        text: "Bạn có đơn hàng mới",
        minutes: 5,
        icon: "info",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Xem đơn đặt hàng",
      }).then((result) => {
        if (result.isConfirmed) { 
          onViewAppearing();
        }
      });
    });
    // Attempt connection and handle connection and error events
    connection.start()
      .done(() => {
        console.log('Kết nối thành công SignalR');
        // Đăng ký người dùng khi kết nối thành công
        if (user !== null) {
          proxy.invoke('SetCustomerId', user.UserId)
            .done(() => {
              console.log('Đăng ký người dùng thành công');
            })
            .fail((error) => {
              console.error('Lỗi đăng ký người dùng:', error);
            });            
        }
      })
      .fail((error) => {
        console.error('Could not connect:', error);
      });
  
    // Log the connection status
    connection.stateChanged((change) => {
      console.log('Connection state changed:', change.newState);
    });
  }, []);

  const handleChangeStartDate = (e) => {
    setStartDate(e.target.value);
    setrequestFillter({
      ...requestFillter,
      startDate: e.target.value
    });
  };

  const handleChangeEndDate = (e) => {
    setEndDate(e.target.value);
    setrequestFillter({
      ...requestFillter,
      endDate: e.target.value
    });
  };

  const handleChangeKeyWord = (e) => {
    setrequestFillter({
      ...requestFillter,
      keyword: e.target.value
    });
  };
  const handleOrderStatusChange = (e) => {
    setrequestFillter({
      ...requestFillter,
      OrderStatus: parseInt(e.target.value, 10)
    });
  };
  useEffect(()=>{
    onViewAppearing();
  },[requestFillter.OrderStatus, requestFillter.keyword]);

  
  return (
    <div className="bg-white-50 h-[100%] basis-80 p-8 overflow-auto no-scrollbar py-5 px-5">
      <ToastContainer/>
      <div className="flex items-center justify-between">
        <div className="flex items-center border-b-2 pb-2 basis-3/3 gap-2">
          <input
          onChange={handleChangeKeyWord}
            type="text"
            placeholder="Nhập mã đơn, tên khách hàng, địa chỉ ..."
            className="border-none outline-none placeholder:text-sm focus:outline-none"
          />
          <BsSearch className="text-hoverColor text-[20px] cursor-pointer" />
        </div>
        <div className="flex items-center border-b-2 pb-2 basis-2/2 gap-2">
          <label htmlFor="">Trạng thái đơn :</label>
          <select 
              name="orderStatus" 
              id="orderStatus" 
              value={requestFillter.OrderStatus} 
              onChange={handleOrderStatusChange}
            >
            <option value="2">Xem tất cả</option>
            <option value="0">Chưa xác nhận</option>
            <option value="1">Đã xác nhận</option>
          </select>
        </div>
        <div className="flex items-center border-b-2 pb-2 basis-2/2 gap-2">
          <label htmlFor="">Từ:</label>
          <input 
            type="date" 
            value={startDate} 
            onChange={handleChangeStartDate}/>
          <label htmlFor="">Đến:</label>
          <input 
            type="date"
             value={endDate}
             onChange={handleChangeEndDate}/>
          <button 
            className="custom-button ml-2"
            onClick={handleFilterClick}>
          <div className="flex">
            <FilterListIcon className="icon" />
          </div>
        </button>
        </div>

        {/* <div className="flex gap-4 items-center">
          <AiOutlineAppstoreAdd className="text-hoverColor cursor-pointer text-[25px] hover:text-[20px] transition-all" />
          <button className="bg-red-600 cursor-pointer text-bodyBg font-semibold py-1 px-4 rounded-[5px] transition-all">
            Quản lý
          </button>
        </div> */}
        {isShown ? (
          <>
            <div className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none">
              <div
                className="relative w-auto my-6 mx-auto max-w-7xl"
                style={{ width: "1000px" }}
              >
                {/*content*/}
                <div className="border-0 rounded-lg shadow-lg relative flex flex-col w-auto bg-white outline-none focus:outline-none">
                  {/*header*/}
                  <div className="flex items-start justify-between p-5 border-b border-solid border-slate-200 rounded-t">
                    <h2 className="text-2xl font-semibold">
                      Xem chi tiết đơn hàng
                    </h2>
                    
                    <button className="p-1 ml-auto bg-transparent border-0 text-black opacity-5 float-right text-3xl leading-none font-semibold outline-none focus:outline-none"
                      class="bg-red"
                      onClick={() => {
                        setIsShown(false);
                      }}>
                          <TbCircleX className="text-2xl" />
                 </button>
                  </div>

                  <div class="flex items-center justify-end p-6 bg-gray-200">
                    <label for="cb_statusOrder" class="mr-2">Trạng thái:</label>
                    <select
                        name="cb_statusOrder"
                        id="cb_statusOrder"
                        className="border rounded-md px-2 py-1"
                      >                 
                      <option value="1">Đang chuẩn bị</option>
                      <option value="2">Đã giao cho tài xế</option>
                      <option value="3">Đã nhận hàng</option>
                    </select>
                 
                  </div>
                  <div class="flex items-center justify-start p-6 bg-gray-200">
                    <label for="cb_statusOrder" class="mr-2">Thông tin khách hàng:</label>
                    <button
                      className={"bg-orange-900 text-white font-bold uppercase text-sm px-3 py-2 rounded shadow hover:shadow-lg outline-none focus:outline-none ml-2 mb-1 ease-linear transition-all duration-150 "}
                      type="button"
                      onClick={handleButtonClick}
                    >
                      <TbInfoCircle className="text-2x1" />{customerDetail.customerName}
                    </button>
                    <button
                      className={"bg-orange-900 text-white font-bold uppercase text-sm px-3 py-2 rounded shadow hover:shadow-lg outline-none focus:outline-none ml-2 mb-1 ease-linear transition-all duration-150 "}
                      type="button"
                      onClick={handleButtonClick}
                    >
                      <a href={`tel:${customerDetail.customerPhone}`}>
                      <TbPhoneCall className="text-2x1" />
                        {customerDetail.customerPhone}
                      </a>

                    </button>
                    <button
                      className={"bg-orange-900 text-white font-bold uppercase text-sm px-3 py-2 rounded shadow hover:shadow-lg outline-none focus:outline-none ml-2 mb-1 ease-linear transition-all duration-150 "}
                      type="button"
                      onClick={handleButtonClick}
                    >
                      <a href={`https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(customerDetail.customerAddress)}`} target="_blank">
                      <TbAddressBook className="text-2x1" />
                      {customerDetail.customerAddress}
                      </a>
                    </button>
                  </div>
                  <div class="flex items-center justify-start p-6 bg-gray-200">
                    <label for="cb_statusOrder" class="mr-2">Thông tin tài xế:</label>
                    {isDriverAvailabel ? (
                      // Hiển thị nội dung khi isDriverAvailabel là true
                      <div>
                        {/* Thêm nội dung ở đây */}
                      </div>
                    ) : (
                      // Hiển thị nút "Yêu cầu tài xế" khi isDriverAvailabel là false
                      <button
                        className="bg-orange-900 text-white font-bold uppercase text-sm px-3 py-2 rounded shadow hover:shadow-lg outline-none focus:outline-none ml-2 mb-1 ease-linear transition-all duration-150"
                        type="button"
                        onClick={handleButtonClick}
                      >
                        Yêu cầu tài xế
                      </button>
                    )}

                    
                
                  </div>
                  {/*body*/}
                  <div className="w-full table-auto">
                    <div>
                      <Table className="w-full table-auto">
                        <Thead className="bg-orange-50">
                          <Tr>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              STT
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Hình ảnh
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Tên món
                            </Th>
                            {/* <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Loại món
                            </Th> */}
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Số lượng
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Giá 
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Ghi chú của khách
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Tổng tiền
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Hành động
                            </Th>
                          </Tr>
                        </Thead>
                        <Tbody className="bg-orange-50">
                          {isLoading ? (
                            <Td colspan="4" className="text-center">
                              <Loader />
                            </Td>
                          ) : dataDetails && dataDetails.length > 0 ? (
                            dataDetails.map((item, index) => (
                              <Tr
                                key={item.OrderLineId}
                                className="bg-orange-50"
                              >
                                <Td className="p-3 text-sm text-orange-900 whitespace-nowrap text-center">
                                  <p className="font-bold text-orange-900 hover:underline">
                                    {index + 1}
                                  </p>
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center flex justify-center">
                                  <img
                                    src={item?.UploadImage}
                                    className="w-20 h-20 max-w-[60px] rounded-full object-contain"
                                    alt=""
                                  />
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {item.FoodName}
                                </Td>
                                {/* <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {item.CategoryId}
                                </Td> */}
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {item.qty}
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {formatMoney(item.Price)}
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-red-600 whitespace-nowrap text-center">
                                  {item.Description}
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {formatMoney(item.TotalPrice)}
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                <button>
                                    <TbCircleX className="text-2xl" />
                                </button>
                                </Td>
                              </Tr>
                            ))
                          ) : (
                            ""
                          )}
                        </Tbody>
                      </Table>
                    </div>
                  </div>
                  {/*footer*/}
                  <div className="flex items-center justify-end p-6 border-t border-solid border-slate-200 rounded-b">
                    <button
                      className="bg-orange-900 text-white active:bg-emerald-600 font-bold uppercase text-sm px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                      type="button"
                      onClick={() => {
                        setIsShown(false);
                      }}
                    >
                      Đóng
                    </button>
                  </div>
                </div>
              </div>
            </div>
            <div className="opacity-25 fixed inset-0 z-40 bg-black"></div>
          </>
        ) : null}
      </div>


      {/* Title Div */}
      <div className="items-center mt-2">
        <div className="title">
          <div className="p-5 h-screen bg-white-100">
            <div className="flex justify-between pb-4 items-center">
              <h1 className="text-xl mb-2 text-orange-900 font-bold">
                Danh Sách Đơn Hàng
              </h1>         
             </div>

            <div className="overflow-auto rounded-lg shadow md:block">
              <Table className="w-full table-auto">
                <Thead className="bg-orange-50">
                  <Tr>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      STT
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Mã đơn hàng
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Khách hàng
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Phí giao hàng
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Tổng tiền
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Ngày tạo
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Thanh toán
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Xem chi tiết
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Xác nhận
                    </Th>
                  </Tr>
                </Thead>
                <Tbody className="bg-orange-50">
                  {isLoading ? (
                    <Td colspan="4" className="text-center">
                      <Loader />
                    </Td>
                  ) : data && data.length > 0 ? (
                    data.map((item, index) => (
                      <Tr key={item.OrderHeaderId} className="bg-orange-50">
                        <Td className="p-3 text-sm text-orange-900 whitespace-nowrap text-center">
                          <p className="font-bold text-orange-900 hover:underline">
                            {index + 1}
                          </p>
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {item.OrderHeaderId}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {item.CustomerName}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {formatMoney(item.TransportFee)}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {formatMoney(item.IntoMoney)}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {formatDate(item.CreationDate)}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                            <label className={item.PaymentStatusID == 2 ? 'text-success' : 'text-danger'}>
                              {item.PaymentStatusID == 2 ? "Đã thanh toán" : "COD"}
                            </label>
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-green-600 whitespace-nowrap text-center">
                          <button
                            className="p-1.5 text-xs font-medium uppercase tracking-wider text-green-800 bg-green-200 rounded-lg bg-opacity-50"
                            onClick={() => {
                              setOrderHeaderId(item.OrderHeaderId);
                              GetOrderLineDetails();
                              setCustomerDetail({
                                customerName: item.CustomerName +"("+item.RecipientName+")",
                                customerAddress: item.FormatAddress,
                                customerPhone: item.RecipientPhone
                              })
                            }}
                          >
                            <TbListDetails className="text-2xl" />
                          </button>
                        </Td>
                        {item.Status === false ? (
                        <Td className="p-3 gap-10 flex text-sm text-orange-900 whitespace-nowrap justify-center">
                        <button
                            className="p-1.5 text-xs font-medium uppercase tracking-wider text-red-800 bg-red-500 rounded-lg bg-opacity-50"
                            onClick={() => {
                              Swal.fire({
                                title: "Xác nhận đơn hàng ?",
                                text: "Bạn muốn xác nhận đơn hàng "+item.OrderHeaderId+" ?",
                                icon: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#3085d6",
                                cancelButtonColor: "#d33",
                                confirmButtonText: "Xác nhận !",
                              }).then((result) => {
                                if (result.isConfirmed) { 
                                  ApproveOrder(item).then((response) => {
                                    if (response.success) {
                                      sendOrderNotification(item);
                                      setNotification({ ...notification, 
                                        to: item.TokenApp,
                                        data: { ...notification.data,            
                                                body:`Đơn hàng ${item.OrderHeaderId} đã được xác nhận.`,
                                                action_link:`http://localhost:3001/order/${item.OrderHeaderId}`
                                              }, 
                                            notification: { ...notification.notification,            
                                              body:`Đơn hàng ${item.OrderHeaderId} đã được xác nhận.`,
                                              action_link:`http://localhost:3001/order/${item.OrderHeaderId}`
                                          } 
                                      });
                                      setNotification({ ...notification, 
                                        to: item.TokenWeb,
                                        data: { ...notification.data,            
                                                body:`Đơn hàng ${item.OrderHeaderId} đã được xác nhận.`,
                                                action_link:`http://localhost:3001/order/${item.OrderHeaderId}`
                                              }, 
                                            notification: { ...notification.notification,            
                                              body:`Đơn hàng ${item.OrderHeaderId} đã được xác nhận.`,
                                              action_link:`http://localhost:3001/order/${item.OrderHeaderId}`
                                          } 
                                          
                                      });
                                    }
                                  })
                                }
                              });
                            }}
                          >
                            <TbCircleX className="text-2xl" />
                          </button>
                        </Td>
                        ):(
                          <Td className="capitalize p-3 text-sm font-bold text-green-600 whitespace-nowrap text-center">
                          <button
                            className="p-1.5 text-xs font-medium uppercase tracking-wider text-green-800 bg-green-500 rounded-lg bg-opacity-50"
                            onClick={() => {
                              Swal.fire({
                                title: "Hủy đơn hàng ?",
                                text: "Bạn muốn hủy đơn hàng "+item.OrderHeaderId+" ?",
                                icon: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#3085d6",
                                cancelButtonColor: "#d33",
                                confirmButtonText: "Xác nhận !",
                              }).then((result) => {
                                if (result.isConfirmed) {
                                  console.log(item.TokenWeb);
                                  ApproveOrder(item).then((response) => {
                                    if (response.success) {
                                      sendOrderNotification(item);
                                      setNotificationCancle({ ...notification, 
                                        to: item.TokenApp,
                                        data: { ...notification.data,            
                                                body:`Đơn hàng ${item.OrderHeaderId} đã bị hủy.`,
                                                action_link:`http://localhost:3001/order/${item.OrderHeaderId}`
                                              }, 
                                            notification: { ...notification.notification,            
                                              body:`Đơn hàng ${item.OrderHeaderId} đã bị hủy.`,
                                              action_link:`http://localhost:3001/order/${item.OrderHeaderId}`
                                            } 
                                          });
                                      setNotificationCancle({ ...notification, 
                                        to: item.TokenWeb,
                                        data: { ...notification.data,            
                                                body:`Đơn hàng ${item.OrderHeaderId} đã bị hủy.`,
                                                action_link:`http://localhost:3001/order/${item.OrderHeaderId}`
                                              }, 
                                            notification: { ...notification.notification,            
                                              body:`Đơn hàng ${item.OrderHeaderId} đã bị hủy.`,
                                              action_link:`http://localhost:3001/order/${item.OrderHeaderId}`
                                            } 
                                          });
                                    }
                                  })
                                }
                              });
                            }}
                          >
                            <TbCheck className="text-2xl" />
                          </button>
                        </Td>
                        )
                        }
                      </Tr>
                    ))
                  ) : (
                    <Td colspan="8" className="text-center">
                      <span>Không có dữ liệu đơn hàng!</span>
                    </Td>

                  )}
                </Tbody>
              </Table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Order;
