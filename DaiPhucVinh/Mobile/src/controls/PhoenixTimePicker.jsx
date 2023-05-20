import moment from "moment";
import * as React from "react";
import { Dimensions, TouchableOpacity } from "react-native";
import { Text, Div, Icon, Button, Modal } from "react-native-magnus";
//https://www.npmjs.com/package/react-native-animated-wheel-picker
import Picker from "react-native-animated-wheel-picker";

export default function PhoenixTimePicker({ label, value, onChange }) {
  const [selectValue, setSelectedValue] = React.useState(value);
  const [visible, setVisible] = React.useState(false);
  const deviceHeight = Dimensions.get("screen").height;
  const DataHours = [...Array(24).keys()].map((i) => {
    return { title: i.toString().padStart(2, "0"), value: i };
  });
  const DataMinutes = [...Array(12).keys()].map((i) => {
    return { title: (i * 5).toString().padStart(2, "0"), value: i * 5 };
  });
  const [hour, setHour] = React.useState(0);
  const [minute, setMinute] = React.useState(0);
  React.useEffect(() => {
    setSelectedValue(value);
  }, [value]);
  React.useEffect(() => {
    var timeArr = selectValue.split(":");
    setHour(Number(timeArr[0]));
    setMinute(Number(timeArr[1]));
  }, [selectValue]);

  function onCancel() {
    setVisible(false);
  }
  function onDone() {
    setVisible(false);
    var hourValue = hour?.title || "00";
    var minuteValue = minute?.title || "00";
    if (onChange) onChange(`${hourValue}:${minuteValue}`);
  }
  return (
    <Div>
      {label && (
        <Text ml={8} mt={8} fontSize={"lg"}>
          {label}
        </Text>
      )}
      <Button
        borderWidth={1}
        rounded={8}
        h={48}
        px={15}
        pt={10}
        mt={4}
        bg="white"
        maxW={Dimensions.get("screen").width * 0.415}
        color="gray900"
        borderColor="gray400"
        suffix={<Icon name="clock" fontFamily="Feather" color="primary" fontSize="xl" />}
        onPress={() => setVisible(true)}
      >
        <Text flex={1} fontSize={"lg"}>
          {selectValue ? selectValue : "--:--"}
        </Text>
      </Button>
      <Modal isVisible={visible} h={deviceHeight * 0.3} roundedTopLeft={25} roundedTopRight={25}>
        <Div flex={1}>
          <Div row alignItems="center" justifyContent="space-between" px={16} py={8} mt={8}>
            <TouchableOpacity onPress={onCancel}>
              <Text fontWeight="bold" color="blue500" fontSize="lg">
                Huỷ
              </Text>
            </TouchableOpacity>
            <Text fontWeight="bold" fontSize="xl" ml={30}>
              Chọn giờ
            </Text>
            <TouchableOpacity onPress={onDone}>
              <Text fontWeight="bold" color="blue500" fontSize="lg">
                Hoàn tất
              </Text>
            </TouchableOpacity>
          </Div>
          <Div row>
            <Picker
              pickerData={DataHours}
              maskedComponents={
                <Div>
                  <Div h={30 * Math.trunc(5 / 2)} bg="gray400" />
                  <Div h={30} bg="black" />
                  <Div h={30 * Math.trunc(5 / 2)} bg="gray400" />
                </Div>
              }
              initialIndex={hour}
              textStyle={{ fontSize: 27, color: "black" }}
              onSelected={(item) => setHour(item)}
            />
            <Picker
              pickerData={DataMinutes}
              maskedComponents={
                <Div>
                  <Div h={30 * Math.trunc(5 / 2)} bg="gray400" />
                  <Div h={30} bg="black" />
                  <Div h={30 * Math.trunc(5 / 2)} bg="gray400" />
                </Div>
              }
              initialIndex={minute / 5}
              textStyle={{ fontSize: 27, color: "black" }}
              onSelected={(item) => setMinute(item)}
            />
          </Div>
        </Div>
      </Modal>
    </Div>
  );
}
