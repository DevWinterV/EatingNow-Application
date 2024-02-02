import React, { useEffect, useState } from "react";
import { BsSearch } from "react-icons/bs";
import { AiOutlineAppstoreAdd } from "react-icons/ai";
import { motion } from "framer-motion";
import { IoFastFood } from "react-icons/io5";
import { BsPlusLg } from "react-icons/bs";

import { TakeCategoryByStoreId } from "../../api/store/storeService.js";
import FoodListAdmin from "../../components/FoodListAdmin.jsx";
import { useStateValue } from "../../context/StateProvider.js";
import { useNavigate } from "react-router-dom";
import Loader from "../../components/Loader.jsx";
import { FilterListIcon, ListColumnsIcon } from "evergreen-ui";
import { colors } from "@material-ui/core";

const Body = () => {
  const [isLoading, setIsLoading] = React.useState(false);
  const history = useNavigate();
  const [filter, setFilter] = useState("");
  const [categories, setCategories] = useState([]);
  const [{ user }] = useStateValue();
  const [requestfilter, setrequestfilter] = useState({
    Id: user?.UserId,
    Qtycontrolled: 2,
    QuantitySupplied: 2,
    ExpiryDate: 2,
    TimeExpiryDate: 2,
    keyWord: "",
    filter: filter
  });
  async function onViewAppearing() {
    setIsLoading(true);
    if (user) {
      let response = await TakeCategoryByStoreId(user?.UserId);
      setCategories(response.data);
    }
    setIsLoading(false);
  }

  useEffect(() => {
    onViewAppearing();
  }, []);

  const handleChangeExpiryDate = (e) => {
    setrequestfilter({
      ...requestfilter,
      ExpiryDate:parseInt(e.target.value, 10)
    });
  };

  const handleChangeTimeExpiryDate = (e) => {
    setrequestfilter({
      ...requestfilter,
      TimeExpiryDate: parseInt(e.target.value, 10)
    });
  };


  const handleChangQtycontrolled = (e) => {
    setrequestfilter({
      ...requestfilter,
      Qtycontrolled: parseInt(e.target.value, 10)
    });
  };
  const handleChangkeyWord = (e) => {
    setrequestfilter({
      ...requestfilter,
      keyWord: e.target.value
    });
  };

  return (
    <div className="bg-white h-[100%] basis-80 p-8 overflow-auto no-scrollbar py-5 px-5">
      <div className="flex items-center justify-start">
        <div className="flex items-center border-b-2 pb-2 basis-2/3 gap-2">
            <label htmlFor="">Kiểm soát số lượng tồn:</label>
            <select name="" id=" " onChange={handleChangQtycontrolled}>
              <option value="2" selected>
                Xem tất cả
              </option>
              <option value="1">
                  Có kiểm soát số lượng tồn
              </option>
              <option value="0">
                  Không có kiểm soạt số lượng tồn
              </option>
            </select>
        </div>
        <div className="flex items-center border-b-2 pb-2 basis-1/3 gap-2">
        <label htmlFor="">Tìm kiếm:</label>
          <input
            type="text"
            placeholder="Nhập tên món ăn, mô tả ... "
            className="border-none outline-none placeholder:text-sm focus:outline-none"
            onChange={handleChangkeyWord}
          />
          <button>       
             <BsSearch className="text-hoverColor text-[20px] cursor-pointer" />
          </button>
        </div>
      </div>
      <div className="flex items-center justify-start">
        <div className="flex items-center border-b-2 pb-2 basis-2/3 gap-1">
            <label htmlFor="">Kiểm soát hạn sử dụng:</label>
            <select name="" id=" " onChange={handleChangeExpiryDate}>
              <option value="2" selected>
                Xem tất cả
              </option>
              <option value="1">
                  Có hạn sử dụng
              </option>
              <option value="0">
                  Không có hạn sử dụng
              </option>
            </select>
        </div>
        <div className="flex items-center border-b-2 pb-2 basis-1/3 gap-2">
            <label htmlFor="">Hạn sử dụng:</label>
            <select name="" id=" " onChange={handleChangeTimeExpiryDate}>
              <option value="2" selected>
                  Xem tất cả
              </option>
              <option value="1">
                  Gần hết hạn
              </option>
              <option value="0">
                  Gần hết hạn 1 tháng 
              </option>
            </select>
        </div>
      </div>
      <div className="flex items-center justify-end">
        <button 
          className="custom-button mt-2 ml-2" 
          onClick={() => {
            setrequestfilter({
              Id: user?.UserId,
              Qtycontrolled: 2,
              QuantitySupplied: 2,
              ExpiryDate: 2,
              TimeExpiryDate: 2,
              keyWord: "",
              filter: ""
            });
          }}
          >
          <div className="flex">
            <ListColumnsIcon className="icon" />
            Xem tất cả
          </div>
        </button>
      </div>


      {/* Title Div */}
      <div className="items-center justify-between mt-2">
        <div className="title">
          <div className="flex justify-between pb-4 items-center">
            <h1 className="text-xl mb-2 text-orange-900 font-bold">
              Danh Sách Món Ăn
            </h1>
            <button
              type="button"
              className="text-red-700 hover:text-white border border-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mb-2 dark:border-green-500 dark:text-green-500 dark:hover:text-white dark:hover:bg-green-600 dark:focus:ring-green-800"
              onClick={() => {
                history("/createfoodlist");
              }}
            >
              <span className="justify-between flex items-center">
                Thêm Món Ăn Mới
                <BsPlusLg className="text-2xl pl-2 font-bold" />
              </span>
            </button>
          </div>
        </div>
      </div>

      {/* Category Div */}
      <div className="w-full flex items-center justify-start lg:justify-center gap-8 py-6 overflow-x-scroll scrollbar-none">
        {isLoading ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
          <>
            <motion.div
              whileTap={{ scale: 0.75 }}
              className={`group ${
                filter === ""
                  ? "bg-orange-200 border-4 border-orange-300"
                  : " bg-card border-2 border-stone-100"
              } w-24 min-w-[94px] h-28 cursor-pointer rounded-lg drop-shadow-xl flex flex-col gap-3 items-center justify-center hover:bg-orange-200 hover:border-4 hover:border-orange-300`}
              onClick={() => {
                setrequestfilter({
                  Id: user?.UserId,
                  Qtycontrolled: 2,
                  QuantitySupplied: 2,
                  ExpiryDate: 2,
                  TimeExpiryDate: 2,
                  keyWord: "",
                  filter: ""  
                });
              }}  >
              <div
                className={`w-10 h-10 rounded-full shadow-lg ${
                  filter === "" ? "bg-orange-400" : "bg-orange-200"
                } group-hover:bg-orange-400 flex items-center justify-center`}
              >
                <IoFastFood
                  className={`${
                    filter === "" ? "text-orange-900" : "text-orange-700"
                  } group-hover:text-orange-900 text-lg`}
                />
              </div>
              <p
                className={`text-sm text-center ${
                  filter === "" ? "text-orange-900" : "text-orange-700"
                } group-hover:text-orange-900 font-bold`}
              >
                Tất cả
              </p>
            </motion.div>

            {categories &&
              categories.map((category) => (
                <motion.div
                  whileTap={{ scale: 0.75 }}
                  key={category.CategoryId}
                  className={`group ${
                    filter === category.CategoryId
                      ? "bg-orange-200 border-4 border-orange-300"
                      : " bg-card border-2 border-stone-100"
                  } w-24 min-w-[94px] h-28 cursor-pointer rounded-lg drop-shadow-xl flex flex-col gap-3 items-center justify-center hover:bg-orange-200 hover:border-4 hover:border-orange-300`}
                  onClick={() => {
                    // setFilter(category.CategoryId);
                    setrequestfilter({
                      Id: user?.UserId,
                      Qtycontrolled: 2,
                      QuantitySupplied: 2,
                      ExpiryDate: 2,
                      TimeExpiryDate: 2,
                      keyWord: "",
                      filter: category.CategoryId  
                    });
                  }}
                >
                  <div
                    className={`w-10 h-10 rounded-full shadow-lg ${
                      filter === category.CategoryId
                        ? "bg-orange-400"
                        : "bg-orange-200"
                    } group-hover:bg-orange-400 flex items-center justify-center`}
                  >
                    <IoFastFood
                      className={`${
                        filter === category.CategoryId
                          ? "text-orange-900"
                          : "text-orange-700"
                      } group-hover:text-orange-900 text-lg`}
                    />
                  </div>
                  <p
                    className={`text-sm text-center ${
                      filter === category.CategoryId
                        ? "text-orange-900"
                        : "text-orange-700"
                    } group-hover:text-orange-900 font-bold`}
                  >
                    {category.CategoryName}
                  </p>
                </motion.div>
              ))}
          </>
        )}
      </div>
                      {/**Danh sách */}
      <div className="w-full">
        {isLoading ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
          <FoodListAdmin filter={requestfilter} />
        )}
      </div>
    </div>
  );
};

export default Body;
