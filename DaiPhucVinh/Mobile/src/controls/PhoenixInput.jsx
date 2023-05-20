import * as React from "react";
import { Div, Input, Text } from "react-native-magnus";

export default function PhoenixInput({
  verticalLabel = true,
  required = false,
  prefix,
  suffix,
  secure,
  label,
  placeholder,
  value,
  onBlur,
  onChangeText,
  keyboard,
  h,
  p,
  bg,
  flex,
}) {
  return (
    <Div my={8} flex={flex}>
      {verticalLabel && (
        <Div row>
          <Text mb={4} fontSize={"lg"}>
            {label}{" "}
          </Text>
          {required && <Text color="red">*</Text>}
        </Div>
      )}
      <Input
        placeholder={placeholder}
        prefix={prefix}
        suffix={suffix}
        onBlur={onBlur}
        keyboardType={keyboard}
        secureTextEntry={secure}
        value={value}
        onChangeText={onChangeText}
        focusBorderColor="primary"
        fontSize={"xl"}
        h={h ? h : 48}
        p={p}
        bg={bg}
        rounded={8}
      />
    </Div>
  );
}
