import React, { useEffect, useState } from "react";
import { IoFastFood } from "react-icons/io5";
import { motion } from "framer-motion";
import RowContainer from "./RowContainer";
import {
  TakeAllFoodListByStoreId,
  TakeFoodListByStoreId,
} from "../api/store/storeService";
import Loader from "./Loader";

const MenuContainer = ({ data, state }) => {
  const [filter, setFilter] = useState("");
  const [rowData, setRowData] = useState([]);
  const [isLoading, setIsLoading] = React.useState(false);

  async function onViewAppearing() {
    setIsLoading(true);
    if (filter === "") {
      let response = await TakeAllFoodListByStoreId(state);
      setRowData(response.data);
    } else {
      let response = await TakeFoodListByStoreId(filter);
      setRowData(response.data);
    }
    setIsLoading(false);
  }
  useEffect(() => {
    onViewAppearing();
  }, [filter]);

  return (
    <section className="w-full my-6" id="menu">
      <div className="w-full flex flex-col items-center justify-center">
        <p className="text-2xl font-semibold capitalize text-headingColor px-4 py-4 relative before:absolute before:rounded-lg before:content before:w-16 before:h-1 before:-bottom-2 before:left-0 before:bg-gradient-to-tr from-orange-400 to-orange-600 transition-all ease-in-out duration-100 mr-auto">
          Menu
        </p>

        <div className="w-full flex items-center justify-start lg:justify-center gap-8 py-6 overflow-x-scroll scrollbar-none">
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
          {data &&
            data.map((category) => (
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
        </div>

        <div className="w-full ">
          {isLoading ? (
            <div className="text-center pt-20">
              <Loader />
            </div>
          ) : (
            <RowContainer flag={false} rowData={rowData} />
          )}
        </div>
      </div>
    </section>
  );
};

export default MenuContainer;
