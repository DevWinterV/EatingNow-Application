import * as React from "react";
import { Breadcrumb } from "../../controls";
import Swal from "sweetalert2";
import { useLocation, useNavigate } from "react-router-dom";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import {
  TakeDistrictById,
  TakeAllDistrict,
} from "../../api/district/districtService";
import { TakeAllProvince } from "../../api/province/provinceService";
import Select from "react-select";
import {
  CreateNewWard,
  TakeWardById,
  UpdateNewWard,
} from "../../api/ward/wardService";
export default function CreateProvince() {
  const history = useNavigate();
  const { state } = useLocation();
  const breadcrumbSources = [
    {
      name: "Danh sách xã",
      href: "/district",
    },
    {
      name: state?.data?.WardId > 0 ? "Cập nhật" : "Thêm mới",
      active: true,
    },
  ];
  const [loading, setLoading] = React.useState(false);
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 20,
    term: "",
    ItemProvinceCode: "",
    ItemDistrictCode: "",
  });
  const [request, setRequest] = React.useState({
    WardId: 0,
    Name: "",
    ItemProvinceCode: "",
    ItemDistrictCode: "",
  });
  const [itemProvince, setItemProvince] = React.useState([]);
  const [defaultItemProvince, setDefaultItemProvince] = React.useState({
    value: "",
    label: "Vui lòng chọn tỉnh",
  });
  async function onFillItemProvince() {
    let itemProvinceResponse = await TakeAllProvince({    term: "",  });
    console.log("ID : ", itemProvinceResponse);
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
  const [itemDistrict, setItemDistrict] = React.useState([]);
  const [defaultItemDistrict, setDefaultItemDistrict] = React.useState({
    value: "",
    label: "Vui lòng chọn huyện",
  });

  function onBack() {
    history("/ward");
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
    if (request.WardId == 0) {
      let response = await CreateNewWard(request);
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
        if (request.WardId == 0) {
          onBack();
        } else {
          // ở lại trang
        }
      }
    } else {
      let response = await UpdateNewWard(request);
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
        if (request.WardId == 0) {
          onBack();
        } else {
          onViewAppearing();
          // ở lại trang
        }
      }
    }
  }
  async function onViewAppearing() {
    setLoading(true);
    let responseWard = await TakeAllDistrict(filter);
    console.log(responseWard);
    if (responseWard.success) {
      setItemDistrict([
        {
          value: "",
          label: "Vui lòng chọn huyện",
        },
        ...responseWard.data.map((e) => {
          return {
            value: e.DistrictId,
            label: e.Name,
          };
        }),
      ]);
    }

    await onFillItemProvince();
    setLoading(false);

    if (state?.data) {
      var response = await TakeWardById(state?.data.WardId);
      setRequest({
        WardId: response.Item.WardId,
        Name: response.Item.Name || "",
      });
    }
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.ItemProvinceCode, filter.ItemDistrictCode]);

  React.useEffect(() => {
    onFillItemProvince();
  }, []);
  return (
    <>
      <Breadcrumb
        title={request.WardId > 0 ? "Cập nhật xã" : "Thêm mới xã"}
        sources={breadcrumbSources}
      />
      <div className="row">
        <div className="col-sm-4">
          <label style={{ fontSize: "12px" }} className="form-label fw-bold">
            Thông tin xã
          </label>
        </div>
      </div>
      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
            {request.WardId == 0 ? (
              <>
                <div className="col-lg-4">
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
                        setFilter({
                          ...filter,
                          page: 0,
                          ItemProvinceCode: e.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="col-sm-4">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Huyện</label>
                    <Select
                      options={itemDistrict}
                      value={defaultItemDistrict}
                      onChange={(e) => {
                        console.log(e);
                        setDefaultItemDistrict({
                          value: e.value,
                          label: e.label,
                        });
                        setRequest({
                          ...request,
                          WardId: 0,
                          ItemDistrictCode: e.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </>
            ) : (
              ""
            )}

            <div className="col-lg-12">
              <div className="mb-3">
                <label className="form-label fw-bold">Tên xã</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Tên huyện..."
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
