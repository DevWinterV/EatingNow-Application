import React, { useEffect, useRef, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.js";
import LeafletGeocoder from "./LeafletGeocoder";

function LeafletMap({ locations }) {
  const [position, setPosition] = useState([10.3759, 105.4185]); // giả sử vị trí mặc định là London
  useEffect(() => {
    navigator.geolocation.getCurrentPosition(
      (position) => {
        setPosition([position.coords.latitude, position.coords.longitude]);
      },
      (error) => {
        console.log(error);
      }
    );
  }, []);
  return (
    <div>
      <MapContainer center={position} zoom={13} scrollWheelZoom={false}>
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        <LeafletGeocoder />
        {locations.map((location) => (
          <Marker
            position={[location.Latitude, location.Longitude]}
            key={location.UserId}
          >
            <Popup>{location.FullName}</Popup>
          </Marker>
        ))}
      </MapContainer>
    </div>
  );
}

let DefaultIcon = L.icon({
  iconUrl: "https://unpkg.com/leaflet@1.6/dist/images/marker-icon.png",
  iconSize: [25, 41],
});
L.Marker.prototype.options.icon = DefaultIcon;
export default LeafletMap;
