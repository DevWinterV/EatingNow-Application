import * as React from "react";
import * as ImagePicker from "expo-image-picker";
import { Button, Div, Icon, Image } from "react-native-magnus";
//import { Image } from "react-native";

export default function PhoenixShooter(props) {
  const [permission, requestPermission] = ImagePicker.useCameraPermissions();
  const [images, setImages] = React.useState([]);
  const [imagePtr, setImagePtr] = React.useState(0);

  function removeImage(index) {
    let newArr = [...images];
    newArr = images.filter((value, i) => {
      return i != index;
    });
    setImagePtr(imagePtr - 1);

    if (newArr.length < props.qty) {
      newArr = [...newArr, ""];
    }
    setImages(newArr);
  }
  const onTakeCamera = async () => {
    if (permission.granted) {
      try {
        let result = await ImagePicker.launchCameraAsync({
          mediaTypes: ImagePicker.MediaTypeOptions.Images,
          allowsEditing: false,
          quality: 1,
        });
        if (!result.canceled) {
          if (imagePtr <= props.qty) {
            let newArr = [...images];
            newArr[imagePtr] = result.assets[0].uri;
            setImages(newArr);
            setImagePtr(imagePtr + 1);
            props?.onCallback(newArr);
          } else {
            setImages((arr) => [...arr, result.assets[0].uri]);
            props?.onCallback((arr) => [...arr, result.assets[0].uri]);
          }
        }
      } catch (err) {
        //console.log(err);
      }
    }
  };
  React.useEffect(() => {
    requestPermission();
    if (props.initData && props.initData.length > 0) {
      setImages(props.initData);
      setImagePtr(props.initData.filter((e) => e.length > 0).length);
    } else {
      setImages(Array.from({ length: props.qty }, () => ""));
      setImagePtr(0);
    }
  }, [props]);
  return (
    <Div alignItems="center" {...props}>
      <Div justifyContent="space-around" row flexWrap="wrap">
        {images.map((item, index) => (
          <Div
            key={index}
            borderWidth={1}
            rounded={10}
            borderStyle="dashed"
            borderColor="text"
            m="sm"
            w={90}
            h={90}
            bg="frame"
            alignItems="center"
            justifyContent="center"
          >
            {item.length > 0 ? (
              <>
                <Image
                  source={{ uri: images[index] }}
                  size={90}
                  resizeMode="cover"
                  borderWidth={2}
                  borderStyle="dashed"
                  w={88}
                  h={88}
                  borderRadius={10}
                />
                <Button
                  h={24}
                  w={24}
                  rounded="circle"
                  bg="gray400"
                  position="absolute"
                  top={-10}
                  right={-10}
                  p={0}
                  onPress={() => removeImage(index)}
                >
                  <Icon name="close-outline" color="red500" fontFamily="Ionicons" fontSize={18} />
                </Button>
              </>
            ) : (
              index === imagePtr && (
                <Button
                  h={98}
                  w={98}
                  rounded="circle"
                  bg="transparent"
                  alignSelf="center"
                  onPress={onTakeCamera}
                >
                  <Icon
                    name="camera-outline"
                    color="green500"
                    fontFamily="Ionicons"
                    fontSize={48}
                  />
                </Button>
              )
            )}
          </Div>
        ))}
      </Div>
    </Div>
  );
}
