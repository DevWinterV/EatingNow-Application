import React from "react";
import Body from "./Body/Body";
import SideMenu from "./SideMenu/SideMenu";

const AdminContainer = () => {
  return (
    <div className="flex h-[p0vh] items-center justify-center w-[vw] my-10 rounded-xl overflow-hidden">
      <SideMenu />
      <Body />
    </div>
  );
};

export default AdminContainer;
