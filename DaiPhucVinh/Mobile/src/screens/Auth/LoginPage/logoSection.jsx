import * as React from "react";
import { Div, Image, Text } from "react-native-magnus";

export default function LogoSection() {
  return (
    <Div alignSelf="center">
      <Div pt={60} alignItems="center">
        <Image
          source={require("../../../../assets/images/logo.png")}
          w={200}
          h={200}
          rounded={100}
        />
        <Text fontWeight="700" fontSize="4xl" color="primary" mt={16}>
          HỆ THỐNG NHÀ HÀNG 79
        </Text>
      </Div>
    </Div>
  );
}
