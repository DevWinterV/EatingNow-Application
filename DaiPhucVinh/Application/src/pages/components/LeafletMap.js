import React, { useRef, useEffect, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.js";
import LeafletGeocoder from "./LeafletGeocoder";


function LeafletMap({ onMapClick }) {
  const [clickLocation, setClickLocation] = useState(null);
  const [currentLocation, setCurrentLocation] = useState(null);
  const greenIcon = new L.Icon({
    iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    tooltipAnchor: [16, -28],
    shadowSize: [41, 41],
    shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
  });
  function handleMapClick(e) {
    setClickLocation(e.latlng);
    const { lat, lng } = e.latlng;
    onMapClick({ lat, lng });
  }
  
  const mapInstance = useRef(null);

  useEffect(() => {
    // Lấy vị trí hiện tại bằng Geolocation API khi component được tạo
    navigator.geolocation.getCurrentPosition(
      (position) => {
        const { latitude, longitude } = position.coords;
        setCurrentLocation([latitude, longitude]);
        mapInstance.current.addEventListener("click", handleMapClick);
        mapInstance.current?.flyTo([latitude, longitude], 10); // Di chuyển đến vị trí hiện tại
      },
      (error) => {
        console.error("Error getting current location:", error);
      }
    );
  }, []);

  return (
    <div>
      <MapContainer
        center={currentLocation || [0, 0]} // Sử dụng currentLocation hoặc tọa độ mặc định [0, 0] nếu chưa có vị trí hiện tại
        zoom={13}
        scrollWheelZoom={false}
        whenCreated={(map) => {
          mapInstance.current = map;
        }}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        <LeafletGeocoder />
        {clickLocation && (
          <Marker position={clickLocation}>
            <Popup>{`Lat: ${clickLocation.lat.toFixed(
              4
            )}, Lng: ${clickLocation.lng.toFixed(4)}`}</Popup>
          </Marker>
        )}
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
  iconUrl: 'https://img.icons8.com/?size=1x&id=pmzAHWwbZBIP&format=png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  tooltipAnchor: [16, -28],
  shadowSize: [41, 41],
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
});
L.Marker.prototype.options.icon = DefaultIcon;
export default LeafletMap;
