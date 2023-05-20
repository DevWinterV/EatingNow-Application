import * as React from "react";
import { Div, Icon, StatusBar, Text } from "react-native-magnus";
import { AuthContext } from "../../framework/context";
import { GetCurrentUser } from "../../api/auth";
import { AppKey, setCache } from "../../framework/cache";

export default function HomeNavigation() {
  const { role } = React.useContext(AuthContext);
  const [authUser, setAuthUser] = React.useState(null);
  async function onViewAppearing() {
    var response = await GetCurrentUser();
    if (response.success) {
      setAuthUser(response.data);
      await setCache(AppKey.CURRENTUSER, response.data);
    }
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [role]);
  return (
    <Div mt={12} bg="body">
      <StatusBar translucent backgroundColor={"transparent"} barStyle={"dark-content"} />
      {role && (
        <Div row>
          <Div flex={1}>
            <Text fontSize={"xl"} fontWeight="bold">
              Chào, {authUser?.CustomerName}
            </Text>
            <Div row my={4}>
              <Icon name="location" fontFamily="Ionicons" color="orange500" />
              <Text>{[authUser?.CustomerAddress, authUser?.CustomerDistrictName].join(", ")}</Text>
            </Div>
          </Div>
        </Div>
      )}
      {/* <PhoenixInput
        verticalLabel={false}
        bg={"gray200"}
        placeholder={"Tìm chi nhánh gần bạn..."}
        prefix={<Icon name="search" fontFamily="Ionicons" fontSize={"xl"} />}
      /> */}
    </Div>
  );
}
