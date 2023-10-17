import React, { useState, useEffect } from "react";
import { useStateValue } from "../context/StateProvider";
import {
  CheckCustomer,
  UpdateInfoCustomer
} from "../api/customer/customerService";
import Loader from "./Loader";
import { FaGoogle } from "react-icons/fa";
import CartContainer from "./CartContainer";
import Swal from "sweetalert2";
import { provider} from "../firebase.config";
import { getAuth,signInWithPopup, GoogleAuthProvider } from "firebase/auth";
import { actionType } from "../context/reducer";
import { CheckCustomerEmail } from "../api/customer/customerService";

export default function Account() {
  const [formData, setFormData] = useState({
    CustomerId: "",
    CompleteName: "",
    Phone: "",
    Address: "",
    Email: "",
    ImageProfile: "",
    ProvinceId: 0,
    DistrictId: 0,
    WardId: 0,
    });
  const [{ cartShow, customer }, dispatch] = useStateValue();
  const [selectedImage, setSelectedImage] = useState(null);
  const [imageShow, setimageShow] = useState(null);
  const [loading, setLoading] = useState(false);
  const [checkCustomer, setCheckCustomer] = useState([]);
  let phonecustomer = JSON.parse(localStorage.getItem('phone'));
  if (phonecustomer!= null &&  phonecustomer.startsWith('84')) {
      phonecustomer = phonecustomer.slice(2); // Bỏ đi 2 ký tự đầu (84)
  }


  async function onChangeCustomer() {
    setLoading(true);
    const checkCustomer = await CheckCustomer({
      CustomerId: customer,
      Phone: JSON.parse(localStorage.getItem('phone'))
    });
    if (checkCustomer.success) {
      setCheckCustomer(checkCustomer);
      if(checkCustomer.data.length > 0){
        setFormData({
          CustomerId: checkCustomer.data[0].CustomerId,
          CompleteName: checkCustomer.data[0].CompleteName,
          Phone: checkCustomer.data[0].Phone,
          Address: checkCustomer.data[0].Address,
          Email: checkCustomer.data[0].Email,
          ImageProfile: checkCustomer.data[0].ImageProfile,
          ProvinceId: checkCustomer.data[0].ProvinceId,
          DistrictId: checkCustomer.data[0].DistrictId,
          WardId: checkCustomer.data[0].WardId,
        });
        setimageShow( checkCustomer.data[0].ImageProfile)
      }
      else{
        setFormData({
          CustomerId: customer,
          Phone: phonecustomer,
        });
      }
      setLoading(false);
    }
  }
  // Sign in with google
  const auth = getAuth();
  function signInGoogle() {
    signInWithPopup(auth, provider)
    .then((result) => {
      // This gives you a Google Access Token. You can use it to access the Google API.
      const credential = GoogleAuthProvider.credentialFromResult(result);
      const token = credential.accessToken;
      // The signed-in user info.
      const user = result.user;
      dispatch({
        type: actionType.SET_EMAIL,
        email: user.email,
      });
      localStorage.setItem("email", JSON.stringify(user.email));
      updateEmail(user.email)
      console.log(formData)
      // IdP data available using getAdditionalUserInfo(result)
      // ...
    }).catch((error) => {
      // Handle Errors here.
      const errorCode = error.code;
      const errorMessage = error.message;
      // The email of the user's account used.
      const email = error.customData.email;
      // The AuthCredential type that was used.
      const credential = GoogleAuthProvider.credentialFromError(error);
      // ...
    });
  }
  // Update Email 
  function UpdateEmailGoogle() {
    signInWithPopup(auth, provider)
    .then(async (result) => {
      // This gives you a Google Access Token. You can use it to access the Google API.
      const credential = GoogleAuthProvider.credentialFromResult(result);
      const token = credential.accessToken;
      // The signed-in user info.
      const user = result.user;
      dispatch({
        type: actionType.SET_EMAIL,
        email: user.email,
      });
      let reponse = await CheckCustomerEmail({
        Email: user.email,
      })
      if(!reponse.success)
      {
        localStorage.setItem("email", JSON.stringify(user.email));
        updateEmail(user.email)
        console.log(formData)
      }
      else
      {
        Swal.fire(
          "Lỗi!",
          `Email: ${user.email} đã được sử dụng bởi 1 tài khoản khác!`,
          "warning"
        );
      }

      // IdP data available using getAdditionalUserInfo(result)
      // ...
    }).catch((error) => {
      // Handle Errors here.
      const errorCode = error.code;
      const errorMessage = error.message;
      // The email of the user's account used.
      const email = error.customData.email;
      // The AuthCredential type that was used.
      const credential = GoogleAuthProvider.credentialFromError(error);
      // ...
    });
  }

  // Hàm để cập nhật giá trị Email
  const updateEmail = (newEmail) => {
    setFormData({
      ...formData,
      Email: newEmail,
    });
  };
  // Hàm lưu thông tin cập nhật của khách hàng
  async function SaveInfoCustomer(request) {
    setLoading(true);
    const result = await UpdateInfoCustomer(request);
    if (result.success) {
      Swal.fire(
        "Thành công!",
        "Cập nhật thông tin thành công.",
        "success"
      );
      console.log("Cập nhật thành công")
      setLoading(false);
    }
  }

  const handleInputChange = (e) => {
    const { name, value, files } = e.target;
    if (name !== "ImageProfile") {
      setFormData({
        ...formData,
        [name]: value,
      });
    } else if (name === "ImageProfile" && files.length > 0) {
      const file = files[0];
      setSelectedImage(files[0]);
      setimageShow(URL.createObjectURL(file))
      setFormData({
        ...formData,
        [name]: file.name,
      });
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const { name, value, files } = e.target;
    if(name !== "SiginGoogle")
    {
      Swal.fire({
        title: "Xác nhận lưu ?",
        text: "Bạn muốn lưu thông tin đã cập nhật không  ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Xác nhận !",
      }).then((result) => {
        if (result.isConfirmed) { 
          let data = new FormData();
          console.log(selectedImage)
          if (selectedImage !== null) {
            data.append("file[]", selectedImage, selectedImage.name);
          }
          data.append("form", JSON.stringify(formData));
          SaveInfoCustomer(data);
        }
      });
    }
  };

  useEffect(() => {
    onChangeCustomer();
  }, [customer]);

  return (
    <div className="bg-white min-h-screen p-5 overflow-y-auto scrollbar py-2 px-2">
      {loading ? (
        <div className="text-center pt-20">
          <Loader />
        </div>
      ) : customer && checkCustomer.data ? (
        <div className="bg-white min-h-screen p-5 overflow-y-auto scrollbar py-2 px-2">
        <div className="flex">
            <div className="flex-1">
              <div className="items-center justify-between mt-0">
                <div className="title">
                  <div className="flex justify-between pb-4 items-center">
                    <h1 className="text-xl mb-2 text-red-700 font-bold">
                      Hồ sơ cá nhân
                    </h1>
                  </div>
                  <div className="flex justify-between pb-4 items-center">
                    <h6 className="text-sm mb-2 text-gray-500 font-normal">
                      Quản lý thông tin hồ sơ để bảo mật tài khoản
                    </h6>
                  </div>
                </div>
              </div>
              <form onSubmit={handleSubmit}>
                <div className="mb-4">
       
                  <div className="flex items-center">
                  <label
                    htmlFor="CompleteName"
                    className="block text-sm font-medium text-gray-700"
                  >
                    Họ tên: 
                  </label>
                  <input
                    type="text"
                    id="CompleteName"
                    name="CompleteName"
                    value={
                      formData.CompleteName
                    }
                    onChange={handleInputChange}
                    className="mt-1 p-2 border rounded-md flex-1"
                  />
                    <div className="ml-4">

                     <div className="relative">
                        {imageShow != null ? (
                          <img
                            src={imageShow}
                            alt="Selected Avatar"
                            className="h-24 w-24 rounded-full object-cover shadow-xl"
                          />
                        ) : (
                          <img
                            src="data:image/jpeg;base64,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"
                            alt="Default Avatar"
                            className="h-24 w-24 rounded-full object-cover shadow-xl"
                          />
                        )}

                        <label
                          htmlFor="ImageProfile"
                          className="absolute bottom-0 right-0 bg-blue-500 rounded-full cursor-pointer hover:bg-blue-600 text-white px-2 py-1 text-xs"
                        >
                          Chọn Ảnh
                        </label>
                        <input
                          type="file"
                          id="ImageProfile"
                          name="ImageProfile"
                          className="hidden"
                          accept=".jpeg, .png"
                          onChange={handleInputChange}
                          style={{ maxWidth: "1MB" }}
                        />
                      </div>

                      <input
                        type="file"
                        id="ImageProfile"
                        className="hidden"
                        accept=".jpeg, .png"
                        style={{ maxWidth: "1MB" }}
                      />

                      <div className="text-start mt-2">
                        <p className="text-xs">Dung lượng file tối đa 1 MB</p>
                        <p className="text-xs">Định dạng: .JPEG, .PNG</p>
                      </div>
                    </div>
                  </div>
                </div>
                {
                  /*
                   <div className="mb-4">
                  <label
                    htmlFor="Address"
                    className="block text-sm font-medium text-gray-700"
                  >
                    Địa chỉ:
                  </label>
                  <input
                    type="text"
                    id="Address"
                    name="Address"
                    value={formData.Address}
                    onChange={handleInputChange}
                    className="mt-1 p-2 border rounded-md w-full"
                  />
                </div>
                  */
                }
               
                <div className="mb-4">
                  <label
                    htmlFor="Phone"
                    className="block text-sm font-medium text-gray-700"
                  >
                    Số điện thoại:
                  </label>
                  <input
                    type="tel"
                    id="Phone"
                    name="Phone"
                    value={
                      formData.Phone != "" ?
                      formData.Phone : (phonecustomer != "undefined" ||phonecustomer != null ? phonecustomer: "")
                    }
                    onChange={handleInputChange}
                    className="mt-1 p-2 border rounded-md w-full"
                    required
                    disabled
                  />
                </div>
                <div className="mt-4">
                  <button
                    type="submit"
                    className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600"
                              
                  >
                    Lưu thông tin
                  </button>
                </div>
              </form>
              <div className="mb-4 flex items-center">
                  <label
                    htmlFor="Email"
                    className="block text-sm font-medium text-gray-700"
                  >
                    Email: 
                  </label>
                  <input
                    type="email"
                    id="Email"
                    name="Email"
                    disabled
                    value={formData.Email}
                    onChange={handleInputChange}
                    className="mt-1 p-2 border rounded-md flex-1"
                  />
                  {
                    !formData.Email ? (
                      <button
                        id="SiginGoogle"
                        name="SiginGoogle"
                      className="ml-2 p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-300 rounded-lg bg-opacity-50"
                      onClick={() => {
                        signInGoogle()
                      }}
                    >
                      <FaGoogle className="text-2xl" /> Liên kết Google
                    </button>
                    ):( 
                      <button
                      id="SiginGoogle"
                      name="SiginGoogle"
                    className="ml-2 p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-300 rounded-lg bg-opacity-50"
                    onClick={() => {
                      UpdateEmailGoogle()
                    }}
                  >
                    <FaGoogle className="text-2xl" /> Cập nhật Email
                  </button>
                    )
                  }
                 
                  
                </div>
            </div>
          </div>
        </div>
      ) : (
        <div>Bạn vui lòng đăng nhập để xem thông tin.</div>
      )}
    </div>
  );
}




