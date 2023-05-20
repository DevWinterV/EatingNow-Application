import React, { useEffect } from "react";
import { motion } from "framer-motion";
import { TbEdit } from "react-icons/tb";
import { FiXCircle } from "react-icons/fi";
import { NotFound } from "../assets";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { DeleteFoodList } from "../api/foodlist/foodListService";
import { useStateValue } from "../context/StateProvider";
import {
  TakeAllFoodListByStoreId,
  TakeFoodListByStoreId,
} from "../api/store/storeService";
import Loader from "./Loader";
import { FaStar } from "react-icons/fa";

const FoodListAdmin = ({ filter }) => {
  const history = useNavigate();
  const [isLoading, setIsLoading] = React.useState(false);
  const [data, setData] = React.useState([]);
  const [{ user }] = useStateValue();

  async function onViewAppearing() {
    setIsLoading(true);
    if (filter === "") {
      let response = await TakeAllFoodListByStoreId(user?.UserId);
      setData(response.data);
    } else {
      let response = await TakeFoodListByStoreId(filter);
      setData(response.data);
    }
    setIsLoading(false);
  }

  useEffect(() => {
    onViewAppearing();
  }, [filter]);

  return (
    <div className="w-full flex items-center gap-5 scroll-smooth overflow-hidden flex-wrap justify-center">
      {isLoading ? (
        <div className="text-center pt-20">
          <Loader />
        </div>
      ) : (
        <>
          {data && data.length > 0 ? (
            data.map((n) => (
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
                      <button className="p-1.5 text-xs font-medium uppercase tracking-wider text-white bg-orange-700 rounded-full bg-opacity-50">
                        <FiXCircle
                          className="text-2xl"
                          onClick={() => {
                            Swal.fire({
                              title: "Xóa món ăn ?",
                              text: "Bạn muốn xóa món ăn này ra khỏi danh sách !",
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
                                        text: "Không thể xóa. Nhóm món ăn này đã tồn tại sản phẩm!",
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
                Chưa có món ăn này!
              </p>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default FoodListAdmin;
