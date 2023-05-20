import React from "react";
import { useNavigate } from "react-router-dom";
import { FaStar } from "react-icons/fa";
import { BsClock } from "react-icons/bs";
import { RxDotFilled } from "react-icons/rx";

const CategoryPageContainer = ({ data }) => {
  function getRandomFloat(min, max, decimals) {
    const str = (Math.random() * (max - min) + min).toFixed(decimals);
    return parseFloat(str);
  }

  const history = useNavigate();
  return (
    <div className="w-full h-full flex items-center gap-3 my-12 scroll-smooth overflow-hidden flex-wrap justify-center">
      {data &&
        data.map((n) => (
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
        ))}
    </div>
  );
};

export default CategoryPageContainer;
