import React, { useState, useEffect  } from 'react';
import { useStateValue } from "../context/StateProvider";
import { useNavigate } from "react-router-dom";
import Modal from 'react-modal';
import SelectProvince from "./SelectProvince";
import SelectDistrict from "./SelectDistrict";
import SelectWard from "./SelectWard";
import MyApp from "./DeliveryAddress";
import { TextInput } from "evergreen-ui";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Swal from "sweetalert2";
import { TakeAllCustomerAddressById, CreateCustomerAddress , DeleteAddress} from '../api/customer/customerService';

export default function CustomrerAddressContainer() {
    const [{customer}, dispatch] =
      useStateValue();  const navigate = useNavigate();
    const [dataModal, setDataModal] = React.useState({
      CustomerId: customer,
      CustomerName: "",
      PhoneCustomer: "",
      ProvinceId: "",
      DistrictId: "",
      WardId: "",
      Format_Address: "",
      Name_Address:"",
      ProvinceName:"",
      DistrictName:"",
      WardName:"",
      AddressId :0,
      Defaut: false,
      Latitude: 0.0,
      Longitude: 0.0,
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
    const [defautChecked,setDefautchecked] = useState(false);
    const [addresses, setAddress] = React.useState([]);
    const [request, setRequest] = useState({
      CustomerId: customer,
      Phone: JSON.parse(localStorage.getItem('phone'))
    });  
    const [location, setlocation] = useState({
      latitude: 0.0,
      longitude: 0.0,
      formatted_address: "",
    });
    const handleCheckboxChange = (e) => {
      const newValue = e.target.checked; // Use checked property to determine the new value
      setDataModal({
        ...dataModal,
        Defaut: newValue,
      });
    };
  
    const [requestAddress, setRequestAddress] = useState({
      Province: "",
      District: "",
      Ward: "",
    });


      // Hàm callback để nhận dữ liệu từ component con
      const handleSelectedAddressChange = (selected) => {
        setDataModal({
          ...dataModal,
          Name_Address: selected.name,
          Format_Address: selected.formatted_address,
          Latitude : selected.lat,
          Longitude: selected.lng,
        })
        console.log(selected);
      };
      // Hàm kiểm tra số điện thoại
      const isValidPhoneNumber = (phoneNumber) => {
        // Định dạng số điện thoại Việt Nam: 10 chữ số, bắt đầu bằng 0 hoặc +84
        const phoneNumberPattern = /^(0|\+84)[0-9]{9}$/;
        return phoneNumberPattern.test(phoneNumber);
      };
    async function onSave(){
      if(dataModal.CustomerName =="" || dataModal.CustomerId =="" || dataModal.PhoneCustomer =="" ){
        toast.warning('Vui lòng điền đầy đủ thông tin!', { autoClose: 3000 });
        return;
      }
      if (!isValidPhoneNumber(dataModal.PhoneCustomer)) {
        toast.warning('Số điện thoại không hợp lệ !', { autoClose: 3000 });
        return false; // Số điện thoại không hợp lệ
      }
      let repsponse  = await CreateCustomerAddress(dataModal)
      if(!repsponse.success)
      {
        toast.error('Lưu không thành công! Vui lòng thử lại', { autoClose: 3000 });
      }
      else{
        toast.success('Lưu địa chỉ thành công', { autoClose: 3000 });
        TakeAllCustomerAddres();
        setIsOpen(false);
        setDefautchecked(false);
        setDataModal({
          ...dataModal,
          ProvinceId: "",
          DistrictId: "",
          WardId: "",
          ProvinceName:"",
          DistrictName:"",
          WardName:"",
          AddressId : 0,
          CustomerName: "",
          PhoneCustomer:"",
          Latitude: 0.0,
          Longitude: 0.0,
          Name_Address: "",
          Format_Address:"",
          Defaut: false,
        })
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
      }
    }
    let subtitle;
  const [modalIsOpen, setIsOpen] = React.useState(false);

  function openModal() {
    setIsOpen(true);
  }
  

  function afterOpenModal() {
    // references are now sync'd and can be accessed.
    subtitle.style.color = '#f00';
  }

  function closeModal() {
    setDataModal({
      ...dataModal,
      ProvinceId: "",
      DistrictId: "",
      WardId: "",
      ProvinceName:"",
      DistrictName:"",
      WardName:"",
      AddressId : 0,
      CustomerName: "",
      PhoneCustomer:"",
      Latitude: 0.0,
      Longitude: 0.0,
      Name_Address: "",
      Format_Address:"",
      Defaut: false,
    })
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
    }
    async function TakeAllCustomerAddres() {
        let checkCustomer = await TakeAllCustomerAddressById(request);
        if (checkCustomer.success) {
        if(checkCustomer.dataCount == 0){
            setDefautchecked(true)
        }
        setAddress(checkCustomer.data);
        }
    }
    useEffect(() =>{
        TakeAllCustomerAddres();
    },[request])

    async function updateAddress(data) {
      // Cập nhật lại Defaut là true
        data.Defaut = true;
        const response = await CreateCustomerAddress(data);
        if (response.success) {
          toast.success('Đã cập nhật lại địa chỉ mặc định', { autoClose: 2000 });
          TakeAllCustomerAddres();
          setIsOpen(false);
          setDefautchecked(false);
          setDataModal({
            ...dataModal,
            ProvinceId: "",
            DistrictId: "",
            WardId: "",
            ProvinceName:"",
            DistrictName:"",
            WardName:"",
            AddressId : 0,
            CustomerName: "",
            PhoneCustomer:"",
            Latitude: 0.0,
            Longitude: 0.0,
            Name_Address: "",
            Format_Address:"",
            Defaut: false,
          })
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
        } else {
          toast.success('Dã xảy ra lỗi! Vui lòng thao tác lại!', { autoClose: 3000 });
          TakeAllCustomerAddres();
          setIsOpen(false);
          setDefautchecked(false);
          setDataModal({
            ...dataModal,
            ProvinceId: "",
            DistrictId: "",
            WardId: "",
            ProvinceName:"",
            DistrictName:"",
            WardName:"",
            AddressId : 0,
            CustomerName: "",
            PhoneCustomer:"",
            Latitude: 0.0,
            Longitude: 0.0,
            Name_Address: "",
            Format_Address:"",
            Defaut: false,
          })
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
                }
      }
    
    
    return(
        <div className="bg-white p-4">
        <div className="card p-4">
          <div className="card-body">
            <div className="mb-1">
            <h1 className="text-xl mb-2 text-red-700 font-bold">     
         Địa chỉ của tôi
     </h1>                </div>
            <div className="flex justify-between items-center mb-4">
              <button
                type="submit"
                className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600"
                onClick={openModal}
              >
                Thêm địa chỉ
              </button>
            </div>
            <ul>
            { addresses &&
              addresses.map((address, index) => (
                // Check if address is not null before rendering
                address !== null && (
                  <li key={index} className="mb-2 border p-4 rounded-lg border-gray-300">
                    <h6 className="text-base text-gray-700 font-normal">
                      {address.CustomerName} | {address.PhoneCustomer}
                    </h6>
                    <p className="text-sm text-gray-500">
                      {address.Name_Address}
                    </p>
                    <p className="text-sm text-gray-500">
                      {address.Format_Address}
                    </p>
                      {address.Defaut ? (
                        <p className="text-sm text-red-500">Địa chỉ mặc định
                        </p>
                      ):
                      (
                      <div>
                      <button
                      onClick={() => {
                          Swal.fire({
                              title: "Thiết lập mặc định ?",
                              text: "Bạn muốn thiết lập mặc định không?",
                              icon: "warning",
                              showCancelButton: true,
                              confirmButtonColor: "#3085d6",
                              cancelButtonColor: "#d33",
                              confirmButtonText: "Xác nhận !",
                            }).then((result) => {
                              if (result.isConfirmed) {
                                updateAddress(address);      
                              }
                            });
                        }}
                        className="btn btn-primary btn-sm"
                        style={{ border: "1px solid grey", backgroundColor: "white" }}
                        >
                        <p className="text-sm text-red-500">Thiết lập địa chỉ mặc định
                        </p>                   
                       </button>
                        </div>

                             
                      )
                    }
                    <div className='row'>

                    </div>
                    <button
                      type="submit"
                      className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 mt-2"
                      onClick={() => {
                        setDataModal({
                          ...dataModal,
                          ProvinceId: address.ProvinceId,
                          DistrictId: address.DistrictId,
                          WardId: address.WardId,
                          Format_Address: address.Format_Address,
                          Name_Address: address.Name_Address,
                          ProvinceName: address.ProvinceName,
                          DistrictName:address.DistrictName,
                          WardName: address.WardName,
                          AddressId : address.AddressId,
                          Defaut: address.Defaut,
                          CustomerName : address.CustomerName,
                          PhoneCustomer: address.PhoneCustomer,
                          Latitude: address.Latitude,
                          Longitude: address.Longitude,
                        });
                        setRequestAddress({
                          Province: address.ProvinceName,
                          District : address.District,
                          Ward: address.Ward,
                        })
                        setlocation({
                          latitude: address.Latitude,
                          longitude: address.Longitude,
                          formatted_address: address.Format_Address,
                        })
                        setIsOpen(true);
                      }}
                    >
                      Cập nhật
                    </button>
                    {address.Defaut ? (
                        null
                      ):
                      <button
                      type="submit"
                      className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 mt-2 ml-2"
                      onClick={() =>{
                        Swal.fire({
                          title: "Xóa địa chỉ ?",
                          text: "Bạn muốn xóa địa chỉ này không!",
                          icon: "warning",
                          showCancelButton: true,
                          confirmButtonColor: "#3085d6",
                          cancelButtonColor: "#d33",
                          confirmButtonText: "Xác nhận !",
                        }).then((result) => {
                          if (result.isConfirmed) {
                            DeleteAddress(address).then(
                              (response) => {
                                if (response.success) {
                                  Swal.fire(
                                    "Hoàn thành!",
                                    "Xóa dữ liệu thành công.",
                                    "success"
                                  );
                                  TakeAllCustomerAddres();
                                } else {
                                  Swal.fire({
                                    title: "Lỗi!",
                                    text: "Không thể xóa!",
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
                      Xóa
                    </button>
                    }
                  </li>
                )
              ))
            }
            </ul>
          </div>
        </div>


        {/*Modal cập nhật/ thêm mới đia chỉ khách hàng*/}
        <Modal
          isOpen={modalIsOpen}
          onAfterOpen={afterOpenModal}
          onRequestClose={closeModal}
          style={customStyles}
          contentLabel="Form"
          shouldCloseOnOverlayClick={false} // Đặt giá trị này thành false để ngăn Modal đóng khi nhấn ở ngoài
        >
        <div className='row' style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
          {
            dataModal.AddressId === 0 ?(
              <h2  className="text-2xl text-red-700 font-bold"
              ref={(_subtitle) => (subtitle = _subtitle)}>Thêm mới địa chỉ</h2>
            ):( 
              <h2  className="text-2xl text-red-700 font-bold"
              ref={(_subtitle) => (subtitle = _subtitle)}>Cập nhật địa chỉ</h2>
            )
          }

          <div className="w-full">
          <label htmlFor="completeName" className="block text-lg font-medium text-gray-700 mb-2">Họ và tên</label>
          <TextInput
            id="completeName"
            className="p-2 rounded-lg border w-full focus:ring focus:ring-blue-500"
            type="text"
            placeholder="Nhập họ tên . . ."
            value={dataModal.CustomerName != null ?dataModal.CustomerName : ""}
            onChange={(e) => {
              setDataModal({
                ...dataModal,
                CustomerName: e.target.value,
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
            placeholder="Nhập số điện thoại . . ."
            value={dataModal.PhoneCustomer != null ?dataModal.PhoneCustomer : ""}
            onChange={(e) => {
              setDataModal({
                ...dataModal,
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
              value: dataModal.ProvinceId,
              label: dataModal.ProvinceName,
            }}
            onSelect={(e) => {
              setDataModal({
                ...dataModal,
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
              value: dataModal.DistrictId,
              label: dataModal.DistrictName,
            }}
            onSelect={(e) => {
              setDataModal({
                ...dataModal,
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
              value: dataModal.WardId,
              label: dataModal.WardName,
            }}
            onSelect={(e) => {
              setDataModal({
                ...dataModal,
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
              <MyApp requestAddress={requestAddress} onSelectedAddressChange={handleSelectedAddressChange} location={location}/>
            </div>
          )
        }
          <div className='row'>
                <input
                    type="checkbox"
                    name="Defaut"
                    checked={defautChecked ? defautChecked:  dataModal.Defaut}
                    onChange={handleCheckboxChange}
                    disabled={dataModal.Defaut} // Sử dụng thuộc tính disabled
                  />
                  <label className='text-1xl text-red-400 font-bold ml-2'>Địa chỉ mặc định</label>
          </div>
        </div>   
        <button
                    type="submit"
                    className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 mt-2"
                    onClick={onSave}
                  >
                    Lưu
        </button>
        <button
                    type="submit"
                    className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 mt-2 ml-2"
                    onClick={closeModal}
                  >
                    Đóng
        </button>            
        </Modal>

        </div>
    )

}
