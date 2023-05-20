import * as React from "react";
import { Div, Image } from "react-native-magnus";

export default function LogoSection() {
  return (
    <Div pt={40} alignItems="center">
      <Image source={require("../../../../assets/images/logo.png")} w={120} h={120} />
    </Div>
  );
}
