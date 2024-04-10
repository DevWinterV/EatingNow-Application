//import TimePicker from '@ashwinthomas/react-time-picker-dropdown';
import TimePicker from 'react-times';
import Swal from "sweetalert2";
import 'react-times/css/material/default.css';
// or you can use classic theme
import 'react-times/css/classic/default.css';
import { TakeStoreInfoByStoreManager, UpdateStoreInfoByStoreManager } from "../../api/store/storeService";
import { useStateValue } from "../../context/StateProvider.js";
import React, { useEffect, useState } from "react";
import { actionType } from '../../context/reducer.js';
const StoreSettings = () => {
  const [selectedImage, setSelectedImage] = React.useState();
  const [selectedImageURL, setselectedImageURL] = React.useState();
  const [{ cartShow, user, linked }, dispatch] = useStateValue();
    //Requets Store
    const [request, setRequest] = React.useState({
      UserId: 0,
      AbsoluteImage: "",
      FullName: "",
      Description: "",
      OpenTime: "",
      ProvinceId: "",
      CuisineId: "",
      Email: "",
      Address: "",
      OwnerName: "",
      Phone: "",
      Latitude: "",
      Longitude: "",
      Status: 1,
      TimeOpen: "06:00:00",
      TimeClose: "12:00:00"
    });

    async function onViewAppearing(){
      let response = await TakeStoreInfoByStoreManager(user?.UserId);
      if(response.success){
        setRequest({
          UserId: response.item.UserId,
          AbsoluteImage:  response.item.AbsoluteImage ?? "",
          FullName: response.item.FullName ?? "",
          Description: response.item.Description ?? "",
          OpenTime:  response.item.OpenTime ?? "",
          Email: response.item.Email ?? "",
          Address: response.item.Address ?? "",
          OwnerName: response.item.OwnerName ?? "",
          Phone: response.item.Phone ?? "",
          TimeOpen:  response.item.TimeOpen ?? "06:00:00",
          TimeClose: response.item.TimeClose ?? "24:00:00",
        });
        setselectedImageURL(response.item.AbsoluteImage);
      }
    }
    

    async function onSaveChange(){
      if(CheckData(request.TimeOpen, request.TimeClose) === false)
        return;
      let data = new FormData();
      if (selectedImage !== undefined) {
        data.append("file[]", selectedImage, selectedImage.name);
      }
      data.append("form", JSON.stringify(request));
      let UpdateResponse = await UpdateStoreInfoByStoreManager(data);
      if(UpdateResponse.success){
        Swal.fire({
          title: "Thành công!",
          text: "Cập nhật dữ liệu thành công",
          icon: "success",
          confirmButtonText: "OK",
        });
        var userStorage = JSON.parse(localStorage.getItem("user"));
        userStorage.StoreName = request.FullName ?? "";
        userStorage.Image = request.AbsoluteImage ?? "";
        localStorage.setItem("user", JSON.stringify(userStorage));
        dispatch({
          type: actionType.SET_USER,
          user: userStorage,
        });
        onViewAppearing();
      }
    }

    
    const handlechangeTimeOpen = (e) => {
      setRequest({
        ...request,
        TimeOpen: `${e.hour}:${e.minute}:${e.meridiem??"00"}`
      });
    }
    const handlechangeTimeClose = (e) => {
      setRequest({
        ...request,
        TimeClose: `${e.hour}:${e.minute}:${e.meridiem??"00"}`
      })
    }

    useEffect(() => {
      onViewAppearing();
    }, [user]);

    function onChange(e) {
      setSelectedImage(e.target.files[0]);
      setRequest({
        ...request,
        AbsoluteImage: e.target.files[0].name,
      });
      const file = e.target.files[0]; // Lấy tệp hình ảnh đầu tiên từ sự kiện
      if (file) {
        setselectedImageURL(URL.createObjectURL(file))
        console.log(selectedImageURL);
      }
    }

    
  function CheckData(openingTime, closingTime) {
    if(request.Phone === "" || request.Phone.length > 11 || request.Phone.length < 10 ){
      Swal.fire({
        title: "Lỗi!",
        text: "Số điện thoại không hợp lệ",
        icon: "error",
        confirmButtonText: "OK",
      });
      // Giờ mở cửa không nhỏ hơn giờ đóng cửa
      return false;
    }
    if(request.Phone === "" || request.OwnerName.length < 2  ){
      Swal.fire({
        title: "Lỗi!",
        text: "Tên người đại diện không hợp lệ",
        icon: "error",
        confirmButtonText: "OK",
      });
      // Giờ mở cửa không nhỏ hơn giờ đóng cửa
      return false;
    }
    if(request.FullName === ""){
      Swal.fire({
        title: "Lỗi!",
        text: "Tên cửa hàng không hợp lệ",
        icon: "error",
        confirmButtonText: "OK",
      });
      // Giờ mở cửa không nhỏ hơn giờ đóng cửa
      return false;
    }
    if(openingTime == "" || openingTime == null){
      Swal.fire({
        title: "Lỗi!",
        text: "Vui lòng chọn giờ mở cửa",
        icon: "error",
        confirmButtonText: "OK",
      });
      // Giờ mở cửa không nhỏ hơn giờ đóng cửa
      return false;
    }
    if(closingTime == "" || closingTime == null){
      Swal.fire({
        title: "Lỗi!",
        text: "Vui lòng chọn giờ đóng cửa",
        icon: "error",
        confirmButtonText: "OK",
      });
      // Giờ mở cửa không nhỏ hơn giờ đóng cửa
      return false;
    }
    // Chuyển đổi các giờ từ chuỗi sang số nguyên để so sánh
    const openingHour = parseInt(openingTime.split(':')[0]);
    const openingMinute = parseInt(openingTime.split(':')[1]);
    const closingHour = parseInt(closingTime.split(':')[0]);
    const closingMinute = parseInt(closingTime.split(':')[1]);
    // So sánh giờ và phút
    if (openingHour < closingHour || (openingHour === closingHour && openingMinute < closingMinute)) {
      // Giờ mở cửa nhỏ hơn giờ đóng cửa
      return true;
    } else {
      Swal.fire({
        title: "Lỗi!",
        text: "Giờ mở cửa phải nhỏ hơn giờ đóng cửa",
        icon: "error",
        confirmButtonText: "OK",
      });
      // Giờ mở cửa không nhỏ hơn giờ đóng cửa
      return false;
    }
  }
  const [activeTab, setActiveTab] = useState("UpdattInfoStore");
  const handleTabClick = (tab) => {
    setActiveTab(tab);

  };

  return (
    <div className="bg-white h-[100%] basis-80 p-8 overflow-auto no-scrollbar py-5 px-5">
      <nav className="navstore">
        <ul>
        <li className={activeTab === "UpdattInfoStore" ? 'active-tabStore' : ''}>
          <button onClick={() => handleTabClick("UpdattInfoStore")}>
            Thông tin cửa hàng
          </button>
        </li>
        <li className={activeTab === "UpdattInfoPayment" ? 'active-tabStore' : ''}>
          <button onClick={() => handleTabClick("UpdattInfoPayment")}>
              Thông tin cổng thanh toán VNPay
          </button>
        </li>
        <li className={activeTab === "updatePassWord" ? 'active-tabStore' : ''}>
          <button onClick={() => handleTabClick("updatePassWord")}>
              Cập nhật mật khẩu
          </button>
        </li>
        </ul>
      </nav>

      <div>
        {
          activeTab === "UpdattInfoStore" ? (
            <UpdateInfoCustomerComponent />
          ) : activeTab === "UpdattInfoPayment" ? (
            <UpdateInfoPayMent />
          ) : activeTab === "updatePassWord" ? (
            <UpdatePassWord />
          ) : null
        }
      </div>  
   </div>

  );
  function UpdateInfoCustomerComponent(){
    return ( 
    <div className="">
    {/* Title */}
      <div className="mt-8">
        <h1 className="text-3xl font-semibold text-gray-800">Cập nhật thông tin cửa hàng</h1>
      </div>
      {/* Store Image */}
      <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-6">
        {/* First Column */}
        <div>
          <label className='ml-1' htmlFor="">Chọn ảnh:</label>
              <input
                  type="file"
                  className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
                  placeholder="Chọn file ảnh"
                  //defaultValue={request.AbsoluteImage}
                  onChange={onChange}
                  style={{ fontSize: "20px" }}
                />
          </div>
        <div>
          <label className='ml-1' htmlFor="">Hình ảnh:</label>
            {request.AbsoluteImage ? (
                <div>
                  {selectedImageURL && (
                          <img
                            src={selectedImageURL}
                            alt="Hình ảnh đã chọn"
                            style={{ maxWidth: "300px", maxHeight: "300px" }}
                          />
                        )}              
                  </div>
              ) : (
                <div>Không có hình ảnh</div>
              )}
        </div>

      </div>
      {/* Store Information Form */}
      <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-6">
        {/* First Column */}
        <div>
          <label className='ml-1' htmlFor="">Tên cửa hàng:</label>
          <input
            type="text"
            placeholder="Tên cửa hàng"
            value={request.FullName}
            onChange={(e)=>{
              setRequest({
                ...request,
                FullName: e.target.value
              })
            }}
            className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
          />
        </div>
        <div>
        <label className='ml-1' htmlFor="">Số điện thoại:</label>
          <input
            type="text"
            placeholder="Số điện thoại"
            value={request.Phone}
            onChange={(e)=>{
              setRequest({
                ...request,
                Phone: e.target.value
              })
            }}
            className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
          />
        </div>
        <div>
          <label className='ml-1' htmlFor="">Email:</label>
          <input
            type="email"
            disabled
            placeholder="Địa chỉ Email"
            value={request.Email}
            onChange={(e)=>{
              setRequest({
                ...request,
                Email: e.target.value
              })
            }}
            className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
          />
        </div>
        {/* Second Column */}
        <div>
          <label className='ml-1' htmlFor="">Tên người đại diện:</label>
          <input
            type="text"
            placeholder="Tên người đại diện"
            value={request.OwnerName}
            onChange={(e)=>{
              setRequest({
                ...request,
                OwnerName: e.target.value
              })
            }}
            className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
          />
        </div>
      </div>
      {/* Store Information Form */}
      <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-6">
        {/* First Column */}
        <div>
          <label className='ml-1' htmlFor="">Mô tả chi tiết cửa hàng:</label>
          <input
            type="text"
            placeholder="Mô tả chi tiết cửa hàng"
            value={request.Description}
            onChange={(e)=>{
              setRequest({
                ...request,
                Description: e.target.value
              })
            }}
            className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
          />
        </div>
        <div>
          <label className='ml-1' htmlFor="">Địa chỉ chi tiết:</label>
          <input
            type="text"
            disabled
            placeholder="Địa chỉ chi tiết cửa hàng"
            value={request.Address}
            onChange={(e)=>{
              setRequest({
                ...request,
                Address: e.target.value
              })
            }}
            className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
          />
        </div>
      </div>
      <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-2">
        <div>
          <label className='ml-1' htmlFor="">Giờ mở cửa:</label>
          <TimePicker
                  theme="classic"
                  onTimeChange={handlechangeTimeOpen}
                  time={request.TimeOpen ?? "00:00"}
                  timeConfig={{
                    minTime: "00:00",
                    maxTime: "23:59",
                    step: 10, // Bước nhảy cho giờ và phút
                    showSecond: false, // Hiển thị giây hoặc không
                    use12Hours: false, // Sử dụng định dạng 12 giờ hoặc không
                    format: 'HH:mm' // Định dạng hiển thị cho giờ và phút
                  }}
                  />
        </div>
        <div>
          <label className='ml-1' htmlFor="">Giờ đóng cửa:</label>
          <TimePicker
                    theme="classic"
                    onTimeChange={handlechangeTimeClose}
                    time={request.TimeClose ?? "23:59"}
                    timeConfig={{
                      minTime: "00:00",
                      maxTime: "23:59",
                      step: 10, // Bước nhảy cho giờ và phút
                      showSecond: false, // Hiển thị giây hoặc không
                      use12Hours: false, // Sử dụng định dạng 12 giờ hoặc không
                      format: 'HH:mm' // Định dạng hiển thị cho giờ và phút
                    }}
                  />  
        </div>
      </div>
      {/* Save Button */}
      <button
        onClick={onSaveChange}
        className="bg-blue-500 text-white font-semibold py-2 px-6 rounded-md mt-6 hover:bg-blue-600 transition duration-300 ease-in-out">
        Lưu cập nhật
      </button>
    </div>
    )
  }
  function UpdateInfoPayMent(){
    return (
      <div>
        {/* Title */}
        <div className="mt-8">
          <h1 className="text-3xl font-semibold text-gray-800">Cập nhật thông tin thanh toán VNPay</h1>
          <h6 className="text-1xl font-semibold text-red-800">Vui lòng nhập chính xác mã Terminal ID / Mã Website (vnp_TmnCode) và Secret Key / Chuỗi bí mật tạo checksum (vnp_HashSecret) do VNPay cung cấp</h6>
                  {/* Store Information Form */}
        <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-6">
          {/* First Column */}
          <div>
            <label className='ml-1' htmlFor="">Mã Terminal ID / Mã Website:</label>
            <input
              type="text"
              placeholder="Nhật mã vnp_TmnCode"
              // value={request.Description}
              // onChange={(e)=>{
              //   setRequest({
              //     ...request,
              //     Description: e.target.value
              //   })
              // }}
              className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
            />
          </div>
          <div>
            <label className='ml-1' htmlFor="">Secret Key / Chuỗi bí mật tạo checksum:</label>
            <input
              type="text"
              placeholder="Nhập mã vnp_HashSecret"
              // value={request.Address}
              // onChange={(e)=>{
              //   setRequest({
              //     ...request,
              //     Address: e.target.value
              //   })
              // }}
              className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
            />
          </div>
        </div>
        {/* Save Button */}
        <button
          // onClick={onSaveChange}
          className="bg-blue-500 text-white font-semibold py-2 px-6 rounded-md mt-6 hover:bg-blue-600 transition duration-300 ease-in-out">
           Lưu thông tin
        </button>
        </div>
      </div>
    )
  }
  function UpdatePassWord(){
    return (
      <div>
        {/* Title */}
        <div className="mt-8">
          <h1 className="text-3xl font-semibold text-gray-800">Cập nhật mật khẩu</h1>
        </div>
        {/* Store Information Form */}
        <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-6">
          {/* First Column */}
          <div>
            <label className='ml-1' htmlFor="">Mật khẩu cũ:</label>
            <input
              type="text"
              placeholder="Nhật mật khẩu đăng nhập cũ"
              // value={request.Description}
              // onChange={(e)=>{
              //   setRequest({
              //     ...request,
              //     Description: e.target.value
              //   })
              // }}
              className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
            />
          </div>
          <div>
            <label className='ml-1' htmlFor="">Mật khẩu mới:</label>
            <input
              type="text"
              placeholder="Nhập mật khẩu đăng nhập mới"
              // value={request.Address}
              // onChange={(e)=>{
              //   setRequest({
              //     ...request,
              //     Address: e.target.value
              //   })
              // }}
              className="border border-gray-300 rounded-md px-4 py-2 w-full focus:outline-none focus:border-blue-500"
            />
          </div>
        </div>
        {/* Save Button */}
        <button
          // onClick={onSaveChange}
          className="bg-blue-500 text-white font-semibold py-2 px-6 rounded-md mt-6 hover:bg-blue-600 transition duration-300 ease-in-out">
           Cập nhật mật khẩu
        </button>
      </div>
    )
  }
};
export default StoreSettings;
