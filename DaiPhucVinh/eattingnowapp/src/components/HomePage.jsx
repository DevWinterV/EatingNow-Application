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
import { actionType } from "../context/reducer";

const HomePage = () => {
  const [{ customer,cartShow,token}, dispatch] = useStateValue();
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
  const [request, setRequest] = useState({
    CustomerId: "",
    Latitude: "",
    Longittude:"",
  });
  const [requestToken, setRequestToken] = useState({
    CustomerId: "",
    TokenWeb: "",
  });

  // Data nhận từ reponse của api TakeRecommendedFoodlist
  const [datafoodlist, setDatafoodlist] = useState([]);

  useEffect(() => {
    // Lấy vị trí hiện tại của người dùng
    const fetchCurrentLocation = async () => {
      try {
        const position = await new Promise((resolve, reject) => {
          navigator.geolocation.getCurrentPosition(resolve, reject);
        });
        const { latitude, longitude } = position.coords;
        setRequest({
          ...request,
          CustomerId: customer,
          Latitude: latitude,
          Longittude:longitude,
        });
        getToken(messaging,     
          { vapidKey: 'BE4q2YdSfFbtH2IHksx62BReD3OzavmAEbIlYHISKx_gDmcMzDWVIRD1fFHJKXdOMnOc92lQ78YG_MmI3QnaqIg'}).then((currentToken) => {
          if (currentToken) {
            // Send the token to your server and update the UI if necessary
            console.log(currentToken);
            setRequestToken({
              CustomerId: customer,
              TokenWeb: currentToken,
            });
            dispatch({
              type: actionType.SET_TOKEN,
              token: currentToken,  
            });
            localStorage.setItem("token", JSON.stringify(currentToken));
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


  // Gọi api Gợi ý món ăn AI
  async function FechDataFoodReccomend(){
    setLoadingrecommendedfood(true);
    let responseRecommen = await TakeRecommendedFoodList(request);
    if (responseRecommen.success) {
      await setDatafoodlist(responseRecommen.data);
    }
    setLoadingrecommendedfood(false);
  }

  //Load danh mục loại hình ăn uống
  async function onViewAppearing() {
    setLoading(true);
    let response = await TakeAllCuisine(filter);
    if (response.success) {
      setData(response.data);
      setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
    }
    let responseUpdateToken = await UpdateToken(requestToken);
    if (responseUpdateToken.success) {
    }
    setLoading(false);
  }

  React.useEffect(() => {
    onViewAppearing();
    FechDataFoodReccomend();
  }, [filter.page, filter.pageSize, request, requestToken]);

  return (
    <div className="w-full h-auto flex flex-col items-center justify-center">
      {/**
       <HomeContainer />
       */}
       
      <section className="w-full my-6">
        <p className="text-2xl font-semibold capitalize text-headingColor relative before:absolute before:rounded-lg before:content before:w-16 before:h-1 before:-bottom-2 before:left-0 before:bg-gradient-to-tr from-orange-400 to-orange-600 transition-all ease-in-out duration-100 mr-auto">
          Danh mục loại món ăn
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
          Gợi ý món ngon cho bạn
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

