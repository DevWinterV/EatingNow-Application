import React, { useEffect } from "react";
import { motion } from "framer-motion";
import { TbEdit, TbGift } from "react-icons/tb";
import { FiXCircle } from "react-icons/fi";
import { NotFound } from "../assets";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { ChangeIsNoiBatFoodList, ChangeIsNewFoodList, DeleteFoodList } from "../api/foodlist/foodListService";
import { useStateValue } from "../context/StateProvider";
import {
  TakeAllFoodListByStoreId,
  TakeFoodListByStoreId,
} from "../api/store/storeService";
import Loader from "./Loader";
import { AiOutlineStar } from "react-icons/ai";
import { BsBookmarkStar } from "react-icons/bs";

const FoodListAdmin = ({ filter }) => {
  const [keyword, setkeyword] = React.useState('');
  const history = useNavigate();
  const [isLoading, setIsLoading] = React.useState(false);
  const [data, setData] = React.useState([]);
  const [dataFiller, setdataFiller] = React.useState([]);
  const [{ user }] = useStateValue();

  async function onViewAppearing() {
    setIsLoading(true);
    if (filter.filter === "") {
      let response = await TakeAllFoodListByStoreId(filter);
      setData(response.data);
      setdataFiller(response.data);
    } else {
      let response = await TakeFoodListByStoreId(filter.filter);
      setData(response.data);
      setdataFiller(response.data);
    }
    setIsLoading(false);
  }
  useEffect(() => {
    onViewAppearing();
  }, [filter]);

  const handleChangkeyWord = (e) => {
    const keyword = e.target.value;
    setkeyword(keyword);
  };

  useEffect(() => {
    if (keyword && keyword.trim() !== "") {
      const filteredData = data.filter(item =>
        item.FoodName.toLowerCase().includes(keyword.toLowerCase()) ||
        item.Description.toLowerCase().includes(keyword.toLowerCase())
      );
      setdataFiller(filteredData);
    } else {
      setdataFiller(data);

    }
  }, [keyword]);

  return (
    <div>
      <div class="search-container">
          <label class="label">Tìm kiếm sản phẩm:</label>
          <div class="">
              <input id="searchInput" value={keyword} type="text" onChange={handleChangkeyWord} placeholder="Nhập tên sản phẩm, mô tả sản phẩm tìm kiếm" class="search-input" />
          </div>
      </div> 
      <div className="w-full flex items-center gap-5 scroll-smooth overflow-hidden flex-wrap justify-center">
        
        {isLoading ? (
        <div className="text-center pt-20">
          <Loader />
        </div>
        ) : (
        <>
          {dataFiller && dataFiller.length > 0 ? (
            dataFiller.map((n) => (
              <div
                key={n.FoodListId}
                className="border-4 border-orange-100 w-275 h-full min-w-[275px] md:w-200 md:min-w-[200px] bg-white shadow-2xl rounded-3xl py-2 px-4 my-4 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative"
              >
                <div className="border-4 border-orange-100 h-36 w-full overflow-hidden rounded-3xl">
                  <img
                    src={n?.UploadImage}
                    alt=""
                    className="w-full h-full object-cover aspect-square hover:scale-110 transition duration-300 ease-in-out"
                  />
                </div>

                <div className="gap-1 w-full flex flex-col items-center justify-center">
                  <div className="w-full flex flex-col items-center justify-center pt-2">
                    <p className="capitalize text-textColor font-semibold text-base md:text-lg">
                      {n?.FoodName}
                    </p>
                  </div>

                  <div className="w-full flex flex-col items-center justify-center">
                    <p className="text-textColor font-semibold text-base md:text-lg">
                      {n?.Price.toLocaleString()}
                      <span className="text-base text-red-500"> vnđ</span>{" "}
                    </p>
                  </div>

                  <div className="w-full flex flex-col items-center justify-center">
                    <td className="text-sm text-gray-700 whitespace-nowrap gap-3 flex">
                      <button
                        onClick={() => {
                          history("/editfoodlist/" + n.FoodListId, {
                            state: { data: n },
                          });
                        }}
                        className="p-1.5 text-xs font-medium uppercase tracking-wider text-white bg-orange-700 rounded-full bg-opacity-50"
                      >
                        <TbEdit className="text-2xl" />
                      </button>
                      
                      <button className={`p-1.5 text-xs font-medium uppercase tracking-wider text-white bg-${n.IsNoiBat === true ? "green-500" : "orange-700"} rounded-full bg-opacity-50`}>
                        <AiOutlineStar
                          className="text-2xl"
                          onClick={() => {
                            Swal.fire({
                              title: n.IsNoiBat ? "Tắt sản phẩm nổi bật !" : "Sản phẩm nổi bật !",
                              text:  n.IsNoiBat ? `Bạn muốn tắt trạng thái nổi bật của ${n.FoodName} không ?` : `Bạn muốn đặt món ${n.FoodName} thành món ăn nổi bật ?` ,
                              icon: "warning",
                              showCancelButton: true,
                              confirmButtonColor: "#3085d6",
                              cancelButtonColor: "#d33",
                              confirmButtonText: "Đồng ý",
                              cancelButtonText: "Hủy",
                            }).then((result) => {
                              if (result.isConfirmed) {
                                ChangeIsNoiBatFoodList(n.FoodListId).then(
                                  (response) => {
                                    if (response.success) {
                                      Swal.fire(
                                        "Thành công!",
                                        "Cập nhật thành công.",
                                        "success"
                                      );
                                      onViewAppearing();
                                    } else {
                                      Swal.fire({
                                        title: "Lỗi!",
                                        text: "Cập nhật không thành công... Vui lòng thử lại",
                                        icon: "error",
                                        confirmButtonText: "OK",
                                      });
                                      return;
                                    }
                                  }
                                );
                              }
                            });
                          }}
                        />
                      </button>

                      <button className={`p-1.5 text-xs font-medium uppercase tracking-wider text-white bg-${n.IsNew === true ? "green-500" : "orange-700"} rounded-full bg-opacity-50`}>
                        <BsBookmarkStar
                          className="text-2xl"
                          onClick={() => {
                            Swal.fire({
                              title: n.IsNew ? "Tắt sản phẩm mới!" : "Sản phẩm mới",
                              text:  n.IsNew ? `Bạn có muốn tắt trạng thái sản phẩm mới của ${n.FoodName} không ?`: `Bạn món đặt sản phẩm ${n.FoodName} thành trạng thái sản phẩm mới ?`,
                              icon: "warning",
                              showCancelButton: true,
                              confirmButtonColor: "#3085d6",
                              cancelButtonColor: "#d33",
                              confirmButtonText: "Đồng ý",
                              cancelButtonText: "Hủy",
                            }).then((result) => {
                              if (result.isConfirmed) {
                                ChangeIsNewFoodList(n.FoodListId).then(
                                  (response) => {
                                    if (response.success) {
                                      Swal.fire(
                                        "Thành công!",
                                        "Cập nhật thành công.",
                                        "success"
                                      );
                                      onViewAppearing();
                                    } else {
                                      Swal.fire({
                                        title: "Lỗi!",
                                        text: "Cập nhật không thành công... Vui lòng thử lại",
                                        icon: "error",
                                        confirmButtonText: "OK",
                                      });
                                      return;
                                    }
                                  }
                                );
                              }
                            });
                          }}
                        />
                      </button>
                      
                      <button className="p-1.5 text-xs font-medium uppercase tracking-wider text-white bg-orange-700 rounded-full bg-opacity-50">
                        <FiXCircle
                          className="text-2xl"
                          onClick={() => {
                            Swal.fire({
                              title: "Xóa sản phẩm ?",
                              text: "Bạn muốn xóa sản phẩm này ra khỏi danh sách !",
                              icon: "warning",
                              showCancelButton: true,
                              confirmButtonColor: "#3085d6",
                              cancelButtonColor: "#d33",
                              confirmButtonText: "Xác nhận !",
                            }).then((result) => {
                              if (result.isConfirmed) {
                                DeleteFoodList(n.FoodListId).then(
                                  (response) => {
                                    if (response.success) {
                                      Swal.fire(
                                        "Thành công!",
                                        "Xóa dữ liệu thành công.",
                                        "success"
                                      );
                                      onViewAppearing();
                                    } else {
                                      Swal.fire({
                                        title: "Lỗi!",
                                        text: "Không thể xóa. Sản phẩm này đã có đơn hàng!",
                                        icon: "error",
                                        confirmButtonText: "OK",
                                      });
                                      return;
                                    }
                                  }
                                );
                              }
                            });
                          }}
                        />
                      </button>
                    </td>
                  </div>
                </div>
              </div>
            ))
          ) : (
            <div className="w-full flex flex-col items-center justify-center">
              <img src={NotFound} className="h-340" />
              <p className="text-xl text-headingColor font-semibold my-2">
                Không có dữ liệu sản phẩm!
              </p>
            </div>
          )}
        </>
      )}
      </div>
    </div>
  );
};

export default FoodListAdmin;
