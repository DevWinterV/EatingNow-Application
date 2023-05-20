import * as React from "react";
import { Dimensions, Pressable } from "react-native";
import moment from "moment";
import CalendarPicker from "react-native-calendar-picker";
import { Div, Icon, Modal, Text } from "react-native-magnus";

export default function PhoenixDatePicker({
  selectedDate,
  verticalLabel,
  label,
  onChange,
  isIcon,
  isTime,
  width = "48%",
}) {
  const [show, setShow] = React.useState(false);
  const [dateValue, setDateValue] = React.useState(moment());
  const deviceHeight = Dimensions.get("screen").height;
  React.useEffect(() => {
    setDateValue(selectedDate);
  }, [selectedDate]);
  return (
    <>
      <Pressable
        onPress={() => {
          setShow(true);
        }}
        style={{ width: width }}
      >
        {verticalLabel && (
          <Text mb={4} mt={8} fontSize={"lg"}>
            {label}
          </Text>
        )}
        <Div
          row
          borderWidth={1}
          h={48}
          rounded={8}
          borderColor="gray400"
          px={15}
          alignItems="center"
        >
          <Text color="text" fontSize="lg">
            {isTime
              ? moment(dateValue).format("DD-MM-yyyy HH:mm")
              : moment(dateValue).format("DD-MM-yyyy")}
          </Text>
          {isIcon && (
            <Icon
              name="calendar-outline"
              fontFamily="Ionicons"
              color="primary"
              fontSize="xl"
              ml={30}
            />
          )}
        </Div>
      </Pressable>

      <Modal
        isVisible={show}
        onBackdropPress={() => setShow(false)}
        statusBarTranslucent
        deviceHeight={deviceHeight}
        h={deviceHeight * 0.5}
        coverScreen={true}
        hasBackdrop
      >
        <Div rounded={10}>
          <CalendarPicker
            startFromMonday
            minDate={new Date(Date.now() + 86400000)}
            maxDate={new Date(2099, 12, 31)}
            weekdays={["T2", "T3", "T4", "T5", "T6", "T7", "CN"]}
            months={[...Array(12).keys()].map((e) => "Tháng " + (e + 1))}
            headerWrapperStyle={{
              height: 50,
            }}
            previousComponent={
              <Icon name="chevron-back" fontFamily="Ionicons" mx={15} color="text" fontSize="xl" />
            }
            nextComponent={
              <Icon
                name="chevron-forward"
                fontFamily="Ionicons"
                mx={15}
                color="text"
                fontSize="xl"
              />
            }
            textStyle={{
              fontFamily: "NunitoSansRegular",
              fontWeight: "500",
            }}
            onDateChange={(day) => {
              setShow(false);
              let date_selected = moment(day, "yyyy-MM-DD");
              setDateValue(date_selected);
              if (onChange) onChange(date_selected);
            }}
            customDatesStyles={[
              {
                date: moment(dateValue).clone(),
                style: {
                  backgroundColor: "#68C5C7",
                },
                textStyle: { color: "white" },
                allowDisabled: true,
              },
            ]}
          />
        </Div>
      </Modal>
    </>
  );
}
