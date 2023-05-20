import * as React from "react";
import { Button, Div, Icon, Image, ScrollDiv, StatusBar, Text } from "react-native-magnus";
import { PhoenixInput, PhoenixPageBody } from "../../../controls";
import MapView, { Marker, PROVIDER_GOOGLE } from "react-native-maps";
import * as Location from "expo-location";
import QRCode from "react-native-qrcode-svg";
import { GetLastBookingByCustomer } from "../../../api/booking/bookingService";

export default function CustomerBookingSuccessPage({ navigation, route }) {
  const { data } = route.params;
  const [request, setRequest] = React.useState({
    BranchId: 1,
    Symptom: "",
    Description: "",
    AncientRoot: "",
    ServiceRequirement: "",
    BookingDate: "",
    BookingTime: "09:00",
    ContactAddress: "",
    ContactPhone: "",
    ContactName: "",
    BokingNote: "",
    Gender: "",
    YOB: "",
    Images: "",
    WardId: "",
    DistrictId: 4308,
    CityId: 472,
    Lat: "",
    Lng: "",
    DistanceEstimate: "",
    CreatedAt: "",
    CreatedBy: "",
    UpdatedAt: "",
    UpdatedBy: "",
    ApprovedBy: "",
    ApprovedDate: "",
    NoteWhenApproved: "",
    NotificationDate: "",
    EmployeeExecuted: "",
    DateExecuted: "",
    NoteWhenExecuted: "",
    IsClosed: "",
    Deleted: "",
  });
  const [location, setLocation] = React.useState({
    longitude: 0,
    latitude: 0,
  });
  async function getLocation() {
    let { status } = await Location.requestForegroundPermissionsAsync();
    if (status !== "granted") {
      //   setAlertProps({
      //     type: "error",
      //     title: "Error",
      //     message: "Permission to access location was denied",
      //     isVisible: true,
      //   });
      return;
    }
    let currentLocation = await Location.getCurrentPositionAsync({
      enableHighAccuracy: true,
      accuracy: Location.Accuracy.High,
    });
    setLocation(currentLocation?.coords);
  }
  function DateTimeToString(dateString, timeString) {
    const dateObj = new Date(dateString);
    const day = dateObj.getDate().toString().padStart(2, "0");
    const month = (dateObj.getMonth() + 1).toString().padStart(2, "0");
    const year = dateObj.getFullYear().toString();
    var timeParts = timeString.split(":");
    const formattedDate = `${day}/${month}/${year}`;
    const formattedTime = `${timeParts[0]}h${timeParts[1]}`;
    return `${formattedTime} ngày ${formattedDate}`;
  }

  async function getLastBookingByCustomer() {
    let response = await GetLastBookingByCustomer();
    if (response.success) {
      setRequest(response.data);
    }
  }
  async function onViewAppearing() {
    //setIsBusy(true);
    if (data != null) setRequest(data);
    else await getLastBookingByCustomer();
    await getLocation();
    //setIsBusy(false);
  }
  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);
  return (
    <PhoenixPageBody bg={"#D49A01"}>
      <StatusBar translucent barStyle={"light-content"} />
      <ScrollDiv>
        <Div alignItems="center" p={24}>
          <Image source={require("../../../../assets/images/successIcon.png")} h={64} w={64} />
          <Text fontWeight="900" color="white" fontSize={"3xl"} my={16}>
            Đặt hẹn thành công!
          </Text>
          <Text textAlign="center" color="white">
            Vui lòng đưa mã QR dưới đây cho nhân viên khi tới thực hiện dịch vụ bạn nhé!
          </Text>
        </Div>
        <Div bg="body" rounded={16} p={16}>
          <Div mt={26}>
            <Div row alignItems="center" justifyContent="space-between">
              <Div w={28} h={28} bg="#D49A01" rounded={14} ml={-28} />
              <QRCode value={request.Code} />
              <Div w={28} h={28} bg="#D49A01" rounded={14} mr={-28} />
            </Div>
            <Text my={8} fontSize={"lg"} textAlign="center">
              Mã đặt hẹn:
            </Text>
            <Text
              fontSize={24}
              fontWeight="800"
              color="primary"
              letterSpacing={1}
              textAlign="center"
            >
              {request.Code}
            </Text>
          </Div>
          <Div h={1} my={16} bg="gray400" />
          <Div rounded={16}>
            <Text textTransform="uppercase" fontWeight="bold" fontSize={"lg"}>
              Thông tin đặt hẹn
            </Text>
            <Div row mt={8} my={4}>
              <Icon name="call-outline" fontFamily="Ionicons" color="black" mr={8} />
              <Text fontWeight="700">{request.ContactPhone}</Text>
            </Div>
            <Div row my={4}>
              <Icon name="person-outline" fontFamily="Ionicons" color="black" mr={8} />
              <Text fontWeight="700">{request.ContactName}</Text>
            </Div>
            <Div row my={4}>
              <Icon name="time-outline" fontFamily="Ionicons" color="black" mr={8} />
              <Text fontWeight="700">
                {DateTimeToString(request.BookingDate, request.BookingTime)}
              </Text>
            </Div>
            <Div row my={4} alignItems="flex-start">
              <Icon name="pin-outline" fontFamily="Ionicons" color="black" mr={8} mt={4} />
              <Div>
                <Text fontSize={"lg"} fontWeight="500" mx={2}>
                  {request.BranchName}
                </Text>
                <Text fontWeight="700" my={4}>
                  {request.BranchAddress}
                </Text>
              </Div>
            </Div>
            <Div row my={4} alignItems="flex-start">
              <Icon name="ribbon-outline" fontFamily="Ionicons" color="black" mr={8} mt={4} />
              <Div>
                <Text fontSize={"lg"} fontWeight="500">
                  Dịch vụ đăng ký
                </Text>
                <Text fontWeight="700" my={4}>
                  {request.ServiceRequirement}
                </Text>
              </Div>
            </Div>
            <Div row my={4} mb={8}>
              <Icon name="analytics-outline" fontFamily="Ionicons" color="black" mr={8} />
              <Text fontWeight="bold">Cách 4.5km</Text>
            </Div>
            <MapView
              style={{
                height: 250,
                borderRadius: 16,
              }}
              region={{
                latitude: location?.latitude || 0,
                longitude: location?.longitude || 0,
                latitudeDelta: 0.01,
                longitudeDelta: 0.01,
              }}
              scrollEnabled={false}
              provider={PROVIDER_GOOGLE}
            >
              <Marker
                coordinate={{
                  latitude: location?.latitude,
                  longitude: location?.longitude,
                }}
                pinColor="red"
                title={"Vị trí của bạn"}
              />
            </MapView>
          </Div>
          <Button
            block
            rounded={12}
            h={50}
            bg="primary"
            mt={8}
            onPress={() => {
              navigation.popToTop();
            }}
          >
            Quay về trang chủ
          </Button>
        </Div>
      </ScrollDiv>
    </PhoenixPageBody>
  );
}
