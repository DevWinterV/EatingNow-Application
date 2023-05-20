import * as React from "react";
import { Breadcrumb } from "../../controls";
import Swal from "sweetalert2";
import { useLocation, useNavigate } from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  TakeProvinceById,
  TakeAllProvince,
} from "../../api/province/provinceService";
import LeafletMap from "../components/LeafletMap";
import Select from "react-select";
import { TakeAllCuisine } from "../../api/categoryItem/categoryItemService";
import { CreateNewStore } from "../../api/store/storeService";
export default function CreateProvince() {
  const [latitude, setLatitude] = React.useState("");
  const [longitude, setLongitude] = React.useState("");
  const [itemCuisine, setItemCuisine] = React.useState([]);
  const [defaultItemCategory, setDefaultItemCategory] = React.useState({
    value: "",
    label: "Tất cả",
  });
  async function onFillItemCategory() {
    let itemCategoryResponse = await TakeAllCuisine();
    if (itemCategoryResponse.success) {
      setItemCuisine([
        {
          value: "",
          label: "Tất cả",
        },
        ...itemCategoryResponse.data.map((e) => {
          return {
            value: e.CuisineId,
            label: e.Name,
          };
        }),
      ]);
    }
  }
  const [selectedImage, setSelectedImage] = React.useState();
  function onChange(e) {
    setSelectedImage(e.target.files[0]);
    setRequest({
      ...request,
      AbsoluteImage: e.target.files[0].name,
    });
  }
  const [itemProvince, setItemProvince] = React.useState([]);
  const [defaultItemProvince, setDefaultItemProvince] = React.useState({
    value: "",
    label: "Vui lòng chọn tỉnh",
  });

  async function onFillItemProvince() {
    let itemProvinceResponse = await TakeAllProvince();
    if (itemProvinceResponse.success) {
      setItemProvince([
        {
          value: "",
          label: "Vui lòng chọn tỉnh",
        },
        ...itemProvinceResponse.data.map((e) => {
          return {
            value: e.ProvinceId,
            label: e.Name,
          };
        }),
      ]);
    }
  }

  const history = useNavigate();
  const { state } = useLocation();
  const breadcrumbSources = [
    {
      name: "Danh sách video",
      href: "/province",
    },
    {
      name: state?.data?.ProvinceId > 0 ? "Cập nhật" : "Thêm mới",
      active: true,
    },
  ];
  const [request, setRequest] = React.useState({
    UserId: 0,
    AbsoluteImage: "",
    FullName: "",
    Description: "",
    OpenTime: "",
    ProvinceId: "",
    CuisineId: "",
    Email: "",
    Address: "",
    OwnerName: "",
    Phone: "",
    Latitude: "",
    Longitude: "",
    Status: 1,
  });

  function onBack() {
    history("/province");
  }
  async function onSubmit() {
    if (request.UserId == 0) {
      let data = new FormData();
      if (selectedImage !== undefined) {
        data.append("file[]", selectedImage, selectedImage.name);
      }
      data.append("form", JSON.stringify(request));
      let response = await CreateNewStore(data);
      if (!response.success) {
        Swal.fire({
          title: "Lỗi!",
          text: "Lưu dữ liệu không thành công, vui lòng kiểm tra lại dữ liệu đã nhập !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      let confirm = await Swal.fire({
        title: "Thành công!",
        text: "Lưu dữ liệu thành công!",
        icon: "success",
        confirmButtonText: "OK",
      });
      if (confirm.isConfirmed) {
        if (request.Id == 0) {
          onBack();
        } else {
          // ở lại trang
        }
      }
    } else {
      let data = new FormData();
      if (selectedImage !== undefined) {
        data.append("file[]", selectedImage, selectedImage.name);
      }
      data.append("form", JSON.stringify(request));
      let response = await UpdateNewCuisine(data);
      if (!response.success) {
        Swal.fire({
          title: "Lỗi!",
          text: "Lưu dữ liệu không thành công 11111, vui lòng kiểm tra lại dữ liệu đã nhập !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      let confirm = await Swal.fire({
        title: "Thành công!",
        text: "Lưu dữ liệu thành công!",
        icon: "success",
        confirmButtonText: "OK",
      });
      if (confirm.isConfirmed) {
        if (request.CuisineId == 0) {
          onBack();
        } else {
          onViewAppearing();
          // ở lại trang
        }
      }
    }
  }
  async function onViewAppearing() {
    if (state?.data) {
      var response = await TakeProvinceById(state?.data.ProvinceId);
      setRequest({
        ProvinceId: response.Item.ProvinceId,
        Name: response.Item.Name || "",
      });
    }
    await onFillItemProvince();
    await onFillItemCategory();
  }

  function CallBackLatLng(lat) {
    setLatitude(lat.lat);
    setLongitude(lat.lng);
  }

  async function onChangeRequest() {
    setRequest({
      ...request,
      Latitude: latitude,
      Longitude: longitude,
    });
  }

  console.log("request ", request);

  React.useEffect(() => {
    onViewAppearing();
  }, []);
  React.useEffect(() => {
    onChangeRequest();
  }, [latitude]);
  return (
    <>
      <Breadcrumb
        title={
          request.ProvinceId > 0 ? "Cập nhật cửa hàng" : "Thêm mới cửa hàng"
        }
        sources={breadcrumbSources}
      />

      <div>
        <LeafletMap onMapClick={CallBackLatLng} />
      </div>

      <div className="row">
        <div className="col-sm-4">
          <label style={{ fontSize: "12px" }} className="form-label fw-bold">
            Thông tin cửa hàng
          </label>
        </div>
      </div>
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Tên cửa hàng</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  defaultValue={request.FullName}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      FullName: e.target.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Giờ mở cửa</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  defaultValue={request.OpenTime}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      OpenTime: e.target.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Tỉnh thành</label>
                <Select
                  options={itemProvince}
                  value={defaultItemProvince}
                  onChange={(e) => {
                    setDefaultItemProvince({
                      value: e.value,
                      label: e.label,
                    });
                    setRequest({
                      ...request,
                      ProvinceId: e.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Email</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  defaultValue={request.Email}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      Email: e.target.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
          </div>
          <div className="row">
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Địa chỉ chi tiết</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  defaultValue={request.Address}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      Address: e.target.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Tên người quản lý</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  defaultValue={request.OwnerName}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      OwnerName: e.target.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Loại hình món ăn</label>
                <Select
                  options={itemCuisine}
                  value={defaultItemCategory}
                  onChange={(e) => {
                    setDefaultItemCategory({
                      value: e.value,
                      label: e.label,
                    });
                    setRequest({
                      ...request,
                      CuisineId: e.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Số điện thoại</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  defaultValue={request.Phone}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      Phone: e.target.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
          </div>
          <div className="row">
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Kinh độ</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  value={request.Latitude}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Vĩ độ</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  value={request.Longitude}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Hình ảnh</label>
                <input
                  type="file"
                  className="form-control"
                  placeholder="Hình ảnh..."
                  defaultValue={request.AbsoluteImage}
                  onChange={onChange}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Mô tả chi tiết</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
                  defaultValue={request.Description}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      Description: e.target.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="row mt-3 mb-4 fixed-bottom">
        <div className="col d-flex justify-content-end align-items-right">
          <div className="text-sm-end">
            <div className="col me-2 d-flex">
              <button
                type="button"
                className="btn btn-success  me-2"
                onClick={onSubmit}
                style={{ fontSize: "12px" }}
              >
                Lưu
              </button>
              <button
                type="button"
                className="btn btn-warning   me-2"
                onClick={onBack}
                style={{ fontSize: "12px" }}
              >
                Trở lại
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
