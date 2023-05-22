import * as React from "react";
import { Dimensions, TouchableOpacity } from "react-native";
import { Div, Image, ScrollDiv, Text } from "react-native-magnus";
import { TakeAllsPost, TakeAllsService } from "../../api/service/service";
import { useState } from "react";
import { TakeAllCuisine } from "../../api/Cuisine/cuisineService";
import { useNavigation } from "@react-navigation/native";
export default function HomeCategories() {
  const navigation = useNavigation();
  const [services, setServices] = useState(null);

  async function ControlInit() {
    let response = await TakeAllCuisine({});
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
      <Div mr={24} my={16} alignItems="center">
        <TouchableOpacity
          onPress={() => {
            navigation.navigate("StoreByCuisine", { dataCuisine: props });
          }}
        >
          <Image
            source={{ uri: props?.AbsoluteImage?.replace("localhost", "192.168.1.50") }}
            h={96}
            w={96}
          />
          <Text textAlign="center" fontWeight="500" my={8}>
            {props.Name}
          </Text>
        </TouchableOpacity>
      </Div>
    );
  };
  return (
    <Div my={8}>
      <Div row justifyContent="center" alignItems="center">
        <Text flex={1} fontWeight={700} fontSize={"2xl"}>
          Menu
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
