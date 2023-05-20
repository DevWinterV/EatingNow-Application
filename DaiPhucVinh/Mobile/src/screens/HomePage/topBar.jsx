import * as React from "react";
import { ImageBackground, Dimensions } from "react-native";
import { Div, Text } from "react-native-magnus";
import moment from "moment";
import { StatusBar } from "expo-status-bar";

export default function TopBar() {
  return (
    <Div row>
      <StatusBar translucent barStyle="dark-content" />
      <ImageBackground
        source={require("../../../assets/images/home.png")}
        style={{
          width: Dimensions.get("screen").width,
          height: 160,
          padding: 16,
          paddingBottom: 32,
          justifyContent: "flex-end",
        }}
      >
        <Text fontSize={"xl"} color="black">
          {moment().format("dddd, MMMM DD")}
        </Text>
        <Text fontWeight="700" fontSize={"3xl"} color="black">
          Hey, Tam Nguyen!
        </Text>
      </ImageBackground>
    </Div>
  );
}
