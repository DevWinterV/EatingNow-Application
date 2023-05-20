import * as React from "react";
import { PhoenixSelect } from "../controls";
import { TakeAllEducationLevel } from "../api/main/common";

export default function EducationLevelSelect({
  verticalLabel,
  label,
  placeholder,
  value,
  onSelectChange,
}) {
  const [data, setData] = React.useState([]);
  async function onControlInit() {
    var district = await TakeAllEducationLevel();
    setData(district.data);
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
