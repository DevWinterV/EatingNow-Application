import * as React from "react";
import { Div, Icon, Image, StatusBar, Text } from "react-native-magnus";
import { PhoenixPageBody } from "../../../controls";
import { Dimensions } from "react-native";
import { ImageBackground } from "react-native";
import { TouchableOpacity } from "react-native";
import { AuthContext } from "../../../framework/context";
import { AppKey, clearAllCache, getCache } from "../../../framework/cache";
import { AppInit } from "../../../api/auth";

export default function CustomerAccountPage({ navigation }) {
  const ProfilePng = "../../../../assets/images/profile.png";
  const UserPng = "../../../../assets/images/mask.png";
  const { role, setRole } = React.useContext(AuthContext);
  const customerMenu = [
    {
      name: "Chi nhánh",
      icon: "aperture-outline",
      iconFamily: "Ionicons",
      onPress: () => {
        navigation.navigate("Location");
      },
    },
    {
      name: "Lịch sử đặt hẹn",
      icon: "calendar-outline",
      iconFamily: "Ionicons",
      onPress: () => {
        navigation.navigate("CustomerBookingHistory");
      },
    },
    {
      name: "Tra cứu kết quả",
      icon: "document-text-outline",
      iconFamily: "Ionicons",
      onPress: () => {
        navigation.navigate("CustomerTrackingService");
      },
    },
    {
      name: "Khảo sát chất lượng",
      icon: "trophy-outline",
      iconFamily: "Ionicons",
      onPress: () => {},
    },
    {
      name: "Hồ sơ cộng tác viên",
      icon: "briefcase-outline",
      iconFamily: "Ionicons",
      onPress: () => {
        navigation.navigate("PartnerAccount");
      },
    },
  ];
  const publicMenu = [
    {
      name: "Đổi mật khẩu",
      icon: "lock-closed-outline",
      iconFamily: "Ionicons",
      onPress: () => {
        navigation.navigate("CustomerChangePassword");
      },
    },
    {
      name: "Đánh giá ứng dụng",
      icon: "star-outline",
      iconFamily: "Ionicons",
      onPress: () => {},
    },
    {
      name: "Vô hiệu hoá tài khoản",
      icon: "person-circle-outline",
      iconFamily: "Ionicons",
      onPress: () => {
        navigation.navigate("DeleteAccount");
      },
    },
    {
      name: "Đăng xuất",
      icon: "log-out-outline",
      iconFamily: "Ionicons",
      onPress: async () => {
        //log-out
        await clearAllCache();
        await AppInit(false);
        setRole(undefined);
      },
    },
  ];
  const [currentUser, setCurrentUser] = React.useState(null);
  async function onViewAppearing() {
    var auth = await getCache(AppKey.CURRENTUSER);
    setCurrentUser(auth);
  }
  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);
  return (
    <Div flex={1}>
      <ImageBackground
        source={require(ProfilePng)}
        style={{
          height: 107,
          width: Dimensions.get("screen").width,
          paddingTop: 36,
        }}
      >
        <Text textAlign="center" color="white" fontSize="xl" fontWeight="bold">
          Tài khoản khách hàng
        </Text>
      </ImageBackground>

      <PhoenixPageBody bg="#d5d5d5">
        <Div row h={100} bg="white" rounded={12} p={16} mt={-64}>
          <Image
            source={
              currentUser?.AbsolutePathImage ? currentUser?.AbsolutePathImage : require(UserPng)
            }
            h={64}
            w={64}
            rounded={"circle"}
          />
          <Div ml={16}>
            <Text fontWeight="600" fontSize={"2xl"}>
              {currentUser?.CustomerName}
            </Text>
            <Text fontWeight="600" fontSize={"md"} my={4}>
              {currentUser?.CustomerPhone}
            </Text>

            <TouchableOpacity
              onPress={() => {
                navigation.navigate("CustomerProfile");
              }}
            >
              <Div row>
                <Text fontWeight="600" fontSize={"lg"}>
                  Thông tin tài khoản
                </Text>
                <Icon
                  name="chevron-right"
                  fontFamily="Feather"
                  color="#000"
                  fontSize={"xl"}
                  ml={16}
                />
              </Div>
            </TouchableOpacity>
          </Div>
        </Div>

        <Div bg="white" mt={16} rounded={12}>
          {customerMenu.map((menu, idx) => (
            <TouchableOpacity key={"menu1_" + idx} onPress={menu.onPress}>
              <Div h={48} row py={14} px={17} alignItems="center">
                <Icon
                  name={menu.icon}
                  fontFamily={menu.iconFamily}
                  fontSize={20}
                  color="iconGray"
                  w={28}
                  h={28}
                  mr={17}
                />
                <Text fontSize="lg" fontWeight="600">
                  {menu.name}
                </Text>
              </Div>
            </TouchableOpacity>
          ))}
        </Div>
        <Div bg="white" mt={16} rounded={12}>
          {publicMenu.map((menu, idx) => (
            <TouchableOpacity key={"menu2_" + idx} onPress={menu.onPress}>
              <Div h={48} row py={14} px={17} alignItems="center">
                <Icon
                  name={menu.icon}
                  fontFamily={menu.iconFamily}
                  fontSize={20}
                  color="iconGray"
                  w={28}
                  h={28}
                  mr={17}
                />
                <Text fontSize="lg" fontWeight="600">
                  {menu.name}
                </Text>
              </Div>
            </TouchableOpacity>
          ))}
        </Div>
      </PhoenixPageBody>
    </Div>
  );
}
