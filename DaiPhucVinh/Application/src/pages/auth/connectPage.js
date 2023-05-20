import * as React from "react";
import { useNavigate } from "react-router-dom";
import { TenantVerify } from "../../api/auth";
import Swal from "sweetalert2";

export default function ConnectPage() {
  const history = useNavigate();
  const [tenant, setTenant] = React.useState("");
  const onLogin = async () => {
    if (tenant.length == 0) {
      Swal.fire({
        title: "Error!",
        text: "You must enter the PartnerId!",
        icon: "error",
        confirmButtonText: "OK",
      });
      return;
    }
    let response = await TenantVerify(tenant);
    if (response.Success) {
      localStorage.setItem("@tenant", tenant);
      history("/login");
    } else {
      Swal.fire({
        title: "Error!",
        text: response.Message,
        icon: "error",
        confirmButtonText: "OK",
      });
    }
  };
  return (
    <div className="account-pages my-4 pt-sm-5">
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-8 col-lg-6 col-xl-5">
            <div className="card overflow-hidden">
              <div className="bg-primary bg-soft">
                <div className="row">
                  <div className="col-7">
                    <div className="text-primary p-4">
                      <h5 className="text-primary">Connecting to PCheck</h5>
                      <p>Enter your Partner ID to connect the system!</p>
                    </div>
                  </div>
                  <div className="col-5 align-self-end">
                    <img
                      src="/assets/images/profile-img.png"
                      alt=""
                      className="img-fluid"
                    />
                  </div>
                </div>
              </div>
              <div className="card-body pt-0">
                <div>
                  <a href="index.html">
                    <div className="avatar-md profile-user-wid mb-4">
                      <span className="avatar-title rounded-circle bg-light">
                        <img
                          src="/assets/images/logo.svg"
                          alt=""
                          className="rounded-circle"
                          height="34"
                        />
                      </span>
                    </div>
                  </a>
                </div>
                <div className="p-2">
                  <div className="user-thumb text-center mb-4">
                    <img
                      src="/assets/images/logo-dark.png"
                      width={120}
                      alt="thumbnail"
                    />
                    <h5 className="font-size-15 mt-3">Enter your partner ID</h5>
                  </div>
                  <div className="mb-3">
                    <label htmlFor="userpassword">Partner ID</label>
                    <input
                      className="form-control"
                      placeholder="Enter partner ID"
                      value={tenant}
                      onChange={(e) => setTenant(e.target.value)}
                    />
                  </div>
                  <div className="text-end">
                    <button
                      className="btn btn-primary w-md waves-effect waves-light"
                      type="button"
                      onClick={onLogin}
                    >
                      Submit
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
