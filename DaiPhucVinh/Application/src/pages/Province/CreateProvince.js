import * as React from "react";
import { Breadcrumb } from "../../controls";
import Swal from "sweetalert2";
import { useLocation, useNavigate } from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  CreateNewProvince,
  UpdateNewProvince,
  TakeProvinceById,
} from "../../api/province/provinceService";
export default function CreateProvince() {
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
    ProvinceId: 0,
    Name: "",
  });

  function onBack() {
    history("/province");
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
    if (request.ProvinceId == 0) {
      let response = await CreateNewProvince(request);
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
        if (request.ProvinceId == 0) {
          onBack();
        } else {
          // ở lại trang
        }
      }
    } else {
      let response = await UpdateNewProvince(request);
      console.log(response);
      if (!response.success) {
        Swal.fire({
          title: "Lỗi!",
          text: "Lưu dữ liệu không thành công , vui lòng kiểm tra lại dữ liệu đã nhập !",
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
        if (request.ProvinceId == 0) {
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
  }
  React.useEffect(() => {
    onViewAppearing();
  }, []);
  return (
    <>
      <Breadcrumb
        title={
          request.ProvinceId > 0 ? "Cập nhật tỉnh thành" : "Thêm mới tỉnh thành"
        }
        sources={breadcrumbSources}
      />
      <div className="row">
        <div className="col-sm-4">
          <label style={{ fontSize: "12px" }} className="form-label fw-bold">
            Thông tin tỉnh thành
          </label>
        </div>
      </div>
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
            <div className="col-lg-12">
              <div className="mb-3">
                <label className="form-label fw-bold">Tên tỉnh thành</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên món..."
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
