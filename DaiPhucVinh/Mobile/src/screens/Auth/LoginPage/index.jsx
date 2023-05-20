import * as React from "react";
import FormSection from "./formSection";
import { PhoenixPageBody } from "../../../controls";

export default function LoginPage({ navigation }) {
  return (
    <PhoenixPageBody>
      <FormSection navigation={navigation} />
    </PhoenixPageBody>
  );
}
