import _ from "lodash";
import * as React from "react";
import { Button, Div, Icon, Text, Select, Image } from "react-native-magnus";

export default function PhoenixSelect({
  verticalLabel = true,
  label,
  placeholder,
  data,
  onSelectChange,
  value,
}) {
  const [selectValue, setSelectedValue] = React.useState(null);
  const selectRef = React.createRef();
  React.useEffect(() => {
    setSelectedValue(data ? data.find((e) => e.Id == value) : value);
  }, [value, data]);
  return (
    <Div mt={8}>
      {verticalLabel && (
        <Text mb={4} fontSize={"lg"}>
          {label}
        </Text>
      )}
      <Button
        block
        borderWidth={1}
        rounded={5}
        h={50}
        bg="white"
        color="gray900"
        borderColor="gray400"
        onPress={() => {
          if (selectRef.current) {
            selectRef.current.open();
          }
        }}
        suffix={<Icon name="chevron-down" fontFamily="Feather" fontSize={20} color="iconGray" />}
      >
        <Text flex={1} fontSize={"lg"} color={selectValue ? "black" : "gray500"}>
          {selectValue ? selectValue.Name : placeholder}
        </Text>
      </Button>
      <Select
        onSelect={(e) => {
          var item = data.find((aa) => aa.Id == e);
          onSelectChange(e, item);
        }}
        ref={selectRef}
        value={selectValue}
        title={
          <Text mx="xl" color="gray500" pb="md" fontSize={"lg"}>
            {label}
          </Text>
        }
        mt="md"
        roundedTop="xl"
        data={data}
        {...(data == null && {
          footer: (
            <Div justifyContent="center" alignItems="center">
              <Image source={require("../../assets/images/empty.png")} w={80} h={80} />
              <Text mt={16} color="gray500" fontSize={"lg"}>
                Không có dữ liệu...
              </Text>
            </Div>
          ),
        })}
        renderItem={(item, index) => (
          <Select.Option value={item.Id} py="md" px="xl">
            <Text>{item.Name}</Text>
          </Select.Option>
        )}
      />
    </Div>
  );
}
