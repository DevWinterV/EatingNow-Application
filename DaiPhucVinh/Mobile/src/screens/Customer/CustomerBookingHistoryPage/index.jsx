import * as React from "react";
import { Button, Div, Icon, Image, ScrollDiv, Tag, Text } from "react-native-magnus";
import { PhoenixInput } from "../../../controls";
import ClearNavigation from "../../layout/ClearNavigation";
import { GetLastBookingByCustomer, TakeAllBooking } from "../../../api/booking/bookingService";
import { TouchableOpacity } from "react-native";

export default function CustomerBookingHistoryPage({ navigation }) {
  const [data, setData] = React.useState([]);
  function DateTimeToString(dateString, timeString) {
    var dateObj = new Date(dateString);
    var day = "";
    var month = "";
    var year = "";
    var formattedDate = "";
    if (!dateString || !timeString) {
      if (!isNaN(dateObj)) {
        day = dateObj.getDate().toString().padStart(2, "0");
        month = (dateObj.getMonth() + 1).toString().padStart(2, "0");
        year = dateObj.getFullYear().toString();
        formattedDate = `${day}/${month}/${year}`;
        return `Ngày ${formattedDate}`;
      }
      return null;
    }

    const [hour, minute] = timeString.split(":");
    if (isNaN(hour) || isNaN(minute)) {
      return null;
    }
    day = dateObj.getDate().toString().padStart(2, "0");
    month = (dateObj.getMonth() + 1).toString().padStart(2, "0");
    year = dateObj.getFullYear().toString();
    formattedDate = `${day}/${month}/${year}`;
    const formattedTime = `${hour}h${minute}`;
    return `${formattedTime} ngày ${formattedDate}`;
  }

  async function getLastBookingByCustomer() {
    let response = await TakeAllBooking();
    if (response.success) {
      setData(response.data);
    }
  }

  async function onViewAppearing() {
    //setIsBusy(true);
    await getLastBookingByCustomer();
    //setIsBusy(false);
  }
  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);
  return (
    <Div bg={"#F2F4F5"} flex={1}>
      <Div p={24} bg="body">
        <ClearNavigation title="Lịch sử đặt hẹn" />
      </Div>
      <ScrollDiv>
        {data.map((his, idx) => {
          return (
            <TouchableOpacity
              key={"hsx_" + idx}
              onPress={() => {
                navigation.navigate("CustomerBookingSuccess", { data: his });
              }}
            >
              <Div row bg="white" m={16} rounded={16} flex={1} p={16}>
                <Div flex={1}>
                  <Div row>
                    <Image source={require("../../../../assets/images/logo.png")} w={64} h={64} />
                    <Text fontWeight="bold" fontSize={"xl"} flex={1} mx={8}>
                      {his.BranchName}
                    </Text>
                  </Div>
                  <Div flex={1}>
                    <Div row my={4}>
                      <Icon
                        name="map-pin"
                        fontFamily="Feather"
                        color="red500"
                        mx={8}
                        alignSelf="flex-start"
                        mt={4}
                      />
                      <Text fontWeight="700" fontSize={"lg"} flex={1}>
                        {his.BranchAddress}
                      </Text>
                    </Div>
                    <Div row>
                      <Text color="gray600" flex={1} ml={8}>
                        {DateTimeToString(his.BookingDate, his.BookingTime)}
                      </Text>
                      <Tag
                        h={20}
                        fontSize={9}
                        color="white"
                        pt={3}
                        px={6}
                        bg={his.IsClose ? "blue600" : "red500"}
                      >
                        {his.IsClose ? "Đã thực hiện" : "Lịch sắp tới"}
                      </Tag>
                    </Div>
                  </Div>
                </Div>
                <Icon name="chevron-right" fontFamily="Feather" color="gray500" fontSize={26} />
              </Div>
            </TouchableOpacity>
          );
        })}
      </ScrollDiv>
    </Div>
  );
}
