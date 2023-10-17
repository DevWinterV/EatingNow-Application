import React, { useEffect, useRef, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.js";
import { useNavigate } from "react-router-dom";
import { FaStar } from "react-icons/fa";
import { BsClock } from "react-icons/bs";
import axios from 'axios';
import { RxDotFilled } from "react-icons/rx";
import { useStateValue } from "../context/StateProvider";

function LeafletMap({ locations }) {
  const history = useNavigate();
  function getRandomFloat(min, max, decimals) {
    const str = (Math.random() * (max - min) + min).toFixed(decimals);
    return parseFloat(str);
  }
  const [{ cartShow, customer }, dispatch] = useStateValue();
  const [request, setRequest] = useState({
    CustomerId: "",
    Latitude: "",
    Longittude:"",
  });

  const greenIcon = new L.Icon({
    iconUrl: 'https://cdn-icons-png.flaticon.com/128/7945/7945007.png',
    iconSize: [30, 30],
    shadowSize: [30, 30],
    shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
  });
  
  const mapInstance = useRef(null);

  const [currentLocation, setCurrentLocation] = useState([10.3759416, 105.4185406]);// Lomng Xuyên An giang
  const [data, setData] = useState([]);
  
  useEffect(() => {
    const fetchCurrentLocation = async () => {
      try {
        const position = await new Promise((resolve, reject) => {
          navigator.geolocation.getCurrentPosition(resolve, reject);
        });
        const { latitude, longitude } = position.coords;
        setCurrentLocation([latitude, longitude]);
        mapInstance.current?.flyTo([latitude, longitude], 10); // Di chuyển đến vị trí hiện tại
        setRequest({
          ...request,
          CustomerId: customer,
          Latitude: latitude,
          Longittude: longitude
        });
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
      // Tốc độ chạy trung bình của tài xế là 40Km/h
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
    const updatedLocations = await Promise.all(
      locations.map(async (location) => {
        const ketqua = await fetchTimeAndDistanceForLocation(currentLocation, location);

        const roundedTime = Math.round(ketqua.timeInMinutes * 10) / 10;
        const roundedDistance = Math.round(ketqua.distanceInKm * 10) / 10;
        return { ...location, Time: roundedTime, Distance: roundedDistance };
      })
    );
    setData(updatedLocations);
  };
  fetchTimeAndDistanceForLocations();
}, [locations, currentLocation, request]);

  return (
    <div>
      <MapContainer
        center={currentLocation}
        zoom={13}
        scrollWheelZoom={false}
        style={{ height: "550px" }}
        whenCreated={(map) => {
          mapInstance.current = map;
        }}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
       
        {data.map((location) => (
          
          <Marker
            position={[location.Latitude, location.Longitude]}
            key={location.UserId}
          >
            <Popup>
              <div>
              <h3 style={{ fontSize: "1.5em", fontWeight: "bold" }}>{location.FullName}</h3>
                <p><strong>Giờ mở cửa:</strong> {location.OpenTime}</p>
                <p><strong>Mô tả quán ăn:</strong> {location.Description}</p>
                <p><strong>Liên hệ:</strong> {location.Phone}</p>
                
                <img src={location.AbsoluteImage} alt={location.FullName} style={{ maxWidth: "100%" }} />
                <p><strong>Địa chỉ:</strong> {location.Address}</p>
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
                    <h1 className="text-base text-gray-400">{location.Time} phút</h1>
                  </div>
                  <RxDotFilled className="text-xl text-gray-400" />
                  <h1 className="text-base text-gray-400">{location.Distance} km</h1>
                </div>
              </div>
                <div className="w-full flex items-center justify-center">
                  <button
                    onClick={() => {
                      history("/restaurant/" + location.UserId, {
                        state: { data: location },
                      });
                    }}
                    className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                  >
                    Xem Menu cửa hàng
                  </button>
                </div>
              </div>
            </Popup>
          </Marker>
        ))}
        {currentLocation && (
          <Marker position={currentLocation} icon={greenIcon}>
            <Popup>Vị trí hiện tại</Popup>
          </Marker>
        )}
      </MapContainer>
    </div>
  );
}

let DefaultIcon = L.icon({
  iconUrl: 'https://cdn-icons-png.flaticon.com/128/5693/5693840.png',
  iconSize: [40, 40],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  tooltipAnchor: [16, -28],
  shadowSize: [41, 41],
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
});
L.Marker.prototype.options.icon = DefaultIcon;
export default LeafletMap;
