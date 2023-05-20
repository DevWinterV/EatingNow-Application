import * as React from "react";
import { Breadcrumb } from "../../controls";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import { Paginate } from "../../controls";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import Select from "react-select";
import { Modal } from "react-bootstrap";
import {
  TakeAllCustomerType,
  TakeAllsCustomer,
  CreateCustomer,
  UpdateCustomer,
  RemoveCustomer,
} from "../../api/customer/customerService";
import { TakeAllProvince } from "../../api/province/provinceService";
import { error } from "jquery";
export default function ListCustomerPage() {
  const history = useNavigate();
  const breadcrumbSources = [
    {
      name: "Khách hàng",
      href: "#",
    },
    {
      name: "Danh sách",
      active: true,
    },
  ];
  const [pageTotal, setPageTotal] = React.useState(1);
  const [data, setData] = React.useState([]);
  const [province, setProvince] = React.useState([]);
  const [defaultProvince, setDefaultProvince] = React.useState({
    value: 0,
    label: "Tất cả",
  });
  const [Loading, setLoading] = React.useState(false);
  const [dropdownEmployeeFilter, setDropdownEmployeeFilter] = React.useState({
    term: "",
    page: 0,
    pageSize: 15,
  });
  const [filterEmployee, setFilterEmployee] = React.useState([]);
  const [employee, setEmployee] = React.useState([]);
  const [defaultEmployee, setDefaultEmployee] = React.useState({
    value: "",
    label: "Tất cả",
  });
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 20,
    term: "",
    TinhThanh_Id: 0,
    ProvinceName: "",
    EmployeeCode: "",
    EmployeeName: "",
    FullName: "",
    customerType,
  });
  const [account, setAccount] = React.useState({
    Id: 0,
    UserName: "",
    DisplayName: "",
    Password: "",
    NewPassword: "",
  });
  const [modalAccountVisible, setModalAccountVisible] = React.useState(false);
  const [modalVisible, setModalVisible] = React.useState(false);
  const [customerType, setCustomerType] = React.useState([]);
  const [defaultCustomerType, setDefaultCustomerType] = React.useState({
    value: 0,
    label: "Chọn loại khách hàng",
  });
  const [provinceModal, setProvinceModal] = React.useState([]);
  const [defaultProvinceModal, setDefaultProvinceModal] = React.useState({
    value: 0,
    label: "Chọn tỉnh thành",
  });
  const [employeeModal, setEmployeeModal] = React.useState([]);
  const [defaultEmployeeModal, setDefaultEmployeeModal] = React.useState({
    value: "",
    label: "Chọn nhân viên kinh doanh",
  });
  const [dataModal, setDataModal] = React.useState({
    Id: 0,
    Name: "",
    Address: "",
    TaxCode: "", //Mã số thuế
    PersonRepresent: "", //người đại diện
    Position: "",
    PhoneNo: "",
    FaxNo: "",
    Email: "",
    Bank: "", //tên ngân hàng
    BankAccount: "", //Số tài khoản
    PersonContact: "", //người liên hệ giao hàng
    ReciverAddress: "", // địa chỉ giao hàng
    LienHeKhac: "",
    CustomerType_Id: 0,
    TinhThanh_Id: 0,
    Tinh: "",
    EmployeeCode: "",
  });

  const onOpenCreate = (e) => {
    setModalAccountVisible(true);
    setAccount({
      Id: 0,
      UserName: "",
      DisplayName: e.Name,
      Password: "",
      NewPassword: "",
      Active: true,
      RoleSystem: "KhachHang",
      CustomerCode: e.Code,
    });
  };
  const onOpenEdit = (e) => {
    setModalAccountVisible(true);
    setAccount({
      Id: e.UserId,
      UserName: e.UserName,
      DisplayName: e.DisplayName,
      Password: "",
      NewPassword: "",
      Active: e.Active,
      RoleSystem: "KhachHang",
      CustomerCode: e.Code,
    });
  };
  async function onSaveAccount() {
    if (account.Id > 0) {
      if (account.DisplayName == "") {
        Swal.fire({
          title: "Lỗi!",
          text: "Tên người dùng không được bỏ trống !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      if (account.UserName == "") {
        Swal.fire({
          title: "Lỗi!",
          text: "Tên đăng nhập không được bỏ trống !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      if (account.Password != account.NewPassword) {
        Swal.fire({
          title: "Lỗi!",
          text: "Mật khẩu không trùng khớp !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }

      let response = await UpdateUser(account);
      if (!response.success) {
        Swal.fire({
          title: "Lỗi!",
          text: "Tên đăng nhập đã tồn tại hoặc nhập lại mật khẩu không trùng khớp !",
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
        onCancel();
        await onViewAppearing();
      }
    } else {
      if (account.DisplayName == "") {
        Swal.fire({
          title: "Lỗi!",
          text: "Tên khách hàng không được bỏ trống !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      if (account.UserName == "") {
        Swal.fire({
          title: "Lỗi!",
          text: "Tên đăng nhập không được bỏ trống !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      if (account.Password == "") {
        Swal.fire({
          title: "Lỗi!",
          text: "Mật khẩu không được bỏ trống !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      if (account.NewPassword == "") {
        Swal.fire({
          title: "Lỗi!",
          text: "Nhập lại mật khẩu không được bỏ trống !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }
      if (account.Password != account.NewPassword) {
        Swal.fire({
          title: "Lỗi!",
          text: "Mật khẩu không trùng khớp !",
          icon: "error",
          confirmButtonText: "OK",
        });
        return;
      }

      let response = await CreateUserCustomer(account);
      if (!response.success) {
        Swal.fire({
          title: "Lỗi!",
          text: "Tên đăng nhập đã tồn tại hoặc nhập lại mật khẩu không trùng khớp !",
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
        onCancel();
        await onViewAppearing();
      }
    }
  }
  async function onSave() {
    if (dataModal.Name == "") {
      Swal.fire({
        title: "Lỗi!",
        text: "Vui lòng nhập tên khách hàng",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    let response;
    if (dataModal.Id == 0) {
      response = await CreateCustomer(dataModal);
    } else {
      response = await UpdateCustomer(dataModal);
    }

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
      if (dataModal.Id == 0) {
        onCancel();
      }
      onViewAppearing();
    }
  }
  async function onCancel() {
    setModalAccountVisible(false);
    setModalVisible(false);
    setDataModal({
      Id: 0,
      Name: "",
      Address: "",
      TaxCode: "",
      PersonRepresent: "",
      Position: "",
      PhoneNo: "",
      FaxNo: "",
      Email: "",
      Bank: "",
      BankAccount: "",
      PersonContact: "",
      ReciverAddress: "",
      LienHeKhac: "",
      CustomerType_Id: 0,
      TinhThanh_Id: 0,
      Tinh: "",
      EmployeeCode: "",
    });
    setDefaultCustomerType({
      value: 0,
      label: "Chọn loại khách hàng",
    });
    setDefaultProvinceModal({
      value: 0,
      label: "Chọn tỉnh thành",
    });
    setDefaultEmployeeModal({
      value: 0,
      label: "Chọn nhân viên kinh doanh",
    });
  }
  async function onFillCustomerType() {
    let customerTypeResponse = await TakeAllCustomerType();
    if (customerTypeResponse.success) {
      setCustomerType([
        ...customerTypeResponse.data.map((e) => {
          return {
            value: e.Id,
            label: e.Name,
          };
        }),
      ]);
    }
  }
  async function onFillProvince() {
    let ProvinceResponse = await TakeAllProvince();
    if (ProvinceResponse.success) {
      setProvince([
        {
          value: 0,
          label: "Tất cả",
        },
        ...ProvinceResponse.data.map((e) => {
          return {
            value: e.Id,
            label: e.Name,
          };
        }),
      ]);
      setProvinceModal([
        ...ProvinceResponse.data.map((e) => {
          return {
            value: e.Id,
            label: e.Name,
          };
        }),
      ]);
    }
  }
  async function onFillEmployee() {
    let employeeResponse = await TakeAllEmployeeCode(dropdownEmployeeFilter);
    if (employeeResponse.success) {
      setFilterEmployee([
        {
          value: "",
          label: "Tất cả",
        },
        ...employee,
        ...employeeResponse.data.map((e) => {
          return {
            value: e.EmployeeCode,
            label: e.FullName,
          };
        }),
      ]);
      setEmployee([
        ...employee,
        ...employeeResponse.data.map((e) => {
          return {
            value: e.EmployeeCode,
            label: e.FullName,
          };
        }),
      ]);
      setEmployeeModal([
        ...employee,
        ...employeeResponse.data.map((e) => {
          return {
            value: e.EmployeeCode,
            label: e.FullName,
          };
        }),
      ]);
    }
  }
  function onPageChange(e) {
    setFilter({
      ...filter,
      page: e.selected,
    });
  }
  async function onViewAppearing() {
    setLoading(true);
    let response = await TakeAllsCustomer(filter);
    if (response.success) {
      setData(response.data);
      setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
    }
    setLoading(false);
  }
  React.useEffect(() => {
    onFillCustomerType();
  }, []);
  React.useEffect(() => {
    onFillEmployee();
  }, [dropdownEmployeeFilter]);
  React.useEffect(() => {
    onFillProvince();
  }, []);
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.TinhThanh_Id, filter.EmployeeCode, filter.page, filter.pageSize]);
  return (
    <>
      <Breadcrumb title="Danh sách khách hàng" sources={breadcrumbSources} />

      <div className="card" style={{ fontSize: "12px" }}>
        <div className="card-body">
          <div className="row">
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="d-flex fw-bold">Tìm kiếm</label>
                <div className="input-group">
                  <input
                    type="search"
                    className="form-control"
                    placeholder="Tên, điện thoại, mã số thuế..."
                    value={filter.term}
                    onChange={(e) =>
                      setFilter({ ...filter, term: e.target.value })
                    }
                    onKeyDown={(e) => {
                      if (e.code == "Enter") {
                        onViewAppearing();
                        setFilter({ ...filter, page: 0 });
                      }
                    }}
                    style={{ fontSize: "12px" }}
                  />
                  <button
                    className="btn btn-success "
                    type="button"
                    onClick={() => {
                      onViewAppearing();
                      setFilter({
                        ...filter,
                        page: 0,
                      });
                    }}
                  >
                    <i className="bx bx-search-alt-2"></i>
                  </button>
                </div>
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Tỉnh thành</label>
                <Select
                  options={province}
                  value={defaultProvince}
                  onChange={(e) => {
                    setDefaultProvince({
                      value: e.value,
                      label: e.label,
                    });
                    setFilter({
                      ...filter,
                      page: 0,
                      TinhThanh_Id: e.value,
                      ProvinceName: e.label,
                    });
                  }}
                />
              </div>
            </div>
            <div className="col-lg-3">
              <div className="mb-3">
                <label className="form-label fw-bold">Nhân viên KD</label>
                <Select
                  options={filterEmployee}
                  value={defaultEmployee}
                  onChange={(e) => {
                    setFilter({
                      ...filter,
                      EmployeeCode: e.value,
                      EmployeeName: e.label,
                      page: 0,
                    });
                    setDefaultEmployee({
                      value: e.value,
                      label: e.label,
                    });
                  }}
                  onInputChange={(e) => {
                    setEmployee([]);
                    setDropdownEmployeeFilter({
                      ...dropdownEmployeeFilter,
                      page: 0,
                      term: e,
                    });
                  }}
                  captureMenuScroll={() => {}}
                  onMenuScrollToBottom={() => {
                    setDropdownEmployeeFilter({
                      ...dropdownEmployeeFilter,
                      page: dropdownEmployeeFilter.page + 1,
                    });
                  }}
                />
              </div>
            </div>
            <div
              className="col d-flex justify-content-end  align-items-right"
              style={{ marginTop: "27px" }}
            >
              <div className="text-sm-end mb-3">
                <button
                  type="button"
                  className="btn btn-info"
                  onClick={() => setModalVisible(true)}
                  style={{ fontSize: "12px" }}
                >
                  <i className="mdi mdi-plus me-1"></i> Thêm
                </button>
              </div>
            </div>
          </div>
          <Modal
            size="xl"
            show={modalVisible}
            onEscapeKeyDown={() => setModalVisible(false)}
            backdrop="static"
            keyboard={false}
            className="fade"
            tabIndex="-1"
            role="dialog"
            aria-labelledby="staticBackdropLabel"
            aria-hidden="true"
            centered
          >
            <Modal.Header>
              {dataModal.Id > 0 ? (
                <span
                  style={{ fontSize: "12px" }}
                  className="modal-title fw-bold"
                  id="staticBackdropLabel"
                >
                  Thông tin khách hàng
                </span>
              ) : (
                <span
                  style={{ fontSize: "12px" }}
                  className="modal-title fw-bold"
                  id="staticBackdropLabel"
                >
                  Thêm khách hàng
                </span>
              )}
              <button
                type="button"
                className="btn-close"
                onClick={() => {
                  onCancel();
                }}
              ></button>
            </Modal.Header>
            <Modal.Body style={{ fontSize: "12px" }}>
              <div className="row">
                <div className="col-lg-4">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      Loại khách hàng
                    </label>
                    <Select
                      className="col"
                      options={customerType}
                      value={defaultCustomerType}
                      onChange={(e) => {
                        setDefaultCustomerType(e);
                        setDataModal({
                          ...dataModal,
                          CustomerType_Id: e.value,
                        });
                      }}
                    ></Select>
                  </div>
                </div>
                <div className="col-lg-4">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Tỉnh thành</label>
                    <Select
                      options={provinceModal}
                      value={defaultProvinceModal}
                      onChange={(e) => {
                        setDefaultProvinceModal(e);
                        setDataModal({
                          ...dataModal,
                          TinhThanh_Id: e.value,
                          Tinh: e.label,
                        });
                      }}
                    />
                  </div>
                </div>
                <div className="col-lg-4">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      Nhân viên kinh doanh
                    </label>
                    <Select
                      options={employeeModal}
                      value={defaultEmployeeModal}
                      onChange={(e) => {
                        setDefaultEmployeeModal(e);
                        setDataModal({
                          ...dataModal,
                          EmployeeCode: e.value,
                        });
                      }}
                      onInputChange={(e) => {
                        setEmployee([]);
                        setDropdownEmployeeFilter({
                          ...dropdownEmployeeFilter,
                          page: 0,
                          term: e,
                        });
                      }}
                      captureMenuScroll={() => {}}
                      onMenuScrollToBottom={() => {
                        setDropdownEmployeeFilter({
                          ...dropdownEmployeeFilter,
                          page: dropdownEmployeeFilter.page + 1,
                        });
                      }}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Tên khách hàng</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Tên khách hàng"
                      defaultValue={dataModal.Name}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          Name: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Số điện thoại</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Số điện thoại"
                      defaultValue={dataModal.PhoneNo}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          PhoneNo: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Địa chỉ</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Địa chỉ"
                      defaultValue={dataModal.Address}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          Address: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Mã số thuế</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Mã số thuế"
                      defaultValue={dataModal.TaxCode}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          TaxCode: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Người đại diện</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Người đại diện"
                      defaultValue={dataModal.PersonRepresent}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          PersonRepresent: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Chức vụ</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Chức vụ"
                      defaultValue={dataModal.Position}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          Position: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Số Fax</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Số Fax"
                      defaultValue={dataModal.FaxNo}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          FaxNo: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Email</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Email"
                      defaultValue={dataModal.Email}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          Email: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Số tài khoản</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Số tài khoản"
                      defaultValue={dataModal.BankAccount}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          BankAccount: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Tên ngân hàng</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Tên ngân hàng"
                      defaultValue={dataModal.Bank}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          Bank: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      Người liên hệ giao hàng
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Người liên hệ giao hàng"
                      defaultValue={dataModal.PersonContact}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          PersonContact: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      Địa chỉ giao hàng
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Địa chỉ giao hàng"
                      defaultValue={dataModal.ReciverAddress}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          ReciverAddress: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-lg-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      Thông tin liên hệ khác
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Thông tin liên hệ khác"
                      defaultValue={dataModal.LienHeKhac}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          LienHeKhac: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </div>
            </Modal.Body>
            <Modal.Footer>
              <button
                type="button"
                className="btn btn-success"
                onClick={onSave}
                style={{ fontSize: "12px" }}
              >
                Lưu
              </button>
              <button
                type="button"
                className="btn btn-warning"
                onClick={onCancel}
                style={{ fontSize: "12px" }}
              >
                Đóng
              </button>
            </Modal.Footer>
          </Modal>
          <Table className="table table-striped">
            <Thead className="table-light">
              <Tr>
                <Th className="align-middle">STT</Th>
                <Th className="align-middle">Tên khách hàng</Th>
                <Th className="align-middle">Điện thoại</Th>
                <Th className="align-middle">Địa chỉ</Th>
                <Th className="align-middle">Mã số thuế</Th>
                <Th className="align-middle">Người liên hệ</Th>
                <Th className="align-middle">Có tài khoản</Th>
                <Th className="align-middle">Sp</Th>
                <Th className="align-middle">Cập nhật</Th>
                <Th className="align-middle">Xóa</Th>
              </Tr>
            </Thead>
            <Tbody style={{ position: "relative" }}>
              {Loading ? (
                <Tr>
                  <Td colSpan={10} style={{ height: "70px" }}>
                    <div
                      className="spinner-border text-info"
                      role="status"
                      style={{
                        position: "absolute",
                        top: "50%",
                        left: "50%",
                      }}
                    >
                      <span className="sr-only"></span>
                    </div>
                  </Td>
                </Tr>
              ) : (
                <>
                  {data.length > 0 ? (
                    data.map((i, idx) => (
                      <Tr key={"row_" + idx}>
                        <Td>{filter.page * filter.pageSize + idx + 1}</Td>
                        <Td>{i.Name}</Td>
                        <Td>{i.PhoneNo}</Td>
                        <Td>{i.Address}</Td>
                        <Td>{i.TaxCode}</Td>
                        <Td>{i.PersonContact}</Td>
                        <Td>
                          {i.IsExistAccount == true ? (
                            <div className="d-flex gap-3">
                              <div
                                className="text-success"
                                onClick={() => {
                                  onOpenEdit(i);
                                }}
                              >
                                <i className="mdi mdi-check-bold font-size-18"></i>
                              </div>
                            </div>
                          ) : (
                            <div className="d-flex gap-3">
                              <div
                                className="text-info"
                                onClick={() => onOpenCreate(i)}
                              >
                                <i className="mdi mdi-plus font-size-18"></i>
                              </div>
                            </div>
                          )}
                        </Td>
                        <Td>
                          <div className="d-flex gap-3">
                            <div
                              className="text-info"
                              onClick={() => {
                                history("/customer/edit/" + i.Id, {
                                  state: { data: i },
                                });
                              }}
                            >
                              <i className="mdi mdi-archive font-size-18"></i>
                            </div>
                          </div>
                        </Td>
                        <Td>
                          <div className="d-flex gap-3">
                            <div
                              className="text-success"
                              onClick={() => {
                                setDataModal({
                                  Id: i.Id,
                                  Code: i.Code,
                                  Name: i.Name,
                                  Address: i.Address,
                                  TaxCode: i.TaxCode,
                                  PersonRepresent: i.PersonEpresent,
                                  Position: i.Position,
                                  PhoneNo: i.PhoneNo,
                                  FaxNo: i.FaxNo,
                                  Email: i.Email,
                                  Bank: i.Bank,
                                  BankAccount: i.BankAccount,
                                  PersonContact: i.PersonContact,
                                  ReciverAddress: i.ReciverAddress,
                                  LienHeKhac: i.LienHeKhac,
                                  CustomerType_Id: i.CustomerType_Id,
                                  TinhThanh_Id: i.TinhThanh_Id,
                                  Tinh: i.Tinh,
                                  EmployeeCode: i.EmployeeCode,
                                });
                                if (i.TinhThanh_Id == 0) {
                                  setDefaultProvinceModal({
                                    value: 0,
                                    label: "Chọn tỉnh thành",
                                  });
                                } else {
                                  setDefaultProvinceModal({
                                    value: i.TinhThanh_Id,
                                    label: i.Tinh,
                                  });
                                }
                                if (
                                  i.EmployeeCode == "" ||
                                  i.EmployeeCode == null
                                ) {
                                  setDefaultEmployeeModal({
                                    value: 0,
                                    label: "Chọn nhân viên kinh doanh",
                                  });
                                } else {
                                  setDefaultEmployeeModal({
                                    value: i.EmployeeCode,
                                    label: i.EmployeeName,
                                  });
                                }
                                if (i.CustomerType_Id == 0) {
                                  setDefaultCustomerType({
                                    value: 0,
                                    label: "Chọn loại khách hàng",
                                  });
                                } else {
                                  setDefaultCustomerType({
                                    value: i.CustomerType_Id,
                                    label: i.CustomerTypeName,
                                  });
                                }
                                setModalVisible(true);
                              }}
                            >
                              <i className="mdi mdi-pencil font-size-18"></i>
                            </div>
                          </div>
                        </Td>
                        <Td>
                          <div className="d-flex gap-3">
                            <div
                              className="text-danger"
                              onClick={() => {
                                Swal.fire({
                                  title: "Xóa khách hàng ?",
                                  text: "Bạn muốn xóa khách hàng này ra khỏi danh sách !",
                                  icon: "warning",
                                  showCancelButton: true,
                                  confirmButtonColor: "#3085d6",
                                  cancelButtonColor: "#d33",
                                  confirmButtonText: "Xác nhận !",
                                }).then((result) => {
                                  if (result.isConfirmed) {
                                    RemoveCustomer(i.Id).then((response) => {
                                      if (response.success) {
                                        Swal.fire(
                                          "Hoàn thành!",
                                          "Xóa dữ liệu thành công.",
                                          "success"
                                        );
                                        onViewAppearing();
                                      } else {
                                        Swal.fire(
                                          "Không thể xóa khách hàng!",
                                          "Khách hàng này đã có phát sinh giao dịch",
                                          "warning"
                                        );
                                      }
                                    });
                                  }
                                });
                              }}
                            >
                              <i className="mdi mdi-delete font-size-18"></i>
                            </div>
                          </div>
                        </Td>
                      </Tr>
                    ))
                  ) : (
                    <Tr>
                      <Td colSpan={10}>Không có dữ liệu</Td>
                    </Tr>
                  )}
                </>
              )}
            </Tbody>
          </Table>
          <Modal
            size="md"
            show={modalAccountVisible}
            onEscapeKeyDown={() => setModalAccountVisible(false)}
            backdrop="static"
            keyboard={false}
            className="fade"
            tabIndex="-1"
            role="dialog"
            aria-labelledby="staticBackdropLabel"
            aria-hidden="true"
            centered
            style={{ fontSize: "12px" }}
          >
            <Modal.Header>
              {account.Id > 0 ? (
                <label
                  style={{ fontSize: "12px" }}
                  className="modal-title fw-bold"
                  id="staticBackdropLabel"
                >
                  Thông tin tài khoản
                </label>
              ) : (
                <label
                  style={{ fontSize: "12px" }}
                  className="modal-title fw-bold"
                  id="staticBackdropLabel"
                >
                  Thêm tài khoản
                </label>
              )}

              <button
                type="button"
                className="btn-close"
                onClick={() => {
                  setModalAccountVisible(false);
                }}
              ></button>
            </Modal.Header>
            <Modal.Body>
              <div className="card-body" style={{ padding: 0 }}>
                <div className="row">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Tên khách hàng</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Tên khách hàng"
                      value={account.DisplayName}
                      onChange={(e) => {
                        setAccount({
                          ...account,
                          DisplayName: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Tên đăng nhập</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Tên đăng nhập"
                      value={account.UserName}
                      onChange={(e) => {
                        setAccount({
                          ...account,
                          UserName: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Mật khẩu</label>
                    <input
                      type="Password"
                      className="form-control"
                      placeholder="Mật khẩu mới"
                      value={account.Password}
                      onChange={(e) => {
                        setAccount({
                          ...account,
                          Password: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      Nhập lại mật khẩu
                    </label>
                    <input
                      type="Password"
                      className="form-control"
                      placeholder="Nhập lại mật khẩu mới"
                      value={account.NewPassword}
                      onChange={(e) => {
                        setAccount({
                          ...account,
                          NewPassword: e.target.value,
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    />
                  </div>
                </div>
              </div>
            </Modal.Body>
            <Modal.Footer>
              {account.Id > 0 ? (
                <>
                  {account.Active ? (
                    <button
                      type="button"
                      className="btn btn-warning"
                      onClick={() => {
                        Swal.fire({
                          title: "Khóa tài khoản khách hàng ?",
                          text: "Hãy xác nhận bạn muốn khóa tài khoản này !",
                          icon: "warning",
                          showCancelButton: true,
                          confirmButtonColor: "#3085d6",
                          cancelButtonColor: "#d33",
                          confirmButtonText: "Xác nhận !",
                        }).then((result) => {
                          if (result.isConfirmed) {
                            LockUser(account.Id).then((response) => {
                              if (response.success) {
                                Swal.fire(
                                  "Hoàn thành!",
                                  "Lưu dữ liệu thành công.",
                                  "success"
                                );
                                onCancel();
                                onViewAppearing();
                              }
                            });
                          }
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    >
                      Khóa tài khoản
                    </button>
                  ) : (
                    <button
                      type="button"
                      className="btn btn-info"
                      onClick={() => {
                        Swal.fire({
                          title: "Mở khóa tài khoản khách hàng ?",
                          text: "Hãy xác nhận bạn muốn mở khóa tài khoản này !",
                          icon: "warning",
                          showCancelButton: true,
                          confirmButtonColor: "#3085d6",
                          cancelButtonColor: "#d33",
                          confirmButtonText: "Xác nhận !",
                        }).then((result) => {
                          if (result.isConfirmed) {
                            UnLockUser(account.Id).then((response) => {
                              if (response.success) {
                                Swal.fire(
                                  "Hoàn thành!",
                                  "Lưu dữ liệu thành công.",
                                  "success"
                                );
                                onCancel();
                                onViewAppearing();
                              }
                            });
                          }
                        });
                      }}
                      style={{ fontSize: "12px" }}
                    >
                      Mở lại tài khoản
                    </button>
                  )}
                  <button
                    type="button"
                    className="btn btn-danger"
                    onClick={() => {
                      Swal.fire({
                        title: "Xóa tài khoản khách hàng ?",
                        text: "Hãy xác nhận bạn muốn xóa tài khoản này !",
                        icon: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#3085d6",
                        cancelButtonColor: "#d33",
                        confirmButtonText: "Xác nhận !",
                      }).then((result) => {
                        if (result.isConfirmed) {
                          RemoveUser(account.Id).then((response) => {
                            if (response.success) {
                              Swal.fire(
                                "Hoàn thành!",
                                "Lưu dữ liệu thành công.",
                                "success"
                              );
                              onCancel();
                              onViewAppearing();
                            }
                          });
                        }
                      });
                    }}
                    style={{ fontSize: "12px" }}
                  >
                    Xóa
                  </button>
                </>
              ) : (
                <></>
              )}
              <button
                type="button"
                className="btn btn-success"
                onClick={onSaveAccount}
                style={{ fontSize: "12px" }}
              >
                Lưu
              </button>
              <button
                type="button"
                className="btn btn-secondary"
                onClick={() => {
                  setModalAccountVisible(false);
                }}
                style={{ fontSize: "12px" }}
              >
                Đóng
              </button>
            </Modal.Footer>
          </Modal>
          <Paginate
            onPageChange={onPageChange}
            pageRangeDisplayed={5}
            pageTotal={pageTotal || 1}
          />
        </div>
      </div>
    </>
  );
}
