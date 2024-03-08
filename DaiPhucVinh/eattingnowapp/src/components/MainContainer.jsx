import React, { useEffect, useState } from "react";
import MenuContainer from "./MenuContainer";
import { useLocation } from "react-router-dom";
import { TakeCategoryByStoreId } from "../api/store/storeService";
import CartContainer from "./CartContainer";
import { useStateValue } from "../context/StateProvider";
import ProfileStoreContainer from "./ProfileStoreContainer";
import Loader from "./Loader";

const MainContainer = () => {
  const [{ cartShow }, dispatch] = useStateValue();
  const { state } = useLocation();
  const [data, setData] = React.useState([]);
  const [isLoading, setIsLoading] = React.useState(false);

  async function onViewAppearing() {
    setIsLoading(true);
    if (state?.data) {
      let response = await TakeCategoryByStoreId(state?.data.UserId);
      setData(response.data);
      console.log(response.data);
    }
    setIsLoading(false);
  }

  const first = data[0];
  
  useEffect(() => {
    onViewAppearing();
  }, [cartShow, state]);

  return (
    <div className="w-full h-auto flex flex-col items-center justify-center">
      <ProfileStoreContainer data={first} />
      {isLoading ? (
        <Loader />
      ) : (
        <MenuContainer data={data} state={state?.data.UserId} />
      )}

      {cartShow && <CartContainer />}
    </div>
  );
};

export default MainContainer;
