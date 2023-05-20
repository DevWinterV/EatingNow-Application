import * as React from "react";
import { Button, Div, Icon, Image, Text } from "react-native-magnus";
import ClearNavigation from "../../layout/ClearNavigation";
import { AppKey, getCache, setCache } from "../../../framework/cache";
import ModalUpdateAdress from "./ModalUpdateAdress";
import ModalUpdateName from "./ModalUpdateName";
import { UpdateUserProfile } from "../../../api/auth";
import { PhoenixConfirm } from "../../../controls";
import { TouchableOpacity } from "react-native";
import * as ImagePicker from "expo-image-picker";

export default function CustomerProfilePage({ navigation }) {
  const [permission, requestPermissions] = ImagePicker.useCameraPermissions();

  const [currentUser, setCurrentUser] = React.useState(null);
  const [isEditName, setIsEditName] = React.useState(false);
  const [isEditAddress, setIsEditAddress] = React.useState(false);
  const [isBusy, setIsBusy] = React.useState(false);
  const [navigateBack, setNavigateBack] = React.useState(false);
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
  const [image, setImage] = React.useState(null);
  async function onSubmit() {
    if (isBusy) return false;
    setIsBusy(true);
    let data = new FormData();
    data.append(
      "form",
      JSON.stringify({
        DisplayName: currentUser.CustomerName,
        Address: currentUser.CustomerAddress,
        CityId: currentUser.CustomerCityId,
        DistrictId: currentUser.CustomerDistrictId,
        WardId: currentUser.CustomerWardId,
      })
    );
    let fileType = image?.substring(image?.lastIndexOf(".") + 1);
    data.append("image", {
      uri: image,
      name: `file.${fileType}`,
      type: `type.${fileType}`,
    });

    var response = await UpdateUserProfile(data);
    setIsBusy(false);

    if (response?.success) {
      setNavigateBack(true);
      setDialog({
        isVisible: true,
        title: "Success",
        message: "Cập nhật thông tin thành công!",
        type: "success",
      });
      await setCache(AppKey.CURRENTUSER, currentUser);
    } else {
      setDialog({
        isVisible: true,
        title: "Error",
        message: response.message,
        type: "error",
      });
    }
  }
  async function onTakeCamera() {
    if (!permission.granted) {
      requestPermissions();
    } else {
      try {
        let result = await ImagePicker.launchCameraAsync({
          mediaTypes: ImagePicker.MediaTypeOptions.Images,
          allowsEditing: false,
          quality: 1,
        });
        if (!result?.canceled) {
          setImage(result.assets[0].uri);
        }
      } catch (err) {
        console.log(err);
      }
    }
  }
  async function onViewAppearing() {
    var auth = await getCache(AppKey.CURRENTUSER);
    setCurrentUser(auth);
    setImage(auth?.AbsolutePathImage);
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
        <ClearNavigation title="Thông tin cá nhân" />
      </Div>

      <Div h={175} alignItems="center" py={26} mt={8} bg="body">
        <TouchableOpacity onPress={onTakeCamera}>
          {image ? (
            <Image source={{ uri: image }} h={120} w={120} bg="gray200" rounded={"circle"} />
          ) : (
            <Icon
              name="camera"
              fontFamily="Feather"
              h={120}
              w={120}
              fontSize={48}
              bg="gray500"
              color="white"
              rounded={"circle"}
            />
          )}
        </TouchableOpacity>
      </Div>

      <Div mt={8} bg="body" p={16}>
        <Div row>
          <Icon
            name="people-outline"
            fontFamily="Ionicons"
            w={24}
            h={24}
            color="primary"
            fontSize="3xl"
          />
          <Text flex={1} fontSize={15} fontWeight="600" ml={8}>
            {currentUser?.CustomerName}
          </Text>
          <Button
            color="sky"
            fontSize={15}
            bg="transparent"
            p={0}
            onPress={() => setIsEditName(true)}
          >
            Thay đổi
          </Button>
        </Div>
        <Div h={1} bg="gray300" my={16} />
        <Div row>
          <Icon
            name="call-outline"
            fontFamily="Ionicons"
            w={24}
            h={24}
            color="primary"
            fontSize="3xl"
          />
          <Text flex={1} fontSize={15} fontWeight="600" ml={8}>
            {currentUser?.CustomerPhone}
          </Text>
        </Div>
        <Div h={1} bg="gray300" my={16} />
        <Div row>
          <Icon
            name="map-outline"
            fontFamily="Ionicons"
            w={24}
            h={24}
            color="primary"
            fontSize="3xl"
          />
          <Text flex={1} fontSize={15} fontWeight="600" mx={8}>
            {[
              currentUser?.CustomerAddress,
              currentUser?.CustomerWardName,
              currentUser?.CustomerDistrictName,
              currentUser?.CustomerCityName,
            ].join(", ")}
          </Text>
          <Button
            color="sky"
            fontSize={15}
            bg="transparent"
            p={0}
            onPress={() => setIsEditAddress(true)}
          >
            Thay đổi
          </Button>
        </Div>
      </Div>
      <Div flex={1} />
      <Div bg="body" py={12} px={24}>
        <Button block rounded={12} h={50} bg="primary" onPress={onSubmit} loading={isBusy}>
          Lưu
        </Button>
      </Div>
      <PhoenixConfirm
        {...dialog}
        onConfirm={() => {
          setDialog({
            ...dialog,
            isVisible: false,
            title: "",
            message: "",
          });
          if (navigateBack) {
            navigation.goBack();
          }
        }}
      />
      <ModalUpdateName
        isVisible={isEditName}
        name={currentUser?.CustomerName}
        onClose={() => setIsEditName(false)}
        onConfirm={(e) => {
          setCurrentUser({
            ...currentUser,
            CustomerName: e.name,
          });
          setIsEditName(false);
        }}
      />

      <ModalUpdateAdress
        isVisible={isEditAddress}
        address={currentUser?.CustomerAddress}
        cityId={currentUser?.CustomerCityId}
        districtId={currentUser?.CustomerDistrictId}
        wardId={currentUser?.CustomerWardId}
        onClose={() => setIsEditAddress(false)}
        onConfirm={(e) => {
          setCurrentUser({
            ...currentUser,
            CustomerAddress: e.address,
            CustomerCityId: e.cityId,
            CustomerCityName: e.cityName,
            CustomerDistrictId: e.districtId,
            CustomerDistrictName: e.districtName,
            CustomerWardId: e.wardId,
            CustomerWardName: e.wardName,
          });
          setIsEditAddress(false);
        }}
      />
    </Div>
  );
}
