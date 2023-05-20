import * as React from "react";
import { Button, Div, Icon } from "react-native-magnus";
import { PhoenixConfirm, PhoenixInput } from "../../../controls";
import ClearNavigation from "../../layout/ClearNavigation";
import { Pressable } from "react-native";
import { ChangePassword } from "../../../api/auth";

export default function CustomerChangePasswordPage({ navigation }) {
  const [isBusy, setIsBusy] = React.useState(false);
  const [password, setPassword] = React.useState("");
  const [passwordNew, setPasswordNew] = React.useState("");
  const [passwordNewRetype, setPasswordNewRetype] = React.useState("");
  const [isSecure, setSecure] = React.useState(true);
  const [isSecureNew, setSecureNew] = React.useState(true);
  const [isSecureNewRetype, setSecureNewRetype] = React.useState(true);
  const [navigateBack, setNavigateBack] = React.useState(false);

  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
  async function onChangePassword() {
    //validate
    if (password.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Mật khẩu cũ không được bỏ trống!",
      });
      return false;
    }
    if (passwordNew.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Mật khẩu mới không được bỏ trống!",
      });
      return false;
    }
    if (passwordNewRetype.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Nhập lại mật khẩu mới không được bỏ trống!",
      });
      return false;
    }
    if (passwordNew != passwordNewRetype) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Nhập lại mật khẩu không đúng!",
      });
      return false;
    }
    setIsBusy(true);
    var response = await ChangePassword({
      PassWord: password,
      RepeatPassWord: passwordNew,
    });
    setIsBusy(false);
    if (response?.success) {
      setNavigateBack(true);
      setDialog({
        isVisible: true,
        title: "Success",
        message: "Đổi mật khẩu thành công!",
        type: "success",
      });
    } else {
      setDialog({
        isVisible: true,
        title: "Error",
        message: response?.message,
        type: "error",
      });
    }
  }

  return (
    <Div bg={"#F2F4F5"} flex={1}>
      <Div p={24} bg="body">
        <ClearNavigation title="Đổi mật khẩu" />
      </Div>

      <Div mt={8} bg="body" p={16}>
        <PhoenixInput
          verticalLabel
          label={"Mật khẩu cũ"}
          placeholder="Mật khẩu cũ"
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
        <PhoenixInput
          verticalLabel
          label={"Mật khẩu mới"}
          placeholder="Mật khẩu mới"
          secure={isSecureNew}
          value={passwordNew}
          onChangeText={(e) => setPasswordNew(e)}
          prefix={<Icon name="lock" fontFamily="Feather" fontSize={16} />}
          suffix={
            <Pressable onPress={() => setSecureNew(!isSecureNew)}>
              <Icon
                name={isSecureNew ? "eye-off" : "eye"}
                fontFamily="Feather"
                fontSize={16}
                color="black"
              />
            </Pressable>
          }
        />
        <PhoenixInput
          verticalLabel
          label={"Nhập lại mật khẩu mới"}
          placeholder="Nhập lại mật khẩu mới"
          secure={isSecureNewRetype}
          value={passwordNewRetype}
          onChangeText={(e) => setPasswordNewRetype(e)}
          prefix={<Icon name="lock" fontFamily="Feather" fontSize={16} />}
          suffix={
            <Pressable onPress={() => setSecureNewRetype(!isSecureNewRetype)}>
              <Icon
                name={isSecureNewRetype ? "eye-off" : "eye"}
                fontFamily="Feather"
                fontSize={16}
                color="black"
              />
            </Pressable>
          }
        />
      </Div>
      <Div flex={1} />
      <Div bg="body" py={12} px={24}>
        <Button block rounded={12} h={50} bg="primary" onPress={onChangePassword} loading={isBusy}>
          Đổi mật khẩu
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
    </Div>
  );
}
