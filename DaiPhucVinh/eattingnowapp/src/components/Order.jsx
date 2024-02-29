import React, { useState } from "react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import { useStateValue } from "../context/StateProvider";
import CartContainer from "./CartContainer";
import { RadioGroup, TextInput } from "evergreen-ui";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import SelectProvince from "./SelectProvince";
import SelectDistrict from "./SelectDistrict";
import SelectWard from "./SelectWard";
import CartItem from "./CartItem";
import { useEffect } from "react";
import { motion } from "framer-motion";
import Modal from 'react-modal';
import {
  CheckCustomer,
  CreateOrderCustomer,
  CheckCustomerAddress,
  CreateCustomerAddress,
  PaymentConfirm
} from "../api/customer/customerService";
import MyApp from "./DeliveryAddress";
import { actionType } from "../context/reducer";
import CustomrerAddressContainer from "./CustomerAddressContainer";
import { TbCircleX, TbInfoCircle} from "react-icons/tb";
import { calculateDistanceAndTimeProxy } from "../api/googleApi/googleApiDirection";
import { TakeStoreLocation } from "../api/store/storeService";
import { BiLoader } from "react-icons/bi";


import $ from 'jquery'; // Ensure you have jQuery installed
window.jQuery = $;
require('signalr'); // Ensure you have the SignalR library installed
//Kết nối đến SignalR Ordernotication
let connection = $.hubConnection('http://localhost:3002/signalr/hubs');
let proxy = connection.createHubProxy('OrderNotificationHub');


let subtitle;

const Order = () => {   
  const locations = useLocation();
  const searchParams = new URLSearchParams(locations.search);
  const vnp_Amount = searchParams.get('vnp_Amount');
  const vnp_BankCode = searchParams.get('vnp_BankCode');
  const vnp_BankTranNo = searchParams.get('vnp_BankTranNo');
  const vnp_CardType = searchParams.get('vnp_CardType');
  const vnp_OrderInfo = searchParams.get('vnp_OrderInfo');
  const vnp_PayDate = searchParams.get('vnp_PayDate');
  const vnp_ResponseCode = searchParams.get('vnp_ResponseCode');
  const vnp_TmnCode = searchParams.get('vnp_TmnCode');
  const vnp_TransactionNo = searchParams.get('vnp_TransactionNo');
  const vnp_TransactionStatus = searchParams.get('vnp_TransactionStatus');
  const vnp_TxnRef = searchParams.get('vnp_TxnRef');
  const vnp_SecureHash = searchParams.get('vnp_SecureHash');

  useEffect(() => {
    const fetchData = async () => {
      // Kiểm tra nếu tất cả dữ liệu không phải là null, "", hoặc undefined
      const allDataExists = [vnp_Amount, vnp_BankCode, vnp_BankTranNo, vnp_CardType, vnp_OrderInfo, vnp_PayDate, vnp_ResponseCode, vnp_TmnCode, vnp_TransactionNo, vnp_TransactionStatus, vnp_TxnRef, vnp_SecureHash].every(data => data != null && data !== "" && data !== undefined);
      if (allDataExists) {
        try {
          let response = await PaymentConfirm({
            vnp_Amount,
            vnp_BankCode,
            vnp_BankTranNo,
            vnp_CardType,
            vnp_OrderInfo,
            vnp_PayDate,
            vnp_ResponseCode,
            vnp_TmnCode,
            vnp_TransactionNo,
            vnp_TransactionStatus,
            vnp_TxnRef,
            vnp_SecureHash, 
            request
          });
          if (response.success) {
            toast.success('Thanh toán và đặt món ăn thành công!', { autoClose: 3000 });
            localStorage.setItem("cartItems", JSON.stringify(null));
            dispatch({
              type: actionType.SET_CARTITEMS,
              cartItems: null,
            });
            // sendOrderNotification(request.UserId);
            // Đợi 2 giây trước khi chuyển về "/"
            setTimeout(function () {
              window.location.href = "/";
            }, 2000);
          } else {
            toast.success(response.message, { autoClose: 3000 });
          }
        } catch (error) {
          // Xử lý lỗi nếu có
          console.error('Error during PaymentConfirm:', error);
          // Hiển thị thông báo lỗi cho người dùng
          toast.error('Có lỗi xảy ra trong quá trình xác nhận thanh toán!', { autoClose: 3000 });
        }
      } else {
        // Hiển thị thông báo hoặc xử lý khác nếu dữ liệu không đầy đủ
        toast.warning('Vui lòng nhập đầy đủ thông tin thanh toán!', { autoClose: 3000 });
      }
    };
  
    // Gọi hàm fetchData ngay lập tức
    fetchData();
  }, [
    vnp_Amount,
    vnp_BankCode,
    vnp_BankTranNo,
    vnp_CardType,
    vnp_OrderInfo,
    vnp_PayDate,
    vnp_ResponseCode,
    vnp_TmnCode,
    vnp_TransactionNo,
    vnp_TransactionStatus,
    vnp_TxnRef,
    vnp_SecureHash
  ]);
  
  const key = 'AIzaSyC-N1CyjegpbGvp_PO666Ie9sNQy9xW2Fo'
  const [location, setlocation] = useState({
    latitude: 0.0,
    longitude: 0.0,
    formatted_address: "",
  });
  const [storelocation , setStorelocation] = useState({
    lat: 0.0,
    lng: 0.0
  })
  const [isloadingFeeShip, setisloadingFeeShip] = useState(false);
  const [km, setKm] = useState(0);
  const customStyles = {
    content: {
      top: '50%',
      left: '50%',
      right: 'auto',
      bottom: 'auto',
      marginRight: '-50%',
      width: '80%', // Để bề rộng bằng với chiều rộng của thiết bị
      transform: 'translate(-50%, -50%)',
      maxHeight: '80vh',
      overflow: 'auto',
    },
  }; 
  const [showinfoShip, setshowinfoShip] = useState(false);
  const [tot, setTot] = useState(0);
  const [flag, setFlag] = useState(1);
  const [{ cartItems,linked }] = useStateValue();
  const navigate = useNavigate();
  const [{ cartShow, customer }, dispatch] = useStateValue();
  const [checkCustomer, setCheckCustomer] = useState(0);
  
  const [request, setRequest] = useState({
    CustomerId: customer,
    Email: "",
    CompleteName: "",
    Phone: "",
    CustomerName:"",
    PhoneCustomer:"",
    ProvinceId: 0,
    DistrictId: 0,
    WardId: 0,
    Format_Address: "",
    Name_Address:"",
    ProvinceName:"",
    DistrictName:"",
    WardName:"",
    Latitude: 0.0,
    Longitude: 0.0,
    Payment: "PaymentOnDelivery",
    TotalAmt: 0,
    TransportFee: 0,
    IntoMoney: 0,
    UserId: 0,
    TokenWeb: "",
    TokenApp:"",
    OrderLine: cartItems,
    AddressId :0,
    Defaut : true,  
    RecipientName: "",
    RecipientPhone: "",
  });
  const [data, setData] = useState([]);

  const [modalIsOpen, setIsOpen] = React.useState(false);
  function openModal() {
    setIsOpen(true);
  }
  function afterOpenModal() {
    // references are now sync'd and can be accessed.
    subtitle.style.color = '#f00';
  }
  function closeModal() {
    setRequestAddress({
      Province: "",
      District : "",
      Ward: "",
    })
    setlocation({
      latitude: 0.0,
      longitude: 0.0,
      formatted_address: "",
    })
    setIsOpen(false);
    checkCustomerAddress()
  }
  function closeModalInShip() {
    setshowinfoShip(false);
  }
  const [selectedAddress, setSelectedAddress] = useState({});
  const [CompleteAddress, setCompleteAddress] = useState(false);

  // Hàm callback để nhận dữ liệu từ component con
  const handleSelectedAddressChange = (selected) => {
    setSelectedAddress(selected);
    setRequest({
      ...request,
      Name_Address: selected.name,
      Latitude: selected.lat,
      Longitude: selected.lng,
      Format_Address: selected.formatted_address
    })
  };
  const [requestAddress, setRequestAddress] = useState({
    Province: "",
    District: "",
    Ward: "",
  });

  function calculateDeliveryCost(km) {
    let delivery = 12000; // Initialize the variable
    km = (km / 1000).toFixed(2) // Chia thành đơn vị Km
    if (km > 2) {
      delivery = (km * 10000) + 2000;
    }
    return delivery; // Return the calculated delivery cost
  }

  async function onChangeCustomer() {
    // Kiểm tra xem khách hàng đã đăng ký tài khoản chưa
    let checkCustomer = await CheckCustomer(request);
    if (checkCustomer.success) {
      console.log(checkCustomer);
      setCheckCustomer(checkCustomer.dataCount);
    }
  }
  function roundToNearestHundred(amount) {
    const roundedAmount = Math.round(amount / 100) * 100;
    return roundedAmount;
  }
  
  async function checkCustomerAddress() {
    let destination ;
    setisloadingFeeShip(true);
    let checklocation = await TakeStoreLocation({
      UserId: cartItems[0].UserId
    });
    if (checklocation.success) {
      destination = [checklocation.data[0].Latitude, checklocation.data[0].Longitude];
    }
    let checkCustomer = await CheckCustomerAddress(request);
    if (checkCustomer.success) {
      console.log(checkCustomer.data);
      // Nếu đã có địa chỉ mặc định rồi thì sử dụng địa chỉ đó
      setCompleteAddress(checkCustomer.success);
      // Dữ liệu của địa chỉ mặc định
      setData(checkCustomer.data);
      // Set dữ liệu vị trí đặt món ăn
      setRequest({
        ...request,
        RecipientName: checkCustomer.data[0].CustomerName,
        RecipientPhone: checkCustomer.data[0].PhoneCustomer,
        Name_Address: checkCustomer.data[0].Name_Address,
        Format_Address: checkCustomer.data[0].Format_Address,
        Latitude:checkCustomer.data[0].Latitude,
        Longitude: checkCustomer.data[0].Longitude,
        AddressId: checkCustomer.data[0].AddressId,
      });
      let origin = [checkCustomer.data[0].Latitude, checkCustomer.data[0].Longitude];
        // Gọi hàm calculateDistanceAndTime
      await calculateDistanceAndTimeProxy(origin, destination)
          .then(result => {
            if (result) {
              const { distance } = result;
              setKm(distance);
              setisloadingFeeShip(false);
            } else {
              console.log('Không thể tính khoảng cách và thời gian.');
            }
          })
          .catch(error => {
            console.error('Lỗi khi gọi hàm calculateDistanceAndTime:', error);
          });
    }

  }

  // Gửi 
  function sendOrderNotification(UserId) {
    connection.start()
      .done(() => {
        console.log('Kết nối thành công SignalR');
        // Đăng ký người dùng khi kết nối thành công
        if (customer !== null) {
          proxy
          .invoke('SendOrderNotificationToUser', 'Thông báo mới', UserId)
          .done(() => {
            console.log('Gửi thông báo thành công');
          })
          .fail((error) => {
            console.error('Lỗi không gửi được thông báo:', error);
          });
        }
      })
      .fail((error) => {
        console.error('Could not connect:', error);
      });

  }
  // Thanh toán
  async function order() {
    if(cartItems != null){
      let response = await CreateOrderCustomer(request);
      console.log(response);
      if (response.success) {
        if (response.message == "") {
          toast.success('Đặt món ăn thành công!', { autoClose: 3000 });
          localStorage.setItem("cartItems", JSON.stringify(null));
          dispatch({  
            type: actionType.SET_CARTITEMS,
            cartItems: null,
          });
          sendOrderNotification(request.UserId);
          // Đợi 2 giây trước khi chuyển về "/"
          setTimeout(function() {
            window.location.href = "/";
          }, 1000);
        }
        else{
          window.location.href = response.message;
        }
      } else {
        if(response.message =="No_order")
          toast.error('Vui lòng điền đầy đủ thông tin!', { autoClose: 3000 });
        else if(response.message =="Not_Payment")
          toast.warning('Cửa hàng hiện chưa sử dụng thanh toán Online ... !', { autoClose: 3000 });
        else
        {
          if(response.customdata != null){
              toast.warning(response.message, { autoClose: 3000 });
          }
        }
      }
    }
    else{

    }
   
  }

  // Hàm kiểm tra số điện thoại
  const isValidPhoneNumber = (phoneNumber) => {
    // Định dạng số điện thoại Việt Nam: 10 chữ số, bắt đầu bằng 0 hoặc +84
    const phoneNumberPattern = /^(0|\+84)[0-9]{9}$/;
    return phoneNumberPattern.test(phoneNumber);
  };

  //Lưu địa điểm đầu tiên của người dùng 
  async function gotoOrder() {
    const isDataValid = () => {
      // Kiểm tra các trường dữ liệu
      if (
        request.CustomerName === "" ||
        request.ProvinceId === 0 ||
        request.DistrictId === 0 ||
        request.WardId === 0 ||
        request.PhoneCustomer === "" ||
        request.Format_Address === "" ||
        request.Longitude === 0.0 ||
        request.Latitude === 0.0 
      ) {
        return false; // Trường dữ liệu nào đó còn trống
      }
      return true; // Tất cả dữ liệu hợp lệ
    };
    if (!isValidPhoneNumber(request.PhoneCustomer)) {
      toast.warning('Vui lòng điền đúng số điện thoại !', { autoClose: 3000 });
      return; // Số điện thoại không hợp lệ
    }
    if (isDataValid()) {
      let repsponse  = await CreateCustomerAddress(request)
      if(!repsponse.success)
      {
        toast.error('Lưu không thành công! Vui lòng thử lại', { autoClose: 3000 });
      }
      else{
        toast.success('Lưu đia chỉ thành công!', { autoClose: 3000 });
        setCompleteAddress(true);
        setRequestAddress({
          Province: "",
          District : "",
          Ward: "",
        })
        setlocation({
          latitude: 0.0,
          longitude: 0.0,
          formatted_address: "",
        })
        setIsOpen(false);
        checkCustomerAddress();
        onChangeCustomer();
      }
    } else {
      toast.error('Vui lòng điền đầy đủ và đúng thông tin!', { autoClose: 3000 });
    }
  }

  const [options] = React.useState([
    { label: "Thanh toán khi nhận hàng", value: "PaymentOnDelivery" },
    { label: "Thanh toán ví Momo", value: "MOMO" },
    { label: "Thanh toán VNPay", value: "VNPay" },
  ]);
  const [value, setValue] = React.useState("PaymentOnDelivery");
  function changeValue(e){
    console.log(e.target.value);
    setValue(e.target.value);
  }
  useEffect(() => {
    let totalPrice = cartItems.reduce(function (accumulator, item) {
      return accumulator + item.qty * item.Price;
    }, 0);
    setTot(totalPrice);
    let transportFee = roundToNearestHundred(calculateDeliveryCost(km));
    let intoMoney =  roundToNearestHundred(calculateDeliveryCost(km)) + totalPrice;
    setRequest({
      ...request,
      CustomerId: customer,
      TotalAmt: totalPrice,
      TransportFee: transportFee,
      IntoMoney :  intoMoney,
      UserId: cartItems[0].UserId,
      Payment: value,
      OrderLine: cartItems,
    });
    console.log(request);
  }, [tot, flag, customer, value, km]);
  useEffect(() =>{
    onChangeCustomer();
    checkCustomerAddress();
  },[]);

  function calculateDeliveryFee(distance) {
    if (distance <= 2) {
      return "Phí giao hàng cơ bản là 12.000 nếu <= 2 Km";
    } else {
      return "Phí giao hàng cơ bản là (Km * 10.000đ) + 2.000 nếu > 2 Km";
    }
  }
  useEffect(() => {
    // Attempt connection and handle connection and error events
    connection.start()
      .done(() => {
        console.log('Kết nối thành công SignalR');
        // Đăng ký người dùng khi kết nối thành công
        if (customer !== null) {
          proxy.invoke('SetCustomerId', customer)
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



  return (
    <section className="bg-min-h-screen flex items-center justify-center">
          {/*Hiển thị thông báo */}
          <ToastContainer />
          {/*Modal xem thông tin phí vận chuyển*/}
          <Modal
                isOpen={showinfoShip}
                onAfterOpen={afterOpenModal}
                onRequestClose={closeModalInShip}
                style={customStyles}
                contentLabel="Form"
                shouldCloseOnOverlayClick={false} // Đặt giá trị này thành false để ngăn Modal đóng khi nhấn ở ngoài
              >
                  
                  <p>- Khoảng cách từ cửa hàng đến vị trí nhận hàng của bạn: {(km / 1000).toFixed(2)} Km.</p>
                  <p>- {calculateDeliveryFee((km / 1000).toFixed(2))}.</p>
                  <button
                    type="button"
                    className="px-2 py-1 bg-red-500 text-white rounded-md hover:bg-red-600 mt-3"
                    onClick={closeModalInShip}
                    style={{
                      left: '0 px', // Điều chỉnh khoảng cách từ phải sang
                    }}
                  >
                  Đóng
                  </button>
              </Modal>
          {/*Modal cập nhật/thêm mới đia chỉ khách hàng*/}
          <Modal
                isOpen={modalIsOpen}
                onAfterOpen={afterOpenModal}
                onRequestClose={closeModal}
                style={customStyles}
                contentLabel="Form"
                shouldCloseOnOverlayClick={false} // Đặt giá trị này thành false để ngăn Modal đóng khi nhấn ở ngoài
              >
                    <button
                    type="submit"
                    className="px-2 py-1 bg-red-500 text-white rounded-md hover:bg-red-600 mt-3"
                    onClick={closeModal}
                    style={{
                      position: 'absolute',
                      top: '10px', // Điều chỉnh khoảng cách từ trên xuống
                      right: '10px', // Điều chỉnh khoảng cách từ phải sang
                    }}
                  >
                  <TbCircleX className="text-2xl" />
                  </button>
                <CustomrerAddressContainer/>
                <div style={{ position: 'relative' }}>
                </div>
          </Modal>
          {/* Nếu lần đầu đặt hàng */}
          <div className="bg-green-200 100vh flex rounded-2xl shadow-lg max-w-4xl p-5 gap-6 items-start text-center">
              {/*Điền thông tin nếu lần đầu mua hàng */}
              {CompleteAddress === false ? (
                <div className="px-6">
                <h2 className="font-bold text-2xl text-[#171a1f] text-center capitalize">
                  Nhập địa chỉ nhận hàng
                </h2>
                <div className="w-100 max-w-lg mx-auto py-5 flex flex-col gap-3 overflow-y-scroll scrollbar-none">
                <div className="w-full">
                  <label htmlFor="completeName" className="block text-lg font-medium text-gray-700 mb-2">Họ và tên</label>
                  <TextInput
                    id="completeName"
                    className="p-2 rounded-lg border w-full focus:ring focus:ring-blue-500"
                    type="text"
                    placeholder="Nhập tên người nhận . . ."
                    onChange={(e) => {
                      setRequest({
                        ...request,
                        CustomerName: e.target.value,
                        RecipientName: e.target.value,
                      });
                    }}
                  />
                </div>
                <div className="w-full">
                  <label htmlFor="phone" className="block text-lg font-medium text-gray-700 mb-2">Số điện thoại</label>
                  <TextInput
                    id="phone"
                    className="p-2 rounded-lg border w-full focus:ring focus:ring-blue-500"
                    type="text"
                    placeholder="Nhập số điện thoại người nhận . . ."
                    onChange={(e) => {
                      setRequest({
                        ...request,
                        RecipientPhone: e.target.value,
                        PhoneCustomer: e.target.value,
                      });
                    }}
                  />
                </div>
                <div className="w-full">
                  <SelectProvince
                    id="province"
                    marginTop={16}
                    selected={{
                      value: request.ProvinceId,
                      label: request.ProvinceName,
                    }}
                    onSelect={(e) => {
                      setRequest({
                        ...request,
                        ProvinceId: e.value,
                        ProvinceName: e.label,
                      });
                      setRequestAddress(
                        {
                          Province: e.label
                        }
                      )
                    }}
                  />
                </div>
                <div className="w-full">
                  <SelectDistrict
                    id="district"
                    marginTop={16}
                    selected={{
                      value: request.DistrictId,
                      label: request.DistrictName,
                    }}
                    onSelect={(e) => {
                      setRequest({
                        ...request,
                        DistrictId: e.value,
                        DistrictName: e.label,
                      });
                      setRequestAddress(
                        {
                          District: e.label
                        }
                      )
                    }}
                  />
                </div>
                <div className="w-full">
                  <SelectWard
                    id="ward"
                    marginTop={16}
                    selected={{
                      value: request.WardId,
                      label: request.WardName,
                    }}
                    onSelect={(e) => {
                      setRequest({
                        ...request,
                        WardId: e.value,
                        WardName: e.label,
                      });
                      setRequestAddress(
                        {
                          Ward: e.label
                        }
                      )
                    }}
                  />
                </div>
                {
                  requestAddress.Province !== "" && requestAddress.District !== "" && requestAddress.Ward !== "" && (
                    <div className="w-full">
                      <MyApp requestAddress={requestAddress} onSelectedAddressChange={handleSelectedAddressChange} location={null}/>
                    </div>
                  )
                }
              </div>
              <div className="w-full text-center">
                <motion.button
                  whileTap={{ scale: 0.95 }}
                  className="bg-blue-600 text-white py-2 rounded-lg w-64 mt-8"
                  onClick={gotoOrder}
                >
                  Đi đến đặt hàng
                </motion.button>
              </div>
              </div>
              ):
              (
                <div>
                  {/* Đã đặt hàng và lưu địa chỉ vào CSDL */}
              {CompleteAddress && ( 
          <div className="px-6">
            <div className="container">
              <div className="marker-container">
                <div className="marker-icon"></div>
                    <h2 className="font-italic text-1xl text-[#171a1f] text-start">
                      Giao hàng đến:
                    </h2>
                  </div>
                  <div >
                    <div className="col-md-1 ">
                      <div className="address-info">
                        {data !== null ? (
                          <div>
                            {data.map((item, index) => (
                              <div key={index}>
                                <p className="font-italic text-1xl text-[#171a1f] text-start capitalize"><strong>Tên:</strong> {item.CustomerName}</p>
                                <p className="font-italic text-1xl text-[#171a1f] text-start capitalize"><strong>Số điện thoại:</strong> {item.PhoneCustomer}</p>
                                <p className="font-italic text-1xl text-[#171a1f] text-start capitalize"><strong>Địa chỉ:</strong>{item.Name_Address} | {item.Format_Address}</p>
                                <button className="custom-button"  onClick={openModal}>
                                  Thay đổi
                                </button>
                              </div>
                            ))}

                          </div>
                        ) :(
                          <div>
                              <div >
                                <p className="font-italic text-1xl text-[#171a1f] text-start capitalize"><strong>Tên:</strong> {request.RecipientName}</p>
                                <p className="font-italic text-1xl text-[#171a1f] text-start capitalize"><strong>Số điện thoại:</strong> {request.RecipientPhone}</p>
                                <p className="font-italic text-1xl text-[#171a1f] text-start capitalize"><strong>Địa chỉ:</strong>{request.Name_Address} | {request.Format_Address}</p>
                                <button className="custom-button"  onClick={CompleteAddress = !CompleteAddress}>
                                  Thay đổi
                                </button>
                              </div>
                          </div>
                        ) }
                      </div>
                    </div>
                  </div>

              </div>
            <div className="flex-center-y">
              <h2 className="font-bold text-2xl text-[#171a1f] text-center mt-2">
                Danh sách đã chọn
              </h2>
              <div className="w-340 h-340 md:h-42 py-5 flex flex-col gap-3 overflow-y-scroll scrollbar-none">
                {/* cart item */}
                {cartItems &&
                  cartItems.map((item) => (
                    <CartItem
                      key={item?.FoodListId}
                      item={item}
                      setFlag={setFlag}
                      flag={flag}
                    />
                  ))}
              </div>
            </div>
            <div className="flex-center-y">
            <div className="w-340 flex-1 rounded-t-[2rem] flex flex-col items-center justify-evenly px-8 py-2">
              <div className="w-full flex items-center justify-between">
                <p className="text-gray-600 text-base font-semibold">Tổng tiền</p>
                <p className="text-textColor text-base font-semibold">
                  {tot.toLocaleString()}
                  <span className="text-base text-red-500"> vnđ</span>
                </p>
              </div>
              <div className="w-full flex items-center justify-between">
                          <p className="text-gray-600 text-base font-semibold">
                            Phí vận chuyển 
                          </p>
                          {
                            !isloadingFeeShip ?(
                              <TbInfoCircle onClick={()=>{setshowinfoShip(!showinfoShip)}}></TbInfoCircle>
                            ):(
                              null
                            )
                          }
                          <p className="text-textColor text-base font-semibold">
                            {roundToNearestHundred(calculateDeliveryCost(km)).toLocaleString()}
                            <span className="text-base text-red-500"> vnđ</span>
                          </p>
                        </div>
          
                        <div className="w-full border-b border-gray-600 my-2"></div>
        
                        <div className="w-full flex items-center justify-between">
                          <p className="text-gray-600 text-lg font-semibold">Thanh toán</p>
                          <p className="text-textColor text-lg font-semibold">
                              {(tot + roundToNearestHundred(calculateDeliveryCost(km))).toLocaleString()}
                            <span className="text-base text-red-500"> vnđ</span>
                          </p>
                        </div>


             
              {checkCustomer > 0  && cartItems != null && (
                <>
                  <RadioGroup
                    label="Phương thức thanh toán"
                    size={16}
                    value={value}
                    options={options}
                    onChange={(event) => 
                      changeValue(event)
                    }
                  />
                  {
                    !isloadingFeeShip ?(
                      <motion.button
                        whileTap={{ scale: 0.75 }}
                        className="bg-[#171a1f] rounded-xl text-white py-2 duration-300 w-64 p-2 mt-8"
                        onClick={order}
                      >
                        Đặt hàng
                      </motion.button>    
                    ):(
                      <BiLoader></BiLoader>
                    )
                  }
                  
                </>
              )}
            </div>
            </div>
          
          </div>
        )}
      </div>

              )
            }
            
          </div>
      {cartShow && <CartContainer />}
    </section>
  );
};

export default Order;
