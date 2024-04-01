import React, { useEffect, useState } from "react";
import { motion } from "framer-motion";
import { useStateValue } from "../../context/StateProvider";
import { ToastContainer, toast } from 'react-toastify';
import {
  MdFastfood,
  MdCloudUpload,
  MdDelete,
  MdAttachMoney,
  MdCreate,
  MdDescription,
  MdMoney,
  MdFoodBank,
  MdNumbers,
  MdDateRange,
} from "react-icons/md";
import { IoArrowBackOutline } from "react-icons/io5";
import { TakeCategoryByStoreId } from "../../api/store/storeService";
import { Loader } from "../../components";
import { useLocation, useNavigate } from "react-router-dom";
import {
  CreateFoodItem,
  UpdateFoodList,
} from "../../api/foodlist/foodListService";
import { FaLessThanEqual } from "react-icons/fa";

const CreateFoodList = () => {
  const { state } = useLocation();
  const [fields, setFields] = useState(false);
  const [alertStatus, setAlertStatus] = useState("danger");
  const [msg, setMsg] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [{ user }] = useStateValue();
  const [categoriesFood, setCategoriesFood] = React.useState([]);
  const [selectedImage, setSelectedImage] = useState();
  const history = useNavigate();
  var copyimg = state?.data.UploadImage;
  const [showExpirationDate, setShowExpirationDate] = useState(false);
  const [showIsNew, setshowIsNew] = useState(false);
  const [showQtycontrolled, setshowQtycontrolled] = useState(false);
  const [showQtySuppliedcontrolled, setshowQtySuppliedcontrolled] = useState(false);

  const [showIsNoiBat, setshowIsNoiBat] = useState(false);

  const handleCheckboxChange = () => {
    setRequest({
      ...request,
      ExpiryDate: "",
    });
    setShowExpirationDate(!showExpirationDate);
  };

  const handleCheckNew = () => {
    setshowIsNew(!showIsNew);
    setRequest({
      ...request,
      IsNew: !showIsNew,
    });
  };

  const handleCheckNoiBat = () => {
    setshowIsNoiBat(!showIsNoiBat);
    setRequest({
      ...request,
      IsNoiBat: !showIsNoiBat,
    });
  };
  const handleCheckSoluong = () => {
    setshowQtycontrolled(!showQtycontrolled);
    setRequest({
      ...request,
      Qtycontrolled: !showQtycontrolled,
    });
  };

  const handleCheckSoluongCungUng = () => {
    setshowQtySuppliedcontrolled(!showQtySuppliedcontrolled);
    setRequest({
      ...request,
      QtySuppliedcontrolled: !showQtySuppliedcontrolled,
    });
  };

  const [request, setRequest] = React.useState({
    FoodListId: 0,
    CategoryId: 0,
    FoodName: "",
    Description: "",
    Price: "",
    qty: 0,
    UploadImage: "",
    Description: "",
    UserId: user.UserId,
    Status: 0,
    QuantitySupplied: 0,
    ExpiryDate: "",
    IsNew: 0,
    IsNoiBat: 0,
    Qtycontrolled: 0,
    QtySuppliedcontrolled: 0
  });

  async function OnLoadCategoryByStoreId() {
    if (user) {
      let response = await TakeCategoryByStoreId(user?.UserId);
      setCategoriesFood(response.data);
    }
  }

  async function onViewAppearing() {
    console.log(state?.data);
    if (state?.data) {
      setRequest({
        FoodListId: state?.data.FoodListId,
        CategoryId: state?.data.CategoryId,
        FoodName: state?.data.FoodName,
        Price: state?.data.Price,
        qty: state?.data.qty,
        QuantitySupplied: state?.data.QuantitySupplied,
        UploadImage: state?.data.UploadImage,
        Description: state?.data.Description,
        UserId: state?.data.UserId,
        Status: state?.data.Status,
        IsNew: state?.data.IsNew,
        IsNoiBat: state?.data.IsNoiBat,
        ExpiryDate: state?.data.ExpiryDate ?? null,
        IsNoiBat: state?.data.IsNoiBat,
        Qtycontrolled: state?.data.Qtycontrolled,
        QtySuppliedcontrolled: state?.data.QtySuppliedcontrolled,

      });
      setShowExpirationDate(state?.data.ExpiryDate != null  ? true : false);
      setshowIsNew(state?.data.IsNew);
      setshowIsNoiBat(state?.data.IsNoiBat);
      setshowQtySuppliedcontrolled(state?.data.QtySuppliedcontrolled);
      setshowQtycontrolled(state?.data.Qtycontrolled);
    }
    await OnLoadCategoryByStoreId();
  }

  function imageChange(e) {
    setIsLoading(true);
    if (e.target.files && e.target.files.length > 0) {
      setSelectedImage(e.target.files[0]);
      setRequest({
        ...request,
        UploadImage: e.target.files[0].name,
      });
      setFields(true);
      setMsg("Chọn ảnh thành công");
      setAlertStatus("success");
      setTimeout(() => {
        setFields(false);
      }, 4000);
    }
    setIsLoading(false);
  }

  function removeSelectedImage() {
    setIsLoading(true);
    setFields(true);
    setMsg("Đã xóa ảnh");
    setAlertStatus("success");
    setTimeout(() => {
      setFields(false);
    }, 4000);
    setSelectedImage();
    setRequest({
      ...request,
      UploadImage: request.FoodListId == 0 ? "" : copyimg,
    });
    setIsLoading(false);
  }

  async function SaveFoodList() {
    if (request.FoodListId === 0) {

      if(  request.qty === null ||
      request.qty === "" &&
      request.Qtycontrolled === true )
      {
        toast.warning('Vui lòng nhập số lượng tồn !', {
          position: 'top-right',
          autoClose: 3000, // 5 seconds
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
        });
        return;
      }

      if(  request.QuantitySupplied === null ||
        request.QuantitySupplied === "" &&
        request.QtySuppliedcontrolled === true )
        {
          toast.warning('Vui lòng nhập số lượng khả năng cung ứng !', {
            position: 'top-right',
            autoClose: 3000, // 5 seconds
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
          });
          return;
        }
  

      if (
        request.FoodName === "" ||
        request.Price === "" ||
        request.CategoryId === 0 ||
        request.UploadImage === "" 
      ) {
        setFields(true);
        setMsg("Bạn phải nhập đầy đủ thông tin");
        setAlertStatus("danger");
        setTimeout(() => {
          setFields(false);
        }, 4000);
      } else {
        let data = new FormData();
        if (selectedImage !== undefined) {
          data.append("file[]", selectedImage, selectedImage.name);
        }
        data.append("form", JSON.stringify(request));
        let response = await CreateFoodItem(data);
        if (!response.success) {
          setFields(true);
          setMsg("Đã xảy ra lỗi khi thêm mới ...");
          setAlertStatus("danger");
          setTimeout(() => {
            setFields(false);
          }, 4000);
          return;
        }
        setIsLoading(true);
        setRequest({
          FoodListId: 0,
          CategoryId: 0,
          FoodName: "",
          Description: "",
          Price: "",
          qty: 0,
          UploadImage: "",
          Description: "",
          UserId: user.UserId,
          Status: 0,
          QuantitySupplied: 0,
          ExpiryDate: "",
          IsNew: 0,
          IsNoiBat: 0,
        });
        toast.success('Thêm sản phẩm thành công', {
          position: 'top-right',
          autoClose: 3000, // 5 seconds
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
        });

      }
    } else {
      if (
        request.FoodName === "" ||
        request.Price === "" ||
        request.CategoryId === 0 ||
        copyimg === ""
      ) {
        setFields(true);
        setMsg("Phải nhập đầy đủ thông tin");
        setAlertStatus("danger");
        setTimeout(() => {
          setFields(false);
        }, 4000);
      } else {
        let data = new FormData();
        if (selectedImage !== undefined) {
          data.append("file[]", selectedImage, selectedImage.name);
        }
        data.append("form", JSON.stringify(request));
        let response = await UpdateFoodList(data);
        if (!response.success) {
          setFields(true);
          setMsg("Cập nhật không thành công");
          setAlertStatus("danger");
          setTimeout(() => {
            setFields(false);
          }, 4000);
          return;
        }
        // setFields(true);
        setRequest({
          FoodListId: 0,
          CategoryId: 0,
          FoodName: "",
          Description: "",
          Price: "",
          qty: 0,
          UploadImage: "",
          Description: "",
          UserId: user.UserId,
          Status: 0,
          QuantitySupplied: 0,
          ExpiryDate: "",
          IsNew: 0,
          IsNoiBat: 0,
        });
        toast.success('Cập nhật thành công !', {
          position: 'top-right',
          autoClose: 3000, // 5 seconds
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
        });
        // setMsg("Cập nhật thành công");
        // setAlertStatus("success");
        // setTimeout(() => {
        //   setFields(false);
        //   onback();
        // }, 500);

      }
    }
    setIsLoading(false);
  }

  function onback() {
    history("/foodlist");
  }

  const getCurrentDate = () => {
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = String(currentDate.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  };

  const handleDateChange = (e) => {
    const selectedDate = e.target.value;
    const currentDate = getCurrentDate();
  
    const selectedDateObject = new Date(selectedDate);
    const currentDateObject = new Date(currentDate);
  
    if (selectedDateObject < currentDateObject) {
      toast.warn('Ngày hết hạn không được bé hơn ngày hiện tại ...', {
        position: 'top-right',
        autoClose: 5000, // 5 seconds
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
      });
      // Reset the input value to the current date
      e.target.value = currentDate;
    } else {
      setRequest({
        ...request,
        ExpiryDate: selectedDate,
      });
    }
  };
  
  function ConvertJsonToDate(jsondate){
    
    const dateObject = new Date(jsondate);
    const year = dateObject.getFullYear();
    const month = String(dateObject.getMonth() + 1).padStart(2, '0');
    const day = String(dateObject.getDate()).padStart(2, '0');
    console.log(`ngày sau khi chuyển đổi .... ${dateObject}`);
    return `${year}-${month}-${day}`;
  }
  
  useEffect(() => {
    onViewAppearing();
  }, []);

  return (
    
    <div className="bg-gray-50 h-[100%] basis-80 p-8 overflow-auto crollbar-hide py-5 px-5">
      <div className="flex items-center max-w-100">
        <button
          type="button"
          className="ml-0 md:mr-auto w-8 md:w-auto border-none outline-none px-3 py-2 rounded-lg text-lg text-white font-semibold"
          onClick={onback}
        >
          <IoArrowBackOutline className="text-3xl text-red-600" />
        </button>
      </div>
      <div className="w-full pt-0 flex items-center justify-center">
        <div className="w-[100%] md:w-[100%] border border-gray-300 rounded-lg p-4 flex flex-col items-center justify-center gap-4">
          {fields && (
            <motion.p
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              exit={{ opacity: 0 }}
              className={`w-full p-2 rounded-lg text-center text-lg font-semibold ${
                alertStatus === "danger"
                  ? "bg-red-400 text-red-800"
                  : "bg-emerald-400 text-emerald-800"
              }`}
            >
              {msg}
            </motion.p>
          )}
          <div className="w-full py-2 border-b border-gray-300 flex items-center gap-2">
            <MdFoodBank className="text-xl text-gray-700" />
            <input
              type="text"
              defaultValue={request.FoodName}
              onChange={(e) => {
                setRequest({
                  ...request,
                  FoodName: e.target.value,
                });
              }}
              placeholder="Nhập tên món ăn . . ."
              className="w-full h-full text-lg bg-transparent outline-none border-none placeholder:text-gray-400 text-textColor"
            />
          </div>
          <div className="w-full py-2 border-b border-gray-300 flex items-center gap-2">
            <MdDescription className="text-xl text-gray-700" />
            <input
              type="text"
              defaultValue={request.Description}
              onChange={(e) => {
                setRequest({
                  ...request,
                  Description: e.target.value,
                });
              }}
              placeholder="Nhập mô tả món ăn . . ."
              className="w-full h-full text-lg bg-transparent outline-none border-none placeholder:text-gray-400 text-textColor"
            />
          </div>

          <div className="flex">
              <div>
                <input
                  type="checkbox"
                  onChange={handleCheckSoluong}
                  checked={showQtycontrolled}
                  id="soluongCheckbox"
                />
                <label htmlFor="soluongCheckbox">Kiểm soát số lượng tồn</label>
              </div>
              {showQtycontrolled && (
                  <div className="w-[100%] py-2 border-b border-gray-300 flex items-start  gap-2">
                           <MdNumbers className="text-xl text-gray-700" />
                           <input
                             type="text"
                             defaultValue={request.qty}
                             onChange={(e) => {
                               setRequest({
                                 ...request,
                                 qty: e.target.value,
                               });
                             }}
                             placeholder="Nhập số lượng "
                             className="w-full h-full text-lg bg-transparent outline-none border-none placeholder:text-gray-400 text-textColor"
                           />
                  </div>
              )}
          </div>
          <div className="flex">
              <div>
                <input
                  type="checkbox"
                  onChange={handleCheckSoluongCungUng}
                  checked={showQtySuppliedcontrolled}
                  id="soluongCungungCheckbox"
                />
                <label htmlFor="soluongCungungCheckbox">Kiểm soát số lượng cung ứng trong ngày</label>
              </div>
              {showQtySuppliedcontrolled && (
                    <div className="w-[100%] py-2 border-b border-gray-300 flex items-start  gap-2">
                        <MdNumbers className="text-xl text-gray-700" />
                        <input
                          type="text"
                          defaultValue={request.QuantitySupplied}
                          onChange={(e) => {
                            setRequest({
                              ...request,
                              QuantitySupplied: e.target.value,
                            });
                          }}
                          placeholder="Nhập số lượng cung ứng mỗi ngày"
                          className="w-full h-full text-lg bg-transparent outline-none border-none placeholder:text-gray-400 text-textColor"
                        />
                    </div>
              )}
    
          </div>
    
          <div className="checkbox-container">
            <input
              type="checkbox"
              
              onChange={handleCheckboxChange}
              checked={showExpirationDate}
              id="expirationCheckbox"
            />
            <label htmlFor="expirationCheckbox">CÓ HẠN SỬ DỤNG</label>
          </div>

          <div className="flex">
            {showExpirationDate && (
              <div className="date-picker-container">
                     {showExpirationDate && (
                        <div className="w-[100%] py-2 flex items-start gap-2">
                          <MdDateRange className="text-xl text-gray-700" />
                          <input
                            type="date"
                            defaultValue={request.ExpiryDate != "0001-01-01T00:00:00+07:00" || request.ExpiryDate != null ?  ConvertJsonToDate(request.ExpiryDate) : getCurrentDate()}
                            onChange={handleDateChange}
                            className="w-full h-full text-lg bg-transparent outline-none border-none placeholder:text-gray-400 text-textColor"
                          />
                        </div>
                      )}
              </div>
            )}
          </div>


          <div className="flex checkbox-group">
            <div className="row">
              <div className="w-full">
                Trạng thái
                {
                  request.Status === true ?
                  <select
                    onChange={(e) => {
                      setRequest({
                        ...request,
                        Status: e.target.value,
                      });
                    }}
                    className="outline-none w-full text-b ase border-b-2 border-gray-200 p-2 rounded-md cursor-pointer"
                  >
                      <option value="false" className='bg-white capitalize'>
                          Chưa mở bán
                      </option>
                      <option value="true" className='bg-white capitalize' selected>
                          Mở bán
                      </option>
                 </select>
                 :
                  <select
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      Status: e.target.value,
                    });
                  }}
                  className="outline-none w-full text-base border-b-2 border-gray-200 p-2 rounded-md cursor-pointer"
                >
                    <option value="false" className='bg-white capitalize' selected>
                        Chưa mở bán
                    </option>
                    <option value="true" className='bg-white capitalize'>
                        Mở bán
                    </option>
                  </select>
                }
              </div>
            </div>
                     
            <div>
              <input
                type="checkbox"
                onChange={handleCheckNoiBat}
                checked={showIsNoiBat}
                id="noiBatCheckbox"
              />
              <label htmlFor="noiBatCheckbox">Sản phẩm nổi bật</label>
            </div>

            <div>
              <input
                type="checkbox"
                onChange={handleCheckNew}
                checked={showIsNew}
                id="newCheckbox"
              />
              <label htmlFor="newCheckbox">Sản phẩm mới</label>
            </div>

          </div>
      
          <div className="w-full">
            <select
              onChange={(e) => {
                setRequest({
                  ...request,
                  CategoryId: e.target.value,
                });
              }}
              className="outline-none w-full text-base border-b-2 border-gray-200 p-2 rounded-md cursor-pointer"
            >
              <option value="other" className="bg-white capitalize">
                Chọn nhóm món ăn
              </option>
              {categoriesFood &&
                categoriesFood.map((item) =>
                  item.CategoryId == request.CategoryId ? (
                    <option
                      selected
                      key={item.CategoryId}
                      className="text-base border-0 outline-none capitalize bg-white text-headingColor"
                      value={item.CategoryId}
                    >
                      {item.CategoryName}
                    </option>
                  ) : (
                    <option
                      key={item.CategoryId}
                      className="text-base border-0 outline-none capitalize bg-white text-headingColor"
                      value={item.CategoryId}
                    >
                      {item.CategoryName}
                    </option>
                  )
                )}
            </select>
          </div>
          <div className="group flex justify-center items-center flex-col border-2 border-dotted border-gray-300 w-full h-225 md:h-340 cursor-pointer rounded-lg">
            {isLoading ? (
              <Loader />
            ) : (
              <>
                {!selectedImage ? (
                  <>
                    <label className="w-full h-full flex flex-col items-center justify-center cursor-pointer">
                      {request.FoodListId == 0 ? (
                        <>
                          <div className="w-full h-full flex flex-col items-center justify-center gap-2">
                            <MdCloudUpload className="text-green-500 text-3xl hover:text-green-700" />
                            <p className="text-gray-500 hover:text-gray-700">
                              Chọn ảnh món ăn
                            </p>
                          </div>
                          <input
                            type="file"
                            name="uploadimage"
                            accept="image/*"
                            onChange={imageChange}
                            className="w-0 h-0"
                          />
                        </>
                      ) : (
                        <div className="relative h-full">
                          <img
                            src={request.UploadImage}
                            alt=""
                            className="w-full h-full object-contain"
                          />
                          <div className="w-12 h-12 items-center justify-center absolute bottom-3 right-3 p-2 rounded-full bg-orange-400 text-xl cursor-pointer outline-none hover:shadow-md  duration-500 transition-all ease-in-out">
                            <div className="flex justify-items-center justify-center items-center">
                              <MdCreate className="text-white text-3xl" />
                              <input
                                type="file"
                                name="uploadimage"
                                accept="image/*"
                                onChange={imageChange}
                                className="w-0 h-0"
                              />
                            </div>
                          </div>
                        </div>
                      )}
                    </label>
                  </>
                ) : (
                  <div className="relative h-full">
                    <img
                      src={URL.createObjectURL(selectedImage)}
                      alt="uploaded image"
                      className="w-full h-full object-cover"
                    />
                    <button
                      type="button"
                      className="absolute bottom-3 right-3 p-3 rounded-full bg-red-500 text-xl cursor-pointer outline-none hover:shadow-md  duration-500 transition-all ease-in-out"
                      onClick={removeSelectedImage}
                    >
                      <MdDelete className="text-white" />
                    </button>
                  </div>
                )}
              </>
            )}
          </div>
          <div className="w-full flex flex-col md:flex-row items-center gap-3">
            <div className="w-full py-2 border-b border-gray-300 flex items-center gap-2">
              <MdMoney className="text-gray-700 text-2xl" />
              <input
                type="number"
                defaultValue={request.Price}
                onChange={(e) => {
                  setRequest({
                    ...request,
                    Price: e.target.value,
                  });
                }}
                placeholder="Nhập vào giá món ăn . . ."
                className="w-full h-full text-lg bg-transparent outline-none border-none placeholder:text-gray-400 text-textColor"
              />
            </div>
          </div>
          <div className="flex items-center w-full">
            <button
              type="button"
              className={`ml-0 md:ml-auto w-full md:w-auto border-none outline-none ${
                request.FoodListId == 0 ? "bg-emerald-500" : "bg-orange-500"
              } px-12 py-2 rounded-lg text-lg text-white font-semibold`}
              onClick={SaveFoodList}
            >
              {request.FoodListId == 0 ? "Lưu" : "Cập nhật"}
            </button>
          </div>
        </div>
      </div>
      <ToastContainer />
    </div>
  );
};

export default CreateFoodList;
