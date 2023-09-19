import React from "react";
import { Route, Routes } from "react-router-dom";
import { useStateValue } from "../context/StateProvider";
import RouterAuth from "./RouterAuth";
import RouterHome from "./RouterHome";

const   Routers = () => {
  const [{ linked }] = useStateValue();
  return (
    <Routes>
      {linked ? (
        <Route path="/*" element={<RouterHome />} />
      ) : (
        <Route path="/*" element={<RouterAuth />} />
      )}
    </Routes>
  );
};

export default Routers;
