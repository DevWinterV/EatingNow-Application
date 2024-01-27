import React, { useState } from "react";
import { bgLogin } from "../assets";
import { motion } from "framer-motion";
import {
  AiFillPhone,
  AiOutlineEye,
  AiOutlineEyeInvisible,
} from "react-icons/ai";
import { Link, useNavigate } from "react-router-dom";
import { useStateValue } from "../context/StateProvider";
import CartContainer from "./CartContainer";
import { actionType } from "../context/reducer";
import Swal from "sweetalert2";
import { LoginInFront } from "../api/auth/authService";
import { Pane, Portal, toaster } from "evergreen-ui";

const Login = () => {
  const navigate = useNavigate();
  const [{ cartShow, user, linked }, dispatch] = useStateValue();
  const [showPass, setShowPass] = useState(false);
  const [request, setRequest] = useState({
    username: "",
    password: "",
  });

  const ChangDisplayPass = () => {
    setShowPass(!showPass);
  };

  async function HandleLogin() {
    if (request.username == "" || request.password == "") {
      toaster.warning("Tài khoản hoặc mật khẩu không  được bỏ trống !!!");
    } else {
      let response = await LoginInFront(request);
      if (response.success) {
        dispatch({
          type: actionType.SET_USER,
          user: response.data[0],
        });
        localStorage.setItem("user", JSON.stringify(response.data[0]));
        navigate("/auth");
      } else {
        toaster.danger("Sai tài khoản hoặc mật khẩu");
        return;
      }
    }
  }

  async function handChangeOTP() {
    navigate("/otpauthen");
  }

  async function handChangeHome() {
    dispatch({
      type: actionType.SET_LINKED,
      linked: !linked,
    });
    navigate("/");
  }

  return (
    <section className="bg-white-100 min-h-screen flex items-center justify-center">
      <div className="bg-gray-100 flex rounded-2xl shadow-lg max-w-3xl p-5 gap-6 items-center text-center">
        <div className="sm:w-1/2 px-14">
          <h2 className="font-bold text-2xl text-[#171a1f]">Đăng nhập</h2>

          <div className="flex mt-4 flex-col gap-4">
            <input
              className="p-2 rounded-xl border"
              type="text"
              name="email"
              placeholder="Email"
              onChange={(e) => {
                setRequest({
                  ...request,
                  username: e.target.value,
                });
              }}
            />
            <div className="relative">
              <input
                className="p-2 rounded-xl border w-full"
                type={`${showPass == false ? "password" : "text"}`}
                name="password"
                placeholder="Password"
                onChange={(e) => {
                  setRequest({
                    ...request,
                    password: e.target.value,
                  });
                }}
              />
              <div onClick={ChangDisplayPass}>
                {showPass == false ? (
                  <AiOutlineEye className="w-5 h-5 absolute top-1/2 right-3 -translate-y-1/2 cursor-pointer" />
                ) : (
                  <AiOutlineEyeInvisible className="w-5 h-5 absolute top-1/2 right-3 -translate-y-1/2 cursor-pointer" />
                )}
              </div>
            </div>
            <motion.button
              whileTap={{ scale: 0.75 }}
              className="bg-orange-600 rounded-xl text-white py-2 duration-300"
              onClick={HandleLogin}
            >
              Đăng nhập
            </motion.button>
          </div>

          <div className="mt-5 text-xs border-b border-[#171a1f] py-4 text-[#171a1f]">
            <a href="#">Quên mật khẩu?</a>
          </div>

          <div className="mt-3 text-xs flex justify-between items-center text-[#171a1f]">
            <p>Không có tài khoản?</p>
            <motion.button
              whileTap={{ scale: 0.75 }}
              className="py-2 px-5 bg-white border rounded-xl duration-300"
            >
              Đăng ký
            </motion.button>
          </div>
        </div>

        <div className="sm:block hidden w-1/2">
          <img className="rounded-2xl" src={bgLogin} alt="" />
        </div>
      </div>
      <Portal>
        <Pane
          className="bg-transparent"
          padding={24}
          position="fixed"
          top={0}
          left={0}
        >
          <motion.button
            whileTap={{ scale: 0.75 }}
            className="mt-2 rounded-xl text-white py-2 duration-300 justify-center items-center"
            onClick={handChangeHome}
          >
            <motion.button
              whileTap={{ scale: 0.75 }}
              style={{ width: "250px" }}
              className="font-bold mt-1 rounded-xl text-orange-800 py-1 duration-300 flex justify-center items-center"
            >
              <span className="text-2xl">XpressEat.</span>
            </motion.button>
          </motion.button>
        </Pane>
      </Portal>
      <Portal>
        <Pane
          className="bg-transparent m-4"
          padding={24}
          position="fixed"
          bottom={0}
          right={0}
        >
          <motion.button
            whileTap={{ scale: 0.75 }}
            className="bg-orange-600 mt-2 rounded-xl text-white py-2 duration-300 justify-center items-center"
            onClick={handChangeOTP}
          >
            <motion.button
              whileTap={{ scale: 0.75 }}
              style={{ width: "200px" }}
              className="mt-1 rounded-xl text-white py-1 duration-300 flex justify-center items-center"
            >
              <AiFillPhone className="mr-2" />
              <span>Số điện thoại</span>
            </motion.button>
          </motion.button>
        </Pane>
      </Portal>
      {cartShow && <CartContainer />}
    </section>
  );
};

export default Login;
