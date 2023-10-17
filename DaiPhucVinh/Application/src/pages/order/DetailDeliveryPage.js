import * as React from "react";
import { Breadcrumb } from "../../controls";
import Swal from "sweetalert2";
import {  useNavigate ,useParams} from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  TakeAllProvince,
} from "../../api/province/provinceService";
import { TakeAllWard } from "../../api/ward/wardService";
import {
  TakeAllDistrict,
} from "../../api/district/districtService";
import LeafletMap from "../components/LeafletMap";
import Select from "react-select";
import { CreateNewStore, CreateNewDeliver, TakeDriverById } from "../../api/store/storeService";


export default function CreateProvince() {
  const [selectedImage, setSelectedImage] = React.useState();
  const [selectedImageURL, setselectedImageURL] = React.useState();
  const [wards, setwards] = React.useState([]);
  const [itemwards, setItemwards] = React.useState([]);
  const [defaultItemwards, setDefaultItemwards] = React.useState({
    value: 0,
    label: "Vui lòng chọn xã",
  });
  function onChange(e) {
    setSelectedImage(e.target.files[0]);
    setRequest({
      ...request,
      UploadImage: e.target.files[0].name,
    });
    const file = e.target.files[0]; // Lấy tệp hình ảnh đầu tiên từ sự kiện
    if (file) {
      setselectedImageURL(URL.createObjectURL(file))
      console.log(selectedImageURL);
    }
  }

  const [itemProvince, setItemProvince] = React.useState([]);
  const [defaultItemProvince, setDefaultItemProvince] = React.useState({
    value: 0,
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
      name: "Danh sách tài xế",
      href: "/delivery",
    },
    {
      name: id != null? "Cập nhật" : "Thêm mới",
      active: true,
    },
  ];

  //Requets Delivery
  const [request, setRequest] = React.useState({
    DeliveryDriverId: 0,
    UploadImage: "",
    CompleteName: "",
    ProvinceId: 0,
    WardId: 0,
    DistrictId: 0,
    Email: "",
    Phone: "",
    UserName:"",
    Latitude: 0.0,
    Longitude: 0.0,
    Status: 0,
  });

  function onBack() {
    history("/delivery");
  }
  //lưu tài xế
  async function onSubmit() {
    if (request.DeliveryDriverId == 0) {
      let data = new FormData();
      if (selectedImage !== undefined) {
        data.append("file[]", selectedImage, selectedImage.name);
      }
      data.append("form", JSON.stringify(request));
      let response = await CreateNewDeliver(data);
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
      let response = await CreateNewDeliver(data);
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
    if (id != 0 || id != null) {
      var response = await TakeDriverById(id);
      if(response.success){
        setRequest({
          DeliveryDriverId: response.item.DeliveryDriverId,
          UploadImage: response.item.UploadImage,
          CompleteName: response.item.CompleteName,
          ProvinceId: response.item.ProvinceId,
          WardId: response.item.WardId,
          DistrictId: response.item.DistrictId,
          Email: response.item.Email,
          Phone: response.item.Phone,
          UserName: response.item.UserName,
          Latitude: response.item.Latitude,
          Longitude: response.item.Longitude,
          Status: response.item.Status,
        });
        setselectedImageURL(response.item.UploadImage);
        setDefaultItemProvince({
          value: response.item.ProvinceId,
          label: response.item.ProvinceName,
        });
         setDefaultItemDistrict({
          value: response.item.DistrictId,
          label: response.item.DistrictName,
        });
         setDefaultItemwards({
          value: response.item.WardId,
          label: response.item.WardName,
        });
      }
     
    }
   
  }

  const [itemDistrict, setItemDistrict] = React.useState([]);
  const [defaultItemDistrict, setDefaultItemDistrict] = React.useState({
    value: "",
    label: "Vui lòng chọn huyện",
  });
  async function onFillDistrict() {
    let response = await TakeAllDistrict();
    if (response.success) {
      setItemDistrict([
        {
          value: "",
          label: "Tất cả",
        },
        ...response.data.map((e) => {
          return {
            value: e.DistrictId,
            label: e.Name,
          };
        }),
      ]);
    }
  }
  async function onFillWar() {
    let response = await TakeAllWard();
    if (response.success) {
      setItemwards([
        {
          value: "",
          label: "Tất cả",
        },
        ...response.data.map((e) => {
          return {
            value: e.WardId,
            label: e.Name,
          };
        }),
      ]);
    }
  }
  React.useEffect(() => {
    onFillDistrict();
    onFillItemProvince();
    onFillWar();
  }, []);
  React.useEffect(() => {
    onViewAppearing();
  }, []);

  return (
    <>
      <Breadcrumb
        title={
          id  > 0 ? "Cập nhật tài xế" : "Thêm mới tài xế"
        }
        sources={breadcrumbSources}
      />
      <div className="row">
        <div className="col-sm-4">
          <label style={{ fontSize: "12px" }} className="form-label fw-bold">
            Thông tin tài xế
          </label>
        </div>
      </div>
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Họ tên</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Họ tên..."
                  defaultValue={request.CompleteName}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      CompleteName: e.target.value,
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
                <label className="form-label fw-bold">Huyện</label>
                <Select
                  options={itemDistrict}
                  value={defaultItemDistrict}
                  onChange={(e) => {
                    setDefaultItemDistrict({
                      value: e.value,
                      label: e.label,
                    });
                    setRequest({
                      ...request,
                      DistrictId: e.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Xã/Phường</label>
                <Select
                  options={itemwards}
                  value={defaultItemwards}
                  onChange={(e) => {
                    setDefaultItemwards({
                      value: e.value,
                      label: e.label,
                    });
                    setRequest({
                      ...request,
                      WardId: e.value,
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
                <label className="form-label fw-bold">Số điện thoại</label>
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
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Tên đăng nhập</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên người quản lý..."
                  defaultValue={request.UserName}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      UserName: e.target.value,
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
                  defaultValue={request.UploadImage}
                  onChange={onChange}
                  style={{ fontSize: "12px" }}
                />
              </div>
          </div>
          <div className="col-lg-3">
              <label className="form-label fw-bold">Hình ảnh tài xế</label>
              {request.UploadImage ? (
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
