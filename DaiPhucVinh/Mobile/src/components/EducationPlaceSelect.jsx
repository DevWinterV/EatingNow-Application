import * as React from "react";
import { PhoenixSelect } from "../controls";
import { TakeAllEducationPlace } from "../api/main/common";

export default function EducationPlaceSelect({
  verticalLabel,
  label,
  placeholder,
  value,
  onSelectChange,
}) {
  const [data, setData] = React.useState([]);
  async function onControlInit() {
    var response = await TakeAllEducationPlace();
    setData(response.data);
  }
  React.useEffect(() => {
    onControlInit();
  }, []);

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
