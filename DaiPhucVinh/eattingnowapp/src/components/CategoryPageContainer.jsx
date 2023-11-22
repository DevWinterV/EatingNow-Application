import { useNavigate } from "react-router-dom";
import { FaStar } from "react-icons/fa";
import { BsClock } from "react-icons/bs";
import { RxDotFilled } from "react-icons/rx";
import React, { useEffect,  useState } from "react";
import Loader from "./Loader";
const CategoryPageContainer = ({ datas }) => {

  function getRandomFloat(min, max, decimals) {
    const str = (Math.random() * (max - min) + min).toFixed(decimals);
    return parseFloat(str);
  }
  const [loading, setLoading] = useState(false);
  const [currentLocation, setCurrentLocation] = useState([10.3759, 105.4185]);
 // const [data, setData] = useState([]);
  
  useEffect(() => {
    const fetchCurrentLocation = async () => {
      try {
        const position = await new Promise((resolve, reject) => {
          navigator.geolocation.getCurrentPosition(resolve, reject);
        });
        const { latitude, longitude } = position.coords;
        setCurrentLocation([latitude, longitude]);
      } catch (error) {
        console.error("Error getting current location:", error);
      }
    };
    fetchCurrentLocation();
  }, []);


  /*
    useEffect(() => {
      const fetchTimeAndDistanceForLocations = async () => {
        try {
          setLoading(true);

          const updatedLocations = await Promise.all(
            datas.map(async (location) => {
              try {
                const result = await calculateDistanceAndTime(currentLocation, [location.Latitude, location.Longitude]);
                if (result) {
                  const { distance, duration } = result;
                  const roundedTime = Math.round(duration / 60 * 10) / 10; // Thời gian được làm tròn đến 1 chữ số thập phân
                  const roundedDistance = Math.round(distance / 1000 * 10) / 10; // Khoảng cách được làm tròn đến 1 chữ số thập phân
                  return { ...location, Time: roundedTime, Distance: roundedDistance };
                } else {
                  console.log('Không thể tính khoảng cách và thời gian cho vị trí:', location);
                  return location; // Trả về đối tượng location ban đầu nếu không thể tính toán
                }
              } catch (error) {
                console.error('Lỗi khi gọi hàm calculateDistanceAndTime:', error);
                return location; // Trả về đối tượng location ban đầu nếu có lỗi
              }
            })
          );

          setData(updatedLocations);
        } catch (error) {
          console.error(error);
        } finally {
          setLoading(false);
        }
      };

      fetchTimeAndDistanceForLocations();
    }, [datas, currentLocation]);
  */
  const history = useNavigate();
  return (
    loading ? (
      <div className="text-center pt-20">
        <Loader />
      </div>
    ) : (
      <div className="w-full h-full flex items-center gap-3 my-12 scroll-smooth overflow-hidden flex-wrap justify-center">
      {datas && datas.length > 0 ? (
        datas.map((n) => 
        (
          <div
            key={n.UserId}
            style={{ height: "300px" }}
            className="border-4 border-orange-100 cursor-pointer gap-3 w-275 h-full min-w-[275px] md:w-200 md:min-w-[200px] bg-white rounded-xl px-1 my-12 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative"
            onClick={() => {
              history("/restaurant/" + n.UserId, {
                state: { data: n },
              });
            }}
          >
            <div className="w-full p-1 rounded-[10px] gap-2 mt-1">
              <div className="h-[150px] w-full overflow-hidden rounded-xl">
                <img
                  src={n.AbsoluteImage}
                  alt=""
                  loading="lazy"
                  className="w-full h-full object-cover aspect-square hover:scale-110 transition duration-300 ease-in-out"
                />
              </div>

              <h1 className="mt-2 text-center text-textColor font-bold">
                {n.FullName}
              </h1>

              <p className="mt-1 text-center text-gray-400 text-sm">
                {n.Description}
              </p>

              <div className="mt-1 mb-1 gap-3 flex justify-center items-center">
                <div className="flex gap-1 items-center justify-center justify-items-center">
                  <FaStar className="text-xl text-amber-300" />
                  <h1 className="text-base text-gray-400">
                    {getRandomFloat(1, 5, 1)}
                  </h1>
                </div>
                <div className="flex items-center justify-center justify-items-center">
                  <div className="flex gap-1 justify-center items-center justify-items-center">
                    <BsClock className="text-xl text-gray-400" />
                    <h1 className="text-base text-gray-400">{n.Time} phút</h1>
                  </div>
                  <RxDotFilled className="text-xl text-gray-400" />
                  <h1 className="text-base text-gray-400">{n.Distance} km</h1>
                </div>
              </div>
            </div>
          </div>
        ))
      ):
       (
        <div className="text-center">
          <span>Chưa có cửa hàng hoạt động trên hệ thống gần khu vực của bạn 😣!</span>
        </div>
      )
      }
    </div>    )

  );
};

export default CategoryPageContainer;
