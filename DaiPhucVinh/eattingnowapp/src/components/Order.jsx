import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useStateValue } from "../context/StateProvider";
import CartContainer from "./CartContainer";
import { RadioGroup, TextInput, toaster } from "evergreen-ui";
import SelectProvince from "./SelectProvince";
import SelectDistrict from "./SelectDistrict";
import SelectWard from "./SelectWard";
import CartItem from "./CartItem";
import { useEffect } from "react";
import { motion } from "framer-motion";
import App from "./DeliveryAddress";
import {
  CheckCustomer,
  CreateOrderCustomer,
} from "../api/customer/customerService";
import MyApp from "./DeliveryAddress";

let delivery = 15000;
const Order = () => {
  const key = 'AIzaSyC-N1CyjegpbGvp_PO666Ie9sNQy9xW2Fo'
  const [formData, setFormData] = useState({
    CompleteAddress: "",
  });
  const [tot, setTot] = useState(0);
  const [flag, setFlag] = useState(1);
  const [{ cartItems }] = useStateValue();
  const navigate = useNavigate();
  const [{ cartShow, customer }, dispatch] = useStateValue();
  const [checkCustomer, setCheckCustomer] = useState(0);
  const [request, setRequest] = useState({
    CustomerId: "",
    CompleteName: "",
    ProvinceId: "",
    DistrictId: "",
    WardId: "",
    Phone: "",
    Email: "",
    Address: "",
    Payment: "",
    TotalAmt: 0,
    TransportFee: 0,
    IntoMoney: 0,
    UserId: 0,
    TokenWeb: "",
    TokenApp:"",
    OrderLine: {},
  });

    const [notification, setNotification] = useState({
      to: "",
      notification: {
        body: "",
        title: "",
        icon: "",
        click_action: "",
      },
    });
  async function onChangeCustomer() {
    let checkCustomer = await CheckCustomer(request);
    if (checkCustomer.success) {
      setCheckCustomer(checkCustomer.dataCount);
    }
  }

  const handleInputChange = (e) => {
    const { name, value, files } = e.target;
      setFormData({
        ...formData,
        [name]: value,
      });
  };

  async function order() {
    let response = await CreateOrderCustomer(request);
    if (response.success) {
      if (response.message != "") {
        window.location.href = `${response.message}`;
      } else {
        toaster.success("Đặt món thành công", {
          duration: 3,
          description: "Món ăn sẽ được giao đến bạn sớm nhất.",
        });
      }
    }
  }
  const [options] = React.useState([
    { label: "Thanh toán khi nhận hàng", value: "PaymentOnDelivery" },
    { label: "Thanh toán ví Momo", value: "MOMO" },
  ]);
  const [value, setValue] = React.useState("PaymentOnDelivery");

  useEffect(() => {
    let totalPrice = cartItems.reduce(function (accumulator, item) {
      return accumulator + item.qty * item.Price;
    }, 0);
    setTot(totalPrice);
    setRequest({
      ...request,
      CustomerId: customer,
      TotalAmt: totalPrice,
      TransportFee: delivery,
      IntoMoney: delivery + totalPrice,
      UserId: cartItems[0].UserId,
      Payment: value,
      OrderLine: cartItems,
    });
    onChangeCustomer();
  }, [tot, flag, customer, value]);

  return (
    <section className="bg-min-h-screen flex items-center justify-center">
      <div className="bg-green-200 h-610 flex rounded-2xl shadow-lg max-w-3xl p-5 gap-6 items-start text-center">
        <div className="w-1/2 px-8">
          <h2 className="font-bold text-2xl text-[#171a1f] text-left capitalize">
            Danh sách món ăn
          </h2>
          <div className="w-340 h-340 md:h-42 py-5 flex flex-col gap-3 overflow-y-scroll scrollbar-none">
            {/* cart item */}

            {cartItems &&
              cartItems.map((item) => (
                <CartItem
                key={item?.FoodListId}
                  item={item}
                  setFlag={setFlag}
                  flag={flag}
                />
              ))}
          </div>
          <div className="w-340 flex-1 rounded-t-[2rem] flex flex-col items-center justify-evenly px-8 py-2">
            <div className="w-full flex items-center justify-between">
              <p className="text-gray-400 text-base font-semibold">Tổng tiền</p>
              <p className="text-textColor text-base font-semibold">
                {tot.toLocaleString()}
                <span className="text-base text-red-500"> vnđ</span>
              </p>
            </div>

            <div className="w-full flex items-center justify-between">
              <p className="text-gray-400 text-base font-semibold">
                Phí vận chuyển
              </p>
              <p className="text-textColor text-base font-semibold">
                {delivery.toLocaleString()}
                <span className="text-base text-red-500"> vnđ</span>
              </p>
            </div>

            <div className="w-full border-b border-gray-600 my-2"></div>

            <div className="w-full flex items-center justify-between">
              <p className="text-textColor text-lg font-semibold">Thanh toán</p>
              <p className="text-textColor text-lg font-semibold">
                {(tot + delivery).toLocaleString()}
                <span className="text-base text-red-500"> vnđ</span>
              </p>
            </div>

            {checkCustomer > 0 && (
              <>
                <RadioGroup
                  label="Phương thức thanh toán"
                  size={16}
                  value={value}
                  options={options}
                  onChange={(event) => setValue(event.target.value)}
                />
                <motion.button
                  whileTap={{ scale: 0.75 }}
                  className="bg-[#171a1f] rounded-xl text-white py-2 duration-300 w-64 p-2 mt-8 "
                  onClick={order}
                >
                  Đặt hàng
                </motion.button>
              </>
            )}
          </div>
        </div>
        {checkCustomer <= 0 && (
          <div className="w-1/2 px-8">
            <h2 className="font-bold text-2xl text-[#171a1f] text-left capitalize">
              Nhập thông tin
            </h2>

            <div className="flex flex-col items-center justify-center">
              <TextInput
                className="p-2 mt-4 rounded-xl border w-64"
                type="text"
                placeholder="Nhập tên . . ."
                onChange={(e) => {
                  setRequest({
                    ...request,
                    CompleteName: e.target.value,
                  });
                }}
              />
              <TextInput
                className="p-2 mt-4 rounded-xl border w-64"
                type="text"
                placeholder="Nhập số điện thoại . . ."
                onChange={(e) => {
                  setRequest({
                    ...request,
                    Phone: e.target.value,
                  });
                }}
              />
              <TextInput
                className="p-2 mt-4 rounded-xl border w-64"
                type="text"
                placeholder="Nhập địa chỉ chi tiêt . . ."
                onChange={(e) => {
                  setRequest({
                    ...request,
                    Address: e.target.value,
                  });
                }}
              />
              <SelectProvince
                marginTop={16}
                selected={{
                  value: request.ProvinceId,
                  label: request.ProvinceName,
                }}
                onSelect={(e) => {
                  setRequest({
                    ...request,
                    ProvinceId: e.value,
                    ProvinceName: e.label,
                  });
                }}
              />
              <SelectDistrict
                marginTop={16}
                selected={{
                  value: request.DistrictId,
                  label: request.DistrictName,
                }}
                onSelect={(e) => {
                  setRequest({
                    ...request,
                    DistrictId: e.value,
                    DistrictName: e.label,
                  });
                }}
              />
              <SelectWard
                marginTop={16}
                selected={{
                  value: request.WardId,
                  label: request.WardName,
                }}
                onSelect={(e) => {
                  setRequest({
                    ...request,
                    WardId: e.value,
                    WardName: e.label,
                  });
                }}
              />
              <RadioGroup
                label="Permissions"
                size={16}
                value={value}
                options={options}
                onChange={(event) => setValue(event.target.value)}
              />
              <motion.button
                whileTap={{ scale: 0.75 }}
                className="bg-[#171a1f] rounded-xl text-white py-2 duration-300 w-64 p-2 mt-8 "
                onClick={order}
              >
                Đặt hàng
              </motion.button>
            </div>
          </div>
        )}
      <div className="flex justify-center">

      <div className="w-full max-w-3xl p-4">
        <MyApp
          googleMapURL={`https://maps.googleapis.com/maps/api/js?key=${key}&callback=initMap`}
          loadingElement={<div style={{ height: `100%` }} />}
          containerElement={<div style={{ height: `30vh`, margin: `auto`, border: '2px solid black' }} />}
          mapElement={<div style={{ height: `100%` }} />}
        />
      </div>
    </div>
      </div>

      {cartShow && <CartContainer />}
    </section>
  );
};

export default Order;
