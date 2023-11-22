import React, { useEffect } from "react";
import { BiMinus, BiPlus } from "react-icons/bi";
import { motion } from "framer-motion";
import { useState } from "react";
import { useStateValue } from "../context/StateProvider";
import { actionType } from "../context/reducer";
let items = [];
const CartItem = ({ item, setFlag, flag }) => {
  const [{ cartItems }, dispatch] = useStateValue();
  const [qty, setQty] = useState(item.qty);
  const [note, setNote] = useState(" ");
  const [showNote, setShowNote] = useState(false);
  const cartDispatch = () => {
    localStorage.setItem("cartItems", JSON.stringify(items));
    dispatch({
      type: actionType.SET_CARTITEMS,
      cartItems: items,
    });
  };

  const updateQty = (action, id) => {
    if (action == "add") {
      setQty(qty + 1);
      cartItems.map((item) => {
        if (item.FoodListId === id) {
          item.qty += 1;
          setFlag(flag + 1);
        }
      });
      cartDispatch();
    } else {
      // initial state value is one so you need to check if 1 then remove it
      if (qty == 1) {
        items = cartItems.filter((item) => item.FoodListId !== id);
        setFlag(flag + 1);
        cartDispatch();
      } else {
        setQty(qty - 1);
        cartItems.map((item) => {
          if (item.FoodListId === id) {
            item.qty -= 1;
            setFlag(flag - 1);
          }
        });
        cartDispatch();
      }
    }
  };
  const updateNote = ( id, note) => {
      cartItems.map((item) => {
        if (item.FoodListId === id) {
          item.Description = note;
        }
      });
      cartDispatch();
  };
  useEffect(() => {
    items = cartItems;
    console.log(note);
    console.log(items);
  }, [qty, items, note]);

  return (
    <div>
      <div className="w-full p-1 px-2 rounded-lg bg-cartItem flex items-center gap-2">
        {/* Hình ảnh*/}
        <img
          src={item?.UploadImage}
          className="w-20 h-20 max-w-[60px] rounded-full object-contain"
          alt=""
        />

        {/* name section */}
        <div className="flex flex-col gap-2">
          {/* Tên*/}
          <p className="text-base text-gray-50">{item?.FoodName}</p>
          {/* Giá*/}
          <p className="text-sm block text-gray-300 font-semibold">
            {(parseFloat(item?.Price) * qty).toLocaleString()}
            <span className="text-base text-red-500"> vnđ</span>{" "}
          </p>
        </div>

        {/* button section */}
        <div className="group flex items-center gap-2 ml-auto cursor-pointer">
          {/* giảm số lượng*/}
          <motion.div
            whileTap={{ scale: 0.75 }}
            onClick={() => updateQty("remove", item?.FoodListId)}
          >
            <BiMinus className="text-gray-50 " />
          </motion.div>

          <p className="w-5 h-5 rounded-sm bg-cartBg text-gray-50 flex items-center justify-center">
            {qty}
          </p>

          {/* Cộng số lượng*/}
          <motion.div
            whileTap={{ scale: 0.75 }}
            onClick={() => updateQty("add", item?.FoodListId)}
          >
            <BiPlus className="text-gray-50 " />
          </motion.div>
        </div>

        {/* Nút Ghi chú */}
        <button
          className="text-blue-500 cursor-pointer"
          onClick={() => setShowNote(!showNote)}
        >
          {showNote ? "Ẩn ghi chú" : "Ghi chú"}
        </button>
      </div>

      {/* Ô ghi chú */}
      {showNote && (
        <div className="w-full">
          <textarea
            className="w-full p-1 rounded-lg border border-gray-300"
            placeholder="Thêm ghi chú để cửa hàng chuẩn bị món ăn vừa ý nhất cho bạn!"
            value={item?.Description}
            onChange={(e) =>{
              setNote(e.target.value);
              updateNote( item?.FoodListId, e.target.value);
            }}
          />
        </div>
      )}
    </div>
  );
};

export default CartItem;
