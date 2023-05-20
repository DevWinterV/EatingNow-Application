import * as React from "react";
import { Button, Div, Icon, Text } from "react-native-magnus";
import { PhoenixConfirm, PhoenixInput } from "../../../controls";
import EducationLevelSelect from "../../../components/EducationLevelSelect";
import EducationPlaceSelect from "../../../components/EducationPlaceSelect";

export default function CertificatePage({
  navigation,
  verticalLabel,
  EducationPlaceId,
  onSelectChange_EducationPlace,
  EducationLevelId,
  onSelectChange_EducationLevel,
  attachList,
}) {
  function formatString(item) {
    const date = new Date(item.DocumentDate);
    const month = date.toLocaleString("vi", { month: "long" });
    const year = date.getFullYear();

    return `${item.Name} -  ${month}/${year}`;
  }

  return (
    <Div>
      <Div h={2} bg="#d3d3d3" />
      <Div row mt={8}>
        <Button flex={1} fontSize={14} roundedTopRight={0} roundedBottomRight={0}>
          Bằng cấp chuyên môn
        </Button>
        <Button flex={1} fontSize={14} roundedTopLeft={0} roundedBottomLeft={0} bg="gray500">
          Quá trình làm việc
        </Button>
      </Div>
      <Div>
        <EducationLevelSelect
          verticalLabel
          label={"Trình độ chuyên môn"}
          placeholder="Chọn trình độ chuyên môn"
          value={EducationLevelId}
          onSelectChange={onSelectChange_EducationLevel}
        />
        <EducationPlaceSelect
          verticalLabel
          label={"Trường đào tạo"}
          placeholder="Trường đào tạo"
          value={EducationPlaceId}
          onSelectChange={onSelectChange_EducationPlace}
        />
        <Div>
          <Div row mt={8} justifyContent="space-between">
            <Text fontWeight="bold" fontSize={"lg"} mt={4}>
              Chứng chỉ/Bằng cấp khác (file đính kèm)
            </Text>
            <Button
              h={30}
              w={30}
              p={0}
              bg="gold"
              onPress={() => {
                navigation.navigate("DocumentWithAttach", { data: null });
              }}
              prefix={
                <Icon name="add-outline" fontFamily="Ionicons" color="#fff" fontSize={"xl"} />
              }
            />
          </Div>
          <Div>
            {attachList?.map((his, idx) => {
              return (
                <Div key={idx}>
                  <Div row my={8}>
                    <Text flex={1}>{formatString(his)}</Text>
                    <Button
                      h={30}
                      w={30}
                      p={0}
                      bg="transparent"
                      onPress={() => {
                        navigation.navigate("DocumentWithAttach", { data: his });
                      }}
                      prefix={
                        <Icon
                          name="pencil-outline"
                          fontFamily="Ionicons"
                          color="gold"
                          fontSize={"5xl"}
                          mx={8}
                        />
                      }
                    />
                  </Div>
                  <Div bg="gray400" h={1} />
                </Div>
              );
            })}
          </Div>

          {/* <Text fontWeight="bold" fontSize={"lg"} mt={16}>
            Chứng chỉ/Bằng cấp đào tạo
          </Text>
          <Div bg="gray400" p={8} rounded={8}>
            <Text fontWeight="bold">
              Tham gia tập huấn nghiệp vụ xét nghiệm Covid do Trường ĐH Y Dược Cần Thơ tổ chức ngày
              25/02/2022
            </Text>
          </Div> */}
        </Div>
      </Div>
    </Div>
  );
}
