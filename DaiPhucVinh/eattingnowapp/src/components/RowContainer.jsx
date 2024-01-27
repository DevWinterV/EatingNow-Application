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


  // Cập nhật Fucntion Thêm mới sản phẩm vào giỏ hàng. 
  // Nếu tồn tại Sp trong cart Item thì tăng số lượng
  const addItemToCart = (itemToAdd) => {
    itemToAdd.Description = "";
    const itemExistsInCart = cartItems.some((item) => item.FoodListId === itemToAdd.FoodListId);
    // Nếu đã có
    if (itemExistsInCart) {
      const updatedCartItems = cartItems.map((item) =>
        item.FoodListId === itemToAdd.FoodListId ? { ...item, qty: item.qty + 1 } : item
      );
      setItems(updatedCartItems);
    } else {
      
      const newItem = { ...itemToAdd, qty: 1, };
      setItems([...cartItems, newItem]);
    }
  };
  
  return (
    <div className="w-full overflow-x-hidden">
      <div
        ref={rowContainer}
        className={`w-full flex gap-3 my-5 ${
          flag
            ? "overflow-x-scroll scrollbar-none"
            : "overflow-hidden flex-wrap justify-center"
        }`}
      >
        {rowData && rowData.length > 0 ? (
          rowData.map((item) => (
            <div
              key={item?.FoodListId}
              className="border-4 border-orange-100 bg-white shadow-2xl w-72 h-[220px] md:w-80 md:h-[250px] rounded-3xl py-6 px-4 my-4 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-between relative"
            >
              <div className="w-full flex items-center justify-center">
                <div className="border-4 border-orange-100 h-28 w-28 md:h-36 md:w-36 overflow-hidden rounded-full">
                  <img
                    src={item?.UploadImage}
                    alt=""
                    className="w-full h-full object-cover hover:scale-110 transition duration-300 ease-in-out"
                  />
                </div>
              </div>

              <div className="w-full flex flex-col items-center justify-center mt-2">
                <p className="capitalize text-textColor font-semibold text-sm md:text-base text-center overflow-hidden overflow-ellipsis whitespace-nowrap max-w-full">
                  {item?.FoodName}
                </p>
                <div className="flex items-center mt-2">
                  <p className="text-base text-headingColor font-semibold">
                    {item?.Price.toLocaleString()}
                    <span className="text-sm text-red-500"> vnđ</span>{" "}
                  </p>
                  <motion.div
                    whileTap={{ scale: 0.75 }}
                    className="w-8 h-8 rounded-full bg-red-600 flex items-center justify-center cursor-pointer hover:shadow-md ml-2"
                    onClick={() => addItemToCart(item)}
                  >
                    <MdShoppingBasket className="text-white" />
                  </motion.div>
                </div>
              </div>
            </div>
          ))
        ) : (
          <div className="w-full flex flex-col items-center justify-center">
            <img src={NotFound} className="h-340" alt="Not Found" />
            <p className="text-xl text-headingColor font-semibold my-2">
            </p>
          </div>
        )}
      </div>
    </div>
  );
};

export default RowContainer;
