import * as React from "react";
import { Button, Div, Icon, Text } from "react-native-magnus";
import { PhoenixConfirm, PhoenixInput } from "../../../controls";
import { Pressable } from "react-native";

export default function FormSection({ navigation }) {
  const [isSecure, setSecure] = React.useState(true);
  const [phone, setPhone] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
  const IsValisPhoneNumber = (value) => {
    try {
      if (value == null) {
        return false;
      }
      const regex = new RegExp("^[0-9]+$");
      if (!regex.test(value)) {
        return false;
      }
      const hntphcm = ["024", "028"];
      const codeDesktop = [
        "0296",
        "0261",
        "0297",
        "0203",
        "0291",
        "0215",
        "0260",
        "0233",
        "0254",
        "0251",
        "0213",
        "0299",
        "0281",
        "0277",
        "0263",
        "0212",
        "0240",
        "0269",
        "0205",
        "0276",
        "0241",
        "0219",
        "0214",
        "0227",
        "0275",
        "0226",
        "0272",
        "0208",
        "0274",
        "0228",
        "0237",
        "0256",
        "0239",
        "0238",
        "0234",
        "0271",
        "0220",
        "0229",
        "0273",
        "0252",
        "0225",
        "0259",
        "0294",
        "0290",
        "0293",
        "0210",
        "0207",
        "0206",
        "0218",
        "0257",
        "0270",
        "0292",
        "0232",
        "0211",
        "0236",
        "0221",
        "0235",
        "0216",
        "0262",
        "0258",
        "0255",
      ];
      const viettel = [
        "096",
        "097",
        "098",
        "032",
        "033",
        "034",
        "035",
        "036",
        "037",
        "038",
        "039",
        "086",
      ];
      const mobi = ["089", "090", "093", "070", "076", "077", "078", "079"];
      const vina = ["091", "094", "081", "082", "083", "084", "085", "088"];
      const vnMobile = ["092", "056", "058", "052"];
      const GMobile = ["099", "059"];
      const Itelcome = ["087"];
      //kiem tra dau so
      let top = value.substring(0, 3);
      if (hntphcm.includes(top)) {
        return value.length == 11;
      }
      top = value.substring(0, 3);
      if (viettel.includes(top)) {
        return value.length == 10;
      }
      if (mobi.includes(top)) {
        return value.length == 10;
      }
      if (vina.includes(top)) {
        return value.length == 10;
      }
      if (vnMobile.includes(top)) {
        return value.length == 10;
      }
      if (GMobile.includes(top)) {
        return value.length == 10;
      }
      if (Itelcome.includes(top)) {
        return value.length == 10;
      }
      top = value.substring(0, 4);
      if (codeDesktop.includes(top)) {
        return value.length == 11;
      }
      return false;
    } catch (error) {
      return false;
    }
  };
  async function onLogin() {
    if (phone.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Số điện thoại không được bỏ trống!",
      });
      return false;
    }
    if (!IsValisPhoneNumber(phone)) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Số điện thoại không hợp lệ!",
      });
      return false;
    }
    if (password.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Mật khẩu không được bỏ trống!",
      });
      return false;
    }
    navigation.navigate("SignupInformation", {
      phone,
      password,
    });
  }
  return (
    <Div flex={1}>
      <Text fontWeight="700" fontSize="3xl" mt={26}>
        Tạo tài khoản
      </Text>

      <PhoenixInput
        verticalLabel
        keyboard={"numeric"}
        label={"Số điện thoại"}
        placeholder="Số điện thoại"
        onChangeText={(e) => setPhone(e)}
        prefix={<Icon name="phone" fontFamily="Feather" fontSize={16} />}
      />
      <Div my={4} />
      <PhoenixInput
        verticalLabel
        label={"Mật khẩu"}
        placeholder="Mật khẩu"
        onChangeText={(e) => setPassword(e)}
        secure={isSecure}
        prefix={<Icon name="lock" fontFamily="Feather" fontSize={16} />}
        suffix={
          <Pressable onPress={() => setSecure(!isSecure)}>
            <Icon
              name={isSecure ? "eye-off" : "eye"}
              fontFamily="Feather"
              fontSize={16}
              color="black"
            />
          </Pressable>
        }
      />
      <Text fontSize="xl" my={16}>
        Tạo tài khoản nghĩa với việc bạn đã đồng ý với các{" "}
        <Text color="red500" fontSize="xl">
          Điều khoản sử dụng
        </Text>{" "}
        của chúng tôi
      </Text>
      <Div row justifyContent="center" mt={16}>
        <Button block bg="primary" shadow="md" onPress={onLogin} h={50} rounded={12}>
          Tạo tài khoản
        </Button>
      </Div>
      <Div flex={1} />
      <Text fontSize="lg" color="gray600" my={16} textAlign="center" mb={70}>
        Bạn đã có tài khoản?{" "}
        <Text fontSize="lg" color="primary" fontWeight="bold" onPress={() => navigation.goBack()}>
          Đăng nhập ngay
        </Text>
      </Text>
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
    </Div>
  );
}
