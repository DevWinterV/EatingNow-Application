import * as React from "react";
import * as Fa from "react-icons/fa";
import { AuthContext } from "./layout";
import { Modal } from "react-bootstrap";
import Swal from "sweetalert2";

export default function TopNavigation() {
  const { auth } = React.useContext(AuthContext);
  const [version, setVersion] = React.useState("");
  const [modalChangePass, setModalChangePass] = React.useState(false);
  const [request, setRequest] = React.useState({
    Password: "",
    NewPassword: "",
  });

  async function onchangePass() {
    if (request.Password.length == 0 || request.NewPassword.length == 0) {
      Swal.fire({
        title: "Lỗi !",
        text: "Mật khẩu không được bỏ trống !",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    if (request.NewPassword.length == 0) {
      Swal.fire({
        title: "Lỗi !",
        text: "Mật khẩu mới không được bỏ trống !",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    var response = await ChangePassword(request);
    if (!response.success) {
      Swal.fire({
        title: "Lỗi!",
        text: "Cập nhật mật khẩu không thành công, vui lòng thử lại !",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    let confirm = await Swal.fire({
      title: "Thành công!",
      text: "Thay đổi mật khẩu thành công!",
      icon: "success",
      confirmButtonText: "OK",
    });
    if (confirm.isConfirmed) {
      onLogout();
    }
  }
  function onLogout() {
    localStorage.removeItem("@auth");
    localStorage.removeItem("@permissions");
    window.location.href = "/";
  }
  function navigationToggle() {
    $("body").toggleClass("sidebar-enable");
    if ($(window).width() >= 992) {
      $("body").toggleClass("vertical-collpsed");
    } else {
      $("body").removeClass("vertical-collpsed");
    }
  }
  function onViewAppearing() {
    var pjson = require("../../package.json");
    setVersion(pjson.version);
  }
  React.useEffect(() => {
    $(".main-content").on("click", autoCloseNavigator);
    function autoCloseNavigator() {
      if ($(window).width() < 992 && $("body").hasClass("sidebar-enable")) {
        $("body").removeClass("sidebar-enable");
      }
    }
    onViewAppearing();
  }, []);

  return (
    <header id="page-topbar">
      <div className="navbar-header" style={{ height: "33px" }}>
        <div className="d-flex">
          <div className="navbar-brand-box">
            <a href="#" className="logo logo-dark">
              <span className="logo-sm">
                <img src="/assets/images/my-logo.png" alt="" height="40" />
              </span>
              <span className="logo-lg" style={{ marginLeft: "-70px" }}>
                <img src="/assets/images/my-logo.png " alt="" height="24" />
              </span>
            </a>
            <a
              href="#"
              className="logo logo-light"
              style={{ fontSize: "22px" }}
            >
              <span className="logo-sm">
                <img src="/assets/images/my-logo.png" alt="" height="80" />
              </span>
              <span className="logo-lg" style={{ marginLeft: "-70px" }}>
                <img src="/assets/images/my-logo.png" alt="" height="50" />
              </span>
              EattingNow
            </a>
          </div>
          <button
            type="button"
            className="btn btn-sm px-3 font-size-16 header-item waves-effect"
            onClick={navigationToggle}
          >
            <Fa.FaBars />
          </button>
          <div style={{ marginTop: "25px" }}>
            <label>Version: {version}</label>
          </div>
        </div>
        <div className="d-flex" style={{ height: "33px" }}>
          <div className="dropdown d-inline-block">
            <button
              type="button"
              className="btn header-item waves-effect"
              id="page-header-user-dropdown"
              data-bs-toggle="dropdown"
              aria-haspopup="true"
              aria-expanded="false"
              style={{ height: "33px" }}
            >
              <img src="/assets/images/generic_avatar.png" alt="" height="24" />
              <span className="d-none d-xl-inline-block ms-1">
                {auth?.FullName}
              </span>
              <i className="mdi mdi-chevron-down d-none d-xl-inline-block"></i>
            </button>
            <div className="dropdown-menu dropdown-menu-end">
              <div
                className="dropdown-item"
                onClick={() => setModalChangePass(true)}
              >
                <i className="bx bx-lock-open font-size-16 align-middle me-1"></i>
                <span key="t-lock-screen">Đổi mật khẩu</span>
              </div>
              <div className="dropdown-divider"></div>
              <div className="dropdown-item text-danger" onClick={onLogout}>
                <i className="bx bx-power-off font-size-16 align-middle me-1 text-danger"></i>
                <span key="t-logout">Đăng xuất</span>
              </div>
            </div>
          </div>
        </div>
      </div>
      <Modal
        size="sm"
        show={modalChangePass}
        onEscapeKeyDown={() => setModalChangePass(false)}
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
          <h5 className="modal-title" id="staticBackdropLabel">
            Đổi mật khẩu
          </h5>
          <button
            type="button"
            className="btn-close"
            onClick={() => setModalChangePass(false)}
          ></button>
        </Modal.Header>
        <Modal.Body>
          <div className="card-body">
            <div className="row">
              <div className="col-lg-12">
                <div className="mb-3">
                  <label className="form-label">Mật khẩu cũ</label>
                  <input
                    type="password"
                    className="form-control"
                    value={request.Password}
                    onChange={(e) =>
                      setRequest({
                        ...request,
                        Password: e.target.value,
                      })
                    }
                  />
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-lg-12">
                <div className="mb-3">
                  <label className="form-label">Mật khẩu mới</label>
                  <input
                    type="password"
                    className="form-control"
                    value={request.NewPassword}
                    onChange={(e) =>
                      setRequest({
                        ...request,
                        NewPassword: e.target.value,
                      })
                    }
                  />
                </div>
              </div>
            </div>
          </div>
        </Modal.Body>
        <Modal.Footer>
          <button
            type="button"
            className="btn btn-success"
            onClick={onchangePass}
          >
            Lưu
          </button>
          <button
            type="button"
            className="btn btn-secondary"
            onClick={() => setModalChangePass(false)}
          >
            Đóng
          </button>
        </Modal.Footer>
      </Modal>
    </header>
  );
}
