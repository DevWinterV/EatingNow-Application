import * as React from "react";
import { Dimensions } from "react-native";
import { Button, Div, Icon, Modal, ScrollDiv, Text } from "react-native-magnus";
import { PhoenixConfirm, PhoenixInput } from "../../../controls";
import { CitySelect, DistrictSelect, WardSelect } from "../../../components";

export default function ModalUpdateAdress({
  isVisible,
  address,
  cityId,
  districtId,
  wardId,
  onClose,
  onConfirm,
}) {
  const deviceHeight = Dimensions.get("screen").height;
  const [visible, setVisible] = React.useState(isVisible);
  const [newAddress, setNewAddress] = React.useState(address);
  const [selectCity, setSelectCity] = React.useState(cityId);
  const [selectCityTmp, setSelectCityTmp] = React.useState(null);
  const [selectDistrict, setSelectDistrict] = React.useState(districtId);
  const [selectDistrictTmp, setSelectDistrictTmp] = React.useState(null);
  const [selectWard, setSelectWard] = React.useState(wardId);
  const [selectWardTmp, setSelectWardTmp] = React.useState(null);
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });

  function onSubmit() {
    if (newAddress.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Địa chỉ không được bỏ trống!",
      });
      return false;
    }
    if (!selectCity) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Tỉnh thành không được bỏ trống!",
      });
      return false;
    }
    if (!selectDistrict) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Quận huyện không được bỏ trống!",
      });
      return false;
    }
    if (!selectWard) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Phường xã không được bỏ trống!",
      });
      return false;
    }

    onConfirm({
      address: newAddress,
      cityId: selectCity,
      cityName: selectCityTmp.Name,
      districtId: selectDistrict,
      districtName: selectDistrictTmp.Name,
      wardId: selectWard,
      wardName: selectWardTmp.Name,
    });
  }

  React.useEffect(() => {
    setNewAddress(address);
    setSelectCity(cityId);
    setSelectDistrict(districtId);
    setSelectWard(wardId);
    setVisible(isVisible);
  }, [isVisible]);
  return (
    <Modal
      avoidKeyboard
      isVisible={visible}
      roundedTopLeft={25}
      roundedTopRight={25}
      h={deviceHeight * 0.5}
      onBackdropPress={onClose}
    >
      <Div bg="body" rounded={10}>
        <Div row p={8}>
          <Button
            bg="transparent"
            prefix={
              <Icon name="chevron-left" fontFamily="Feather" color="iconGray" fontSize={"3xl"} />
            }
            onPress={onClose}
          />
          <Text fontSize={"lg"} fontWeight="bold" flex={1} textAlign="center">
            Thay đổi địa chỉ
          </Text>
          <Div w={40} h={40} />
        </Div>
        <Div h={2} bg="gray200" />
        <Div px={24}>
          <ScrollDiv
            contentContainerStyle={{
              paddingBottom: 150,
            }}
          >
            <PhoenixInput
              verticalLabel
              label="Địa chỉ"
              value={newAddress}
              onChangeText={(e) => setNewAddress(e)}
            />
            <CitySelect
              verticalLabel
              label="Tỉnh / Thành phố"
              placeholder="Chọn Tỉnh / Thành phố"
              value={selectCity}
              onSelectChange={(e, city) => {
                setSelectCity(e);
                setSelectCityTmp(city);
                setSelectDistrict(null);
                setSelectWard(null);
              }}
            />
            <DistrictSelect
              verticalLabel
              label="Quận / Huyện"
              placeholder="Chọn Quận / Huyện"
              value={selectDistrict}
              cityRequired={selectCity}
              onSelectChange={(e, district) => {
                setSelectDistrict(e);
                setSelectDistrictTmp(district);
                setSelectWard(null);
              }}
            />
            <WardSelect
              verticalLabel
              label="Phường / Xã"
              placeholder="Chọn Phường / Xã"
              value={selectWard}
              districtRequired={selectDistrict}
              onSelectChange={(e, ward) => {
                setSelectWard(e);
                setSelectWardTmp(ward);
              }}
            />
            <Div my={12} mx={24}>
              <Button bg="primary" block h={50} rounded={12} fontWeight="bold" onPress={onSubmit}>
                Hoàn thành
              </Button>
            </Div>
          </ScrollDiv>
        </Div>
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
        }}
      />
    </Modal>
  );
}
