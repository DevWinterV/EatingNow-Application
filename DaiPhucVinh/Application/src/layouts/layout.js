import * as React from "react";
import TopNavigation from "./topNavigation";
import Routers from "../Routers";
import { BrowserRouter as Router } from "react-router-dom";
import LeftSidebar from "./leftSideBar";
import { decrypt } from "../framework/encrypt";
import moment from "moment";
import { useEffect, useState } from "react";
const AuthContext = React.createContext();
export { AuthContext };

const Layout = () => {
 
  const [auth, setAuth] = React.useState();
  const value = React.useMemo(() => ({ auth, setAuth }), [auth]);
  async function onViewAppearing() {
    let authCache = localStorage.getItem("@auth");
    if (authCache) {
      const data = decrypt(authCache);
      const timeNow = moment().format("DD-MM-YYYY HH:mm:ss");
      const ExpiredAt = moment(data.ExpiredAt).format("DD-MM-YYYY HH:mm:ss");
      if (data && ExpiredAt < timeNow) {
        localStorage.removeItem("@auth");
        localStorage.removeItem("@permissions");
        setAuth(null);
        window.location = "/login";
      } else {
        setAuth(data);
      }
    }
  }


  React.useEffect(() => {
    onViewAppearing();

  }, []);

  return (
    <AuthContext.Provider value={value}>
      {!value.auth ? (
        <Router>
          <Routers auth={auth} />
        </Router>
      ) : (
        <Router>
          <div id="layout-wrapper">
            <TopNavigation />
            <LeftSidebar />
            <div className="main-content" style={{ fontSize: "12px" }}>
              <div
                className="page-content bg-gray"
                style={{ padding: "47px 12px 60px 12px" }}
              >
                <div className="container-fluid bg-gray">
                  <Routers auth={auth} />
                </div>
              </div>
            </div>
          </div>
        </Router>
      )}
    </AuthContext.Provider>
  );
};

export default Layout;
