const API_KEY = "AIzaSyDNdJLXYty0U3TSvWO9CRd6NejdrpOi7yQ";
export const loadGoogleMapScript = (callback) => {
  if (
    typeof window.google === "object" &&
    typeof window.google.maps === "object"
  ) {
    callback();
  } else {
    const googleMapScript = document.createElement("script");
    googleMapScript.src = `https://maps.googleapis.com/maps/api/js?key=${API_KEY}&libraries=places&region=VN&language=vi`;
    window.document.body.appendChild(googleMapScript);
    googleMapScript.addEventListener("load", callback);
  }
};
//geocoderRequest: { address: "" } or { location: e.latLng }
export const GetGeocode = async (geocoderRequest) => {
  if (!geocoderRequest.address || geocoderRequest.address.length < 1) {
    return null;
  }
  try {
    let geocoder = new window.google.maps.Geocoder();
    const result = await geocoder.geocode({
      address: geocoderRequest.address,
    });
    if (!result || !result.results) {
      return null;
    }
    const location = result.results[0]?.geometry?.location;
    if (location) {
      return {
        lat: location.lat(),
        lng: location.lng(),
      };
    }
    return null;
  } catch (err) {
    alert("Geocode was not successful for the following reason: " + err);
    return null;
  }
  // .then((result) => {
  //   const { results } = result;
  //   console.log("geocoder result: ", results);
  // })
  // .catch((e) => {
  //   alert("Geocode was not successful for the following reason: " + e);
  // });
};
//GetCoordinatesFromAddress is deprecated. Use GetGeocode instead.
export const GetCoordinatesFromAddress = async (address) => {
  return await GetGeocode({
    address: address,
  });
  // const googleGeoApi = "https://maps.googleapis.com/maps/api/geocode/json";
  // if (!address) {
  //   console.log("Provided address is invalid", true);
  //   return Promise.reject(new Error("Provided address is invalid"));
  // }
  // let url = `${googleGeoApi}?address=${encodeURIComponent(address)}`;
  // if (API_KEY) {
  //   url += `&key=${API_KEY}`;
  // }
  // if (LANGUAGE) {
  //   url += `&language=${LANGUAGE}`;
  // }
  // if (REGION) {
  //   url += `&region=${encodeURIComponent(REGION)}`;
  // }
  // return await handleGeoUrl(url);
};
// const handleGeoUrl = async (url) => {
//   const response = await fetch(url).catch(() =>
//     Promise.reject(new Error("Error fetching data"))
//   );
//   const json = await response.json().catch(() => {
//     return Promise.reject(new Error("Error parsing server response"));
//   });
//   if (json.status === "OK") {
//     console.log(json);
//     return json;
//   }
// };
