import * as React from "react";
import { Breadcrumb } from "../../controls";
import Swal from "sweetalert2";
import { useLocation, useNavigate } from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  CreateNewAccountType,
  TakeAccountTypeById,
  UpdateNewAccountType,
} from "../../api/accounttype/accountTypeService";
export default function CreateProvince() {
  const history = useNavigate();
  const { state } = useLocation();
  const breadcrumbSources = [
    {
      name: "Danh sách loại tài khoản",
      href: "/accounttype",
    },
    {
      name: state?.data?.Id > 0 ? "Cập nhật" : "Thêm mới",
      active: true,
    },
  ];
  const [request, setRequest] = React.useState({
    Id: 0,
    Name: "",
    Status: 0,
  });

  function onBack() {
    history("/accounttype");
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
    if (request.Id == 0) {
      let response = await CreateNewAccountType(request);
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
      let response = await UpdateNewAccountType(request);
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
      var response = await TakeAccountTypeById(state?.data.Id);
      console.log(response.Item);
      setRequest({
        Id: response.Item.Id,
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
          request.Id > 0 ? "Cập nhật loại tài khoản" : "Thêm mới loại tài khoản"
        }
        sources={breadcrumbSources}
      />
      <div className="row">
        <div className="col-sm-4">
          <label style={{ fontSize: "12px" }} className="form-label fw-bold">
            Thông tin loại tài khoản
          </label>
        </div>
      </div>
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
            <div className="col-lg-12">
              <div className="mb-3">
                <label className="form-label fw-bold">Tên loại tài khoản</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên loại tài khoản"
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
