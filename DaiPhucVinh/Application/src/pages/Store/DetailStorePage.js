import * as React from "react";
import { Breadcrumb } from "../../controls";
import Swal from "sweetalert2";
import { useLocation, useNavigate ,useParams} from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  TakeProvinceById,
  TakeAllProvince,
} from "../../api/province/provinceService";
import LeafletMap from "../components/LeafletMap";
import Select from "react-select";
import { TakeAllCuisine } from "../../api/categoryItem/categoryItemService";
import { CreateNewStore, UpdateNewStore, TakeStoreById } from "../../api/store/storeService";
import {  searchAddress} from "../../api/googleSearchApi/googleApiService";

export default function CreateProvince() {
  const [latitude, setLatitude] = React.useState("");
  const [longitude, setLongitude] = React.useState("");
  const [storeaddress, setStoreaddress] = React.useState("");

  const [itemCuisine, setItemCuisine] = React.useState([]);
  const [locationStore, setLocationStore] = React.useState({lat:0, lng:0});
  const [defaultItemCategory, setDefaultItemCategory] = React.useState({
    value: "0",
    label: "Vui lòng chọn loại hình món ăn",
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
  const [selectedImageURL, setselectedImageURL] = React.useState();

  function onChange(e) {
    setSelectedImage(e.target.files[0]);
    setRequest({
      ...request,
      AbsoluteImage: e.target.files[0].name,
    });
    const file = e.target.files[0]; // Lấy tệp hình ảnh đầu tiên từ sự kiện
    if (file) {
      setselectedImageURL(URL.createObjectURL(file))
      console.log(selectedImageURL);
    }
  }
  const [itemProvince, setItemProvince] = React.useState([]);
  const [defaultItemProvince, setDefaultItemProvince] = React.useState({
    value: "0",
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
  const { id } = useParams();
  const breadcrumbSources = [
    {
      name: "Danh sách cửa hàng",
      href: "/store",
    },
    {
      name: id != null? "Cập nhật" : "Thêm mới",
      active: true,
    },
  ];

  //Requets Store
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
    history("/store");
  }
  //
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
        if (request.UserId == 0) {
          onBack();
        } else {
          // ở lại trang
        }
      }
    } else {
      let data = new FormData();
      console.log(request)
      if (selectedImage !== undefined) {
        data.append("file[]", selectedImage, selectedImage.name);
      }
      data.append("form", JSON.stringify(request));
      let response = await CreateNewStore(data);
      if (!response.success) {
        Swal.fire({
          title: "Lỗi!",
          text: "Cập nhật dữ liệu không thành công, vui lòng kiểm tra lại dữ liệu đã nhập !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      let confirm = await Swal.fire({
        title: "Thành công!",
        text: "Cập nhật dữ liệu thành công!",
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
    if (id != 0) {
      var response = await TakeStoreById(id);
      if(response.success){
        setLocationStore({
          lat:response.item.Latitude,
          lng:response.item.Longitude,
        })
        setRequest({
          UserId: response.item.UserId,
          AbsoluteImage: response.item.AbsoluteImage,
          FullName: response.item.FullName,
          Description: response.item.Description,
          OpenTime: response.item.OpenTime,
          CuisineId: response.item.CuisineId,
          ProvinceId: response.item.ProvinceId,
          Email: response.item.Email,
          Address: response.item.Address,
          OwnerName: response.item.OwnerName,
          Phone: response.item.Phone,
          Latitude:  response.item.Latitude,
          Longitude:  response.item.Longitude,
          Status:  response.item.Status,
        });
        setselectedImageURL(response.item.AbsoluteImage);
        setDefaultItemProvince({
          value: response.item.ProvinceId,
          label: response.item.Province,
        });
        setDefaultItemCategory(
          {
            value: response.item.CuisineId,
            label: response.item.Cuisine,
          }
        )
      }
      await onFillItemProvince();
      await onFillItemCategory();
    }
  }

  async function CallBackLatLng(lat) {
    setLatitude(lat.lat);
    setLongitude(lat.lng);
    searchAddress(lat.lat + "," + lat.lng).then(
      (data) => {
        if (data && data.status === "OK" && data.results.length > 0) {
          setStoreaddress(data.results[0].formatted_address);
        }else{
          console.log("Đã xảy ra lỗi khi lấy vị trí!");
        }
      }
    );

  }

  async function onChangeRequest() {
    setRequest({
      ...request,
      Address:  storeaddress,
      Latitude: latitude,
      Longitude: longitude,
    });
  }



  React.useEffect(() => {
    onViewAppearing();

  }, []);
  React.useEffect(() => {
    onChangeRequest();
  }, [latitude, longitude, storeaddress]);
  return (
    <>
      <Breadcrumb
        title={
          id  > 0 ? "Cập nhật cửa hàng" : "Thêm mới cửa hàng"
        }
        sources={breadcrumbSources}
      />

      <div>
        <LeafletMap onMapClick={CallBackLatLng} locationStore={locationStore} />
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
                  placeholder="Tên cửa hàng..."
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
                  placeholder="Giờ mở cửa..."
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
                  placeholder="Địa chỉ Email..."
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
                  placeholder="Địa chỉ chi tiết..."
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
                  placeholder="Tên người quản lý..."
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
                  value={ defaultItemCategory}
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
                  placeholder="Số điện thoại..."
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
                <label className="form-label fw-bold">Kinh độ</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Kinh độ..."
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
                  placeholder="Vĩ độ..."
                  value={request.Longitude}
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
                  placeholder="Mô tả chi tiết..."
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
          <div className="row">
            <div className="col-lg-4">
              <label className="form-label fw-bold">Hình ảnh cửa hàng</label>
              {request.AbsoluteImage ? (
                <div>
                  {selectedImageURL && (
                          <img
                            src={selectedImageURL}
                            alt="Hình ảnh đã chọn"
                            style={{ maxWidth: "100px", maxHeight: "100px" }}
                          />
                        )}              
                  </div>
              ) : (
                <div>Không có hình ảnh</div>
              )}
            </div>
          </div>
        </div>
      </div>

      <div className="row mt-3 mb-4 fixed-bottom">
        <div className="col d-flex justify-content-end align-items-right">
          <div className="text-sm-end">
            <div className="col me-2 d-flex">
                {
                  id == null ?(
                    <button
                    type="button"
                    className="btn btn-success  me-2"
                    onClick={onSubmit}
                    style={{ fontSize: "12px" }}
                  >
                   
                    Lưu
                  </button>
                  ): (
                    <button
                    type="button"
                    className="btn btn-success  me-2"
                    onClick={onSubmit}
                    style={{ fontSize: "12px" }}
                  >
                   
                    Cập nhật
                  </button>
                  )
                }
             
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
