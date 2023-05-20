import * as React from "react";
import LogoSection from "./logoSection";
import ClearNavigation from "../../layout/ClearNavigation";
import { PhoenixConfirm, PhoenixPageBody } from "../../../controls";
import { Button, Div, ScrollDiv, Text } from "react-native-magnus";
import { clearAllCache } from "../../../framework/cache";
import { AuthContext } from "../../../framework/context";
import { AppInit, DeleteAccount } from "../../../api/auth";

export default function DeleteAccountPage({ navigation }) {
  const { setRole } = React.useContext(AuthContext);
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
  function onDeleteAccount() {
    setDialog({
      isVisible: true,
      title: "Vô hiệu hoá tài khoản",
      message: "Bạn vẫn muốn vô hiệu hoá tài khoản chứ?",
      type: "error",
    });
  }

  return (
    <PhoenixPageBody>
      <ClearNavigation title={"Vô hiệu hoá tài khoản"} />
      <LogoSection navigation={navigation} />
      <ScrollDiv mt={8}>
        <Text fontWeight="bold" fontSize="xl" textAlign="center">
          Vô hiệu hoá tài khoản
        </Text>
        <Text fontWeight="700" fontSize="lg" textAlign="justify" mt={8}>
          Bạn có thể vô hiệu hoá tài khoản HomeCare247 thay vì xoá nó đi.
        </Text>
        <Text fontWeight="700" fontSize="lg" textAlign="justify" mt={8}>
          Dữ liệu của bạn sẽ bị ẩn đi thay vì bị xoá khỏi hệ thống! Gồm có:
        </Text>
        <Text fontWeight="700" fontSize="lg" textAlign="justify" mt={8}>
          Số điện thoại, Họ tên, địa chỉ, hình ảnh, thông tin đặt hẹn, hồ sơ đối tác
        </Text>
        <Text fontWeight="bold" fontSize="xl" mt={24}>
          Bảo mật dữ liệu cho bạn
        </Text>
        <Text fontWeight="700" fontSize="lg" textAlign="justify" mt={8}>
          Tất cả các dữ liệu của bạn bao gồm:
        </Text>
        <Text fontWeight="bold" fontSize="lg" textAlign="justify" mt={8}>
          • Số điện thoại, Họ tên, địa chỉ, hình ảnh
        </Text>
        <Text fontWeight="bold" fontSize="lg" textAlign="justify" mt={8}>
          • Thông tin, lịch sử đặt hẹn
        </Text>
        <Text fontWeight="bold" fontSize="lg" textAlign="justify" mt={8}>
          • Hồ sơ thông tin đối tác, văn bằng, chứng chỉ liên quan
        </Text>

        <Text fontWeight="700" fontSize="lg" textAlign="justify" mt={8}>
          Tất cả các dữ liệu trên đều được chúng tôi bảo mật, mã hoá và được sử dụng nhằm mục đích
          tối ưu hoá trải nghiệm dịch vụ cho bạn khi sử dụng các dịch vụ tại các chi nhánh của Hệ
          thống Y Tế Tại Gia 247.
        </Text>

        <Text fontWeight="700" fontSize="lg" textAlign="justify" mt={8}>
          Khi bạn nhấn vào nút bên dưới, hồ sơ, thông tin của bạn ngay lập tức sẽ bị ẩn đi cho đến
          khi bạn đăng nhập và kích hoạt lại tài khoản của mình!
        </Text>
      </ScrollDiv>
      <Div justifyContent="center">
        <Button block bg="primary" rounded={8} onPress={onDeleteAccount}>
          Vô hiệu hoá tài khoản
        </Button>
      </Div>
      <PhoenixConfirm
        {...dialog}
        hasCancel
        onConfirm={async () => {
          setDialog({
            ...dialog,
            isVisible: false,
            title: "",
            message: "",
          });
          var deleteResponse = await DeleteAccount();
          if (deleteResponse.success) {
            await clearAllCache();
            setRole(undefined);
            await AppInit(false);
            navigation.goBack();
          }
        }}
        onCancel={() => {
          setDialog({
            ...dialog,
            isVisible: false,
            title: "",
            message: "",
          });
        }}
      />
    </PhoenixPageBody>
  );
}
