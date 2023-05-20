import React, { useEffect, useRef, useState } from "react";
import { MdShoppingBasket } from "react-icons/md";
import { motion } from "framer-motion";
import { NotFound } from "../assets";
import { useStateValue } from "../context/StateProvider";
import { actionType } from "../context/reducer";
import { FaStar } from "react-icons/fa";

const RowContainer = ({ flag, rowData, scrollValue }) => {
  const rowContainer = useRef();
  const [{ cartItems }, dispatch] = useStateValue();
  const [items, setItems] = useState(cartItems);

  const addtocart = () => {
    dispatch({
      type: actionType.SET_CARTITEMS,
      cartItems: items,
    });
    localStorage.setItem("cartItems", JSON.stringify(items));
  };

  useEffect(() => {
    rowContainer.current.scrollLeft += scrollValue;
  }, [scrollValue]);

  useEffect(() => {
    addtocart();
  }, [items]);

  return (
    <div
      ref={rowContainer}
      className={`w-full flex gap-3 my-5 scroll-smooth ${
        flag
          ? "overflow-x-scroll scrollbar-none"
          : "overflow-hidden flex-wrap justify-center"
      }`}
    >
      {rowData && rowData.length > 0 ? (
        rowData.map((item) => (
          <div
            key={item?.FoodListId}
            className="border-4 border-orange-100 bg-white shadow-2xl w-275 h-[195px] min-w-[275px] md:w-300 md:min-w-[300px] rounded-3xl py-12 px-4 my-4 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative"
          >
            <div className="w-full flex items-center justify-between">
              <div className="border-4 border-orange-100 h-36 w-full overflow-hidden rounded-3xl">
                <img
                  src={item?.UploadImage}
                  alt=""
                  className="w-full h-full object-cover aspect-square hover:scale-110 transition duration-300 ease-in-out"
                />
              </div>

              <div className="gap-2 w-full flex flex-col items-center justify-center">
                <p className="capitalize text-textColor font-semibold text-base md:text-lg text center">
                  {item?.FoodName}
                </p>
                <div className="flex items-center gap-8">
                  <p className="text-lg text-headingColor font-semibold">
                    {item?.Price.toLocaleString()}
                    <span className="text-base text-red-500"> vnđ</span>{" "}
                  </p>
                </div>
                <motion.div
                  whileTap={{ scale: 0.75 }}
                  className="w-8 h-8 rounded-full bg-red-600 flex items-center justify-center cursor-pointer hover:shadow-md"
                  onClick={() => setItems([...cartItems, item])}
                >
                  <MdShoppingBasket className="text-white" />
                </motion.div>
              </div>
            </div>
          </div>
        ))
      ) : (
        <div className="w-full flex flex-col items-center justify-center">
          <img src={NotFound} className="h-340" />
          <p className="text-xl text-headingColor font-semibold my-2">
            Chưa có món ăn này!
          </p>
        </div>
      )}
    </div>
  );
};

export default RowContainer;
