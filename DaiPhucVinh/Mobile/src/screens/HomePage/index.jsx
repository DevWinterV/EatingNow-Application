import * as React from "react";
import { ScrollDiv } from "react-native-magnus";
import HomeNavigation from "./HomeNavigation";
import { PhoenixPageBody } from "../../controls";
import HomeCategories from "./HomeCategories";
import HomeNewDe79 from "./HomeNew";
import { AppKey, getCache } from "../../framework/cache";
import { AuthContext } from "../../framework/context";
import HomeMonAnDeXuat from "./HomeMonAnDeXuat";
import HomeMonAnMoi from "./HomeMonAnMoi";

export default function HomePage({ navigation }) {
  // await setCache(AppKey.AUTH, response);
  const [cartItems, setCartItems] = React.useState([]);
  //       setRole(response?.Role);
  const { setRole } = React.useContext(AuthContext);
  async function onViewAppearing() {
    var auth = await getCache(AppKey.AUTH);
    var sve = await getCache(AppKey.SERVERENDPOINT);
    setRole(auth?.Role);
  }
  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);
  return (
    <PhoenixPageBody>
      <HomeNavigation />
      <ScrollDiv flex={1} contentContainerStyle={{ paddingBottom: 75 }}>
        <HomeCategories />
        <HomeMonAnDeXuat />
        <HomeMonAnMoi />
        <HomeNewDe79 />
      </ScrollDiv>
    </PhoenixPageBody>
  );
}
