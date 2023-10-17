import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
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
  TakeAllCustomerAddressById,
  DeleteAddress
} from "../api/customer/customerService";
import MyApp from "./DeliveryAddress";
import { actionType } from "../context/reducer";
import CustomrerAddressContainer from "./CustomerAddressContainer";
import { TbCircleX} from "react-icons/tb";

let subtitle;
let delivery = 15000;
const Order = () => {
  const key = 'AIzaSyC-N1CyjegpbGvp_PO666Ie9sNQy9xW2Fo'
  const [location, setlocation] = useState({
    latitude: 0.0,
    longitude: 0.0,
    formatted_address: "",
  });

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
    Payment: "",
    TotalAmt: 0,
    TransportFee: 0,
    IntoMoney: 0,
    UserId: 0,
    TokenWeb: "",
    TokenApp:"",
    OrderLine: {},
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

  async function onChangeCustomer() {
    // Kiểm tra xem khách hàng đã đăng ký tài khoản chưa
    let checkCustomer = await CheckCustomer(request);
    if (checkCustomer.success) {
      setCheckCustomer(checkCustomer.dataCount);
    }
  }

  
  async function checkCustomerAddress() {
    let checkCustomer = await CheckCustomerAddress(request);
    if (checkCustomer.success) {
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
    }
    else{

    }
  }
  
  async function order() {
    let response = await CreateOrderCustomer(request);
    if (response.success) {
      if (response.message == "") {
        toast.success('Đặt món ăn thành công!', { autoClose: 3000 });
        localStorage.setItem("cartItems", JSON.stringify(null));
        dispatch({  
          type: actionType.SET_CARTITEMS,
          cartItems: null,
        });
        // Đợi 2 giây trước khi chuyển về "/"
        setTimeout(function() {
          window.location.href = "/";
        }, 1000);
      }
      else{
        window.location.href = response.message;
      }
    } else {
      toast.error('Vui lòng điền đầy đủ thông tin!', { autoClose: 3000 });
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
  ]);
  const [value, setValue] = React.useState("PaymentOnDelivery");
  function changeValue(e){
    setValue(e.target.value);
    setRequest({
      ...request,
      Payment: e.target.value
    }
    )
  }

  useEffect(() => {
    console.log("Tính tiền");
    let totalPrice = cartItems.reduce(function (accumulator, item) {
      return accumulator + item.qty * item.Price;
    }, 0);
    setTot(totalPrice);
    setRequest({
      ...request,
      CustomerId: customer,
      TotalAmt: totalPrice,
      TransportFee: delivery,
      IntoMoney: delivery + totalPrice,
      UserId: cartItems[0].UserId,
      Payment: value,
      OrderLine: cartItems,
    });
    console.log(request);
    checkCustomerAddress()
  }, [tot, flag, customer, value]);

  useEffect(() =>{
    onChangeCustomer();
  },[])


  return (
    <section className="bg-min-h-screen flex items-center justify-center">
          <ToastContainer />
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
              {CompleteAddress && ( 
          <div className="px-6">
            <div className="container">
              <div className="marker-container">
                <div className="marker-icon"></div>
                    <h2 className="font-italic text-1xl text-[#171a1f] text-start capitalize">
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
                        ) : null}
                      </div>
                    </div>
                  </div>

              </div>
            <div className="flex-center-y">
              <h2 className="font-bold text-2xl text-[#171a1f] text-center capitalize">
                Danh sách món ăn
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
                <p className="text-gray-400 text-base font-semibold">Tổng tiền</p>
                <p className="text-textColor text-base font-semibold">
                  {tot.toLocaleString()}
                  <span className="text-base text-red-500"> vnđ</span>
                </p>
              </div>

              <div className="w-full flex items-center justify-between">
                <p className="text-gray-400 text-base font-semibold">
                  Phí vận chuyển
                </p>
                <p className="text-textColor text-base font-semibold">
                  {delivery.toLocaleString()}
                  <span className="text-base text-red-500"> vnđ</span>
                </p>
              </div>

              <div className="w-full border-b border-gray-600 my-2"></div>

              <div className="w-full flex items-center justify-between">
                <p className="text-textColor text-lg font-semibold">Thanh toán</p>
                <p className="text-textColor text-lg font-semibold">
                  {(tot + delivery).toLocaleString()}
                  <span className="text-base text-red-500"> vnđ</span>
                </p>
              </div>
              {checkCustomer > 0 && (
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
                  <motion.button
                    whileTap={{ scale: 0.75 }}
                    className="bg-[#171a1f] rounded-xl text-white py-2 duration-300 w-64 p-2 mt-8"
                    onClick={order}
                  >
                    Đặt hàng
                  </motion.button>
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
