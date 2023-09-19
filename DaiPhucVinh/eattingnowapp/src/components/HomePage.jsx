import * as React from "react";
import  { useEffect, useRef, useState } from "react";
import { TakeRecommendedFoodList } from "../api/foodlist/foodListService";
import HomeContainer from "./HomeContainer";
import CategoryContainer from "./CategoryContainer";
import RowContainer from "./RowContainer";
import { TakeAllCuisine } from "../api/Cuisine/cuisineService";
import { UpdateToken } from "../api/customer/customerService";
import { useStateValue } from "../context/StateProvider";
import { messaging } from "../firebase.config";
import { getMessaging, getToken ,onMessage} from "firebase/messaging";
import CartContainer from "./CartContainer";
import Loader from "./Loader";
import L from "leaflet";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.js";
const HomePage = () => {

  const [{ cartShow,customer }] = useStateValue();
  const [loading, setLoading] = React.useState(false);
  // Loading của gợi ý món ăn yêu thích
  const [loadingrecommendedfood, setLoadingrecommendedfood] = React.useState(false);
  const [data, setData] = React.useState([]);
  const [pageTotal, setPageTotal] = React.useState(1);
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 20,
    term: "",
  });

  const [currentLocation, setCurrentLocation] = useState([10.3759, 105.4185]);

  const [request, setRequest] = useState({
    CustomerId: "",
    Latitude: "",
    Longittude:"",
    TokenWeb: "",
  });
  // Data nhận từ reponse của api TakeRecommendedFoodlist
  const [datafoodlist, setDatafoodlist] = useState([]);


  useEffect(() => {
    // Lấy vị trí hiện tại của người dùng
    const fetchCurrentLocation = async () => {
      setLoadingrecommendedfood(true);
      try {
        const position = await new Promise((resolve, reject) => {
          navigator.geolocation.getCurrentPosition(resolve, reject);
        });
        const { latitude, longitude } = position.coords;
        setCurrentLocation([latitude, longitude]);
        setLoadingrecommendedfood(false);

        getToken(messaging, { vapidKey: 'BEk8bm2SlIuRZyiG5peYbc6jS2C0oqzK5w-wcT4TUTOsyAvZLVGM_5wxd8_f6sPSZZ_3v2tmT7n1jyXUjhpgriQ'}).then((currentToken) => {
          if (currentToken) {
            // Send the token to your server and update the UI if necessary
            console.log(currentToken);
            setRequest({
              ...request,
              CustomerId: customer,
              Latitude: 10.493838,
              Longittude: 105.83838,
              TokenWeb: currentToken
            });
          } else {
            // Show permission request UI
            console.log('No registration token available. Request permission to generate one.');
            // ...
          }
        }).catch((err) => {
          console.log('An error occurred while retrieving token. ', err);
          // ...
        });
      } catch (error) {
        console.error("Error getting current location:", error);
      }
    };
    fetchCurrentLocation();
  }, []);


  async function onViewAppearing() {
    setLoading(true);
    let response = await TakeAllCuisine(filter);
    if (response.success) {
      setData(response.data);
      setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
    }
    // Gọi api Gợi ý món ăn
    let responseRecommen = await TakeRecommendedFoodList(request);
    if (responseRecommen.success) {
      setDatafoodlist(responseRecommen.data);
    }
    let reponseUpdateToken = await UpdateToken(request);
    if (reponseUpdateToken.success) {
      console.log("Cập nhật token thành công");
    }
    setLoading(false);
  }


  function onPageChange(e) {
    setFilter({
      ...filter,
      page: e.selected,
    });
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.page, filter.pageSize, request]);

  return (
    <div className="w-full h-auto flex flex-col items-center justify-center">
      <HomeContainer />
      <section className="w-full my-6">
        <p className="text-2xl font-semibold capitalize text-headingColor relative before:absolute before:rounded-lg before:content before:w-16 before:h-1 before:-bottom-2 before:left-0 before:bg-gradient-to-tr from-orange-400 to-orange-600 transition-all ease-in-out duration-100 mr-auto">
          Danh mục loại hình ăn uống
        </p>
        {loading ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
          <CategoryContainer data={data} />
        )}
      </section>
      <section className="w-full my-6">
        <p className="text-2xl font-semibold capitalize text-headingColor relative before:absolute before:rounded-lg before:content before:w-16 before:h-1 before:-bottom-2 before:left-0 before:bg-gradient-to-tr from-orange-400 to-orange-600 transition-all ease-in-out duration-100 mr-auto">
          Có thể bạn sẽ thích
        </p>
        {loadingrecommendedfood ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
          <RowContainer flag={true} rowData={datafoodlist} />
          )}
      </section>
      

      {cartShow && <CartContainer />}
    </div>
  );
};

export default HomePage;

