import React, { useEffect, useState } from "react";
import CategoryPageContainer from "./CategoryPageContainer";
import { useLocation, useNavigate } from "react-router-dom";
import { TakeStoreByCuisineId } from "../api/store/storeService";
import { useStateValue } from "../context/StateProvider";
import CartContainer from "./CartContainer";
import LeafletMap from "./LeafletMap";
import Loader from "./Loader";
import { calculateDistanceAndTimeProxy } from "../api/googleApi/googleApiDirection";

const CategoryPage = () => {
  const [{ cartShow }] = useStateValue();
  const history = useNavigate();
  const { state } = useLocation();
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState([]);
  const [newdata, setNewData] = useState([]);

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
      const options = {
        enableHighAccuracy: true,
        maximumAge: 0,
        timeout: 5000,
      };
      const position = await getCurrentPosition(options);
      const latitude = position.coords.latitude;
      const longitude = position.coords.longitude;
      setCurrentLocation({ latitude, longitude });
      console.log(currentLocation);
    } catch (error) {
      console.error(error);
    }
  }

  async function getCurrentPosition(options = {}) {
    return new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(resolve, reject, options);
    });
  }
  const [scrollUp, setScrollUp] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      if (window.scrollY > 130) {
        setScrollUp(true);
      } else {
        setScrollUp(false);
      }
    };

    window.addEventListener('scroll', handleScroll);

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      try {
        await getCurrentLocation();
      } catch (error) {
        console.error("Lỗi khi lấy vị trí hiện tại:", error);
      }
    };
    fetchData();
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

  useEffect(() => {
    const fetchTimeAndDistanceForLocations = async () => {
      try {
        setLoading(true);
        const locationPromises = data.map(async (location) => {
          try {
            const result = await calculateDistanceAndTimeProxy(
              [currentLocation.latitude, currentLocation.longitude],
              [location.Latitude, location.Longitude]
            );
            if (result) {
              const { distance, duration } = result;
              const roundedTime = Math.round(duration / 60 * 10) / 10;
              const roundedDistance = Math.round(distance / 1000 * 10) / 10;
              return { ...location, Time: roundedTime, Distance: roundedDistance };
            } else {
              console.log("Không thể tính khoảng cách và thời gian cho vị trí:", location);
              return location;
            }
          } catch (error) {
            console.error("Lỗi khi tính khoảng cách và thời gian:", error);
            return location;
          }
        });
        const updatedLocations = await Promise.all(locationPromises);
        setNewData(updatedLocations);
        setLoading(false);
      } catch (error) {
        console.error("Lỗi khi tải dữ liệu khoảng cách và thời gian:", error);
      } finally {
      }
    };
     fetchTimeAndDistanceForLocations();
  }, [data, currentLocation]);

  return (
    <div className="w-full h-auto flex flex-col items-center justify-center">
      <div className="py-2 flex-1 flex flex-col items-start justify-center gap-6">
        {cartShow ? (
          <CartContainer />
        ) : (
          <div className={`map-container ${scrollUp ? 'fade-out' : ''}`}>
            <LeafletMap locations={newdata} />
          </div>
        )}
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
          <CategoryPageContainer datas={newdata} />
        )}
      </section>
    </div>
  );
};

export default CategoryPage;
