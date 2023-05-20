import React, { useEffect, useRef } from "react";
import { loadGoogleMapScript } from "../framework/mapApi";
import _ from "lodash";
import { SearchLocation } from "../api/location/locationService";

const PlaceAutocomplete = (props) => {
  const placeInputRef = useRef(null);
  const [center, setCenter] = React.useState({ lat: 0, lng: 0 });
  const mapRef = useRef(null);
  useEffect(() => {
    loadGoogleMapScript(() => {
      initPlaceAPI();
      if (props?.hasGoogleMaps) {
        initGoogleMap();
      }
    });
  }, []);
  useEffect(() => {
    if (props.currentMarker && props.isLoadLatLng) {
      console.log(
        "currentMarker",
        props.currentMarker,
        "isLoadLatLng",
        props.isLoadLatLng
      );
      if (!windowMap || !windowMarker) {
        initPlaceAPI();
        if (props?.hasGoogleMaps) {
          initGoogleMap();
        }
      }
    }
  }, [props.currentMarker, props.isLoadLatLng]);
  let windowMap = undefined;
  let windowMarker = undefined;

  async function initGoogleMap() {
    var request = {};
    let centerDefault = { lat: 10.958707525394423, lng: 106.89836112597757 };
    var userLoginResponse = await TakeEmployee_ByUserLogin();
    if (userLoginResponse.success) {
      if (
        userLoginResponse.data.LocationCode != "" &&
        userLoginResponse.data.LocationCode != null
      ) {
        var locationResponse = await SearchLocation(
          request,
          userLoginResponse.data.LocationCode
        );
        if (locationResponse.success) {
          if (
            locationResponse.data.Lat != null &&
            locationResponse.data.Lat != 0 &&
            locationResponse.data.Long != null &&
            locationResponse.data.Long != 0
          ) {
            centerDefault.lat = locationResponse.data.Lat;
            centerDefault.lng = locationResponse.data.Long;
          }
        }
      }
    }
    const mapOptions = {
      center: {
        lat:
          props.currentMarker?.lat && props.currentMarker?.lat != 0
            ? props.currentMarker.lat
            : centerDefault.lat,
        lng:
          props.currentMarker?.lng && props.currentMarker?.lng != 0
            ? props.currentMarker.lng
            : centerDefault.lng,
      },
      zoom: 17,
    };
    var map = new window.google.maps.Map(mapRef.current, mapOptions);
    if (props.currentMarker) {
      const markerOptions = {
        map: map,
        position: { ...mapOptions.center },
      };
      windowMarker = new window.google.maps.Marker(markerOptions);
    }
    windowMap = map;
  }
  async function getCenter() {
    console.log("getCenter");
    var request = {};
    let centerDefault = { lat: 10.958707525394423, lng: 106.89836112597757 };
    var userLoginResponse = await TakeEmployee_ByUserLogin();
    if (userLoginResponse.success) {
      if (
        userLoginResponse.data.LocationCode != "" &&
        userLoginResponse.data.LocationCode != null
      ) {
        var locationResponse = await SearchLocation(
          request,
          userLoginResponse.data.LocationCode
        );
        if (locationResponse.success) {
          if (
            locationResponse.data.Lat != null &&
            locationResponse.data.Lat != 0 &&
            locationResponse.data.Long != null &&
            locationResponse.data.Long != 0
          ) {
            centerDefault.lat = locationResponse.data.Lat;
            centerDefault.lng = locationResponse.data.Long;
            setCenter({
              lat: center.lat,
              lng: center.lng,
            });
            return centerDefault;
          } else {
            setCenter({
              lat: centerDefault.lat,
              lng: centerDefault.lng,
            });
            return centerDefault;
          }
          // else {
          //   return {
          //     // chi nhánh Đồng Nai
          //     lat: 10.3722015,
          //     lng: 105.43268,
          //   };
          // }
        }
      }
    }
  }

  async function moveToMarker(location) {
    var center = new window.google.maps.LatLng(location.lat, location.lng);
    windowMap.panTo(center);
    if (!_.isEmpty(center) && center.lat() !== 0 && center.lng() !== 0) {
      const markerOptions = {
        map: windowMap,
        position: center,
      };
      windowMarker?.setMap(null);
      windowMarker = new window.google.maps.Marker(markerOptions);
    }
  }
  const initPlaceAPI = () => {
    let autocomplete = new window.google.maps.places.Autocomplete(
      placeInputRef.current,
      {
        types: ["establishment", "geocode"],
        strictBounds: false,
        //componentRestrictions: { country: ["vi"] },
        fields: ["formatted_address", "geometry"],
      }
    );
    new window.google.maps.event.addListener(
      autocomplete,
      "place_changed",
      async function () {
        if (!props?.disabled) {
          let place = autocomplete.getPlace();
          localStorage.setItem("@PlaceAddress", JSON.stringify(place));
          if (place?.geometry?.location) {
            const location = place?.geometry?.location;
            moveToMarker({
              lat: location.lat(),
              lng: location.lng(),
            });
          }
          if (props.onCallback) props?.onCallback(place);
        }
      }
    );
  };
  return (
    <>
      <input
        value={props.value}
        onChange={props.onChange}
        ref={placeInputRef}
        data-test="input-address-test"
        placeholder="Nhập địa chỉ thực hiện công việc"
        type="text"
        style={styles.styleInput}
        disabled={props?.disabled}
        className="form-control"
      />
      {props?.hasGoogleMaps && (
        <div className="intro-x mt-3">
          <div
            ref={mapRef}
            id="map"
            style={{
              width: "auto",
              height: 500,
              position: "relative",
              overflow: "hidden",
            }}
          ></div>
        </div>
      )}
    </>
  );
};
const styles = {
  styleInput: {
    backgroundColor: "#f9f9f9",
    color: "black",
  },
  styleText: {
    color: "#FFFFFF",
    opacity: "0.6",
  },
};
export default PlaceAutocomplete;
