import * as React from "react";
import { Div, Icon, ScrollDiv, Text } from "react-native-magnus";
import { LinearGradient } from "expo-linear-gradient";
import { ImageBackground } from "react-native";
import { useState } from "react";
import { TouchableOpacity } from "react-native";
import { useNavigation } from "@react-navigation/native";
import { DateTimeToString } from "../../utils/DateTimeExtension";
import { TakeFoodListByHint } from "../../api/foodlist/foodListService";

export default function HomeMonAnDeXuat() {
  const [posts, setPosts] = useState(null);
  const navigation = useNavigation();
  async function ControlInit() {
    let response = await TakeFoodListByHint();
    if (response.success) {
      setPosts(response.data);
    }
  }
  function convertToCurrency(value) {
    return value.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
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
      <Div mr={16} my={16}>
        <TouchableOpacity
          onPress={() => {
            navigation.navigate("HomePostDetail", { data: props });
          }}
        >
          <ImageBackground
            source={{
              uri: props?.UploadImage?.replace("localhost:3000", "192.168.1.50:3000"),
            }}
            style={{ height: 160, width: 160 }}
            imageStyle={{ borderRadius: 16 }}
          ></ImageBackground>
          <Div row flex={1} justifyContent="space-between">
            <Text textAlign="justify" my={8} fontSize={16} fontWeight="700" mx={8}>
              {props.FoodName}
            </Text>
            <Icon color="orange" name="shopping-cart" fontFamily="Feather" fontSize={"2xl"} />
          </Div>
          <Div row flex={1}>
            <Text textAlign="justify" fontSize={14} fontWeight="300" mx={8}>
              {convertToCurrency(props.Price)} đ
            </Text>
          </Div>
        </TouchableOpacity>
      </Div>
    );
  };
  return (
    <Div my={8}>
      <Div row justifyContent="center" alignItems="center">
        <Text flex={1} fontWeight="bold" fontSize={"2xl"}>
          Món ăn đề xuất
        </Text>
        {/* <Text color="sky">Xem tất cả</Text> */}
      </Div>
      <ScrollDiv horizontal flexDir="row">
        {posts?.map((cat, idx) => {
          return <ItemTemplate {...cat} key={idx} />;
        })}
      </ScrollDiv>
    </Div>
  );
}
