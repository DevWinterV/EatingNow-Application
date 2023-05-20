import { useNavigation } from "@react-navigation/native";
import * as React from "react";
import { Button, Div, Icon, Text } from "react-native-magnus";

export default function ClearNavigation({ title, hasBack = true, suffix }) {
  const history = useNavigation();
  return (
    <Div h={44} row mt={8} alignItems="center">
      {hasBack ? (
        <Button
          p={10}
          bg="transparent"
          onPress={() => history.goBack()}
          prefix={<Icon name="arrow-left" fontFamily="Feather" color="#393E42" fontSize={24} />}
        />
      ) : (
        <Div w={16} h={16} />
      )}

      {title && (
        <Text textAlign="center" flex={1} fontSize={"xl"} fontWeight="bold">
          {title}
        </Text>
      )}

      {suffix ? suffix : <Div w={16} h={16} />}
    </Div>
  );
}
