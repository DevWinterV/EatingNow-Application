import * as React from "react";
import { DrawerContentScrollView, DrawerItem } from "@react-navigation/drawer";
import { ImageBackground } from "react-native";
import { Text, Icon, Div } from "react-native-magnus";

export default function DrawerNavigation({ props, navigation }) {
  const background = "./../assets/pattern1.png";

  const MenuItem = ({ title, icon, family, navigate }) => {
    return (
      <DrawerItem
        label={({ focused, color }) => (
          <Text focused={focused} color={color} ml={-15} fontSize="2xl">
            {title}
          </Text>
        )}
        style={{ backgroundColor: "#fff", color: "#fff", marginRight: 0 }}
        icon={() => <Icon name={icon} fontFamily={family} fontSize="2xl" />}
        onPress={() => navigation.navigate(navigate)}
      />
    );
  };

  return (
    <Div flex={1}>
      <DrawerContentScrollView {...props} style={{ marginTop: -35 }}>
        <ImageBackground
          source={require(background)}
          style={{
            width: 320,
            height: 180,
            flex: 1,
            justifyContent: "flex-end",
          }}
        />
        <Div flex={1} style={{ flexGrow: 1 }}>
          <MenuItem
            title="Đổi mật khẩu"
            icon="lock"
            fontFamily="Feather"
            navigate="ChangePassword"
          />
        </Div>
      </DrawerContentScrollView>
      <Div h={1} bg="#3d3d3d" opacity={0.15} />
      <DrawerItem
        label={({ color }) => (
          <Text color={color} style={{ marginLeft: -15 }} fontSize="2xl">
            Đăng xuất
          </Text>
        )}
        style={{ backgroundColor: "#fff", color: "#fff" }}
        icon={() => <Icon name="log-out" fontFamily="Feather" />}
        onPress={() => navigation.replace("LoginPage")}
      />
      <Div h={1} bg="#3d3d3d" opacity={0.15} />
      <Div alignItems="center" my={10}>
        <Text>© 2022 by PhoenixCompany</Text>
      </Div>
    </Div>
  );
}
