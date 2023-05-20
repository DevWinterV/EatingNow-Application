import {
  Button,
  SelectMenu,
  Pane,
  InfoSignIcon,
  Heading,
  IconButton,
  CrossIcon,
  Text,
} from "evergreen-ui";
import * as React from "react";
import { TakeAllWard } from "../api/ward/wardService";

export default function SelectWard({
  height,
  selected,
  onSelect,
  isInvalid,
  isShowValidate = true,
  validationMessage,
  marginTop,
}) {
  const [options, setOptions] = React.useState([]);
  const [defaultSelected, setDefaultSelected] = React.useState(selected);
  const [closeSelected, setCloseSelected] = React.useState(false);
  async function ControlInit() {
    var response = await TakeAllWard();
    if (response.success) {
      setOptions(
        response.data.map((opt) => ({ label: opt.Name, value: opt.WardId }))
      );
    }
  }
  React.useEffect(() => {
    ControlInit();
  }, []);
  React.useEffect(() => {
    setDefaultSelected(selected);
    setCloseSelected(true);
  }, [selected]);
  return (
    <Pane width="100%" marginTop={marginTop}>
      <SelectMenu
        title="Chọn xã"
        filterPlaceholder="Tìm..."
        options={options}
        selected={defaultSelected?.value || ""}
        closeOnSelect={closeSelected}
        titleView={({ close, title, headerHeight }) => {
          return (
            <Pane
              display="flex"
              alignItems="center"
              borderBottom="default"
              padding={8}
              height={headerHeight}
              boxSizing="border-box"
            >
              <Pane flex="1" display="flex" alignItems="center">
                <Text size={400}>{title}</Text>
              </Pane>
              <IconButton
                icon={CrossIcon}
                appearance="minimal"
                height={24}
                fontSize={12}
                onClick={() => {
                  setDefaultSelected({
                    value: "",
                    label: "--- Chọn xã ---",
                  });
                  onSelect({
                    value: "",
                    label: "--- Chọn xã ---",
                  });
                  close();
                }}
              />
            </Pane>
          );
        }}
        onSelect={(e) => {
          setDefaultSelected(e);
          onSelect(e);
        }}
      >
        {isShowValidate ? (
          <Button width="100%" className="dropdown-button">
            {defaultSelected?.label || "--- Chọn xã ---"}
          </Button>
        ) : (
          <Button
            width="100%"
            intent="danger"
            style={{
              border: "solid 1px red",
            }}
            className="dropdown-button"
          >
            {defaultSelected?.label || "--- Chọn xã ---"}
          </Button>
        )}
      </SelectMenu>
      {isInvalid && (
        <Pane display="flex" marginTop={10}>
          <InfoSignIcon color="danger" marginRight={4} />
          <Heading size={200} color="red">
            {validationMessage}
          </Heading>
        </Pane>
      )}
    </Pane>
  );
}
