import React, { useEffect } from "react";
import { AiOutlineAppstoreAdd } from "react-icons/ai";
import { BsSearch } from "react-icons/bs";
import { TbListDetails } from "react-icons/tb";
import { useStateValue } from "../../context/StateProvider";

import { Loader } from "../../components";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  GetListOrderLineDetails,
  TakeOrderHeaderByStoreId,
} from "../../api/store/storeService";

const Order = () => {
  const [isLoading, setIsLoading] = React.useState(false);
  const [data, setData] = React.useState([]);
  const [dataDetails, setDataDetails] = React.useState([]);
  const [{ user }] = useStateValue();
  const [isShown, setIsShown] = React.useState(false);
  const [orderHeaderId, setOrderHeaderId] = React.useState("");

  async function onViewAppearing() {
    setIsLoading(true);
    if (user) {
      let response = await TakeOrderHeaderByStoreId(user?.UserId);
      setData(response.data);
    }
    setIsLoading(false);
  }

  function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = String(date.getFullYear());
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    return `${day}/${month}/${year} ${hours}:${minutes}`;
  }

  function formatMoney(amount) {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
      minimumFractionDigits: 0,
    }).format(amount);
  }

  async function GetOrderLineDetails() {
    setIsLoading(true);
    if (orderHeaderId != "") {
      setIsShown(true);
      let response = await GetListOrderLineDetails(orderHeaderId);
      setDataDetails(response.data);
    }
    setIsLoading(false);
  }

  useEffect(() => {
    onViewAppearing();
  }, []);

  useEffect(() => {
    GetOrderLineDetails();
  }, [orderHeaderId]);

  return (
    <div className="bg-orange-50 h-[100%] basis-80 p-8 overflow-auto no-scrollbar py-5 px-5">
      <div className="flex items-center justify-between">
        <div className="flex items-center border-b-2 pb-2 basis-1/2 gap-2">
          <BsSearch className="text-hoverColor text-[20px] cursor-pointer" />
          <input
            type="text"
            placeholder="Tìm nhóm món ăn..."
            className="border-none outline-none placeholder:text-sm focus:outline-none"
          />
        </div>

        <div className="flex gap-4 items-center">
          <AiOutlineAppstoreAdd className="text-hoverColor cursor-pointer text-[25px] hover:text-[20px] transition-all" />
          <button className="bg-red-600 cursor-pointer text-bodyBg font-semibold py-1 px-4 rounded-[5px] transition-all">
            Quản lý
          </button>
        </div>
        {isShown ? (
          <>
            <div className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none">
              <div
                className="relative w-auto my-6 mx-auto max-w-7xl"
                style={{ width: "1000px" }}
              >
                {/*content*/}
                <div className="border-0 rounded-lg shadow-lg relative flex flex-col w-auto bg-white outline-none focus:outline-none">
                  {/*header*/}
                  <div className="flex items-start justify-between p-5 border-b border-solid border-slate-200 rounded-t">
                    <h2 className="text-2xl font-semibold">
                      Danh sách chi tiết đơn hàng
                    </h2>
                    <button className="p-1 ml-auto bg-transparent border-0 text-black opacity-5 float-right text-3xl leading-none font-semibold outline-none focus:outline-none">
                      <span className="bg-transparent text-black opacity-5 h-6 w-6 text-2xl block outline-none focus:outline-none">
                        ×
                      </span>
                    </button>
                  </div>
                  {/*body*/}
                  <div className="w-full table-auto">
                    <div>
                      <Table className="w-full table-auto">
                        <Thead className="bg-orange-50">
                          <Tr>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              STT
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Hình ảnh
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Tên món
                            </Th>
                            {/* <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Loại món
                            </Th> */}
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Số lượng
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Giá bán
                            </Th>
                            <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              Tổng tiền
                            </Th>
                          </Tr>
                        </Thead>
                        <Tbody className="bg-orange-50">
                          {isLoading ? (
                            <Td colspan="4" className="text-center">
                              <Loader />
                            </Td>
                          ) : dataDetails && dataDetails.length > 0 ? (
                            dataDetails.map((item, index) => (
                              <Tr
                                key={item.OrderLineId}
                                className="bg-orange-50"
                              >
                                <Td className="p-3 text-sm text-orange-900 whitespace-nowrap text-center">
                                  <p className="font-bold text-orange-900 hover:underline">
                                    {index + 1}
                                  </p>
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center flex justify-center">
                                  <img
                                    src={item?.UploadImage}
                                    className="w-20 h-20 max-w-[60px] rounded-full object-contain"
                                    alt=""
                                  />
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {item.FoodName}
                                </Td>
                                {/* <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {item.CategoryId}
                                </Td> */}
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {item.qty}
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {formatMoney(item.Price)}
                                </Td>
                                <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                  {formatMoney(item.TotalPrice)}
                                </Td>
                              </Tr>
                            ))
                          ) : (
                            ""
                          )}
                        </Tbody>
                      </Table>
                    </div>
                  </div>
                  {/*footer*/}
                  <div className="flex items-center justify-end p-6 border-t border-solid border-slate-200 rounded-b">
                    <button
                      className="bg-orange-600 text-white active:bg-emerald-600 font-bold uppercase text-sm px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                      type="button"
                      onClick={() => {
                        setIsShown(false);
                      }}
                    >
                      Đóng
                    </button>
                  </div>
                </div>
              </div>
            </div>
            <div className="opacity-25 fixed inset-0 z-40 bg-black"></div>
          </>
        ) : null}
      </div>

      {/* Title Div */}
      <div className="items-center mt-8">
        <div className="title">
          <div className="p-5 h-screen bg-orange-100">
            <div className="flex justify-between pb-4 items-center">
              <h1 className="text-xl mb-2">Danh Sách Đơn Hàng</h1>
            </div>

            <div className="overflow-auto rounded-lg shadow md:block">
              <Table className="w-full table-auto">
                <Thead className="bg-orange-50">
                  <Tr>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      STT
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Mã đơn hàng
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Khách hàng
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Thành tiền
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Ngày tạo
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                      Chi tiết
                    </Th>
                  </Tr>
                </Thead>
                <Tbody className="bg-orange-50">
                  {isLoading ? (
                    <Td colspan="4" className="text-center">
                      <Loader />
                    </Td>
                  ) : data && data.length > 0 ? (
                    data.map((item, index) => (
                      <Tr key={item.OrderHeaderId} className="bg-orange-50">
                        <Td className="p-3 text-sm text-orange-900 whitespace-nowrap text-center">
                          <p className="font-bold text-orange-900 hover:underline">
                            {index + 1}
                          </p>
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {item.OrderHeaderId}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {item.CustomerName}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {formatMoney(item.IntoMoney)}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {formatDate(item.CreationDate)}
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-green-600 whitespace-nowrap text-center">
                          <button
                            className="p-1.5 text-xs font-medium uppercase tracking-wider text-green-800 bg-green-200 rounded-lg bg-opacity-50"
                            onClick={() => {
                              setOrderHeaderId(item.OrderHeaderId);
                              GetOrderLineDetails();
                            }}
                          >
                            <TbListDetails className="text-2xl" />
                          </button>
                        </Td>
                      </Tr>
                    ))
                  ) : (
                    <div className="text-center">
                      <span>Không có dữ liệu!</span>
                    </div>
                  )}
                </Tbody>
              </Table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Order;
