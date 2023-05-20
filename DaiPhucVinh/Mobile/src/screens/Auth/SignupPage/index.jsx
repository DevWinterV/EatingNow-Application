import * as React from "react";
import FormSection from "./formSection";
import ClearNavigation from "../../layout/ClearNavigation";
import { PhoenixPageBody } from "../../../controls";

export default function SignupPage({ navigation }) {
  return (
    <PhoenixPageBody>
      <ClearNavigation />
      <FormSection navigation={navigation} />
    </PhoenixPageBody>
  );
}
