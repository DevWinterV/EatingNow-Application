import React, { useEffect } from "react";
import { Route, Routes  } from "react-router-dom";
import { Login, OTPAuthen } from "../components";
import {
  Body,
  CreateFoodList,
  GroupFoods,
  Order,
  Setting,
  Statistical,
} from "./Body";
import SideMenu from "./SideMenu/SideMenu";
import {  CheckStatusAccout } from "../api/auth/authService";
import { toaster } from "evergreen-ui";
import { actionType } from "../context/reducer";
import { useStateValue } from "../context/StateProvider";

const RouterAuth = () => {
  const [{ customer, linked, token ,user}, dispatch] = useStateValue();
  async function onViewAppearing() {
    if (user) {
      var reponse = await CheckStatusAccout({
        "Username": user.Username
      });
      if(reponse.success != true){
        dispatch({
          type: actionType.SET_USER,
          user: null,
        });
        localStorage.setItem("user", null);
        toaster.danger(reponse.message);
      }
    }
  }

  useEffect(()=> {
    onViewAppearing();
  },[])
  return (
    <div className="w-screen h-auto flex flex-col bg-white">
      {user != null && user.AccountId == 1 ?
        (
          <div className="flex items-center justify-center bg-white">
          <div className="flex shadow-2xl m-auto h-[100vh] items-center justify-center w-[100vw]  rounded-xl overflow-hidden">
            <SideMenu />
            <Routes>
              <Route path="/*" element={<Body />} />
              <Route path="/statistical" element={<Statistical />} />
              <Route path="/groupfoods" element={<GroupFoods />} />
              <Route path="/foodlists" element={<Body />} />
              <Route path="/orders" element={<Order />} />
              <Route path="/settings" element={<Setting />} />
              <Route path="/createfoodlist" element={<CreateFoodList />} />
              <Route path="/editfoodlist/:id" element={<CreateFoodList />} />
            </Routes>
          </div>
          </div>
        ) : user != null && user.AccountId == 7 ? (
          <div className="flex items-center justify-center bg-white">
          <div className="flex shadow-2xl m-auto h-[100vh] items-center justify-center w-[100vw]  rounded-xl overflow-hidden">
            <SideMenu />
            <Routes>
              <Route path="/*" element={<Body />} />
              <Route path="/statistical" element={<Statistical />} />
              <Route path="/foodlists" element={<Body />} />
              <Route path="/orders" element={<Order />} />
            </Routes>
          </div>
          </div>
        ) : (
          <div>
            <Routes>
              <Route path="/*" element={<Login />} />
              <Route path="/otpauthen" element={<OTPAuthen />} />
            </Routes>
          </div>
        )
      }
    </div>
  );
};

export default RouterAuth;
