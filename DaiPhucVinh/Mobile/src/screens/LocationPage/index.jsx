import * as React from "react";
import { Button, Div, Icon, Image, ScrollDiv, Tag, Text } from "react-native-magnus";
import ClearNavigation from "../layout/ClearNavigation";
import { TakeAllBranchs } from "../../api/main/branch";
import * as Location from "expo-location";
import { PhoenixConfirm } from "../../controls";
import { getDistance } from "../../utils/PositionExtension";
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
  }
  async function onViewAppearing() {
    setIsBusy(true);
    let response = await TakeAllBranchs(filter);
    if (response.success) {
      setData(response.data);
    }
    setIsBusy(false);
    await getLocation();
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
        <ClearNavigation title="Chi nhánh" />
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
              <Div key={"hsx_" + idx} row bg="white" m={16} rounded={16} flex={1} p={16}>
                <Div flex={1}>
                  <Div row flex={1}>
                    <Image source={require("../../../assets/images/logo.png")} w={64} h={64} />
                    <Div ml={8} flex={1}>
                      <Text fontWeight="bold" fontSize={"xl"}>
                        {his.Name}
                      </Text>
                    </Div>
                  </Div>
                  <Div my={4} row>
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
                  </Div>
                  <Div row>
                    <Button
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
                      py={8}
                      // onPress={() =>
                      //   Linking.openURL(`http://maps.apple.com/maps?q=${his.Lat}, ${his.Lng}`)
                      // }
                    >
                      Chỉ đường
                    </Button>
                    <Div flex={1} />
                    {his.distance > 0 && (
                      <Div row alignItems="center">
                        <Icon name="compass" fontFamily="FontAwesome5" color="primary" mx={8} />
                        <Text fontWeight="700" fontSize={"lg"}>
                          {his.distance} km
                        </Text>
                      </Div>
                    )}
                  </Div>
                </Div>
                <Icon name="chevron-right" fontFamily="Feather" color="gray500" fontSize={26} />
              </Div>
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
