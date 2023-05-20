import * as React from "react";
import { Button, Div, Icon, Image, ScrollDiv, Text } from "react-native-magnus";
import { PhoenixInput } from "../../../controls";
import ClearNavigation from "../../layout/ClearNavigation";
import { Dimensions } from "react-native";

export default function CustomerTrackingServicePage({ navigation }) {
  const [isShowResult, setIsShowResult] = React.useState(false);
  async function onViewAppearing() {
    //setIsBusy(true);
    //setIsBusy(false);
  }
  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);
  return (
    <Div bg={"#F2F4F5"} flex={1}>
      <Div p={24} bg="body">
        <ClearNavigation title="Tra cứu kết quả xét nghiệm" />
      </Div>
      <ScrollDiv>
        <Div mt={8} bg="body" p={16} flex={1}>
          <PhoenixInput verticalLabel label={"Mã kết quả xét nghiệm"} required placeholder="" />
          <Text fontWeight="900" fontSize={28}>
            8957
          </Text>
          <PhoenixInput verticalLabel label={"Mã xác thực"} required placeholder="" />
          <Button
            block
            bg="primary"
            mt={16}
            rounded={8}
            disabled
            onPress={() => setIsShowResult(true)}
          >
            Tra cứu
          </Button>
          {!isShowResult ? (
            <Div>
              <Text fontWeight="bold" color="gold" mt={16} fontSize={"lg"}>
                Hướng dẫn tra cứu kết quả xét nghiệm
              </Text>
              <Div mt={16}>
                <Div>
                  <Text fontWeight="bold" mx={8} textAlign="justify">
                    <Icon name="search" fontFamily="Feather" color="gold" /> Bước 1{" "}
                    <Text textAlign="justify">
                      Nhập mã kết quả được gửi qua Zalo số điện thoại của khách hàng
                    </Text>
                  </Text>
                </Div>
                <Div>
                  <Text fontWeight="bold" mx={8} my={16} textAlign="justify">
                    <Icon name="search" fontFamily="Feather" color="gold" /> Bước 2{" "}
                    <Text textAlign="justify">Nhập mã xác thực hiển thị trên màn hình</Text>
                  </Text>
                </Div>
                <Div>
                  <Text fontWeight="bold" mx={8} textAlign="justify">
                    <Icon name="search" fontFamily="Feather" color="gold" /> Bước 3{" "}
                    <Text textAlign="justify">
                      Bấm tra cứu để xem kết quả (Kết quả chỉ thể hiện trong vòng 03 ngày kể từ ngày
                      có kết quả chính thức)
                    </Text>
                  </Text>
                </Div>
              </Div>
            </Div>
          ) : (
            <Div flex={1}>
              <Text fontWeight="900" fontSize={"3xl"} mt={16} textDecorLine="underline">
                Kết quả xét nghiệm:
              </Text>
              <Image
                source={require("../../../../assets/images/kqxetnghiem.png")}
                h={540}
                w={Dimensions.get("screen").width * 0.92}
                resizeMode="contain"
              />
            </Div>
          )}
        </Div>
      </ScrollDiv>
    </Div>
  );
}
