import * as React from "react";
import { Div, Image, StatusBar, Text } from "react-native-magnus";
import Animated, { FadeInDown, FadeInUp } from "react-native-reanimated";

const SplashScreen = () => {
  return (
    <Div flex={1} justifyContent="center" alignItems="center" bg="#ffedd5">
      <StatusBar translucent backgroundColor={"transparent"} barStyle={"light-content"} />
      <Div bg="#fff7ed" flex={1} mt={160} roundedTop={200}>
        <Animated.View
          entering={FadeInUp.duration(1000)}
          style={{ paddingHorizontal: 58, paddingTop: 58 }}
        >
          <Div m={10} alignItems="center">
            <Image h={160} w={160} m={10} source={require("../assets/images/my-logo.png")} />
          </Div>
        </Animated.View>
        <Animated.View entering={FadeInDown.duration(1000)} style={{ alignItems: "center" }}>
          <Text fontSize={32} fontWeight="bold" color="#ea5b10" fontFamily="DMSerifText">
            EATTING NOW
          </Text>
          <Text fontSize={30} fontWeight="bold" color="#ea5b10" fontFamily="GreatVibes">
            Điểm xuyến ẩm thực việt
          </Text>
        </Animated.View>
      </Div>
    </Div>
  );
};
export default SplashScreen;
