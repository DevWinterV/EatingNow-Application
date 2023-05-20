import * as React from "react";
import { Button, Div, Icon, Text } from "react-native-magnus";
import { PhoenixConfirm, PhoenixInput } from "../../../controls";
import { Pressable } from "react-native";
import { AuthContext } from "../../../framework/context";
import FacebookSVG from "../../../../assets/svg/facebook.svg";
import GoogleSVG from "../../../../assets/svg/google.svg";
import { Login } from "../../../api/auth";
import { AppKey, setCache } from "../../../framework/cache";
import { LoginInFront } from "../../../api/authThen/authService";

export default function FormSection({ navigation }) {
  const [username, setUsername] = React.useState("0383311707");
  const [password, setPassword] = React.useState("123456");
  const [isSecure, setSecure] = React.useState(true);
  const [isBusy, setIsBusy] = React.useState(false);
  const { setRole } = React.useContext(AuthContext);
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
  async function onLogin() {
    if (username.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Tên đăng nhập không được bỏ trống!",
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
    setIsBusy(true);
    var response = await LoginInFront({
      username,
      password,
    });
    setIsBusy(false);
    if (!response) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Không kết nối được đến máy chủ! Vui lòng thử lại...",
      });
      return false;
    } else if (!response.IsError) {
      await setCache(AppKey.AUTH, response);
      setRole(response?.Role);
    } else {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: response.ErrorDescription,
      });
      return false;
    }
  }
  return (
    <Div flex={1}>
      <Text fontWeight="700" fontSize="3xl" mt={60}>
        Đăng nhập
      </Text>

      <PhoenixInput
        verticalLabel
        label={"Số điện thoại"}
        placeholder="Số điện thoại"
        value={username}
        onChangeText={(e) => setUsername(e)}
        prefix={<Icon name="phone" fontFamily="Feather" fontSize={16} />}
      />
      <Div my={4} />
      <PhoenixInput
        verticalLabel
        label={"Mật khẩu"}
        placeholder="Mật khẩu"
        secure={isSecure}
        value={password}
        onChangeText={(e) => setPassword(e)}
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
      <Text fontSize="md" color="red500" textAlign="right">
        Quên mật khẩu?
      </Text>

      <Div row justifyContent="center" mt={32}>
        <Button
          block
          bg="primary"
          shadow="md"
          onPress={onLogin}
          h={50}
          rounded={12}
          loading={isBusy}
        >
          Đăng nhập
        </Button>
      </Div>
      {/* <Div row mt={32} alignItems="center">
        <Div h={1} bg="gray400" flex={1} />
        <Text mx={16} fontSize={"xl"}>
          hoặc
        </Text>
        <Div h={1} bg="gray400" flex={1} />
      </Div>
      <Div row justifyContent="center" mt={16}>
        <Button block bg="secondary" shadow="md" onPress={onLogin} h={50} rounded={12}>
          <FacebookSVG />
          <Text fontSize={"lg"} fontWeight="bold" mx={32}>
            Đăng nhập bằng Facebook
          </Text>
        </Button>
      </Div>
      <Div row justifyContent="center" mt={16}>
        <Button block bg="secondary" shadow="md" onPress={onLogin} h={50} rounded={12}>
          <GoogleSVG />
          <Text fontSize={"lg"} fontWeight="bold" mx={40}>
            Đăng nhập bằng Google
          </Text>
        </Button>
      </Div> */}
      <Div flex={1} />
      <Text fontSize="lg" color="gray600" my={16} textAlign="center" mb={70}>
        Bạn chưa có tài khoản?{" "}
        <Text
          fontSize="lg"
          color="primary"
          fontWeight="bold"
          onPress={() => navigation.navigate("Signup")}
        >
          Tạo tài khoản
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
