import React from "react";
import { AiOutlineAppstoreAdd } from "react-icons/ai";
import { BsSearch } from "react-icons/bs";

const Statistical = () => {
  return (
    <div className="bg-bodyBg h-[100%] basis-80 p-8">
      {/* <div className="flex items-center justify-between">
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
          <button className="bg-sideMenuBg cursor-pointer text-bodyBg font-semibold py-1 px-4 rounded-[5px] hover:bg-[#55545e] transition-all">
            Quản lý
          </button>
        </div>
      </div> */}

      {/* Title Div */}
      <div className="flex items-center justify-between mt-2">
        <div className="title">
          <h1 className="text-[25px] text-titleColor tracking-[1px] font-black">
            Cài đặt
          </h1>
        </div>
      </div>

      {/* Form chỉnh sửa thông tin */}
      <div className="flex items-center justify-between mt-2">
        <div className="flex">
          <div className="mt-2">
            <input type="text"
                placeholder="Nhập tên cửa hàng ...."
            />
          </div>

          <div className="mt-2 ml-2">
            <input type="text"
                  placeholder="Nhập địa chỉ cửa hàng ...."
            />
          </div>

          <div className="mt-2 ml-2">
            <input type="text"
                  placeholder="Giờ mở cửa của cửa hàng ?...."
            />
          </div>
        </div>

        <div className="flex">
          <div className="mt-2 ml-2">
            <input type="text"
              placeholder="Tên người đại diện ..."
            />
          </div>
          <div className="mt-2 ml-2">
            <select name="selectTinh" id="selectTinh">
            </select>
          </div>
          <div className="mt-2 ml-2">
            <select name="selectLoai" id="selectLoai">
            </select>
          </div>
        </div>
      </div>


      <button>Lưu cập nhật</button>

    </div>
  );
};

export default Statistical;
