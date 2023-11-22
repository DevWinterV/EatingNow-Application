import React, { useEffect, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import { TakeAllStore } from "../../api/store/storeService"; // Thay thế đường dẫn tới file API của bạn
import Select from "react-select";
import { TakeAllProvince } from "../../api/province/provinceService";
import { Breadcrumb } from "../../controls";
import { Button } from "bootstrap";

function MapsPage() {
  const [stores, setStores] = useState([]);
  const [currentLocation, setCurrentLocation] = useState([10.3759, 105.4185]); // Vị trí mặc định
  const [provinceModal, setProvinceModal] = React.useState([]);
  const [province, setProvince] = React.useState([]);
  const [filter, setFilter] = React.useState({
    ItemCategoryCode: 0,
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
  const breadcrumbSources = [
    {
      name: "Cửa hàng",
      href: "#",
    },
    {
      name: "Bản đồ",
      active: true,
    },
  ];
  

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
    async function fetchStores() {
      const response = await TakeAllStore(filter);
      if (response.success) {
        setStores(response.data);
      }
    }
    fetchStores();
    onFillProvince();
  }, [filter]);

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
    <>
       <Breadcrumb title="Bản đồ" sources={breadcrumbSources} />
       <div className="card" style={{ fontSize: "12px" }}>
          <div className="row">
            <div className="col-lg-3 m-10">
                  <div className="mb-3 ">
                    <p class="text-start font-weight-bold bg-light p-3 rounded">
                      Số lượng cửa hàng trên hệ thống: <span id="driver-count" class="text-danger">{stores.length}</span>
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
              <MapContainer center={currentLocation} zoom={10} style={{ height: "100vh" }}>
                <TileLayer
                  attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                  url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
              
                {stores.map((store) => (
                  <Marker key={store.UserId} position={[store.Latitude, store.Longitude]}>
                  <Popup>
                      <div>
                        <h3>{store.FullName}</h3>
                        <p><strong>Giờ mở cửa:</strong> {store.OpenTime}</p>
                        <p><strong>Mô tả quán ăn:</strong> {store.Description}</p>
                        <p><strong>Liên hệ:</strong> {store.Phone}</p>
                        <img src={store.AbsoluteImage} alt={store.FullName} style={{ maxWidth: "100%" }} />
                        <p><strong>Địa chỉ:</strong> {store.Address}</p>  
                        <button
                          onClick={() => {
                            window.open("/store/detail/" + store.UserId);
                            localStorage.Code = store.Code;
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
     
    </>
  );
}

export default MapsPage;
