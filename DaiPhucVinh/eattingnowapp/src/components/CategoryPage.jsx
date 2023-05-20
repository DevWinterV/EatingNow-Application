import * as React from "react";
import CategoryPageContainer from "./CategoryPageContainer";
import { useLocation, useNavigate } from "react-router-dom";
import { TakeStoreByCuisineId } from "../api/store/storeService";
import { useStateValue } from "../context/StateProvider";
import CartContainer from "./CartContainer";
import GetCurrentLocation from "./GetCurrentLocation";
import Loader from "./Loader";
import LeafletMap from "./LeafletMap";
import { useState } from "react";
import MapWithLocateControl from "./MapWithLocateControl";

const CategoryPage = () => {
  const [{ cartShow }] = useStateValue();
  const history = useNavigate();
  const { state } = useLocation();
  const [loading, setLoading] = React.useState(false);
  const [data, setData] = React.useState([]);
  const [filter, setFilter] = React.useState({
    CuisineId: "",
    latitude: "",
    longitude: "",
  });
  const [request, setRequest] = React.useState({
    UserId: 0,
    FullName: "",
    AbsoluteImage: "",
  });
  async function onViewAppearing() {
    setLoading(true);

    if (state?.data) {
      const { latitude, longitude } = await getCurrentLocation();
      setFilter({
        CuisineId: state?.data.CuisineId,
        latitude: latitude,
        longitude: longitude,
      });
      let response = await TakeStoreByCuisineId(filter);
      setData(response.data);
    }
    setLoading(false);
  }

  function getCurrentPosition(options = {}) {
    return new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(resolve, reject, options);
    });
  }

  async function getCurrentLocation() {
    try {
      const position = await getCurrentPosition({
        enableHighAccuracy: true,
        maximumAge: 0,
        timeout: 5000,
      });
      const latitude = position.coords.latitude;
      const longitude = position.coords.longitude;
      return { latitude, longitude };
    } catch (error) {
      console.error(error);
      return null;
    }
  }

  React.useEffect(() => {
    onViewAppearing();
  }, [state, filter.CuisineId]);

  return (
    <div className="w-full h-auto flex flex-col items-center justify-center">
      <div>
        {cartShow ? <CartContainer /> : <LeafletMap locations={data} />}
      </div>
      <section className="w-full my-6">
        <p className="text-2xl font-semibold capitalize text-headingColor relative before:absolute before:rounded-lg before:content before:w-16 before:h-1 before:-bottom-2 before:left-0 before:bg-gradient-to-tr from-orange-400 to-orange-600 transition-all ease-in-out duration-100 mr-auto">
          Danh mục loại hình ăn uống
        </p>
        {loading ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
          <CategoryPageContainer data={data} />
        )}
      </section>
    </div>
  );
};

export default CategoryPage;
