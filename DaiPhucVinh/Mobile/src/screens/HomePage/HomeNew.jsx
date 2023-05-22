import * as React from "react";
import { Button, Div, Icon, Image, ScrollDiv, Text } from "react-native-magnus";
import { useState } from "react";
import { TouchableOpacity, Dimensions, Linking } from "react-native";
import { useNavigation } from "@react-navigation/native";
import { DateTimeToString } from "../../utils/DateTimeExtension";
import { TakeStoreByUserLogin } from "../../api/store/storeService";
import { getDistance } from "../../utils/PositionExtension";
import * as Location from "expo-location";

export default function HomeNewDe79() {
  const [posts, setPosts] = useState(null);
  const [stores, setStores] = React.useState([]);
  const screenWidth = Dimensions.get("window").width - 48;
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
  const [location, setLocation] = React.useState({
    longitude: 0,
    latitude: 0,
  });
  const navigation = useNavigation();

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
      setStores(distanceStores);
    }
  }
  async function onViewAppearing() {
    //setIsBusy(true);
    await getLocation();
    //setIsBusy(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, []);
  const ItemTemplate = (cat) => {
    return (
      <TouchableOpacity
        onPress={() => {
          navigation.navigate("HomePostDetail", { data: cat });
        }}
      >
        <Div row bg="white" mt={16} rounded={16} style={{ width: screenWidth }} flex={1}>
          <Div flex={1}>
            <Div row flex={1}>
              <Image
                rounded={16}
                source={{
                  uri: cat?.AbsoluteImage?.replace("localhost", "192.168.1.50"),
                }}
                style={{ height: 160, width: screenWidth }}
                imageStyle={{ borderRadius: 16 }}
              />
              <Div ml={8} flex={1}>
                <Text fontWeight="bold" fontSize={"xl"}>
                  {cat.Name}
                </Text>
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
                {cat.FullName}
              </Text>
              <Div row alignItems="center">
                <Icon name="compass" fontFamily="FontAwesome5" color="primary" mx={8} />
                <Text fontWeight="700" fontSize={"lg"}>
                  {cat.Distance} km
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
                {cat.Address}
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
                  const mapUrl = `https://www.google.com/maps/search/?api=1&query=${cat.Latitude},${cat.Longitude}`;
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
  };
  return (
    <Div my={8}>
      <Div row justifyContent="center" alignItems="center">
        <Text flex={1} fontWeight="bold" fontSize={"2xl"}>
          Cửa hàng gần bạn
        </Text>
        {/* <Text color="sky">Xem tất cả</Text> */}
      </Div>
      <ScrollDiv horizontal="false" flexDir="row">
        {stores?.map((cat, idx) => {
          return <ItemTemplate {...cat} key={"act_" + idx} />;
        })}
      </ScrollDiv>
    </Div>
  );
}
