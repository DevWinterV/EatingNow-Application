import React, { useState, useEffect, useRef } from 'react';
//import { withGoogleMap, withScriptjs, GoogleMap, Marker } from 'react-google-maps';
import { FaExchangeAlt, FaLocationArrow, FaSearch, FaServer } from 'react-icons/fa';
import { handleSearchAddress ,searchAddressGGAPI} from "../api/googleApi/googleApiService";
import debounce from 'lodash/debounce';
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";

function Maps({ requestAddress , onSelectedAddressChange, location}) {
  const [currentLocation, setCurrentLocation] = useState([location != null ?location.latitude : 10.3753212, location != null ?location.longitude : 105.4185406]);// Lomng Xuyên An giang
  const [selectedAddress, setselectedAddress] = useState({
    lat: 0, 
    lng: 0, 
    formatted_address: 'Vị trí hiện tại của bạn',
    name: '',
  });
  const mapRef = useRef(null);
  const [isAddress, setisAddress] = useState(false);
  const [searchAddress, setSearchAddress] = useState(''); // Thêm state cho địa chỉ tìm kiếm

  const handleGetCurrentLocation = () => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        const { latitude, longitude } = position.coords;
        setCurrentLocation({ lat: latitude, lng: longitude });
        setisAddress(!isAddress);
  
         // Di chuyển bản đồ đến vị trí mới khi currentLocation thay đổi
        if (mapRef.current) {
          mapRef.current.setView(currentLocation, mapRef.current.getZoom());
        }
        searchAddressGGAPI(latitude + "," + longitude).then(
          (data) => {
            if (data.status === "OK" && data.results.length > 0) {
              setsearchData(null);
              setsearchData(data.results);
              // Update the selectedAddress state
              const newSelectedAddress = {
                lat: data.results[0].geometry.location.lat,
                lng: data.results[0].geometry.location.lng,
                name: data.results[0].name,
                formatted_address: data.results[0].formatted_address,
              };
              // Use the setselectedAddress function to update the state
              setselectedAddress(newSelectedAddress);
            }
          }
        );
      });
    } else {
      alert("Geolocation is not supported by your browser.");
    }
  };

  // Hàm để thay đổi dữ liệu selectedAddress trong component con
  const handleChangeSelectedAddress = (selected) => {
    setselectedAddress(selected);
    // Gọi hàm callback để truyền dữ liệu lên component cha
    onSelectedAddressChange(selected);
  };



  const handleDebouncedSearch = debounce((searchValue) => {
    // Thực hiện tìm kiếm với giá trị searchValue ở đây
    handleSearch(searchValue);
  }, 1000); // Đợi 1 giây trước khi thực hiện tìm kiếm
  /*
  const handleInputChange = (event) => {
    const searchValue = event.target.value;
    setSearchAddress(event.target.value);
  
    // Sử dụng hàm xử lý tìm kiếm với độ trễ
    handleDebouncedSearch(searchValue);
  };*/
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setSearchAddress(value);
    if(value === ''){
      setsearchData(null)
    }else{
      const fullAddress = `${value} ${requestAddress.Ward} ${requestAddress.District} ${requestAddress.Province}`;
      handleSearch(fullAddress);
    }
  };

  const handleChangeAddress =() =>{
    setisAddress(!isAddress)
    setsearchData(null)
  }
  const handleClick= (location) => {
        setisAddress(!isAddress)
        setCurrentLocation({ lat: location.geometry.location.lat, lng: location.geometry.location.lng });
          // Di chuyển bản đồ đến vị trí mới khi currentLocation thay đổi
          if (mapRef.current) {
            mapRef.current.setView(currentLocation, mapRef.current.getZoom());
          }
        // Update the selectedAddress state
        const newSelectedAddress = {
          lat: location.geometry.location.lat,
          lng: location.geometry.location.lng,
          name: location.name,
          formatted_address: location.formatted_address,
        };
        // Use the setselectedAddress function to update the state
        setselectedAddress(
          newSelectedAddress
        )
        handleChangeSelectedAddress(newSelectedAddress)

  };

  const [searchData, setsearchData] = useState([]);
  
  const handleSearch = (value) => {
    if(value != ""){
      searchAddressGGAPI(value).then(
        (data) => {
          if (data && data.status === "OK" && data.results.length > 0) 
            {
              setsearchData(null)
              setsearchData(data.results)
            }
        }
      )
    }else{
      setsearchData(null)
    }

  };
  
  useEffect(() => {
        // Di chuyển bản đồ đến vị trí mới khi currentLocation thay đổi
        if(location != null){
          setCurrentLocation({lat: location.latitude, lng: location.longitude})
          setselectedAddress({
            ...selectedAddress,
            formatted_address: location.formatted_address,
           } )
           setSearchAddress(location.formatted_address);
        }
        if (mapRef.current) {
          mapRef.current.setView(currentLocation, mapRef.current.getZoom());
        }
    }, [ currentLocation.latitude, currentLocation.longitude, location]); // Empty dependency array to run this effect once when the component is mounted

  return (
    <div className='w-100' > 
      {
        /*
        <GoogleMap
          defaultZoom={8}
          defaultCenter={{ lat: 10.3716558, lng: 105.4323389 }}
          ref={mapRef}
          style={{ borderRadius: 4 }} // Remove rounded corners
        >
          {currentLocation && <Marker position={currentLocation} />}
        </GoogleMap>
        */
      }
       
        {
        !isAddress ? (
          <div>
            {
/*
 <button
              id="getcurrentlocation"
              name="getcurrentlocation"
              className="ml-2 p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-300 rounded-lg bg-opacity-50"
              onClick={() => {
                handleGetCurrentLocation();
              }}
            >
              <FaLocationArrow className="text-2xl" /> Sử dụng vị trí hiện tại
            </button>
*/
            }
           
              <div> 
              <input
              className="p-2 mt-4 rounded-xl border w-full focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              type="text"
              placeholder={
                selectedAddress.formatted_address == ""?(
                  "Nhập địa chỉ cụ thể"
                ):(
                  selectedAddress.formatted_address
                )
              }
              value={searchAddress}
                onChange={(e) => handleInputChange(e)}
                onKeyDown={(e) => {
                  if (e.key === "Enter") {
                  if(  requestAddress != null ){
                      // Thực hiện hành động khi người dùng nhấn Enter
                      handleSearch(searchAddress +" "+ requestAddress.Ward +" "+requestAddress.District +" " +requestAddress.Province)
                  }     
                  }
                }}
            />
            <div className="max-h-60 overflow-y-auto mt-2">
              <ul className="list-none p-0 m-0">
                {searchData && searchData.length > 0 ? (
                  searchData.map((result, index) => (
                    <li
                      key={index}
                      className="bg-white shadow-lg rounded-lg p-4 mb-4 cursor-pointer"
                      onClick={() => handleClick(result)
                      }
                    >
                      <img src={result.icon} alt="" />
                      <h2 className="text-lg font-semibold mb-2">
                        {result.name}
                      </h2>
                      <p className="text-sm text-gray-600">
                        {result.formatted_address}
                      </p>
                    </li>
                  ))
                ) : null}
              </ul>
            </div></div>
          </div>
        ) : (
          <div>
              <h2 className="font-bold text-2xl text-[#171a1f] text-center capitalize mb-4 mt-2">
                 Địa chỉ:
              </h2>
        
            <div className='text-center'>
            {selectedAddress.formatted_address && (
                <span className="text-blue-500 ml-2">
                  {selectedAddress.formatted_address}
                </span>
              )}

            </div>
        
        
              <button
              id="getcurrentlocation"
              name="getcurrentlocation"
              className="ml-2 mt-2 p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-300 rounded-lg bg-opacity-50"
              onClick={() => {
                handleChangeAddress();
              }}
            >
              <FaExchangeAlt className="text-2xl" /> Thay đổi vị trí
            </button>
          </div>
        )
        }
    
      <MapContainer
        center={ currentLocation}
        zoom={15}
        style={{ height: '300px', width: '100%' }}
        ref={mapRef}
      >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        />
        <Marker position={currentLocation}>
          <Popup>
            {selectedAddress.formatted_address}
          </Popup>
        </Marker>
      </MapContainer>
  
    </div>
  );
};

export default Maps;
