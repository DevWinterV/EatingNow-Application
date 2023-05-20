import * as React from "react";
import { GoogleMap, useLoadScript, Marker } from "@react-google-maps/api";
import usePlacerAutocomplete, {
  getGeocode,
  getLatLng,
} from "use-places-autocomplete";
import {
  Combobox,
  ComboboxInput,
  ComboboxPopover,
  ComboboxList,
  ComboboxOption,
} from "@reach/combobox";
import "@reach/combobox/styles.css";

export default function Places() {
  const { isLoaded } = useLoadScript({
    googleMapsApiKey: "AIzaSyDNdJLXYty0U3TSvWO9CRd6NejdrpOi7yQ",
  });

  if (!isLoaded) return <div>Loading...</div>;
  return <Map />;
}

function Map() {
  const center = React.useMemo(() => ({ lat: 44, lng: -80 }), []);
  const [selected, setSelected] = React.useState(null);
  return (
    <>
      {/* <div className="places_container">
        <PlacesAutocomplete setSelected={setSelected} />
      </div> */}
      <GoogleMap
        zoom={10}
        center={center}
        mapContainerClassName="map-container"
      >
        {/* {selected && <Marker position={selected} />} */}
        <Marker position={center} />
      </GoogleMap>
    </>
  );
}

const PlacesAutocomplete = ({ setSelected }) => {
  const {
    ready,
    value,
    setValue,
    suggestions: { status, data },
    clearSuggestions,
  } = usePlacerAutocomplete();
  const handleSelect = async (address) => {
    setValue(address, false);
    clearSuggestions();

    const results = await getGeocode({ address });
    const { lat, lng } = await getLatLng(results[0]);
    setSelected({ lat, lng });
  };
  return (
    <Combobox onSelect={handleSelect}>
      <ComboboxInput
        value={value}
        onChange={(e) => setValue(e.target.value)}
        disable={!ready}
        className="combobox-input"
        placehoder="Nhập địa chỉ"
      />
      <ComboboxPopover>
        <ComboboxList>
          {status === "OK" &&
            data.map(({ place_id, description }) => (
              <ComboboxOption key={place_id} value={description} />
            ))}
        </ComboboxList>
      </ComboboxPopover>
    </Combobox>
  );
};
