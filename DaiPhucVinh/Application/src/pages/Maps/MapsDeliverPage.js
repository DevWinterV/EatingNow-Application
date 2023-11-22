import React, { useEffect, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import { TakeAllDeliveryDriver } from "../../api/store/storeService"; // Thay thế đường dẫn tới file API của bạn
import {  TakeAllProvince } from "../../api/province/provinceService"; // Thay thế đường dẫn tới file API của bạn
// ... import statements
import Select from "react-select";
import { Breadcrumb } from "../../controls";

function MapsDeliverPage() {
  const [deliver, setDelivers] = useState([]);
  const [provinceModal, setProvinceModal] = React.useState([]);
  const [province, setProvince] = React.useState([]);
  const [filter, setFilter] = React.useState({
    ProvinceId: 0,
  });
  const [defaultProvinceModal, setDefaultProvinceModal] = React.useState({
    value: 0,
    label: "Chọn tỉnh thành",
  });
  const [defaultProvince, setDefaultProvince] = React.useState({
    value: 0,
    label: "Tất cả",
  });
  
  //lấy dữ liệu tỉnh thành
  async function onFillProvince() {
    let ProvinceResponse = await TakeAllProvince();
    if (ProvinceResponse.success) {
      setProvince([
        {
          value: 0,
          label: "Tất cả",
        },
        ...ProvinceResponse.data.map((e) => {
          return {
            value: e.ProvinceId,
            label: e.Name,
          };
        }),
      ]);
      setProvinceModal([
        ...ProvinceResponse.data.map((e) => {
          return {
            value: e.ProvinceId,
            label: e.Name,
          };
        }),
      ]);
    }
  }

  useEffect(() => {
    async function fetchDelivers() {
      const response = await TakeAllDeliveryDriver(filter);
      if (response.success) {
        setDelivers(response.data);
      }
    }
    fetchDelivers();
  }, [filter.ProvinceId]);
  useEffect(() => {
    onFillProvince();
  }, []);
  const breadcrumbSources = [
    {
      name: "Tài xế",
      href: "#",
    },
    {
      name: "Bản đồ",
      active: true,
    },
  ];

  // Define a custom icon for the driver marker
  const driverIcon = L.icon({
    iconUrl: "https://img.icons8.com/?size=1x&id=QPHuc4xZpn0y&format=png", // Thay thế bằng đường dẫn đến hình ảnh của tài xế
    iconSize: [40, 40], // Kích thước của biểu tượng
    iconAnchor: [16, 32], // Vị trí neo biểu tượng (x, y)
    popupAnchor: [0, -32], // Vị trí của popup khi mở (x, y)
  });

    let DefaultIcon = L.icon({
      iconUrl: "  https://cdn-icons-png.flaticon.com/128/5216/5216405.png", // Thay thế bằng đường dẫn đến hình ảnh của tài xế
      iconSize: [46, 46],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
      tooltipAnchor: [16, -28],
      shadowSize: [41, 41],
      shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
    });

  

  return (
    <div>
      <Breadcrumb title="Bản đồ" sources={breadcrumbSources} />
       <div className="card" style={{ fontSize: "12px" }}>
       <div className="row">
          <div className="col-lg-3">
              <div className="mb-3">
                <p class="text-start font-weight-bold bg-light p-3 rounded">
                  Số lượng tài xế trên hệ thống: <span id="driver-count" class="text-danger">{deliver.length}</span>
                </p>
                <div className="mb-3">
                    <label className="form-label fw-bold">Tỉnh thành</label>
                    <Select
                      options={province}
                      value={defaultProvince}
                      onChange={(e) => {
                        setDefaultProvince({
                          value: e.value,
                          label: e.label,
                        });
                        setFilter({
                          ...filter,
                          page: 0,
                          pageSize:20,
                          ProvinceId: e.value,
                        });
                      }}
                    />
                  </div>
              </div>

          </div>  
          <div className="col-lg">
          <MapContainer center={[10.3759, 105.4185]} zoom={10} style={{ height: "100vh" }}>
              <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
              />
              
              {deliver.map((driver) => (
                <Marker key={driver.DeliveryDriverId} position={[driver.Latitude, driver.Longitude]} icon={DefaultIcon}>
                  <Popup>
                    <div>
                      <h4>Tài xế: {driver.CompleteName}</h4>
                      <p><strong>Liên hệ:</strong> {driver.Phone}</p>
                      <p><strong>Email:</strong> {driver.Email}</p>
                      <p>
                        <strong>Tọa độ:</strong> {driver.Latitude},{driver.Longitude}
                      </p>
                      <img src={driver.UploadImage} alt={driver.CompleteName} style={{ maxWidth: "100%" }} />
                      <button
                          onClick={() => {
                            window.open("/delivery/edit/"+driver.DeliveryDriverId);
                            localStorage.Code = driver.DeliverDriverId;
                          }}
                          className="bg-info text-white font-bold py-2 px-4 rounded"
                        >
                          Xem chi tiết
                      </button>
                    </div>
                  </Popup>
                </Marker>
              ))}
            </MapContainer>
          </div> 
        </div>   
       </div>
       
    </div>

  );
}

export default MapsDeliverPage;
