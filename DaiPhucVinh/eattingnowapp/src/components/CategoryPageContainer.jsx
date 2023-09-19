import { useNavigate } from "react-router-dom";
import { FaStar } from "react-icons/fa";
import { BsClock } from "react-icons/bs";
import { RxDotFilled } from "react-icons/rx";
import React, { useEffect,  useState } from "react";
import axios from 'axios';
import Loader from "./Loader";

const CategoryPageContainer = ({ datas }) => {

  function getRandomFloat(min, max, decimals) {
    const str = (Math.random() * (max - min) + min).toFixed(decimals);
    return parseFloat(str);
  }
  const [loading, setLoading] = useState(false);
  const [currentLocation, setCurrentLocation] = useState([10.3759, 105.4185]);
  const [data, setData] = useState([]);
  
  useEffect(() => {
    const fetchCurrentLocation = async () => {
      try {
        const position = await new Promise((resolve, reject) => {
          navigator.geolocation.getCurrentPosition(resolve, reject);
        });
        const { latitude, longitude } = position.coords;
        setCurrentLocation([latitude, longitude]);
      } catch (error) {
        console.error("Error getting current location:", error);
      }
    };
    fetchCurrentLocation();
  }, []);


// sử dụng API của OSRM
async function calculateTimeAndDistance(startPoint, endPoint) {
  const OSRM_SERVER_URL = 'http://router.project-osrm.org/'; 
  try {
    const response = await axios.get(`${OSRM_SERVER_URL}route/v1/driving/${startPoint[1]},${startPoint[0]};${endPoint[1]},${endPoint[0]}`);
    
    if (response.status === 200) {
      const route = response.data.routes[0];
      const distanceInKm = route.distance / 1000; // Khoảng cách tính bằng kilômét
      const timeInMinutes = (distanceInKm / 40)*60; // Thời gian tính bằng phút
      return { timeInMinutes, distanceInKm };
    } else {
      throw new Error('Error calculating time and distance');
    }
  } catch (error) {
    console.error('Error:', error);
    throw error;
  }
}

// Sử dụng hàm để tính thời gian và khoảng cách giữa currentLocation và một địa điểm cụ thể
async function fetchTimeAndDistanceForLocation(currentLocation, location) {
  try {
    const { Latitude, Longitude } = location;
    const result = await calculateTimeAndDistance(currentLocation, [Latitude, Longitude]);
    return result;
  } catch (error) {
    console.error("Error calculating time and distance:", error);
    return { timeInMinutes: null, distanceInKm: null };
  }
}

useEffect(() => {
  const fetchTimeAndDistanceForLocations = async () => {
    try{
    setLoading(true);
    const updatedLocations = await Promise.all(
      datas.map(async (location) => {
        const ketqua = await fetchTimeAndDistanceForLocation(currentLocation, location);
        const roundedTime = Math.round(ketqua.timeInMinutes * 10) / 10;
        const roundedDistance = Math.round(ketqua.distanceInKm * 10) / 10;
        return { ...location, Time: roundedTime, Distance: roundedDistance };
      })
    );
    setData(updatedLocations);
  } catch (error) {
    console.error(error);
  } finally {
    setLoading(false);
  }
  };
  fetchTimeAndDistanceForLocations();
}, [datas, currentLocation]);

  const history = useNavigate();
  return (


    loading ? (
      <div className="text-center pt-20">
        <Loader />
      </div>
    ) : (
      <div className="w-full h-full flex items-center gap-3 my-12 scroll-smooth overflow-hidden flex-wrap justify-center">
      {data && data.length > 0 ? (
        data.map((n) => 
        (
          <div
            key={n.UserId}
            style={{ height: "300px" }}
            className="border-4 border-orange-100 cursor-pointer gap-3 w-275 h-full min-w-[275px] md:w-200 md:min-w-[200px] bg-white rounded-xl px-1 my-12 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative"
            onClick={() => {
              history("/restaurant/" + n.UserId, {
                state: { data: n },
              });
            }}
          >
            <div className="w-full p-1 rounded-[10px] gap-2 mt-1">
              <div className="h-[150px] w-full overflow-hidden rounded-xl">
                <img
                  src={n.AbsoluteImage}
                  alt=""
                  className="w-full h-full object-cover aspect-square hover:scale-110 transition duration-300 ease-in-out"
                />
              </div>

              <h1 className="mt-2 text-center text-textColor font-bold">
                {n.FullName}
              </h1>

              <p className="mt-1 text-center text-gray-400 text-sm">
                {n.Description}
              </p>

              <div className="mt-1 mb-1 gap-3 flex justify-center items-center">
                <div className="flex gap-1 items-center justify-center justify-items-center">
                  <FaStar className="text-xl text-amber-300" />
                  <h1 className="text-base text-gray-400">
                    {getRandomFloat(1, 5, 1)}
                  </h1>
                </div>
                <div className="flex items-center justify-center justify-items-center">
                  <div className="flex gap-1 justify-center items-center justify-items-center">
                    <BsClock className="text-xl text-gray-400" />
                    <h1 className="text-base text-gray-400">{n.Time} phút</h1>
                  </div>
                  <RxDotFilled className="text-xl text-gray-400" />
                  <h1 className="text-base text-gray-400">{n.Distance} km</h1>
                </div>
              </div>
            </div>
          </div>
        ))
      ):
       (
        <div className="text-center">
          <span>Chưa có cửa hàng hoạt động trên hệ thống gần khu vực của bạn 😣!</span>
        </div>
      )
      }
    </div>    )

  );
};

export default CategoryPageContainer;
