import React, { useEffect, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import { TakeAllDeliveryDriver } from "../../api/store/storeService"; // Thay thế đường dẫn tới file API của bạn
// ... import statements

function MapsDeliverPage() {
  const [deliver, setDelivers] = useState([]);

  useEffect(() => {
    async function fetchDelivers() {
      const response = await TakeAllDeliveryDriver();
      if (response.success) {
        setDelivers(response.data);
      }
    }
    fetchDelivers();
  }, []);

  // Define a custom icon for the driver marker
  const driverIcon = L.icon({
    iconUrl: "https://img.icons8.com/?size=1x&id=QPHuc4xZpn0y&format=png", // Thay thế bằng đường dẫn đến hình ảnh của tài xế
    iconSize: [40, 40], // Kích thước của biểu tượng
    iconAnchor: [16, 32], // Vị trí neo biểu tượng (x, y)
    popupAnchor: [0, -32], // Vị trí của popup khi mở (x, y)
  });

  return (
    <div>
      <MapContainer center={[10.3759, 105.4185]} zoom={10} style={{ height: "600px" }}>
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        
        {deliver.map((driver) => (
          <Marker key={driver.DeliverDriverId} position={[driver.Latitude, driver.Longitude]} icon={driverIcon}>
            <Popup>
              <div>
                <h4>{driver.CompleteName}</h4>
                <p><strong>Liên hệ:</strong> {driver.Phone}</p>
                <p><strong>Email:</strong> {driver.Email}</p>
                <p>
                  <strong>Tọa độ:</strong> {driver.Latitude},{driver.Longitude}
                </p>
                <img src={driver.UploadImage} alt={driver.CompleteName} style={{ maxWidth: "100%" }} />
              </div>
            </Popup>
          </Marker>
        ))}
      </MapContainer>
    </div>
  );
}

export default MapsDeliverPage;
