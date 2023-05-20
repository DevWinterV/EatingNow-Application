import * as React from "react";
import { Dimensions } from "react-native";
import { Button, Div, Icon, Modal, ScrollDiv, Text } from "react-native-magnus";
import { PhoenixConfirm, PhoenixInput } from "../../../controls";

export default function ModalUpdateName({ isVisible, name, onClose, onConfirm }) {
  const deviceHeight = Dimensions.get("screen").height;
  const [visible, setVisible] = React.useState(isVisible);
  const [newName, setNewName] = React.useState(name);

  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });

  function onSubmit() {
    if (newName?.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Họ tên không được bỏ trống!",
      });
      return false;
    }

    onConfirm({
      name: newName,
    });
  }

  React.useEffect(() => {
    setNewName(name);
    setVisible(isVisible);
  }, [isVisible]);
  return (
    <Modal
      avoidKeyboard
      isVisible={visible}
      roundedTopLeft={25}
      roundedTopRight={25}
      h={deviceHeight * 0.3}
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
            Thay đổi họ và tên
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
              label="Họ và tên"
              value={newName}
              onChangeText={(e) => setNewName(e)}
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
