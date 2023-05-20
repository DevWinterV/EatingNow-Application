import * as React from "react";
import { Button, Div, Icon, Input, Text } from "react-native-magnus";

export default function PhoenixInputNumber(props) {
  const [value, setValue] = React.useState(undefined);
  React.useEffect(() => {
    setValue(props.value);
  }, [props.value]);
  return (
    <Div row minW={"48%"}>
      <Button
        bg="primary"
        prefix={<Icon name="minus" fontFamily="Feather" color="white" />}
        w={35}
        h={35}
        mt={22}
        onPress={() => {
          if (value != undefined) {
            setValue(Number(value) - 1);
            if (props?.onChangeText) props.onChangeText(Number(value) - 1);
          } else {
            setValue(0);
            if (props?.onChangeText) props.onChangeText(0);
          }
        }}
      />
      <Div flex={1}>
        {props.label && <Text ml={8}>{props.label}</Text>}
        <Input
          {...props}
          value={value?.toString()}
          selectTextOnFocus
          h={35}
          flex={1}
          mx={8}
          pt={0}
          pb={-10}
          textAlign="center"
          focusBorderColor="primary"
          keyboardType="numeric"
        />
      </Div>

      <Button
        bg="primary"
        prefix={<Icon name="plus" fontFamily="Feather" color="white" />}
        w={35}
        h={35}
        mt={22}
        onPress={() => {
          if (value != undefined) {
            setValue(Number(value) + 1);
            if (props?.onChangeText) props.onChangeText(Number(value) + 1);
          } else {
            setValue(0);
            if (props?.onChangeText) props.onChangeText(0);
          }
        }}
      />
    </Div>
  );
}
