import * as React from "react";
import { createStackNavigator } from "@react-navigation/stack";
import { NavigationContainer } from "@react-navigation/native";
import { CurvedBottomBar } from "react-native-curved-bottom-bar";
import { Badge, Div, Icon, Text } from "react-native-magnus";
import Animated, { BounceInUp, Keyframe } from "react-native-reanimated";
import { TouchableOpacity, StyleSheet, Platform } from "react-native";
import { AuthContext } from "./framework/context";
import { LoginPage, SignupPage, SignupInformationPage } from "./screens/Auth";
import HomePage from "./screens/HomePage";
import { SiteMap } from "./screens/layout/SiteMap";
import { CustomerAccountPage, CustomerBookingPage } from "./screens/Customer";
import LocationPage from "./screens/LocationPage";

const Stack = createStackNavigator();
const iOSPlatform = Platform.OS === "ios";
const AnimatedNavigation = () => {
  const AuthNavigation = () => {
    return (
      <Stack.Navigator initialRouteName="Login">
        <Stack.Screen name="Login" component={LoginPage} options={{ headerShown: false }} />
        <Stack.Screen name="Signup" component={SignupPage} options={{ headerShown: false }} />
        <Stack.Screen
          name="SignupInformation"
          component={SignupInformationPage}
          options={{ headerShown: false }}
        />
      </Stack.Navigator>
    );
  };
  const { role } = React.useContext(AuthContext);
  const _renderIcon = (routeName, selectedTab) => {
    let icon = "";
    let title = "";
    switch (routeName) {
      case "Home":
        icon = "home-outline";
        title = "Trang chủ";
        break;
      case "Profile":
      case "Login":
        icon = "person-outline";
        title = "Cá nhân";
        break;
      case "Location":
        title = "Chi nhánh";
        icon = "map-outline";
        break;
      case "Booking":
        title = "Đặt hẹn";
        icon = "ribbon-outline";
        break;
      case "CustomerAccount":
        title = "Cá nhân";
        icon = "person-outline";
        break;
      case "Alert":
        icon = "notifications-outline";
        break;
    }
    const scaleUp = new Keyframe({
      0: {
        transform: [{ scale: 1 }],
      },
      50: {
        transform: [{ scale: 1.5 }],
      },
      100: {
        transform: [{ scale: 1 }],
      },
    }).duration(1500);
    var markupIcon =
      routeName === selectedTab ? (
        <Animated.View entering={scaleUp.duration(500)}>
          <Div
            {...(iOSPlatform && {
              pb: 16,
            })}
          >
            <Icon name={icon} fontSize="4xl" fontFamily="Ionicons" color={"primary"} />
            <Text fontSize={"sm"} color="primary">
              {title}
            </Text>
          </Div>
        </Animated.View>
      ) : (
        <Div
          {...(iOSPlatform && {
            pb: 16,
          })}
        >
          <Icon name={icon} fontSize="4xl" fontFamily="Ionicons" color={"gray800"} />
          <Text fontSize={"sm"} color="gray800">
            {title}
          </Text>
        </Div>
      );
    return markupIcon;
  };
  const renderTabBar = ({ routeName, selectedTab, navigate }) => {
    return (
      <TouchableOpacity
        onPress={() => navigate(routeName)}
        style={{
          flex: 1,
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        {_renderIcon(routeName, selectedTab)}
      </TouchableOpacity>
    );
  };
  const SwithNavigation = () => {
    switch (role) {
      case "Admin":
      case "Customer":
        return <CustomerNavigation />;
      default:
        return <HomeNavigation />;
    }
  };
  const CustomerNavigation = () => (
    <CurvedBottomBar.Navigator
      initialRouteName="Search"
      style={styles.bottomBar}
      strokeWidth={0.5}
      height={iOSPlatform ? 65 : 55}
      circleWidth={55}
      bgColor="white"
      swipeEnabled
      borderTopLeftRight
      renderCircle={({ selectedTab, navigate }) => (
        <Animated.View style={styles.btnCircle}>
          <TouchableOpacity
            style={{
              flex: 1,
              justifyContent: "center",
            }}
            onPress={() => {
              navigate("CustomerBookingHistory");
            }}
          >
            <Icon name="calendar-outline" fontFamily="Ionicons" color="#fff" fontSize={"4xl"} />
          </TouchableOpacity>
        </Animated.View>
      )}
      tabBar={renderTabBar}
    >
      <CurvedBottomBar.Screen
        name="Home"
        options={{ headerShown: false }}
        component={HomePage}
        position="LEFT"
      />
      <CurvedBottomBar.Screen
        name="Booking"
        options={{ headerShown: false }}
        component={CustomerBookingPage}
        position="LEFT"
      />
      <CurvedBottomBar.Screen
        name="Location"
        options={{ headerShown: false }}
        component={LocationPage}
        position="RIGHT"
      />
      <CurvedBottomBar.Screen
        name="CustomerAccount"
        options={{ headerShown: false }}
        component={CustomerAccountPage}
        position="RIGHT"
      />
    </CurvedBottomBar.Navigator>
  );
  const HomeNavigation = () => (
    <CurvedBottomBar.Navigator
      initialRouteName="Search"
      style={styles.bottomBar}
      strokeWidth={0.5}
      height={iOSPlatform ? 65 : 55}
      circleWidth={55}
      bgColor="white"
      swipeEnabled
      renderCircle={({ selectedTab, navigate }) => (
        <Animated.View style={styles.btnCircle}>
          <TouchableOpacity
            style={{
              flex: 1,
              justifyContent: "center",
            }}
            onPress={() => {
              //navigate.navigate("CustomerBooking");
            }}
          >
            <Icon name="calendar-outline" fontFamily="Ionicons" color="#fff" fontSize={"4xl"} />
          </TouchableOpacity>
        </Animated.View>
      )}
      tabBar={renderTabBar}
    >
      <CurvedBottomBar.Screen
        name="Home"
        options={{ headerShown: false }}
        component={HomePage}
        position="LEFT"
      />
      <CurvedBottomBar.Screen
        name="Profile"
        options={{ headerShown: false }}
        component={AuthNavigation}
        position="RIGHT"
      />
    </CurvedBottomBar.Navigator>
  );
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="HomeStack">
        <Stack.Screen name="Login" component={LoginPage} options={{ headerShown: false }} />
        <Stack.Screen name="Signup" component={SignupPage} options={{ headerShown: false }} />
        <Stack.Screen
          name="HomeStack"
          component={SwithNavigation}
          options={{ headerShown: false }}
        />
        {SiteMap.map((props) => (
          <Stack.Screen {...props} key={"page_" + props.name} options={{ headerShown: false }} />
        ))}
      </Stack.Navigator>
    </NavigationContainer>
  );
};
const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
  },
  button: {
    marginVertical: 5,
  },
  bottomBar: {},
  btnCircle: {
    width: 60,
    height: 60,
    borderRadius: 35,
    alignItems: "center",
    justifyContent: "center",
    backgroundColor: "#119ca6",
    padding: 10,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 0.5,
    },
    shadowOpacity: 0.2,
    shadowRadius: 1.41,
    elevation: 2,
    bottom: 30,
  },
});
export default AnimatedNavigation;
