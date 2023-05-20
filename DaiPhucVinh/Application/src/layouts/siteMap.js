import * as React from "react";
import * as Fc from "react-icons/fc";
const SiteMap = [
  {
    tabs: [
      {
        name: "Dashboard",
        icon: <Fc.FcHome />,
        href: "/",
        isShow: false,
      },
      {
        name: "Cửa hàng",
        icon: <Fc.FcTodoList />,
        isShow: false,
        tabs: [
          {
            name: "Loại hình thức ăn",
            href: "/cuisine",
            role: "CUISINE",
          },
          {
            name: "Danh sách cửa hàng",
            href: "/store",
            role: "STORE",
          },
        ],
      },
      {
        name: "Địa điểm",
        icon: <Fc.FcTodoList />,
        isShow: false,
        tabs: [
          {
            name: "Tỉnh thành",
            href: "/province",
            role: "TINH",
          },
          {
            name: "Huyện",
            href: "/district",
            role: "HUYEN",
          },
          {
            name: "Thị xã",
            href: "/ward",
            role: "XA",
          },
        ],
      },
      {
        name: "Đơn Hàng",
        icon: <Fc.FcTodoList />,
        isShow: false,
        tabs: [
          {
            name: "Danh sách đơn hàng",
            href: "/cuisine",
            role: "NHOMSANPHAM",
          },
          {
            name: "Người giao hàng",
            href: "/products",
            role: "SANPHAM",
          },
        ],
      },
      {
        name: "Khách hàng",
        icon: <Fc.FcTodoList />,
        isShow: false,
        tabs: [
          {
            name: "Thông tin khách hàng",
            href: "/cuisine",
            role: "NHOMSANPHAM",
          },
        ],
      },
      {
        name: "Phân quyền",
        icon: <Fc.FcTodoList />,
        isShow: false,
        tabs: [
          {
            name: "Loại tài khoản",
            href: "/accounttype",
            role: "LOAITAIKHOAN",
          },
          {
            name: "Tài khoản",
            href: "/account",
            role: "TAIKHOAN",
          },
        ],
      },
    ],
  },
];
export { SiteMap };
