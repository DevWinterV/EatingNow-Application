import * as React from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../../layouts/layout";
import { Login } from "../../api/auth";
import { decrypt, encrypt } from "../../framework/encrypt";
import Swal from "sweetalert2";

export default function LoginPage() {
  const history = useNavigate();
  const { setAuth } = React.useContext(AuthContext);
  const [username, setUsername] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [showPassword, setShowPassword] = React.useState(false);
  // const [isChecked, setIsChecked] = React.useState(false);
  // function changeRemember(e) {
  //   setIsChecked(!isChecked);
  // }
  async function onLogin() {
    if (username.length == 0) {
      Swal.fire({
        title: "Error!",
        text: "Nhập tên đăng nhập !",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    if (password.length == 0) {
      Swal.fire({
        title: "Error!",
        text: "Vui lòng nhập mật khẩu !",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    // if (isChecked !== "") {
    //   localStorage.username = username;
    //   localStorage.password = password;
    //   localStorage.isChecked = isChecked;
    // }
    let response = await Login(username, password);
    if (response && !response.IsError) {
      setAuth(response);
      localStorage.setItem("@auth", encrypt(response));
      if (response.RoleSystem != "Admin") {
        //#region Load UserPermissions
        let permissions = localStorage.getItem("@permissions");
        let userPermissions = decrypt(permissions);
        if (userPermissions.includes("THONGKEDOANHTHU")) {
          history("/revenuestatistics");
        } else {
          history("/");
        }
        //#endregion
      } else {
        history("/");
      }
    } else {
      Swal.fire({
        title: "Error!",
        text: response.ErrorDescription,
        icon: "error",
        confirmButtonText: "OK",
      });
    }
  }
  // React.useEffect(() => {
  //   if (localStorage.isChecked == "true") {
  //     setIsChecked(true);
  //     setUsername(localStorage.username);
  //     setPassword(localStorage.password);
  //   }
  //   if (localStorage.isChecked == "false") {
  //     setIsChecked(true);
  //     setUsername("");
  //     setPassword("");
  //   }
  // }, []);
  return (
    <div className="account-pages my-4 pt-sm-5">
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-8 col-lg-6 col-xl-5">
            <div className="card overflow-hidden">
              <div className="row">
                <div
                  className="text-center"
                  style={{ marginTop: "20px", marginBottom: "20px" }}
                >
                  {/* <img
                    width={100}
                    height={100}
                    src="assets/images/my-logo.png"
                    alt=""
                    className="img-fluid"
                  /> */}
                </div>
              </div>

              <div className="card-body pt-0">
                <div className="p-2">
                  <form className="form-horizontal" action="index.html">
                    <div className="mb-3">
                      <label htmlFor="username" className="form-label">
                        Tên đăng nhập
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="username"
                        placeholder="Nhập tên đăng nhập"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                      />
                    </div>

                    <div className="mb-3">
                      <label className="form-label">Mật khẩu</label>
                      <div className="input-group auth-pass-inputgroup">
                        <input
                          type={showPassword ? "input" : "password"}
                          className="form-control"
                          placeholder="Nhập mật khẩu"
                          aria-label="Password"
                          aria-describedby="password-addon"
                          value={password}
                          onChange={(e) => setPassword(e.target.value)}
                          onKeyDown={(e) => {
                            if (["Enter", "NumpadEnter"].includes(e.code)) {
                              onLogin();
                            }
                          }}
                        />
                        <button
                          className="btn btn-light "
                          type="button"
                          id="password-addon"
                          onClick={() => {
                            setShowPassword(!showPassword);
                          }}
                        >
                          <i
                            className={
                              showPassword
                                ? "mdi mdi-eye-off-outline"
                                : "mdi mdi-eye-outline"
                            }
                          ></i>
                        </button>
                      </div>
                    </div>

                    {/* <div className="form-check mb-3">
                      <label
                        className="form-check-label fw-bold"
                        htmlFor="invalidCheck"
                      >
                        Nhớ mật khẩu
                      </label>
                      <input
                        className="form-check-input"
                        type="checkbox"
                        id="invalidCheck"
                        checked={isChecked}
                        onChange={(e) => changeRemember(e)}
                        style={{ fontSize: "12px" }}
                      />
                    </div> */}
                    <div className="row mt-3 ">
                      <div className="offset-3 col-6 d-grid">
                        <button
                          type="button"
                          onClick={onLogin}
                          className="btn btn-success waves-effect waves-light"
                        >
                          Đăng nhập
                        </button>
                      </div>
                    </div>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
