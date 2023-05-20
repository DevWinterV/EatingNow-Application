import * as React from "react";
import { Button, Div, Icon, Image, ScrollDiv, StatusBar, Text } from "react-native-magnus";
import { PhoenixConfirm, PhoenixInput, PhoenixPageBody } from "../../../controls";
import { Dimensions } from "react-native";
import { ImageBackground } from "react-native";
import { TouchableOpacity } from "react-native";
import { useNavigation } from "@react-navigation/native";
import CertificatePage from "./CertificatePage";
import { useState } from "react";
import { GetCurrentDocument, UpdateOrCreateDocument } from "../../../api/document/documentService";
import { GetCurrentUser } from "../../../api/auth";
import { CitySelect, DistrictSelect } from "../../../components";
import { AppKey, getCache } from "../../../framework/cache";

export default function PartnerAccountPage() {
  const ProfilePng = "../../../../assets/images/profile.png";
  const UserPng = "../../../../assets/images/mask.png";
  const navigation = useNavigation();
  const [currentUser, setCurrentUser] = useState();
  const [document, setDocument] = useState({
    Code: "",
    FullName: "",
    Gender: "",
    DOB: "",
    Email: "",
    CellPhone: "",
    Address: "",
    WardId: "",
    DisctrictId: "",
    CityId: "",
    CityName: "",
    FullAddress: "",
    ImageId: "",
    Images: "",
    Lat: "",
    Lng: "",
    DocumentStatusId: "",
    DocumentStatusName: "",
    EducationLevelId: "",
    EducationLevelName: "",
    EducationPlaceId: "",
    EducationPlaceName: "",
    CertificateId: "",
    CertificateName: "",
    WorkingPlace: "",
    WorkingHistory: "",
    EducationHistory: "",
  });
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "success",
  });
  async function GetUserDocument() {
    let response = await GetCurrentDocument();
    if (response.success) {
      setDocument(response.data);
    }
  }
  async function onViewAppearing() {
    //setIsBusy(true);
    await GetUserDocument();
    var auth = await getCache(AppKey.CURRENTUSER);
    setCurrentUser(auth);
    //setIsBusy(false);
  }
  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);

  async function onSubmit() {
    let response = await UpdateOrCreateDocument(document);
    if (response.success) {
      setDocument(response.data);
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Thành công",
        message: "Cập nhật thành công! ",
      });
    }
  }
  return (
    <Div flex={1}>
      <PhoenixConfirm
        {...dialog}
        onConfirm={() => {
          setDialog({
            ...dialog,
            isVisible: false,
            title: "",
            message: "",
          });
        }}
      />
      <ImageBackground
        source={require(ProfilePng)}
        style={{
          height: 107,
          width: Dimensions.get("screen").width,
          paddingTop: 36,
        }}
      >
        <Text textAlign="center" color="white" fontSize="xl" fontWeight="bold">
          Hồ sơ cộng tác viên
        </Text>
      </ImageBackground>

      <PhoenixPageBody>
        <Div
          row
          h={100}
          bg="white"
          rounded={12}
          p={16}
          mt={-64}
          borderColor="#d5d5d5"
          borderWidth={1}
        >
          <Image source={require(UserPng)} h={64} w={64} />
          <Div ml={16}>
            <Text fontWeight="600" fontSize={"2xl"}>
              {currentUser?.CustomerName}
            </Text>
            <Text fontWeight="600" fontSize={"md"} my={4}>
              {currentUser?.CustomerPhone}
            </Text>

            <TouchableOpacity
              onPress={() => {
                navigation.goBack();
              }}
            >
              <Div row>
                <Text fontWeight="600" fontSize={"lg"}>
                  Thông tin tài khoản
                </Text>
                <Icon
                  name="chevron-right"
                  fontFamily="Feather"
                  color="#000"
                  fontSize={"xl"}
                  ml={16}
                />
              </Div>
            </TouchableOpacity>
          </Div>
        </Div>
        <ScrollDiv>
          <Div pb={8}>
            <PhoenixInput
              verticalLabel
              label={"Email liên hệ"}
              placeholder="Nhập email liên hệ..."
              value={document?.Email}
              onChangeText={(e) => {
                setDocument({
                  ...document,
                  Email: e,
                });
              }}
            />
            <PhoenixInput
              verticalLabel
              label={"Địa chỉ liên hệ"}
              placeholder="Nhập địa chỉ liên hệ..."
              value={document?.Address}
              onChangeText={(e) => {
                setDocument({
                  ...document,
                  Address: e,
                });
              }}
            />
            <CitySelect
              verticalLabel
              label={"Tỉnh / Thành phố"}
              placeholder="Chọn Tỉnh / Thành phố"
              value={document?.CityId}
              onSelectChange={(e) =>
                setDocument({ ...document, CityId: e, DisctrictId: null, WardId: null })
              }
            />
            <DistrictSelect
              verticalLabel
              label={"Quận / huyện"}
              placeholder="Chọn Quận / huyện"
              value={document?.DisctrictId}
              cityRequired={document?.CityId}
              onSelectChange={(e) => setDocument({ ...document, DisctrictId: e, WardId: null })}
            />
          </Div>

          <CertificatePage
            attachList={document?.DocumentWitAttachs}
            navigation={navigation}
            EducationPlaceId={document?.EducationPlaceId}
            EducationLevelId={document?.EducationLevelId}
            onSelectChange_EducationLevel={(e) => {
              setDocument({
                ...document,
                EducationLevelId: e,
              });
            }}
            onSelectChange_EducationPlace={(e) => {
              setDocument({
                ...document,
                EducationPlaceId: e,
              });
            }}
          />

          <Div h={2} bg="#d3d3d3" my={8} />
          <Div row>
            {/* <Button flex={1} fontSize={14} roundedTopRight={0} roundedBottomRight={0}>
              Chi nhánh cộng tác
            </Button>
            <Button flex={1} fontSize={14} roundedTopLeft={0} roundedBottomLeft={0} bg="gray500">
              Dịch vụ cộng tác
            </Button>
          </Div>
          <Div my={16}>
            <Div>
              <Div row>
                <Text fontWeight="bold" fontSize="lg" flex={1}>
                  Y Tế Tại Gia 247 - Cao Lãnh
                </Text>
                <Icon
                  name="radio-button-on-outline"
                  fontFamily="Ionicons"
                  color="red600"
                  fontSize={"2xl"}
                />
              </Div>
              <Div row my={4} mb={8}>
                <Icon name="analytics-outline" fontFamily="Ionicons" color="gray600" mr={8} />
                <Text fontWeight="bold" color="gray600">
                  Cách 4.5km
                </Text>
              </Div>
              <Text>Số 140, Đường 30-4, Phường 1, TP. Cao Lãnh, Tỉnh Đồng Tháp</Text>
            </Div>
            <Div h={1} bg="gray400" my={16} />
            <Div>
              <Div row>
                <Text fontWeight="bold" fontSize="lg" flex={1}>
                  Y Tế Tại Gia 247 - Long Xuyên
                </Text>
                <Icon
                  name="radio-button-on-outline"
                  fontFamily="Ionicons"
                  color="blue500"
                  fontSize={"2xl"}
                />
              </Div>
              <Div row my={4} mb={8}>
                <Icon name="analytics-outline" fontFamily="Ionicons" color="gray600" mr={8} />
                <Text fontWeight="bold" color="gray600">
                  Cách 30km
                </Text>
              </Div>
              <Text>182, Khóm Đông Thịnh 8, Phường Mỹ Phước, TP. Long Xuyên, Tỉnh An Giang</Text>
            </Div>
            <Div h={1} bg="gray400" my={16} />
            <Div>
              <Text fontWeight="bold" fontSize={"lg"} mb={8}>
                Các quận/ huyện có thể thực hiện dịch vụ:
              </Text>
              <Div bg="gray400" p={8} rounded={8}>
                <Text fontWeight="bold">
                  Sa Đéc, Tam Nông, Tràm Chim, Lai Vung, Vàm Cống, Thốt Nốt
                </Text>
              </Div>
            </Div>
            <Div h={1} bg="gray400" my={16} />
            <Div row>
              <Text mr={8}>Hình ảnh đính kèm</Text>
              <Div row flex={1} justifyContent="space-between">
                <Div borderWidth={1} borderStyle="dashed">
                  <Image source={require("../../../../assets/images/addImage.jpg")} h={40} w={40} />
                </Div>
                <Div borderWidth={1} borderStyle="dashed">
                  <Image source={require("../../../../assets/images/addImage.jpg")} h={40} w={40} />
                </Div>
                <Div borderWidth={1} borderStyle="dashed">
                  <Image source={require("../../../../assets/images/addImage.jpg")} h={40} w={40} />
                </Div>
              </Div>
            </Div> */}
            <Button
              block
              rounded={12}
              h={50}
              bg="primary"
              mt={16}
              fontSize={16}
              fontWeight="600"
              onPress={() => {
                onSubmit();
              }}
            >
              Cập nhật thông tin
            </Button>
          </Div>
        </ScrollDiv>
      </PhoenixPageBody>
    </Div>
  );
}
