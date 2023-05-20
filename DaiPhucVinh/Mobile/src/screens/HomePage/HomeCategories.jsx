import * as React from "react";
import { Dimensions } from "react-native";
import { Div, Image, ScrollDiv, Text } from "react-native-magnus";
import { TakeAllsPost, TakeAllsService } from "../../api/service/service";
import { useState } from "react";
export default function HomeCategories() {
  const [services, setServices] = useState(null);

  async function ControlInit() {
    let response = await TakeAllsService({
      page: 0,
      pageSize: 100,
    });
    if (response.success) {
      setServices(response.data);
    }
  }

  async function onViewAppearing() {
    //setIsBusy(true);
    await ControlInit();
    //setIsBusy(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, []);

  const ItemTemplate = (props) => {
    return (
      <Div mr={16} my={16} alignItems="center">
        <Image
          source={{ uri: props?.ImageMainAbsolutePath?.replace("localhost", "192.168.1.28") }}
          h={64}
          w={64}
        />
        <Text textAlign="center" my={8}>
          {props.Title}
        </Text>
      </Div>
    );
  };
  return (
    <Div my={8}>
      <Div row justifyContent="center" alignItems="center">
        <Text flex={1} fontWeight="bold" fontSize={"2xl"}>
          Dịch vụ
        </Text>
        {/* <Text color="sky">Xem tất cả</Text> */}
      </Div>
      <ScrollDiv horizontal flexDir="row">
        {services?.map((ser, idx) => {
          return <ItemTemplate {...ser} key={idx} />;
        })}
      </ScrollDiv>
    </Div>
  );
}
