import * as React from "react";
import ReactNativeModal from "react-native-modal";
import LottieView from "lottie-react-native";
import { Dimensions } from "react-native";
import { Button, Div, Text } from "react-native-magnus";

export default function PhoenixConfirm({
  isVisible,
  title,
  message,
  type,
  onConfirm,
  hasCancel,
  onCancel,
  confirmText = "OK",
  confirmDisable = false,
  calcelText = "Huỷ",
}) {
  const deviceHeight = Dimensions.get("screen").height;
  const [visible, setVisible] = React.useState(isVisible);
  React.useEffect(() => {
    setVisible(isVisible);
  }, [isVisible]);
  function Require(rule) {
    if (!rule) return;
    switch (rule) {
      case "success":
        return require("../../assets/lotties/success.json");
      case "warn":
        return require("../../assets/lotties/warn.json");
      default:
        return require("../../assets/lotties/error.json");
    }
  }
  return (
    <ReactNativeModal isVisible={visible} statusBarTranslucent deviceHeight={deviceHeight}>
      <Div bg="body" alignItems="center" rounded={10} pb="md">
        <LottieView
          source={Require(type)}
          autoPlay
          style={{
            width: 80,
            height: 80,
            alignSelf: "center",
          }}
        />
        <Text color="text" fontWeight="bold" fontSize="2xl" mt={-10}>
          {title}
        </Text>
        <Text color="text" fontSize="lg" my={8} px={8} textAlign="center">
          {message}
        </Text>
        <Div row>
          <Button
            w={"30%"}
            m={5}
            mt="lg"
            rounded={10}
            bg="green500"
            onPress={() => {
              setVisible(false);
              if (onConfirm) onConfirm({ isVisible: false });
            }}
            disabled={confirmDisable}
          >
            {confirmText}
          </Button>
          {hasCancel && (
            <Button
              w={"30%"}
              m={5}
              mt="lg"
              rounded={10}
              bg="gray500"
              onPress={() => {
                setVisible(false);
                if (onCancel) onCancel();
              }}
            >
              {calcelText}
            </Button>
          )}
        </Div>
      </Div>
    </ReactNativeModal>
  );
}
