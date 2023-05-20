import * as React from "react";
import { Button, Div, Icon, ScrollDiv, Text } from "react-native-magnus";
import {
  PhoenixConfirm,
  PhoenixDatePicker,
  PhoenixInput,
  PhoenixSelect,
  PhoenixTimePicker,
} from "../../../controls";
import ClearNavigation from "../../layout/ClearNavigation";
import MapView, { Marker, PROVIDER_GOOGLE } from "react-native-maps";
import * as Location from "expo-location";
import { DistrictSelect, WardSelect } from "../../../components";
import { CreateBooking } from "../../../api/booking/bookingService";
import { TakeAllBranchs } from "../../../api/main/branch";
import { getDistance } from "../../../utils/PositionExtension";
import { AppKey, getCache } from "../../../framework/cache";

export default function CustomerBookingPage({ navigation }) {
  const [request, setRequest] = React.useState({
    BranchId: 1,
    Symptom: "",
    Description: "",
    AncientRoot: "",
    ServiceRequirement: "",
    BookingDate: Date.now() + 86400000,
    BookingTime: "09:00",
    ContactAddress: "",
    ContactPhone: "",
    ContactName: "",
    BokingNote: "",
    Gender: "",
    YOB: "",
    Images: "",
    WardId: null,
    DistrictId: null,
    CityId: null,
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
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
  const [branchs, setBranchs] = React.useState([]);
  const [selectedBranch, setSelectedBranch] = React.useState(null);
  async function getLocation() {
    let { status } = await Location.requestForegroundPermissionsAsync();
    if (status !== "granted") {
      setDialog({
        type: "error",
        title: "Error",
        message: "Yêu cầu truy cập vị trí bị từ chối!",
        isVisible: true,
      });
      return;
    }
    let currentLocation = await Location.getCurrentPositionAsync({
      enableHighAccuracy: true,
      accuracy: Location.Accuracy.High,
    });
    setLocation(currentLocation?.coords);
    let response = await TakeAllBranchs({
      page: 0,
      pageSize: 50,
    });
    if (response.success) {
      var distanceBranchs = response.data
        .map((e) => {
          e.distance = currentLocation?.coords.latitude
            ? getDistance(
                currentLocation?.coords.latitude,
                currentLocation?.coords.longitude,
                e.Lat,
                e.Lng
              )
            : 0;
          return e;
        })
        .sort((a, b) => a.distance - b.distance);
      setBranchs(distanceBranchs);
      if (distanceBranchs.length > 0) setSelectedBranch(distanceBranchs[0].Id);
    }
  }
  async function onBooking() {
    let bookingResponse = await CreateBooking(request);
    if (bookingResponse.success) {
      navigation.navigate("CustomerBookingSuccess", { data: null });
    } else {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Đặt lịch thất bại! " + bookingResponse.message,
      });
    }
  }
  async function onViewAppearing() {
    //setIsBusy(true);
    var currentUser = await getCache(AppKey.CURRENTUSER);
    setRequest({
      ...request,
      ContactName: currentUser.CustomerName,
      ContactPhone: currentUser.CustomerPhone,
      ContactAddress: currentUser.CustomerAddress,
      CityId: currentUser.CustomerCityId,
      DistrictId: currentUser.CustomerDistrictId,
      WardId: currentUser.CustomerWardId,
    });
    await getLocation();
    //setIsBusy(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, []);
  // React.useEffect(() => {
  //   const unsubscribe = navigation.addListener("focus", () => {
  //     return onViewAppearing();
  //   });
  //   return unsubscribe;
  // }, [navigation]);
  React.useEffect(() => {
    setRequest({
      ...request,
      Lng: location.longitude,
      Lat: location.latitude,
    });
  }, [location]);
  return (
    <Div bg={"#F2F4F5"} flex={1}>
      <PhoenixConfirm
        {...dialog}
        onConfirm={() => {
          setDialog({
            ...dialog,
            isVisible: false,
            title: "",
            message: "",
          });
        }}
      />
      <Div p={24} bg="body" h={80}>
        <ClearNavigation title="Đặt hẹn" hasBack={false} />
      </Div>
      <ScrollDiv contentContainerStyle={{ paddingBottom: 120 }}>
        <Div mt={8} bg="body" p={16}>
          <PhoenixSelect
            verticalLabel
            label={"Chọn chi nhánh (đang chọn chi nhánh 247 gần bạn)"}
            data={branchs}
            value={selectedBranch}
            onSelectChange={(e) => {
              setSelectedBranch(e);
              setRequest({
                ...request,
                BranchId: e,
              });
            }}
          />
          <PhoenixInput
            value={request.Symptom}
            onChangeText={(e) => {
              setRequest({
                ...request,
                Symptom: e,
              });
            }}
            verticalLabel
            label={"Triệu chứng/ Tình trạng (Mô tả vấn đề của bạn)"}
            placeholder="Nhập triệu chứng, tình trạng của bạn"
          />
          <PhoenixInput
            verticalLabel
            label={"Dịch vụ muốn thực hiện (Theo đề xuất của bạn)"}
            placeholder="Khám bệnh tại gia, Lấy máu tại nhà,..."
            value={request.ServiceRequirement}
            onChangeText={(e) => {
              setRequest({
                ...request,
                ServiceRequirement: e,
              });
            }}
          />
          <Div row>
            <PhoenixDatePicker
              selectedDate={request.BookingDate}
              isIcon={true}
              verticalLabel
              required
              flex={1}
              label={"Ngày hẹn"}
              placeholder="Chọn ngày"
              onChange={(e) => {
                setRequest({
                  ...request,
                  BookingDate: e,
                });
              }}
            />
            <Div mx={10} />
            <PhoenixTimePicker
              label={"Giờ hẹn"}
              value={request.BookingTime}
              onChange={(e) => {
                setRequest({
                  ...request,
                  BookingTime: e,
                });
              }}
            />
          </Div>
          <Div mt={8}>
            <Text color="red500" fontSize="md" fontWeight="bold">
              (Chúng tôi sẽ gọi điện lại xác nhận trước giờ hẹn với bạn)
            </Text>
          </Div>
          <PhoenixInput
            verticalLabel
            label={"Người liên hệ"}
            placeholder="Người liên hệ"
            value={request.ContactName}
            onChangeText={(e) => {
              setRequest({
                ...request,
                ContactName: e,
              });
            }}
          />
          <PhoenixInput
            verticalLabel
            label={"Số điện thoại"}
            placeholder="Nhập số điện thoại"
            value={request.ContactPhone}
            onChangeText={(e) => {
              setRequest({
                ...request,
                ContactPhone: e,
              });
            }}
          />
          <Text fontWeight="bold" fontSize="lg" mt={8}>
            Địa chỉ thực hiện (Nhập chi tiết bạn nhé)
          </Text>
          <DistrictSelect
            verticalLabel
            label="Quận / Huyện"
            placeholder="Chọn Quận / Huyện"
            value={request.DistrictId}
            cityRequired={request.CityId}
            onSelectChange={(e) => {
              setRequest({ ...request, DistrictId: e });
            }}
          />
          <WardSelect
            verticalLabel
            label={"Phường / xã"}
            placeholder="Chọn Phường / xã"
            districtRequired={request.DistrictId}
            value={request.WardId}
            onSelectChange={(e) => setRequest({ ...request, WardId: e })}
          />
          <PhoenixInput
            verticalLabel
            label={"Địa chỉ"}
            placeholder="Nhập địa chỉ cụ thể: Số nhà, đường,..."
            value={request.ContactAddress}
            onChangeText={(e) => {
              setRequest({
                ...request,
                ContactAddress: e,
              });
            }}
          />

          <Text fontWeight="bold" fontSize="lg" mt={8}>
            Bản đồ (Vị trí hiện tại của bạn)
          </Text>
          <Div mt={16} rounded={16}>
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
        </Div>
        <Div flex={1} />
        <Div bg="body" py={12} px={24}>
          <Button block rounded={12} h={50} bg="primary" onPress={onBooking}>
            Đặt hẹn
          </Button>
        </Div>
      </ScrollDiv>
    </Div>
  );
}
