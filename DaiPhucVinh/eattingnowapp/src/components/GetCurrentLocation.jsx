import axios from "axios";
import React, { useEffect, useState } from "react";
import { MdLocationOn } from "react-icons/md";

const API_endpoint = `https://api.openweathermap.org/data/2.5/weather?`;
const API_key = `edb5f4a6fe4b1b16d2a481e6f5db51b1`;

const GetCurrentLocation = () => {
  const [latitude, setLatitude] = useState("");
  const [longitude, setLongitude] = useState("");
  const [responseData, setResponseData] = useState("");

  useEffect(() => {
    navigator.geolocation.getCurrentPosition((position) => {
      setLatitude(position.coords.latitude);
      setLongitude(position.coords.longitude);
    });

    let finalAPIEndPoint = `${API_endpoint}lat=${latitude}&lon=${longitude}&exclude=hourly,daily&appid=${API_key}`;

    if (latitude != "" && longitude != "") {
      axios
        .get(finalAPIEndPoint)
        .then((response) => {
          setResponseData(response.data);
        })
        .catch(function (error) {});
    }
  }, [latitude, longitude]);

  return (
    <div>
      <div className="text-[2.0rem] lg:text-[2.0rem] font-bold tracking-wide py-4 text-headingColor">
        <div className="relative flex items-center justify-center text-[2.0rem] lg:text-[2.0rem]">
          <MdLocationOn className="text-red-700 text-2xl cursor-pointer w-8 h-8 " />
          Các cửa hàng gần khu vực{" "}
          <span className="text-orange-600 text-[2.0rem] lg:text-[2.0rem]">
            {responseData.name}
          </span>
        </div>
      </div>
    </div>
  );
};

export default GetCurrentLocation;
