import * as React from "react";
import { Div, Image, Text } from "react-native-magnus";

export default function LogoSection() {
  return (
    <Div alignSelf="center">
      <Div pt={80} alignItems="center">
        <Image source={require("../../../../assets/images/logo.png")} w={120} h={120} />
        <Text fontWeight="700" fontSize="4xl">
          Medical Reminder
        </Text>
      </Div>
    </Div>
  );
}
