import * as React from "react";
import { PhoenixSelect } from "../controls";
import { TakeAllWards } from "../api/main/common";

export default function WardSelect({
  verticalLabel,
  label,
  placeholder,
  value,
  onSelectChange,
  districtRequired,
}) {
  const [data, setData] = React.useState([]);
  async function onControlInit() {
    if (districtRequired) {
      var ward = await TakeAllWards(districtRequired);
      setData(ward.data);
    } else {
      setData([]);
    }
  }
  React.useEffect(() => {
    onControlInit();
  }, [districtRequired]);
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
