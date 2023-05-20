import * as React from "react";
import { ScrollDiv } from "react-native-magnus";
import HomeNavigation from "./HomeNavigation";
import { PhoenixPageBody } from "../../controls";
import HomeCategories from "./HomeCategories";
import HomeNewFood from "./HomeActivity";
import HomeNewDe79 from "./HomeNew";
import { AppKey, getCache } from "../../framework/cache";
import { AuthContext } from "../../framework/context";

export default function HomePage({ navigation }) {
  // await setCache(AppKey.AUTH, response);
  //       setRole(response?.Role);
  const { setRole } = React.useContext(AuthContext);
  async function onViewAppearing() {
    var auth = await getCache(AppKey.AUTH);
    var sve = await getCache(AppKey.SERVERENDPOINT);
    setRole(auth?.Role);
    console.log("ServerEndPoint", sve);
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
        <HomeNewFood />
        <HomeNewDe79 />
      </ScrollDiv>
    </PhoenixPageBody>
  );
}
