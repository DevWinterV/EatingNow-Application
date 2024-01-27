import React from "react";
import { Route, Routes } from "react-router-dom";
import { Login, OTPAuthen } from "../components";
import { useStateValue } from "../context/StateProvider";
import {
  Body,
  CreateFoodList,
  GroupFoods,
  Order,
  Setting,
  Statistical,
} from "./Body";
import SideMenu from "./SideMenu/SideMenu";

const RouterAuth = () => {
  const [{ user }] = useStateValue();
  return (
    <div className="w-screen h-auto flex flex-col bg-white">
      {user != null ? (
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
      ) : (
        <div>
          <Routes>
            <Route path="/*" element={<Login />} />
            <Route path="/otpauthen" element={<OTPAuthen />} />
          </Routes>
        </div>
      )}
    </div>
  );
};

export default RouterAuth;
