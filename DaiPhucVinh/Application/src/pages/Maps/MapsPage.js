import React, { useEffect, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import { TakeAllStore } from "../../api/store/storeService"; // Thay thế đường dẫn tới file API của bạn

function MapsPage() {
  const [stores, setStores] = useState([]);
  const [currentLocation, setCurrentLocation] = useState([10.3759, 105.4185]); // Vị trí mặc định

  useEffect(() => {
    async function fetchStores() {
      const response = await TakeAllStore();
      if (response.success) {
        setStores(response.data);
      }
    }
    fetchStores();
  }, []);

   // Lấy vị trí hiện tại khi component mount
   useEffect(() => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const { latitude, longitude } = position.coords;
          setCurrentLocation([latitude, longitude]);
        },
        (error) => {
          console.error("Error getting current location:", error);
        }
      );
    } else {
      console.error("Geolocation is not supported by this browser.");
    }
  }, []);

  return (
    <div>
      <MapContainer center={currentLocation} zoom={10} style={{ height: "600px" }}>
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
       
        {stores.map((store) => (
          <Marker key={store.StoreId} position={[store.Latitude, store.Longitude]}>
          <Popup>
              <div>
                <h3>{store.FullName}</h3>
                <p><strong>Giờ mở cửa:</strong> {store.OpenTime}</p>
                <p><strong>Mô tả quán ăn:</strong> {store.Description}</p>
                <p><strong>Liên hệ:</strong> {store.Phone}</p>
                <img src={store.AbsoluteImage} alt={store.FullName} style={{ maxWidth: "100%" }} />
                <p><strong>Địa chỉ:</strong> {store.Address}</p>
              </div>
            </Popup>          
            </Marker>
        ))}
      </MapContainer>
    </div>
  );
}

export default MapsPage;
