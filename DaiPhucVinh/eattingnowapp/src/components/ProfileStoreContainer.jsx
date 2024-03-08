import React from "react";
import { GiAlarmClock } from "react-icons/gi";
import LeafletMapStore from "./LeafletMapStore";
import { useStateValue } from "../context/StateProvider";
import CartContainer from "./CartContainer";
import { useState, useEffect } from "react";
const ProfileStoreContainer = ({ data }) => {
  const [{ cartShow }] = useStateValue();

  const [scrollUp, setScrollUp] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      if (window.scrollY > 50) {
        setScrollUp(true);
      } else {
        setScrollUp(false);
      }
    };

    window.addEventListener('scroll', handleScroll);

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);
  return (
    <section
      key={data?.CategoryId}
      className="grid grid-cols-1 md:grid-cols-2 gap-2 w-full"
      id="home"
    >
      <div className="py-2 flex-1 flex flex-col items-start justify-center gap-6">
        <div className="flex items-center gap-2 justify-center bg-orange-100 px-4 py-4 rounded-full">
          <p className="text-[2.2rem] lg:text-[2.2rem] text-base text-orange-500 font-semibold">
            {data?.NameStore}
          </p>
        </div>
        <div className="flex items-center gap-2 justify-center px-4 rounded-full">
          <p className="text-[1.2rem] lg:text-[1.2rem] font-bold tracking-wide text-headingColor">
            {data?.DescriptionStore}
          </p>
        </div>
        <div className="px-4 relative flex items-center justify-center text-[1.2rem] lg:text-[1.2rem] font-semibold">
          Địa chỉ:
          <span> {data?.Address}</span> {/* Address */}
        </div>
        <div className="px-4 relative flex items-center justify-center text-[1.2rem] lg:text-[1.2rem] font-semibold">
          Giờ mở cửa:
          <GiAlarmClock className="text-orange-800 text-2xl cursor-pointer w-10 h-10 " />
          <span className="text-orange-800 text-[1.5rem] lg:text-[1.5rem]">
            {data?.OpenTime}
          </span>
        </div>
        <div className="px-4 relative flex items-center justify-center text-[1.2rem] lg:text-[1.2rem] font-semibold">
          Trạng thái hoạt động:
          {data?.Status === true ? (
            <span className="text-green-800 text-[1.5rem] lg:text-[1.5rem]">
              Đang hoạt động
            </span>
          ) : (
            <span className="text-red-800 text-[1.5rem] lg:text-[1.5rem]">
              Đã đóng cửa
            </span>
          )}
        </div>
      </div>

      <div className="py-2 flex-1 flex flex-col items-start justify-center gap-6">
        {cartShow ? (
          <CartContainer />
        ) : (
          <div className={`map-container ${scrollUp ? 'fade-out' : ''}`}>
            <LeafletMapStore locations={data} />
          </div>
        )}
      </div>
    </section>
  );
};

export default ProfileStoreContainer;
