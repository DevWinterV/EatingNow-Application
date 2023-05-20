import * as React from "react";
import FormSection from "./formSection";
import { PhoenixPageBody } from "../../../controls";
import ClearNavigation from "../../layout/ClearNavigation";

export default function SignupInformationPage({ route, navigation }) {
  return (
    <PhoenixPageBody>
      <ClearNavigation />
      <FormSection navigation={navigation} route={route} />
    </PhoenixPageBody>
  );
}
