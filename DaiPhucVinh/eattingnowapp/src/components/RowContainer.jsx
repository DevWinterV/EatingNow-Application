import React, { useEffect, useRef, useState } from "react";
import { NotFound } from "../assets";
import { useStateValue } from "../context/StateProvider";
import { actionType } from "../context/reducer";
import Modal from 'react-modal';
import { ToastContainer, toast } from "react-toastify";
const customStyles = {
  content: {
    top: '50%',
    left: '50%',
    right: 'auto',
    bottom: 'auto',
    marginRight: '-50%',
    transform: 'translate(-50%, -50%)',
  },
};

const RowContainer = ({ flag, rowData, scrollValue }) => {
  const rowContainer = useRef();
  const [{ cartItems }, dispatch] = useStateValue();
  const [items, setItems] = useState(cartItems);
  const [selecteditems, setselecteditems] = useState(null);
  const [qtyBuy, setqtyBuy] = useState(1);

  let subtitle;
  const [modalIsOpen, setIsOpen] = React.useState(false);

  function openModal(item) {
    if(item.qty == 0 || item == null){
      toast.warning("Sản phẩm hết số lượng");
      return;
    }
    setIsOpen(true);
    setselecteditems(item);
  }

  function afterOpenModal() {
    // references are now sync'd and can be accessed.
    subtitle.style.color = '#f00';
  }

  function closeModal() {
    setselecteditems(null);
    setqtyBuy(1);
    setIsOpen(false);
  }



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
    if(itemToAdd.Qtycontrolled){
      if(qtyBuy > itemToAdd.qty){
        toast.warning(`Số lượng sản phẩm hiện còn ${itemToAdd.qty}`);
        return;
      }
    }
    itemToAdd.Description = "";
    const itemExistsInCart = cartItems.some((item) => item.FoodListId === itemToAdd.FoodListId);
    // Nếu đã có
    if (itemExistsInCart) {
      const updatedCartItems = cartItems.map((item) =>
        item.FoodListId === itemToAdd.FoodListId ? { ...item, qty: qtyBuy } : item
      );
      setItems(updatedCartItems);
      toast.success(`Đã cập nhật ${itemToAdd.FoodName} vào giỏ hàng`);
    } else {
      const newItem = { ...itemToAdd, qty: qtyBuy };
      setItems([...cartItems, newItem]);
      toast.success(`Đã thêm ${itemToAdd.FoodName} vào giỏ hàng`);
    }
    closeModal();
    setqtyBuy(1)
    setselecteditems(null);
  };
  
  return (
    <div className="w-full overflow-x-hidden">
          <ToastContainer/>
          <Modal
            
              isOpen={modalIsOpen}
              onAfterOpen={afterOpenModal}
              onRequestClose={closeModal}
              style={customStyles}
              contentLabel="Nhập số lượng mua"
          >
              <div className="modal-content">
                  <h2 className="modal-title">
                      Nhập số lượng mua cho {selecteditems?.FoodName}
                  </h2>
                  {
                      selecteditems?.Qtycontrolled &&
                      <h3 className="qty-remaining">
                          Số lượng còn {selecteditems?.qty}
                      </h3>
                  }
                  <div className="input-container">
                      <label htmlFor="quantity-input">Nhập số lượng:</label>
                      <input
                          id="quantity-input"
                          className="quantity-input"
                          type="number"
                          min="1"
                          max="20"
                          value={qtyBuy}
                          onChange={(e) => {
                              setqtyBuy(e.target.value);
                          }}
                      />
                  </div>
                  <div className="button-container">
                      <button
                          className="add-to-cart-btn"
                          onClick={() => {
                              addItemToCart(selecteditems);
                          }}
                      >
                          Thêm vào giỏ hàng
                      </button>
                      <button className="close-modal-btn" onClick={closeModal}>
                          Đóng
                      </button>
                  </div>
              </div>
          </Modal>

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
              className="border-4 border-orange-100 bg-white shadow-2xl w-72 h-[310px] md:w-80 md:h-[310px] rounded-3xl py-6 px-4 my-4 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-between relative"
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
         
                </div>
              </div>
         
              <div className="p-3 w-full flex flex-col items-center justify-center text-black">
                {
                    item.qty > 0 && item.Qtycontrolled  ? 
                      <button
                          whileTap={{ scale: 0.70 }}
                          className="rounded-full bg-gradient-to-r from-red-600 to-red-400 text-white flex items-center cursor-pointer hover:shadow-md ml-2 px-3 py-1"
                          onClick={() => openModal(item)}
                      >
                          Chọn mua
                      </button>
                      
                      : item.Qtycontrolled == false ? 
                      <button
                      whileTap={{ scale: 0.70 }}
                      className="rounded-full bg-gradient-to-r from-red-600 to-red-400 text-white flex items-center cursor-pointer hover:shadow-md ml-2 px-3 py-1"
                      onClick={() => openModal(item)}
                      >
                          Chọn mua
                      </button>
                      : 
                    <label>
                      Hết số lượng
                    </label>
                }
               
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
