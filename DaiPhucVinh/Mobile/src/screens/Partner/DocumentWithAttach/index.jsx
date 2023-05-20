import * as React from "react";
import { Button, Div, Image, ScrollDiv, Text } from "react-native-magnus";
import { PhoenixConfirm, PhoenixDatePicker, PhoenixInput } from "../../../controls";
import ClearNavigation from "../../layout/ClearNavigation";
import { DocumentTypeSelect } from "../../../components";
import {
  CreateDocumentWithAttach,
  DeleteDocumentWithAttach,
  UpdateDocumentWithAttach,
} from "../../../api/document/documentService";
import * as ImagePicker from "expo-image-picker";
import { useState } from "react";
import { TouchableOpacity } from "react-native";

export default function CertificatePage({ navigation, route }) {
  const { data } = route.params;
  const [permission, requestPermissions] = ImagePicker.useCameraPermissions();
  const [image, setImage] = useState(data != null ? data.AbsolutePath : null);
  const [request, setRequest] = React.useState({
    DocumentTypeId: "",
    DocumentDate: new Date(),
    Name: "",
    Description: "",
    Note: "",
  });
  const [isEdit, setIsEdit] = React.useState(false);
  const [dialog, setDialog] = React.useState({
    isVisible: false,
    title: "",
    message: "",
    type: "error",
  });
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
  async function onSubmit() {
    if (request.DocumentTypeId.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Vui lòng chọn loại chứng chỉ!",
      });
      return false;
    }
    if (request.Name.length === 0) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Vui lòng nhập tên chứng chỉ!",
      });
      return false;
    }
    if (image == null) {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Vui lòng cung cấp hình ảnh chứng chỉ!",
      });
      return false;
    }
    let data = new FormData();
    data.append("form", JSON.stringify(request));
    let fileType = image?.substring(image?.lastIndexOf(".") + 1);
    data.append("image", {
      uri: image,
      name: `file.${fileType}`,
      type: `type/${fileType}`,
    });

    if (isEdit) {
      let bookingResponse = await UpdateDocumentWithAttach(data);
      if (bookingResponse.success) {
        navigation.navigate("PartnerAccount", { data: null });
      } else {
        setDialog({
          ...dialog,
          isVisible: true,
          title: "Lỗi",
          message: "Cập nhật thất bại! " + bookingResponse.message,
        });
      }
    } else {
      let bookingResponse = await CreateDocumentWithAttach(data);
      if (bookingResponse.success) {
        navigation.navigate("PartnerAccount", { data: null });
      } else {
        setDialog({
          ...dialog,
          isVisible: true,
          title: "Lỗi",
          message: "Thêm thất bại! " + bookingResponse.message,
        });
      }
    }
  }
  async function onDelete() {
    let bookingResponse = await DeleteDocumentWithAttach(data);
    if (bookingResponse.success) {
      navigation.navigate("PartnerAccount", { data: null });
    } else {
      setDialog({
        ...dialog,
        isVisible: true,
        title: "Lỗi",
        message: "Xóa thất bại! " + bookingResponse.message,
      });
    }
  }
  async function onViewAppearing() {
    //setIsBusy(true);
    if (data != null) {
      // setImage(request.AbsolutePath);
      setRequest(data);
      setIsEdit(true);
    }
    //setIsBusy(false);
  }
  React.useEffect(() => {
    onViewAppearing();
  }, []);

  React.useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      return onViewAppearing();
    });
    return unsubscribe;
  }, [navigation]);

  return (
    <Div bg={"#F2F4F5"} flex={1}>
      <Div p={24} bg="body" h={80}>
        <ClearNavigation
          title={isEdit ? "Sửa chứng chỉ / bằng cấp" : "Thêm chứng chỉ / bằng cấp"}
          hasDelete={true}
          suffix={
            isEdit ? (
              <Button
                bg="transparent"
                color="red"
                fontWeight="bold"
                onPress={() => {
                  setDialog({
                    ...dialog,
                    isVisible: true,
                    title: "Cảnh báo",
                    message: "Xác nhận xóa ? ",
                    hasCancel: true,
                    onCancel: () => {
                      setDialog({
                        ...dialog,
                        isVisible: false,
                        title: "",
                        message: "",
                      });
                    },
                  });
                }}
              >
                Xóa
              </Button>
            ) : null
          }
        />
      </Div>
      <ScrollDiv contentContainerStyle={{ paddingBottom: 120 }}>
        <Div mt={8} bg="body" p={16}>
          <DocumentTypeSelect
            verticalLabel
            label={"Loại chứng chỉ"}
            placeholder={"Chọn loại chứng chỉ"}
            value={request.DocumentTypeId}
            onSelectChange={(e) => {
              setRequest({
                ...request,
                DocumentTypeId: e,
              });
            }}
          />
          <PhoenixInput
            value={request.Name}
            onChangeText={(e) => {
              setRequest({
                ...request,
                Name: e,
              });
            }}
            verticalLabel
            label={"Tên chứng chỉ"}
            placeholder="Nhập tên chứng chỉ"
          />

          <PhoenixDatePicker
            verticalLabel
            width="100%"
            required
            label={"Ngày cấp"}
            placeholder="Chọn ngày"
            onChange={(e) => {
              setRequest({
                ...request,
                DocumentDate: e,
              });
            }}
          />
          <PhoenixInput
            verticalLabel
            label={"Mô tả"}
            placeholder="Nhập mô tả"
            value={request.Description}
            onChangeText={(e) => {
              setRequest({
                ...request,
                Description: e,
              });
            }}
          />
          <PhoenixInput
            verticalLabel
            label={"Ghi chú"}
            placeholder="Nhập ghi chú"
            value={request.Note}
            onChangeText={(e) => {
              setRequest({
                ...request,
                Note: e,
              });
            }}
          />

          <Text fontWeight="bold" fontSize="lg" mt={8}>
            Chụp hình chứng chỉ
          </Text>
          <Div mt={16} rounded={16} w={120}>
            <TouchableOpacity
              onPress={() => {
                onTakeCamera();
              }}
            >
              <Div borderWidth={1} borderStyle="dashed">
                <Image
                  source={
                    image ? { uri: image } : require("../../../../assets/images/addImage.jpg")
                  }
                  h={120}
                  w={120}
                />
              </Div>
            </TouchableOpacity>
          </Div>
        </Div>
        <Div flex={1} />
        <Div bg="body" py={12} px={24}>
          <Button block rounded={12} h={50} fontWeight="600" bg="primary" onPress={onSubmit}>
            Lưu
          </Button>
        </Div>
      </ScrollDiv>
      <PhoenixConfirm
        {...dialog}
        onConfirm={() => {
          onDelete();
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
