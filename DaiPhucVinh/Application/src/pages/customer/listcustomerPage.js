import * as React from "react";
import { Breadcrumb } from "../../controls";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
import "react-super-responsive-table/dist/SuperResponsiveTableStyle.css";
import { Paginate } from "../../controls";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { Modal } from "react-bootstrap";

import {
  TakeAllCustomer,
  TakeAllCustomerByProvinceId,
  CreateCustomer,
  UpdateCustomer,
  RemoveCustomer,
} from "../../api/customer/customerService";

export default function ListCustomerPage() {
  const history = useNavigate();
  const breadcrumbSources = [
    {
      name: "Danh sách khách hàng",
      href: "#",
    },
    {
      name: "Danh sách",
      active: true,
    },
  ];
  const [pageTotal, setPageTotal] = React.useState(1);
  const [data, setData] = React.useState([]);
  const [Loading, setLoading] = React.useState(false);
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 20,
    term: "",
    ProvinceName: "",
    EmployeeCode: "",
    EmployeeName: "",
    FullName: "",
    ProvinceId: 0,
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
  const [selectedImageURL, setselectedImageURL] = React.useState();

  const [dataModal, setDataModal] = React.useState({
    CustomerId: "",
    CompleteName: "",
    Address: "",
    Phone: "",
    Email: "",
    ProvinceId: 0,
    DistrictId: 0,
    WardId: 0,
    Status :true,
    ImageProfile:"",
  });
  const [imageSrc, setImageSrc] = React.useState(); // Đường dẫn đến hình ảnh

  // Xử lý sự kiện khi người dùng chọn hình ảnh mới
  const handleImageChange = (e) => {
    setImageSrc(e.target.files[0]);
    const file = e.target.files[0]; // Lấy tệp hình ảnh đầu tiên từ sự kiện
    if (file) {
      setselectedImageURL(URL.createObjectURL(file))
    } 
  };

  // Hàm lưu tài khoản đăng nhập
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
    console.log(dataModal);
    if (dataModal.CompleteName == "" || dataModal.Phone =="") {
      Swal.fire({
        title: "Lỗi!",
        text: "Vui lòng nhập đủ thông tin khách hàng ( Họ tên và số điện thoại)",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    let response;
    let data = new FormData();
    if (imageSrc !== undefined) {
      data.append("file[]", imageSrc, imageSrc.name);
    }
    data.append("form", JSON.stringify(dataModal));
    // Thêm mới nếu Id là rỗng
    response = await UpdateCustomer(data);
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
      CustomerId: "",
      CompleteName: "",
      Address: "",
      Phone: "",
      Email: "",
      ProvinceId: 0,
      DistrictId: 0,
      WardId:0,
      Status :true,
      ImageProfile:"",
    });
    setselectedImageURL('');
  }


  function onPageChange(e) {
    setFilter({
      ...filter,
      page: e.selected,
    });
  }
  async function onViewAppearing() {
    setLoading(true);
    let response = await TakeAllCustomer(filter);
    if (response.success) {
      setData(response.data);
      setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
    }
    setLoading(false);
  }

  //Load dữ liệu khách hàng
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.term, filter.page, filter.pageSize]);
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
                    placeholder="Tên khách hàng..."
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

          
            {
/*
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
                      pageSize:20,
                      ProvinceId: e.value,
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
*/
            }
   
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
                  <i className="mdi mdi-plus me-1"></i> Thêm khách hàng
                </button>
              </div>
            </div>
          </div>
          {/* Form cập nhật thông tin khách hàng */}
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
              {dataModal.CustomerId != "" ? (
                <span
                  style={{ fontSize: "12px" }}
                  className="modal-title fw-bold"
                  id="staticBackdropLabel"
                >
                  Cập nhật khách hàng
                </span>
              ) : (
                <span
                  style={{ fontSize: "12px" }}
                  className="modal-title fw-bold"
                  id="staticBackdropLabel"
                >
                  Thêm mới khách hàng
                </span>
              )
              }
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
                {
                  /*
                  Loại khách hàng
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
                  */
                }
                {
                  /*
       <div className="col-lg-4">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Tỉnh thành</label>
                    <Select
                      options={province}
                      value={defaultProvinceModal}
                      onChange={(e) => {
                        setDefaultProvinceModal(e);
                        setDataModal({
                          ...dataModal,
                          ProvinceId: e.value,
                        });
                      }}
                    />
                  </div>
                </div>
                <div className="col-lg-4">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Huyện</label>
                    <Select
                      options={District}
                      value={defaultDistrictModal}
                      onChange={(e) => {
                        setdefaultDistrictModal(e);
                        setDataModal({
                          ...dataModal,
                          DistrictId: e.value,
                        });
                      }}
                    />
                  </div>
                </div><div className="col-lg-4">
                  <div className="mb-3">
                    <label className="form-label fw-bold">Xã/Phường</label>
                    <Select
                      options={Ward}
                      value={defaultWardModal}
                      onChange={(e) => {
                        setdefaultWardModal(e);
                        console.log(e.label);
                        setDataModal({
                          ...dataModal,
                          WardId: e.value,
                        });
                      }}
                    />
                  </div>
                </div>
                  */
                }
         
            
                {
                  /*
                  Nhân viên kinh doanh
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
                  */
                }
          
              </div>
              <div className="row">
                <div className="col-lg-6">
                    <div className="mb-3">
                      {/* Hình ảnh */}
                      <img 
                          src={selectedImageURL}
                          alt={dataModal.CustomerId}
                          style={{ width: '120px', height: '120px', marginRight: 10 }}
                        />
                      {/* Input để chọn hình ảnh */}
                      <input type="file" accept="image/*" onChange={handleImageChange} />
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
                      defaultValue={dataModal.CompleteName}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          CompleteName: e.target.value,
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
                      defaultValue={dataModal.Phone}
                      onChange={(e) => {
                        setDataModal({
                          ...dataModal,
                          Phone: e.target.value,
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
                    <label className="form-label fw-bold">Email</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Địa chỉ"
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
              {/*Số tài khoản */}
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
              {/*
              Người liên hệ giao hàng
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
              </div>
                    */}
          
              {/*
              Liên hệ khác */}

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

          {/*Danh sách khách hàng */}
          <Table className="table table-striped">
            <Thead className="table-light">
              <Tr>
                <Th className="align-middle">STT</Th>
                <Th className="align-middle">Tên khách hàng</Th>
                <Th className="align-middle">Điện thoại</Th>
                <Th className="align-middle">Email</Th>
                <Th className="align-middle">Đơn hàng</Th>
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
                        <Td>{i.CompleteName}</Td>
                        <Td>{i.Phone}</Td>
                        <Td>{i.Email}</Td>
                        <Td>
                          <div className="d-flex gap-3">
                            <div
                              className="text-info"
                              onClick={() => {
                                history("/customer/detail/" + i.CustomerId, {
                                  state: { data: i },
                                });
                              }}
                            >
                              <i className="mdi mdi-archive font-size-18"></i>
                            </div>
                          </div>
                        </Td>
                          {/* Cập nhật khách hàng */}
                        <Td>
                          <div className="d-flex gap-3">
                            <div
                              className="text-success"
                              onClick={() => {
                                setDataModal({
                                  CustomerId: i.CustomerId,
                                  CompleteName: i.CompleteName,
                                  Phone: i.Phone,
                                  Email: i.Email,
                                  Status : i.Status,
                                  ImageProfile: i.ImageProfile,
                                });
                                setselectedImageURL(i.ImageProfile || "")
                                setModalVisible(true);
                              }}
                            >
                              <i className="mdi mdi-pencil font-size-18"></i>
                            </div>
                          </div>
                        </Td>
                          {/*Xóa khách hàng*/}
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
                                    RemoveCustomer({
                                      "CustomerId":i.CustomerId
                                    } ).then((response) => {
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
            {/* thêm mới tài khoản khách hàng */}
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
