import React, { useRef } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.js";
import LeafletGeocoder from "./LeafletGeocoder";

function LeafletMap({ onMapClick }) {
  const position = [10.3759, 105.4185];
  const [clickLocation, setClickLocation] = React.useState(null);
  const mapInstance = useRef(null);

  function handleMapClick(e) {
    setClickLocation(e.latlng);
    const { lat, lng } = e.latlng;
    onMapClick({ lat, lng });
  }
  return (
    <div>
      <MapContainer
        center={position}
        zoom={13}
        scrollWheelZoom={false}
        whenCreated={(map) => {
          mapInstance.current = map;
          mapInstance.current.addEventListener("click", handleMapClick);
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
