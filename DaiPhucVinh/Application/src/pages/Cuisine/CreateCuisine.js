import * as React from "react";
import { Breadcrumb } from "../../controls";
import Swal from "sweetalert2";
import { useLocation, useNavigate } from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  CreateNewCuisine,
  UpdateNewCuisine,
  TakeCuisineById,
} from "../../api/categoryItem/categoryItemService";
import FormData from "form-data";
export default function CreateVideosPage() {
  const history = useNavigate();
  const { state } = useLocation();
  const breadcrumbSources = [
    {
      name: "Danh sách loại hình kinh doanh",
      href: "/cuisine",
    },
    {
      name: state?.data?.CuisineId > 0 ? "Cập nhật" : "Thêm mới",
      active: true,
    },
  ];
  const [request, setRequest] = React.useState({
    CuisineId: 0,
    Name: "",
    AbsoluteImage: "",
  });

  function onBack() {
    history("/cuisine");
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
    } 
  }
  async function onSubmit() {
    if (request.Name.length == 0) {
      Swal.fire({
        title: "Lỗi!",
        text: "Tên không được để trống!",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    if (request.AbsoluteImage == "") {
      Swal.fire({
        title: "Lỗi!",
        text: "Vui lòng chọn hình ảnh!",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    if (request.CuisineId == 0) {
      let data = new FormData();
      if (selectedImage !== undefined) {
        data.append("file[]", selectedImage, selectedImage.name);
      }
      data.append("form", JSON.stringify(request));
      let response = await CreateNewCuisine(data);
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
      var response = await TakeCuisineById(state?.data.CuisineId);
      console.log(response);
      setRequest({
        CuisineId: response.item.CuisineId,
        Name: response.item.Name || "",
        AbsoluteImage: response.item.AbsoluteImage || "",
      });
      setselectedImageURL(response.item.AbsoluteImage || "")
    }
  }
  React.useEffect(() => {
    onViewAppearing();
  }, []);
  return (
    <>
      <Breadcrumb
        title={
          request.CuisineId > 0
            ? "Cập nhật loại hình kinh doanh"
            : "Thêm mới loại hình kinh doanh"
        }
        sources={breadcrumbSources}
      />
      <div className="row">
        <div className="col-sm-4">
          <label style={{ fontSize: "12px" }} className="form-label fw-bold">
            Thông tin loại hình kinh doanh
          </label>
        </div>
      </div>
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
            <div className="col-lg-12">
              <div className="mb-3">
                <label className="form-label fw-bold">
                  Tên loại hình kinh doanh
                </label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên loại hình kinh doanh..."
                  defaultValue={request.Name}
                  onChange={(e) => {
                    setRequest({
                      ...request,
                      Name: e.target.value,
                    });
                  }}
                  style={{ fontSize: "12px" }}
                />
              </div>
            </div>
          </div>
          <div className="row">
            <div className="col-lg-12">
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
          </div>
          <div className="row">
            <div className="col-lg-4">
              <label className="form-label fw-bold">Hình ảnh</label>
              {selectedImageURL ? (
                <div>
                  <img width="100%" src={selectedImageURL} />
                </div>
              ) : (
                <div>không có hình ảnh</div>
              )}
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
