import React, { useEffect, useState } from "react";
import { motion } from "framer-motion";
import { useStateValue } from "../../context/StateProvider";
import {
  MdFastfood,
  MdCloudUpload,
  MdDelete,
  MdAttachMoney,
  MdCreate,
} from "react-icons/md";
import { IoArrowBackOutline } from "react-icons/io5";
import { TakeCategoryByStoreId } from "../../api/store/storeService";
import { Loader } from "../../components";
import { useLocation, useNavigate } from "react-router-dom";
import {
  CreateFoodItem,
  UpdateFoodList,
} from "../../api/foodlist/foodListService";

const CreateFoodList = () => {
  const { state } = useLocation();
  const [fields, setFields] = useState(false);
  const [alertStatus, setAlertStatus] = useState("danger");
  const [msg, setMsg] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [{ user }] = useStateValue();
  const [categoriesFood, setCategoriesFood] = React.useState([]);
  const [selectedImage, setSelectedImage] = useState();
  const [FoodListId, setFoodListId] = useState("");
  const history = useNavigate();
  var copyimg = state?.data.UploadImage;

  const [request, setRequest] = React.useState({
    FoodListId: 0,
    CategoryId: 0,
    FoodName: "",
    Price: "",
    qty: 1,
    UploadImage: "",
    Description: "",
    UserId: user.UserId,
    Status: 0,
  });

  async function OnLoadCategoryByStoreId() {
    if (user) {
      let response = await TakeCategoryByStoreId(user?.UserId);
      setCategoriesFood(response.data);
    }
  }

  async function onViewAppearing() {
    if (state?.data) {
      setRequest({
        FoodListId: state?.data.FoodListId,
        CategoryId: state?.data.CategoryId,
        FoodName: state?.data.FoodName,
        Price: state?.data.Price,
        qty: state?.data.qty,
        UploadImage: state?.data.UploadImage,
        Description: state?.data.Description,
        UserId: state?.data.UserId,
        Status: state?.data.Status,
      });
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
      setMsg("Chọn ảnh thành công 😊");
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
    setMsg("Xoá ảnh đã chọn thành công 😊!");
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
    setIsLoading(true);
    if (request.FoodListId === 0) {
      if (
        request.FoodName === "" ||
        request.Price === "" ||
        request.CategoryId === 0 ||
        request.UploadImage === ""
      ) {
        setFields(true);
        setMsg("Phải nhập đầy đủ thông tin 😣!");
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
          setMsg("Thêm món mới không thành công 😣!");
          setAlertStatus("danger");
          setTimeout(() => {
            setFields(false);
          }, 4000);
          return;
        }
        setFields(true);
        setMsg("Thêm món mới thành công 😊!");
        setAlertStatus("success");
        setTimeout(() => {
          setFields(false);
        }, 4000);
      }
    } else {
      if (
        request.FoodName === "" ||
        request.Price === "" ||
        request.CategoryId === 0 ||
        copyimg === ""
      ) {
        setFields(true);
        setMsg("Phải nhập đầy đủ thông tin 😣!");
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
          setMsg("Cập nhật không thành công 😣!");
          setAlertStatus("danger");
          setTimeout(() => {
            setFields(false);
          }, 4000);
          return;
        }
        setFields(true);
        setMsg("Cập nhật thành công 😊!");
        setAlertStatus("success");
        setTimeout(() => {
          setFields(false);
          onback();
        }, 500);
      }
    }
    setIsLoading(false);
  }

  function onback() {
    history("/foodlist");
  }

  useEffect(() => {
    onViewAppearing();
  }, []);

  return (
    <div className="bg-gray-50 h-[100%] basis-80 p-8 overflow-auto crollbar-hide py-5 px-5">
      <div className="flex items-center max-w-200">
        <button
          type="button"
          className="ml-0 md:mr-auto w-8 md:w-auto border-none outline-none px-3 py-2 rounded-lg text-lg text-white font-semibold"
          onClick={onback}
        >
          <IoArrowBackOutline className="text-3xl text-red-600" />
        </button>
      </div>
      <div className="w-full pt-0 flex items-center justify-center">
        <div className="w-[90%] md:w-[60%] border border-gray-300 rounded-lg p-4 flex flex-col items-center justify-center gap-4">
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
            <MdFastfood className="text-xl text-gray-700" />
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
              <MdAttachMoney className="text-gray-700 text-2xl" />
              <input
                type="text"
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
    </div>
  );
};

export default CreateFoodList;
