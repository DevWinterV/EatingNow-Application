import * as React from "react";
import { Div, Icon, Image, ScrollDiv, Text } from "react-native-magnus";
import { useState } from "react";
import { TakeAllsPost } from "../../api/service/service";
import { TouchableOpacity } from "react-native";
import { useNavigation } from "@react-navigation/native";
import { DateTimeToString } from "../../utils/DateTimeExtension";

export default function HomeNewDe79() {
  const [posts, setPosts] = useState(null);
  const navigation = useNavigation();
  async function ControlInit() {
    let response = await TakeAllsPost({
      PostTypeId: 2,
      page: 0,
      pageSize: 100,
    });
    if (response.success) {
      setPosts(response.data);
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
      <TouchableOpacity
        onPress={() => {
          navigation.navigate("HomePostDetail", { data: props });
        }}
      >
        <Div mr={16} my={16}>
          <Image source={{ uri: props?.ImageMainAbsolutePath }} h={130} w={230} rounded={16} />
          <Text
            my={8}
            fontSize={"lg"}
            fontWeight="bold"
            textAlign="justify"
            w={230}
            ellipsizeMode="tail"
            numberOfLines={3}
          >
            {props.Title}
          </Text>
          <Div row>
            <Icon name="clock" fontFamily="Feather" color="iconGray" mr={8} />
            <Text color="iconGray">{DateTimeToString(props.CreatedAt)}</Text>
          </Div>
        </Div>
      </TouchableOpacity>
    );
  };
  return (
    <Div my={8}>
      <Div row justifyContent="center" alignItems="center">
        <Text flex={1} fontWeight="bold" fontSize={"2xl"}>
          Hoạt động chuyên gia
        </Text>
        {/* <Text color="sky">Xem tất cả</Text> */}
      </Div>
      <ScrollDiv horizontal flexDir="row">
        {posts?.map((cat, idx) => {
          return <ItemTemplate {...cat} key={"act_" + idx} />;
        })}
      </ScrollDiv>
    </Div>
  );
}
