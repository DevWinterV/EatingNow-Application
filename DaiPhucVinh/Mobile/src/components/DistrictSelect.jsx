import * as React from "react";
import { PhoenixSelect } from "../controls";
import { TakeAllDistricts } from "../api/main/common";

export default function DistrictSelect({
  verticalLabel,
  label,
  placeholder,
  value,
  onSelectChange,
  cityRequired,
}) {
  const [data, setData] = React.useState([]);
  async function onControlInit() {
    if (cityRequired) {
      var district = await TakeAllDistricts(cityRequired);
      setData(district.data);
    } else {
      setData([]);
    }
  }
  React.useEffect(() => {
    onControlInit();
  }, [cityRequired]);
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
