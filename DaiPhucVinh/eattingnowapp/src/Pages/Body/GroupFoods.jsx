import React, { useEffect } from "react";
import { AiOutlineAppstoreAdd } from "react-icons/ai";
import { BsSearch } from "react-icons/bs";
import { TakeCategoryByStoreId } from "../../api/store/storeService";
import { TbEdit } from "react-icons/tb";
import { IoMdAdd } from "react-icons/io";
import {
  AiOutlineDelete,
  AiOutlineCheck,
  AiOutlineClose,
} from "react-icons/ai";
import { useStateValue } from "../../context/StateProvider";

import Swal from "sweetalert2";
import {
  CreateCategoryList,
  DeleteCategoryList,
  UpdateCategoryList,
} from "../../api/CategoryList/categoryListService";
import { Loader } from "../../components";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";

const Statistical = () => {
  const [isLoading, setIsLoading] = React.useState(false);
  const [data, setData] = React.useState([]);
  const [{ user }] = useStateValue();
  const [showModal, setShowModal] = React.useState(false);
  const [checkEdit, setCheckEdit] = React.useState(null);
  const [request, setRequest] = React.useState({
    CategoryId: 0,
    CategoryName: "",
    UserId: user.UserId,
    Status: 0,
  });
  async function onViewAppearing() {
    setIsLoading(true);
    if (user) {
      let response = await TakeCategoryByStoreId(user?.UserId);
      setData(response.data);
    }
    setIsLoading(false);
  }

  async function SaveGroupsFood() {
    if (request.CategoryName.trim() == "") {
      Swal.fire("Lỗi!", "Tên không được để trống!", "error");
    } else {
      let response = await CreateCategoryList(request);
      if (!response.success) {
        Swal.fire({
          title: "Lỗi!",
          text: "Lưu dữ liệu không thành công, vui lòng kiểm tra lại dữ liệu đã nhập !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      } else {
        Swal.fire({
          title: "Thành công!",
          text: "Lưu dữ liệu thành công!",
          icon: "success",
          confirmButtonText: "OK",
        });
        onViewAppearing();
        setShowModal(false);
      }
    }
  }

  useEffect(() => {
    onViewAppearing();
  }, []);

  const [currentItem, setCurrentItem] = React.useState({
    CategoryId: 0,
    CategoryName: "",
    UserId: user.UserId,
    Status: 0,
  });

  async function UpdateGroupsFood() {
    if (currentItem.CategoryName.trim() == "") {
      Swal.fire("Lỗi!", "Tên không được để trống!", "error");
    } else {
      let response = await UpdateCategoryList(currentItem);
      if (!response.success) {
        Swal.fire({
          title: "Lỗi!",
          text: "Lưu dữ liệu không thành công, vui lòng kiểm tra lại dữ liệu đã nhập !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      } else {
        Swal.fire({
          title: "Thành công!",
          text: "Lưu dữ liệu thành công!",
          icon: "success",
          confirmButtonText: "OK",
        });
        onViewAppearing();
        setCheckEdit(false);
      }
    }
  }

  return (
    <div className="bg-white h-[100%] basis-80 p-8 overflow-auto no-scrollbar py-5 px-5">
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
      </div>

      {/* Title Div */}
      <div className="items-center mt-8">
        <div className="title">
          <div className="p-5 h-screen bg-white">
            <div className="flex justify-between pb-4 items-center">
              <h1 className="text-xl mb-2">Danh Sách Nhóm Món Ăn</h1>

              <button
                type="button"
                className="text-red-700 hover:text-white border border-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 dark:border-green-500 dark:text-green-500 dark:hover:text-white dark:hover:bg-green-600 dark:focus:ring-green-800"
                onClick={() => setShowModal(true)}
              >
                <span className="justify-between flex items-center">
                  Thêm mới
                  <IoMdAdd className="text-2xl pl-1" />
                </span>
              </button>
              {showModal ? (
                <>
                  <div className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none">
                    <div className="relative w-auto my-6 mx-auto max-w-sm">
                      {/*content*/}
                      <div className="border-0 rounded-lg shadow-lg relative flex flex-col w-full bg-white outline-none focus:outline-none">
                        {/*header*/}
                        <div className="flex items-start justify-between p-5 border-b border-solid border-slate-200 rounded-t">
                          <h2 className="text-2xl font-semibold">
                            Thêm nhóm món mới
                          </h2>
                          <button
                            className="p-1 ml-auto bg-transparent border-0 text-black opacity-5 float-right text-3xl leading-none font-semibold outline-none focus:outline-none"
                            onClick={() => setShowModal(false)}
                          >
                            <span className="bg-transparent text-black opacity-5 h-6 w-6 text-2xl block outline-none focus:outline-none">
                              ×
                            </span>
                          </button>
                        </div>
                        {/*body*/}
                        <div className="p-4">
                          <div>
                            <label className="block mb-2 text-sm font-medium text-black">
                              Tên Nhóm Món
                            </label>
                            <input
                              type="text"
                              className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:border-gray-500 dark:placeholder-gray-400 text-black"
                              placeholder="Vd: Cơm sườn cay . . ."
                              onChange={(e) => {
                                setRequest({
                                  ...request,
                                  CategoryName: e.target.value,
                                });
                              }}
                            />
                          </div>
                        </div>
                        {/*footer*/}
                        <div className="flex items-center justify-end p-6 border-t border-solid border-slate-200 rounded-b">
                          <button
                            className="text-red-500 background-transparent font-bold uppercase px-6 py-2 text-sm outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                            type="button"
                            onClick={() => setShowModal(false)}
                          >
                            Đóng
                          </button>
                          <button
                            className="bg-emerald-500 text-white active:bg-emerald-600 font-bold uppercase text-sm px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                            type="button"
                            onClick={SaveGroupsFood}
                          >
                            Lưu
                          </button>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="opacity-25 fixed inset-0 z-40 bg-black"></div>
                </>
              ) : null}
            </div>

            <div className="overflow-auto rounded-lg shadow md:block">
              <Table className="w-full table-auto">
                <Thead className="bg-orange-50">
                  <Tr>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-left text-center">
                      STT
                    </Th>
                    <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-left text-center">
                      Tên Nhóm Món
                    </Th>
                    <Th
                      colspan="2"
                      className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center"
                    >
                      Chức Năng
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
                      <Tr key={item.CategoryId} className="bg-orange-50">
                        <Td className="p-3 text-sm text-orange-900 whitespace-nowrap text-center">
                          <p className="font-bold text-orange-900 hover:underline">
                            {index + 1}
                          </p>
                        </Td>
                        <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                          {checkEdit == item.CategoryId ? (
                            <input
                              type="text"
                              value={currentItem.CategoryName}
                              className="text-center m-auto block max-w-sm bg-gray-50 border border-gray-300 font-bold text-orange-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:border-gray-500 dark:placeholder-gray-400 text-black"
                              placeholder="Vd: Cơm sườn cay . . ."
                              onChange={(e) => {
                                setCurrentItem({
                                  ...currentItem,
                                  CategoryName: e.target.value,
                                });
                              }}
                            />
                          ) : (
                            item.CategoryName
                          )}
                        </Td>
                        <Td className="p-3 gap-10 flex text-sm text-orange-900 whitespace-nowrap justify-center">
                          {checkEdit == item.CategoryId ? (
                            <>
                              <button
                                onClick={UpdateGroupsFood}
                                className="p-1.5 text-xs font-medium uppercase tracking-wider text-green-900 bg-green-300 rounded-lg bg-opacity-50"
                              >
                                <AiOutlineCheck className="text-2xl" />
                              </button>
                              <button
                                onClick={() => setCheckEdit(null)}
                                className="p-1.5 text-xs font-medium uppercase tracking-wider text-orange-900 bg-orange-300 rounded-lg bg-opacity-50"
                              >
                                <AiOutlineClose className="text-2xl" />
                              </button>
                            </>
                          ) : (
                            <button
                              onClick={() => {
                                setCheckEdit(item.CategoryId);
                                setCurrentItem(item);
                              }}
                              className="p-1.5 text-xs font-medium uppercase tracking-wider text-green-800 bg-green-200 rounded-lg bg-opacity-50"
                            >
                              <TbEdit className="text-2xl" />
                            </button>
                          )}
                        </Td>
                        <Td>
                          <button
                            className="p-1.5 text-xs font-medium uppercase tracking-wider text-red-800 bg-red-200 rounded-lg bg-opacity-50"
                            onClick={() => {
                              Swal.fire({
                                title: "Xóa nhóm món ăn ?",
                                text: "Bạn muốn xóa nhóm món ăn này ra khỏi danh sách !",
                                icon: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#3085d6",
                                cancelButtonColor: "#d33",
                                confirmButtonText: "Xác nhận !",
                              }).then((result) => {
                                if (result.isConfirmed) {
                                  DeleteCategoryList(item.CategoryId).then(
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
                          >
                            <AiOutlineDelete className="text-2xl" />
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

export default Statistical;
