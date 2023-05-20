import * as React from "react";
import { Div, Icon, ScrollDiv, Text } from "react-native-magnus";
import { LinearGradient } from "expo-linear-gradient";
import { ImageBackground } from "react-native";
import { useState } from "react";
import { TakeAllsPost } from "../../api/service/service";
import { TouchableOpacity } from "react-native";
import { useNavigation } from "@react-navigation/native";
import { DateTimeToString } from "../../utils/DateTimeExtension";

export default function HomeNewFood() {
  const [posts, setPosts] = useState(null);
  const navigation = useNavigation();
  async function ControlInit() {
    let response = await TakeAllsPost({
      PostTypeId: 1,
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
      <Div mr={16} my={16}>
        <TouchableOpacity
          onPress={() => {
            navigation.navigate("HomePostDetail", { data: props });
          }}
        >
          <ImageBackground
            source={{ uri: props?.ImageMainAbsolutePath?.replace("localhost", "192.168.1.28") }}
            style={{ height: 360, width: 230 }}
            imageStyle={{ borderRadius: 16 }}
          >
            <Div row flex={1}>
              <LinearGradient
                colors={["rgba(0,0,0,0)", "rgba(0,0,0,0.3)", "rgba(0,0,0,0.7)"]}
                style={{ justifyContent: "flex-end", width: "100%", borderRadius: 16 }}
              >
                <Text
                  textAlign="justify"
                  my={8}
                  color="white"
                  fontSize={16}
                  fontWeight="700"
                  mx={8}
                >
                  {props.Title}
                </Text>
                <Div row mx={8} mb={16}>
                  <Icon name="clock" fontFamily="Feather" mx={2} color="white" />
                  <Text color="white" mx={6}>
                    {DateTimeToString(props.CreatedAt)}
                  </Text>
                </Div>
              </LinearGradient>
            </Div>
          </ImageBackground>
        </TouchableOpacity>
      </Div>
    );
  };
  return (
    <Div my={8}>
      <Div row justifyContent="center" alignItems="center">
        <Text flex={1} fontWeight="bold" fontSize={"2xl"}>
          Tin tức mới
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
