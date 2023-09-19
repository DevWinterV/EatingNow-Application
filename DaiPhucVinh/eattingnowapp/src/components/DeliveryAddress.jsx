import React, { useState, useEffect, useRef } from 'react';
import { withGoogleMap, withScriptjs, GoogleMap, Marker } from 'react-google-maps';
import { FaExchangeAlt, FaLocationArrow, FaSearch, FaServer } from 'react-icons/fa';
import { handleSearchAddress } from "../api/googleSearchApi/googleApiService";

const Map = () => {
  const [currentLocation, setCurrentLocation] = useState({ lat: 10.776889, lng: 106.700897 });
  const [selectedAddress, setselectedAddress] = useState({
    lat: 0, 
    lng: 0, 
    formatted_address: '',
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
  
        if (mapRef.current) {
          // Access the GoogleMap component and fit the map bounds to the marker
          const bounds = new window.google.maps.LatLngBounds();
          bounds.extend(new window.google.maps.LatLng(latitude, longitude));
          mapRef.current.fitBounds(bounds);
        }
  
        handleSearchAddress(latitude + "," + longitude).then(
          (response) => {
            if (response.data.status === "OK" && response.data.results.length > 0) {
              setsearchData(null);
              setsearchData(response.data.results);
              // Update the selectedAddress state
              const newSelectedAddress = {
                lat: response.data.results[0].geometry.location.lat,
                lng: response.data.results[0].geometry.location.lng,
                name: response.data.results[0].name,
                formatted_address: response.data.results[0].formatted_address,
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
  


  const handleChangeAddress =() =>{
    setisAddress(!isAddress)
    setsearchData(null)
  }
  const handleClick= (location) => {
        setisAddress(!isAddress)
        setCurrentLocation({ lat: location.geometry.location.lat, lng: location.geometry.location.lng });
        if (mapRef.current) {
          // Access the GoogleMap component and fit the map bounds to the marker
          const bounds = new window.google.maps.LatLngBounds();
          bounds.extend(new window.google.maps.LatLng(location.geometry.location.lat, location.geometry.location.lng));
          mapRef.current.fitBounds(bounds);
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


  };

  const [searchData, setsearchData] = useState([]);
  const handleSearch = (searchAddress) => {
    if(searchAddress != ""){
      handleSearchAddress(searchAddress).then(
        (response) => {
          if (response.data.status === "OK" && response.data.results.length > 0) 
            {
              setsearchData(null)
              setsearchData(response.data.results)
            }
        }
      )
    }else{
      setsearchData(null)
    }

  };
  
  

  useEffect(() => {
        if (mapRef.current) {
          const bounds = new window.google.maps.LatLngBounds();
          bounds.extend(new window.google.maps.LatLng(currentLocation.lat, currentLocation.lng));
          mapRef.current.fitBounds(bounds);
        }
    handleSearch(searchAddress)
    }, [searchAddress, currentLocation]); // Empty dependency array to run this effect once when the component is mounted

  return (
    <div>
       
        
        {
  !isAddress ? (

    <div>
          <h2 className="font-bold text-2xl text-[#171a1f] text-left capitalize mb-4">
    Chọn địa chỉ nhận hàng:
  </h2>
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
      <input
        className="p-2 mt-4 rounded-xl border w-64 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        type="text"
        placeholder="Nhập địa chỉ cần tìm kiếm..."
        value={searchAddress}
        onChange={(e) => setSearchAddress(e.target.value)}
      />
      <div className="max-h-60 overflow-y-auto mt-4">
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
      </div>
    </div>
  ) : (
    <div>
     <h2 className="font-bold text-2xl text-[#171a1f] text-left capitalize mb-4">
        Địa chỉ nhận hàng:
      </h2>
      {selectedAddress.formatted_address && (
          <span className="text-blue-500 ml-2">
            {selectedAddress.formatted_address}
          </span>
        )}
        <button
        id="getcurrentlocation"
        name="getcurrentlocation"
        className="ml-2 p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-300 rounded-lg bg-opacity-50"
        onClick={() => {
          handleChangeAddress();
        }}
      >
        <FaExchangeAlt className="text-2xl" /> Thay đổi vị trí
      </button>
    </div>
  
  

  )
}

            
    <GoogleMap
      defaultZoom={5}
      defaultCenter={{ lat: 10.3716558, lng: 105.4323389 }}
      ref={mapRef}
      style={{ borderRadius: 4 }} // Remove rounded corners
    >
      {currentLocation && <Marker position={currentLocation} />}
    </GoogleMap>

    </div>
  );
};

export default withScriptjs(withGoogleMap(Map));
