import * as React from "react";
import * as Fc from "react-icons/fc";
const SiteMap = [
  {
    tabs: [
      {
        name: "Bản điều khiển",
        icon: <Fc.FcStatistics />,
        href: "/",
        isShow: false,
      },
      {
        name: "Bản đồ",
        icon: <Fc.FcMindMap/>,
        isShow: false,
        tabs: [
          {
            name: "Xem vị trí cửa hàng",
            href: "/maps",
            role: "BANDO",
          },
          {
            name: "Xem vị trí tài xế",
            href: "/mapsdlv",
            role: "BANDO",
          },
        ],
      },
      {
        name: "Cửa hàng",
        icon: <Fc.FcShop />,
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
        icon: <Fc.FcAbout />,
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
        icon: <Fc.FcList />,
        isShow: false,
        tabs: [
          {
            name: "Danh sách đơn hàng",
            href: "/order",
            role: "DONHANG",
          },
          {
            name: "Người giao hàng",
            href: "/delivery",
            role: "SHIPPER",
          },
        ],
      },
      {
        name: "Khách hàng",
        icon: <Fc.FcBusinessContact />,
        isShow: false,
        tabs: [
          {
            name: "Thông tin khách hàng",
            href: "/customer",
            role: "KHACHHANG",
          },
        ],
      },
      {
        name: "Phân quyền",
        icon: <Fc.FcSettings />,
        isShow: false,
        tabs: [
          {
            name: "Loại tài khoản",
            href: "/accounttype",
            role: "LOAITAIKHOAN",
          },
          {
            name: "Tài khoản",
            href: "/accounts",
            role: "TAIKHOAN",
          },
        ],
      },
    ],
  },
];
export { SiteMap };
