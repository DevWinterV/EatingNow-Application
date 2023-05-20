import * as React from "react";
import { Button, Div, Icon, ScrollDiv, Text } from "react-native-magnus";
import { PhoenixConfirm, PhoenixInput } from "../../../controls";
import { CitySelect, DistrictSelect, WardSelect } from "../../../components";
import { Signup } from "../../../api/auth";

export default function FormSection({ route, navigation }) {
  const { phone, password } = route.params;
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
  const [request, setRequest] = React.useState({
    phone: phone,
    password: password,
    name: "",
    address: "",
    cityId: null,
    districtId: null,
    wardId: null,
  });
  const [isBusy, setIsBusy] = React.useState(false);
  const [backToRoot, setBackToRoot] = React.useState(false);

  async function onSignup() {
    if (request.name?.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Họ tên không được bỏ trống!",
      });
      return false;
    }
    // if (request.address?.length === 0) {
    //   setDialog({
    //     ...dialog,
    //     isVisible: true,
    //     title: "Lỗi",
    //     message: "Địa chỉ không được bỏ trống!",
    //   });
    //   return false;
    // }
    if (request.cityId == null) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Tỉnh thành không được bỏ trống!",
      });
      return false;
    }
    // if (request.districtId == null) {
    //   setDialog({
    //     ...dialog,
    //     isVisible: true,
    //     title: "Lỗi",
    //     message: "Quận huyện không được bỏ trống!",
    //   });
    //   return false;
    // }
    // if (request.wardId == null) {
    //   setDialog({
    //     ...dialog,
    //     isVisible: true,
    //     title: "Lỗi",
    //     message: "Phường Xã không được bỏ trống!",
    //   });
    //   return false;
    // }
    if (isBusy) return false;
    setIsBusy(true);
    let signupResponse = await Signup(request);
    setIsBusy(false);
    if (signupResponse.success) {
      setBackToRoot(true);
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Đăng ký thành công",
        message: "Vui lòng đăng nhập với thông tin của bạn",
      });
    } else {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: signupResponse.message,
      });
    }
  }
  return (
    <ScrollDiv contentContainerStyle={{ paddingBottom: 70 }}>
      <Text fontWeight="700" fontSize="3xl" mt={26}>
        Thông tin tài khoản
      </Text>

      <PhoenixInput
        verticalLabel
        required
        label={"Họ và tên"}
        placeholder="Nhập họ và tên"
        onChangeText={(e) => setRequest({ ...request, name: e })}
        prefix={<Icon name="user" fontFamily="Feather" fontSize={16} />}
      />

      <PhoenixInput
        verticalLabel
        required
        label={"Địa chỉ"}
        placeholder="Nhập địa chỉ cụ thể: Số nhà, đường..."
        onChangeText={(e) => setRequest({ ...request, address: e })}
        prefix={<Icon name="mail" fontFamily="Feather" fontSize={16} />}
      />
      <CitySelect
        verticalLabel
        label={"Tỉnh / Thành phố"}
        placeholder="Chọn Tỉnh / Thành phố"
        value={request.cityId}
        onSelectChange={(e) =>
          setRequest({ ...request, cityId: e, districtId: null, wardId: null })
        }
      />
      <DistrictSelect
        verticalLabel
        label={"Quận / huyện"}
        placeholder="Chọn Quận / huyện"
        value={request.districtId}
        cityRequired={request.cityId}
        onSelectChange={(e) => setRequest({ ...request, districtId: e, wardId: null })}
      />
      <WardSelect
        verticalLabel
        label={"Phường / xã"}
        placeholder="Chọn Phường / xã"
        value={request.wardId}
        districtRequired={request.districtId}
        onSelectChange={(e) => setRequest({ ...request, wardId: e })}
      />

      <Div row justifyContent="center" mt={16}>
        <Button
          block
          bg="primary"
          shadow="md"
          onPress={onSignup}
          h={50}
          rounded={12}
          loading={isBusy}
        >
          Hoàn thành
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
          if (backToRoot) {
            navigation.popToTop();
          }
        }}
      />
    </ScrollDiv>
  );
}
