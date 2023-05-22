import * as React from "react";
import { Button, Div, Icon, StatusBar, Text } from "react-native-magnus";
import { AuthContext } from "../../framework/context";
import { GetCurrentUser } from "../../api/auth";
import { AppKey, setCache } from "../../framework/cache";
import AsyncStorage from "@react-native-async-storage/async-storage";

export default function HomeNavigation() {
  // const { role } = React.useContext(AuthContext);
  // const [authUser, setAuthUser] = React.useState(null);
  const [cartItems, setCartItems] = React.useState([]);
  const role = "user";
  const authUser = {
    CustomerName: "...",
  };
  async function onViewAppearing() {
    // var response = await GetCurrentUser();
    // if (response.success) {
    //   setAuthUser(response.data);
    //   await setCache(AppKey.CURRENTUSER, response.data);
    // }
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [role]);
  React.useEffect(() => {
    const fetchCartItems = async () => {
      try {
        const storedCartItems = await AsyncStorage.getItem("cartItems");
        if (storedCartItems) {
          setCartItems(JSON.parse(storedCartItems));
        }
      } catch (error) {
        console.log("Error retrieving cart items:", error);
      }
    };
    fetchCartItems();
  }, []);
  return (
    <Div mt={12} bg="transparent" minH={40} justifyContent="space-between" row>
      <StatusBar translucent backgroundColor={"transparent"} barStyle={"dark-content"} />
      {role && (
        <Div flex={1}>
          <Text fontSize={"xl"} fontWeight="bold">
            Chào, {authUser?.CustomerName}
          </Text>
        </Div>
      )}
      <Button
        bg="transparent"
        prefix={
          <React.Fragment>
            <Icon color="orange" name="shopping-cart" fontFamily="Feather" fontSize={"5xl"} />
            {cartItems.length > 0 && (
              <Text color="orange" top={-15} fontSize={14} fontWeight="bold">
                {cartItems.length}
              </Text>
            )}
          </React.Fragment>
        }
      />
      {/* <PhoenixInput
        verticalLabel={false}
        bg={"gray200"}
        placeholder={"Tìm chi nhánh gần bạn..."}
        prefix={<Icon name="search" fontFamily="Ionicons" fontSize={"xl"} />}
      /> */}
    </Div>
  );
}
