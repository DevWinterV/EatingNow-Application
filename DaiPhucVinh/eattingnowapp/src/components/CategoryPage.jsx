import React, { useEffect, useState } from "react";
import CategoryPageContainer from "./CategoryPageContainer";
import { useLocation, useNavigate } from "react-router-dom";
import { TakeStoreByCuisineId } from "../api/store/storeService";
import { useStateValue } from "../context/StateProvider";
import CartContainer from "./CartContainer";
import LeafletMap from "./LeafletMap";
import Loader from "./Loader";

const CategoryPage = () => {
  const [{ cartShow }] = useStateValue();
  const history = useNavigate();
  const { state } = useLocation();
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState([]);
  const [currentLocation, setCurrentLocation] = useState(null);

  async function fetchStoresByCuisineId(cuisineId, latitude, longitude) {
    try {
      setLoading(true);
      const response = await TakeStoreByCuisineId({
        CuisineId: cuisineId,
        latitude: latitude,
        longitude: longitude,
      });
      setData(response.data);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
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
      setCurrentLocation({ latitude, longitude });
    } catch (error) {
      console.error(error);
    }
  }

  async function getCurrentPosition(options = {}) {
    return new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(resolve, reject, options);
    });
  }

  useEffect(() => {
    getCurrentLocation();
  }, []);

  useEffect(() => {
    if (state?.data && currentLocation) {
      fetchStoresByCuisineId(
        state.data.CuisineId,
        currentLocation.latitude,
        currentLocation.longitude
      );
    }
  }, [state, currentLocation]);


  

  return (
    <div className="w-full h-auto flex flex-col items-center justify-center">
      <div>
        {cartShow ? <CartContainer /> : <LeafletMap locations={data} />}
      </div>
      <section className="w-full my-6">
        <p className="text-2xl font-semibold capitalize text-headingColor relative before:absolute before:rounded-lg before:content before:w-16 before:h-1 before:-bottom-2 before:left-0 before:bg-gradient-to-tr from-orange-400 to-orange-600 transition-all ease-in-out duration-100 mr-auto">
          Các cửa hàng gần bạn nhất
        </p>
        {loading ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
          <CategoryPageContainer datas={data} />
        )}
      </section>
    </div>
  );
};

export default CategoryPage;
