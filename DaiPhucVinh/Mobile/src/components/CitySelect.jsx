import * as React from "react";
import { PhoenixSelect } from "../controls";
import { TakeAllCities } from "../api/main/common";

export default function CitySelect({ verticalLabel, label, placeholder, value, onSelectChange }) {
  const [data, setData] = React.useState([]);
  async function onControlInit() {
    var city = await TakeAllCities();

    setData(city.data);
  }
  React.useEffect(() => {
    onControlInit();
  }, [value]);
  return (
    <PhoenixSelect
      verticalLabel={verticalLabel}
      label={label}
      placeholder={placeholder}
      data={data}
      value={value}
      onSelectChange={onSelectChange}
    />
  );
}
