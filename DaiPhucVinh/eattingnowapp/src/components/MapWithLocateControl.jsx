// import React, { useState } from "react";
// import {
//   MapContainer,
//   Marker,
//   Popup,
//   TileLayer,
//   useMapEvents,
// } from "react-leaflet";
// import { GeoSearchControl, OpenStreetMapProvider } from "leaflet-geosearch";

// function MapWithLocateControl() {
//   const [position, setPosition] = useState(null);

//   function LocationMarker() {
//     useMapEvents({
//       click() {
//         map.locate();
//       },
//       locationfound(e) {
//         setPosition(e.latlng);
//         map.flyTo(e.latlng, map.getZoom());
//       },
//     });

//     return position === null ? null : (
//       <Marker position={position}>
//         <Popup>You are here</Popup>
//       </Marker>
//     );
//   }

//   const provider = new OpenStreetMapProvider();
//   const searchControl = new GeoSearchControl({
//     provider: provider,
//     style: "bar",
//     autoClose: true,
//     searchLabel: "Enter address",
//     keepResult: true,
//     showMarker: true,
//   });

//   const map = useMapEvents({
//     locationfound(e) {
//       setPosition(e.latlng);
//       map.flyTo(e.latlng, map.getZoom());
//     },
//   });

//   return (
//     <MapContainer center={[10.762622, 106.660172]} zoom={12}>
//       <TileLayer
//         url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
//         attribution='&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors'
//       />
//       <LocationMarker />
//       <GeoSearchControl
//         position="topleft"
//         provider={provider}
//         showMarker={false}
//         showPopup={false}
//         retainZoomLevel={false}
//         animateZoom={true}
//         autoClose={false}
//         searchLabel={"Enter address"}
//         keepResult={true}
//         popupFormat={({ query, result }) => result.label}
//         maxMarkers={3}
//         maxSuggestions={6}
//         zIndex={10000}
//       />
//     </MapContainer>
//   );
// }

// export default MapWithLocateControl;
