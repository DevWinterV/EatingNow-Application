import * as React from "react";
import { Button, Div, Icon, ScrollDiv, StatusBar, Text } from "react-native-magnus";
import { ImageBackground } from "react-native";
import { Dimensions } from "react-native";
import { DateTimeToString } from "../../utils/DateTimeExtension";
import { TakePostById } from "../../api/service/service";

export default function HomePostDetailPage({ navigation, route }) {
  const { data } = route.params;
  const [isBusy, setIsBusy] = React.useState(false);
  const [post, setPost] = React.useState(null);

  async function onViewAppearing() {
    setIsBusy(true);
    var post = await TakePostById({
      Id: data.Id,
    });
    setPost(post?.data);
    setIsBusy(false);
  }
  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);
  return (
    <Div flex={1}>
      <StatusBar translucent barStyle={"light-content"} />
      <ImageBackground
        source={{ uri: data.ImageMainAbsolutePath }}
        style={{
          height: 200,
          width: Dimensions.get("window").width,
          paddingTop: 36,
          paddingHorizontal: 12,
        }}
      >
        <Div row>
          <Button
            bg="transparent"
            onPress={() => navigation.goBack()}
            prefix={<Icon name="arrow-left" fontFamily="Feather" color="white" fontSize={"5xl"} />}
          />
          <Div flex={1} />
          <Button
            bg="transparent"
            prefix={<Icon name="share-2" fontFamily="Feather" color="white" fontSize={"5xl"} />}
          />
        </Div>
      </ImageBackground>

      <Div bg="body" flex={1}>
        <Div bg="white" py={16} px={24}>
          <Text fontWeight="bold" fontSize={"3xl"} mb={12} textAlign="justify">
            {data?.Title?.trim()}
          </Text>
          <Text textAlign="right" fontStyle="italic">
            {DateTimeToString(data.CreatedAt)}
          </Text>
          <Text fontSize={"lg"} textAlign="justify">
            {post?.Description ?? "Đang cập nhật"}
          </Text>
        </Div>
        <Div flex={1} />
        <Div py={12} px={24}>
          <Button bg="primary" block onPress={() => navigation.goBack()}>
            Quay lại
          </Button>
        </Div>
      </Div>
    </Div>
  );
}
