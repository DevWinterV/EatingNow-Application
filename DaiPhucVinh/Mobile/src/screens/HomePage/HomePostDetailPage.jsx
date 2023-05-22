import * as React from "react";
import {
  Badge,
  Button,
  Div,
  Icon,
  Image,
  ScrollDiv,
  Snackbar,
  StatusBar,
  Tag,
  Text,
} from "react-native-magnus";
import { ImageBackground } from "react-native";
import { Dimensions } from "react-native";
import { DateTimeToString } from "../../utils/DateTimeExtension";
import { TakePostById } from "../../api/service/service";
import { PostAllFoodListByStoreId, TakeFoodListByStoreId } from "../../api/store/storeService";
import { TouchableOpacity } from "react-native-gesture-handler";
import AsyncStorage from "@react-native-async-storage/async-storage";

export default function HomePostDetailPage({ navigation, route }) {
  const { data } = route.params;
  const [isBusy, setIsBusy] = React.useState(false);
  const [post, setPost] = React.useState(null);
  const [cartItems, setCartItems] = React.useState([]);
  const snackbarRef = React.createRef();

  const handleAddToCart = async (item) => {
    if (cartItems?.includes(item)) {
      if (snackbarRef.current) {
        snackbarRef.current.show("Thêm vào giỏ hàng thành công!", {
          duration: 2000,
          suffix: <Icon name="checkcircle" color="white" fontSize="md" fontFamily="AntDesign" />,
        });
      }
      return;
    }

    const updatedCartItems = [...cartItems, item];
    setCartItems(updatedCartItems);
    try {
      await AsyncStorage.setItem("cartItems", JSON.stringifsy(updatedCartItems));
      if (snackbarRef.current) {
        snackbarRef.current.show("Thêm vào giỏ hàng thành công!", {
          duration: 2000,
          suffix: <Icon name="checkcircle" color="white" fontSize="md" fontFamily="AntDesign" />,
        });
      }
    } catch (error) {
      if (snackbarRef.current) {
        snackbarRef.current.show("Thêm vào giỏ hàng thành công!", {
          duration: 2000,
          suffix: <Icon name="checkcircle" color="white" fontSize="md" fontFamily="AntDesign" />,
        });
      }
    }
  };

  async function onViewAppearing() {
    setIsBusy(true);
    var post = await PostAllFoodListByStoreId({
      Id: data?.UserId,
    });
    setPost(post?.data);
    setIsBusy(false);
  }
  function convertToCurrency(value) {
    return value.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
  }
  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);

  React.useEffect(() => {
    const fetchCartItems = async () => {
      try {
        const storedCartItems = await AsyncStorage.getItem("cartItems");
        if (storedCartItems) {
          setCartItems(JSON.parse(storedCartItems));
        }
      } catch (error) {
        console.log("Error retrieving cart items:", error);
      }
    };

    fetchCartItems();
  }, []);
  return (
    <Div flex={1}>
      <StatusBar translucent barStyle={"light-content"} />
      <ImageBackground
        source={{ uri: data.AbsoluteImage?.replace("localhost:3000", "192.168.61.195") }}
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
            prefix={
              <React.Fragment>
                <Icon color="orange" name="shopping-cart" fontFamily="Feather" fontSize={"5xl"} />
                {cartItems.length > 0 && (
                  <Text color="orange" top={-15} fontSize={14} fontWeight="bold">
                    {cartItems.length}
                  </Text>
                )}
              </React.Fragment>
            }
          />
        </Div>
      </ImageBackground>
      <ScrollDiv>
        <Div bg="body" flex={1}>
          {post?.map((his, idx) => {
            return (
              <Div key={"hsx_" + idx} row bg="white" m={16} rounded={16} flex={1} p={12}>
                <Div flex={1}>
                  <Div row>
                    <Image
                      source={{
                        uri: his?.UploadImage?.replace("localhost:3000", "192.168.61.195:3000"),
                      }}
                      w={96}
                      h={64}
                    />
                    <Div flex={1} ml={6}>
                      <Text fontWeight="bold" fontSize={"xl"} flex={1} mx={8}>
                        {his.FoodName}
                      </Text>
                      <Text color="gray500" fontWeight="bold" fontSize={"xl"} flex={1} mx={8}>
                        {convertToCurrency(his.Price)}đ
                      </Text>
                      <Text
                        color="orange"
                        fontWeight="md"
                        fontStyle="italic"
                        fontSize={"xl"}
                        flex={1}
                        mx={8}
                      >
                        {his.Category}
                      </Text>
                    </Div>
                  </Div>
                </Div>
                <Button
                  onPress={() => {
                    handleAddToCart(his.FoodListId);
                  }}
                  mt="lg"
                  px="xl"
                  py="lg"
                  bg="transparent"
                  color="white"
                  suffix={
                    <Icon name="cart-plus" fontFamily="FontAwesome5" color="orange" fontSize={26} />
                  }
                ></Button>
              </Div>
            );
          })}
          <Div flex={1} />
        </Div>
      </ScrollDiv>
      <Snackbar mb={70} ref={snackbarRef} bg="green600" color="white"></Snackbar>

      <Div py={12} px={24}>
        <Button bg="#ea5b10" block onPress={() => navigation.goBack()}>
          Quay lại
        </Button>
      </Div>
    </Div>
  );
}
