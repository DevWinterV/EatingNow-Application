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

const Body = () => {
  const [isLoading, setIsLoading] = React.useState(false);
  const history = useNavigate();
  const [filter, setFilter] = useState("");
  const [categories, setCategories] = useState([]);
  const [{ user }] = useStateValue();

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

  return (
    <div className="bg-orange-50 h-[100%] basis-80 p-8 overflow-auto no-scrollbar py-5 px-5">
      <div className="flex items-center justify-between">
        <div className="flex items-center border-b-2 pb-2 basis-1/2 gap-2">
          <BsSearch className="text-hoverColor text-[20px] cursor-pointer" />
          <input
            type="text"
            placeholder="Tìm món ăn..."
            className="border-none outline-none placeholder:text-sm focus:outline-none"
          />
        </div>

        <div className="flex gap-4 items-center">
          <AiOutlineAppstoreAdd className="text-hoverColor cursor-pointer text-[25px] hover:text-[20px] transition-all" />
          <button className="bg-red-600 cursor-pointer text-bodyBg font-semibold py-1 px-4 rounded-[5px] transition-all">
            Quản lý
          </button>
        </div>
      </div>

      {/* Title Div */}
      <div className="items-center justify-between mt-8">
        <div className="title">
          <div className="flex justify-between pb-4 items-center">
            <h1 className="text-xl mb-2 text-red-700 font-bold">
              Danh Sách Món Ăn
            </h1>
            <button
              type="button"
              className="text-white bg-red-600 focus:ring-4 focus:outline-none focus:ring-red-300 font-bold rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2"
              onClick={() => {
                history("/createfoodlist");
              }}
            >
              <span className="justify-between flex items-center">
                Thêm Món Mới
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
              onClick={() => setFilter("")}
            >
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
                  onClick={() => setFilter(category.CategoryId)}
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

      <div className="w-full">
        {isLoading ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
          <FoodListAdmin filter={filter} />
        )}
      </div>
    </div>
  );
};

export default Body;
