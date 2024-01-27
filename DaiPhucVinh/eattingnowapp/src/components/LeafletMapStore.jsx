import React, { useEffect, useRef, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.js";
import { useNavigate } from "react-router-dom";
import { FaStar } from "react-icons/fa";
import { BsClock } from "react-icons/bs";
import { RxDotFilled } from "react-icons/rx";
import { useStateValue } from "../context/StateProvider";

function LeafletMapStore({ locations }) {
  const greenIcon = new L.Icon({
    iconUrl: 'https://cdn-icons-png.flaticon.com/128/7945/7945007.png',
    iconSize: [30, 30],
    shadowSize: [30, 30],
    shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
  });
  
  const mapInstance = useRef(null);
  return (
    <div >
      {locations ? (
        <MapContainer
          center={[locations.Latitude, locations.Longitude]}
          zoom={13}
          scrollWheelZoom={false}
          style={
            { height: "300px", width:"500px" }
          }
          whenCreated={(map) => {
            // Assuming you have a useRef to store the map instance
            mapInstance.current = map;
          }}
        >
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />
          <Marker position={[locations.Latitude, locations.Longitude]}>
            <Popup>
              <div>
                <h3 style={{ fontSize: '1.5em', fontWeight: 'bold' }}>{locations.FullName}</h3>
                <p><strong>Giờ mở cửa:</strong> {locations.OpenTime}</p>
                <p><strong>Mô tả quán ăn:</strong> {locations.Description}</p>
                <p><strong>Liên hệ:</strong> {locations.Phone}</p>
                <p><strong>Địa chỉ:</strong> {locations.Address}</p>
                <div className="mt-1 mb-1 gap-3 flex justify-center items-center">
                  <div className="flex gap-1 items-center justify-center justify-items-center">
                    <FaStar className="text-xl text-amber-300" />
                    {/* Add your star rating value here */}
                    <h1 className="text-base text-gray-400">{/* Your rating value */}</h1>
                  </div>
                </div>
              </div>
            </Popup>
          </Marker>
        </MapContainer>
      ) : (
        <p>Đang tải bản đồ vị trí cửa hàng ...</p>
      )}
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
export default LeafletMapStore;
