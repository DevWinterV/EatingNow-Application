import * as React from "react";
import { Button, Div, Icon, Image, ScrollDiv, Tag, Text } from "react-native-magnus";
import ClearNavigation from "../layout/ClearNavigation";
import { TakeAllBranchs } from "../../api/main/branch";
import * as Location from "expo-location";
import { PhoenixConfirm } from "../../controls";
import { getDistance } from "../../utils/PositionExtension";
import { TakeStoreByUserLogin } from "../../api/store/storeService";
import { Linking } from "react-native";
import { TouchableOpacity } from "react-native";
// import * as Linking from "expo-linking";

export default function LocationPage({ navigation }) {
  const [data, setData] = React.useState([]);
  const [isBusy, setIsBusy] = React.useState(false);
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 50,
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
    let response = await TakeStoreByUserLogin({
      latitude: currentLocation?.coords?.latitude,
      longitude: currentLocation?.coords?.longitude,
      Count: 20,
    });
    if (response.success) {
      var distanceStores = response.data
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
      setData(distanceStores);
    }
  }
  async function onViewAppearing() {
    setIsBusy(true);
    await getLocation();
    setIsBusy(false);
  }
  React.useEffect(() => {
    console.log("onview appearing");
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);
  return (
    <Div bg={"#F2F4F5"} flex={1}>
      <Div p={24} bg="body">
        <ClearNavigation title="Cửa hàng" hasBack={false} />
      </Div>
      <ScrollDiv contentContainerStyle={{ paddingBottom: 80 }}>
        {data
          .map((e) => {
            e.distance = location.latitude
              ? getDistance(location.latitude, location.longitude, e.Lat, e.Lng)
              : 0;
            return e;
          })
          .sort((a, b) => a.distance - b.distance)
          .map((his, idx) => {
            return (
              <TouchableOpacity
                key={"hsx_" + idx}
                onPress={() => {
                  navigation.navigate("HomePostDetail", { data: his });
                }}
              >
                <Div row bg="white" m={16} rounded={16} flex={1} p={8}>
                  <Div flex={1}>
                    <Div row flex={1}>
                      <Image
                        rounded={10}
                        source={{
                          uri: his?.AbsoluteImage?.replace("localhost", "192.168.1.50"),
                        }}
                        w={96}
                        h={64}
                      />
                      <Div ml={8} flex={1}>
                        <Text fontWeight="bold" fontSize={"xl"}>
                          {his.FullName}
                        </Text>
                        <Div mt={6} row pr={16}>
                          <Icon
                            name="clock"
                            fontFamily="FontAwesome5"
                            color="red500"
                            mr={8}
                            alignSelf="flex-start"
                            mt={4}
                          />
                          <Text fontSize={"xl"} fontStyle="italic">
                            {his.OpenTime}
                          </Text>
                        </Div>
                      </Div>
                    </Div>
                    <Div mt={6} row pr={16}>
                      <Icon
                        name="home"
                        fontFamily="Feather"
                        color="red500"
                        mx={8}
                        alignSelf="flex-start"
                        mt={4}
                      />
                      <Text fontWeight="700" fontSize={"lg"} flex={1}>
                        {his.FullName}
                      </Text>
                      <Div row alignItems="center">
                        <Icon name="compass" fontFamily="FontAwesome5" color="primary" mx={8} />
                        <Text fontWeight="700" fontSize={"lg"}>
                          {his.Distance} km
                        </Text>
                      </Div>
                    </Div>
                    <Div my={4} row pr={16}>
                      <Icon
                        name="map-pin"
                        fontFamily="Feather"
                        color="red500"
                        mx={8}
                        alignSelf="flex-start"
                        mt={4}
                      />
                      <Text fontWeight="700" fontSize={"lg"} flex={1}>
                        {his.Address}
                      </Text>
                      <Button
                        mb={6}
                        bg="transparent"
                        prefix={
                          <Icon
                            name="directions"
                            fontFamily="FontAwesome5"
                            color="blue500"
                            mr={8}
                            fontSize={"lg"}
                          />
                        }
                        color="blue500"
                        fontWeight="bold"
                        p={0}
                        onPress={() => {
                          const mapUrl = `https://www.google.com/maps/search/?api=1&query=${his.Latitude},${his.Longitude}`;
                          Linking.openURL(mapUrl).catch((error) =>
                            console.error("An error occurred", error)
                          );
                        }}
                      >
                        Chỉ đường
                      </Button>
                    </Div>
                  </Div>
                </Div>
              </TouchableOpacity>
            );
          })}
      </ScrollDiv>
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
    </Div>
  );
}
