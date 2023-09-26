import React, { useState } from "react";
import { Logo, Avatar } from "../assets";
import { MdShoppingBasket, MdAdd, MdLogout, MdLogin, MdAccountBox, MdAccountCircle, MdSettings } from "react-icons/md";
import { motion } from "framer-motion";
import { Link } from "react-router-dom";
import { useStateValue } from "../context/StateProvider";
import { actionType } from "../context/reducer";

const Header = () => {
  const [isMenu, setIsMenu] = useState(false);

  const Login = () => {
    setIsMenu(!isMenu);
  };

  const [{ cartShow, cartItems, linked, user, customer }, dispatch] =
    useStateValue();
  const ShowCart = () => {
    dispatch({
      type: actionType.SET_CART_SHOW,
      cartShow: !cartShow,
    });
  };

  const handleLogin = () => {
    dispatch({
      type: actionType.SET_LINKED,
      linked: !linked,
    });
    <Link to={"/otpauthen"}></Link>;
  };
  const handleAccount = () => {
    dispatch({
      type: actionType.SET_LINKED,
      linked: !linked,
    });
    <Link to={"/personalaccount"}></Link>;
  };

  const handleLogout = () => {
    dispatch({
      type: actionType.SET_CUSTOMER,
      customer: null,
    });
    localStorage.setItem("customer", JSON.stringify(null));
  };

  console.log("customer", customer);
  return (
    <header className="fixed z-50 w-screen p-3 px-8 md:p-6 md:px-16 bg-orange-50">
      {/* desktop & tablet  */}
      <div className="hidden md:flex w-full h-full items-center justify-between">
        <Link to={"/"} className="flex items-center gap-2 cursor-pointer">
          <p className="text-orange-600 text-xl font-bold px-4">EATTINGNOW.</p>
        </Link>

        <div className="flex items-center gap-8">
          <motion.ul
            initial={{ opacity: 0, x: 200 }}
            animate={{ opacity: 1, x: 0 }}
            exit={{ opacity: 0, x: 200 }}
            className="flex items-center gap-8"
          >
            <li className="text-base text-orange-900 hover:text-orange-900 duration-100 transition-all ease-in-out cursor-pointer">
              <Link to={`/`}>Trang chủ</Link>
            </li>
          </motion.ul>

          <div
            className="relative flex items-center justify-center"
            onClick={ShowCart}
          >
            <MdShoppingBasket className="text-textColor text-2xl cursor-pointer" />
            {cartItems && cartItems.length > 0 && (
              <div className="absolute -top-3 -right-3 w-5 h-5 rounded-full bg-cartNumbg flex items-center justify-center">
                <p className="text-xs text-white font-semibold">
                  {cartItems.length}
                </p>
              </div>
            )}
          </div>
          

          <div className="relative">
            <motion.img
              whileTap={{ scale: 0.6 }}
              src={user ? user.Image : Avatar}
              className="w-10 min-w-[40px] h-10 min-h-[40px] shadow-2xl cursor-pointer rounded-full"
              alt="userprofile"
              onClick={Login}
            />

            {isMenu && (
              <motion.div
                initial={{ opacity: 0, scale: 0.6 }}
                animate={{ opacity: 1, scale: 1 }}
                exit={{ opacity: 0, scale: 0.6 }}
                className="w-40 bg-gray-50 shadow-xl rounded-lg flex flex-col absolute top-12 right-0"
              >
                {/* <p className="px-4 py-2 flex items-center gap-3 cursor-pointer hover:bg-slate-100 transition-all duration-100 ease-in-out text-textColor text-base justify-between">
                  Thông tin <MdAdd />
                </p> */}
            <div>
                {customer !== null ? (
                  <div>
                    <a href="/account">
                      <p
                        className="m-2 p-2 rounded-md shadow-md flex items-center justify-center bg-gray-200 gap-3 cursor-pointer hover:bg-slate-300 transition-all duration-100 ease-in-out text-textColor text-base"
                      >
                        Tài khoản <MdAccountCircle />
                      </p>
                    </a>
                    <p
                      onClick={handleLogout}
                      className="m-2 p-2 rounded-md shadow-md flex items-center justify-center bg-gray-200 gap-3 cursor-pointer hover:bg-slate-300 transition-all duration-100 ease-in-out text-textColor text-base">
                        Đăng xuất <MdLogout />
                      </p>
                  </div>
                ) : (
                  <div>
                    <p
                      onClick={handleLogin}
                      className="px-4 py-2 flex items-center gap-3 cursor-pointer hover:bg-slate-100 transition-all duration-100 ease-in-out text-textColor text-base justify-between"
                    >
                      Đăng nhập <MdLogin />
                    </p>
                  </div>
                )}
              </div>

              </motion.div>
            )}
          </div>
        </div>
      </div>

      {/* mobile */}
      <div className="flex items-center justify-between md:hidden w-full h-full">
        <div
          className="relative flex items-center justify-center"
          onClick={ShowCart}
        >
          <MdShoppingBasket className="text-textColor text-2xl cursor-pointer" />
          {cartItems && cartItems.length > 0 && (
            <div className="absolute -top-3 -right-3 w-5 h-5 rounded-full bg-cartNumbg flex items-center justify-center">
              <p className="text-xs text-white font-semibold">
                {cartItems.length}
              </p>
            </div>
          )}
        </div>

        <Link to={"/"} className="flex items-center gap-2 cursor-pointer">
          <img src={Logo} className="w-16 object-cover" alt="logo" />
          <p className="text-headingColor text-xl font-bold">EattingNow</p>
        </Link>
        <div className="relative">
          <motion.img
            whileTap={{ scale: 0.6 }}
            src={Avatar}
            className="w-10 min-w-[40px] h-10 min-h-[40px] shadow-2xl cursor-pointer"
            alt="userprofile"
            onClick={Login}
          />

          {isMenu && (
            <motion.div className="w-40 bg-gray-50 shadow-xl rounded-lg flex flex-col absolute top-12 right-0">
              <ul className="flex flex-col">   
                <div>
                  {customer ? (
                    <div>
                    <a href="/account">
                      <p
                      className="m-2 p-2 rounded-md shadow-md flex items-center justify-center bg-gray-200 gap-3 cursor-pointer hover:bg-slate-300 transition-all duration-100 ease-in-out text-textColor text-base">
                        Tài khoản <MdAccountCircle />
                      </p>
                      </a>
                      <p
                      onClick={handleLogout}
                      className="m-2 p-2 rounded-md shadow-md flex items-center justify-center bg-gray-200 gap-3 cursor-pointer hover:bg-slate-300 transition-all duration-100 ease-in-out text-textColor text-base">
                        Đăng xuất <MdLogout />
                      </p>
                    </div>
                  ) : (
                    <p
                    onClick={handleLogin}
                    className="px-4 py-2 flex items-center gap-3 cursor-pointer hover:bg-slate-100 transition-all duration-100 ease-in-out text-textColor text-base justify-between"
                    >
                      Đăng nhập <MdLogin />
                    </p>
                  )}
                </div>
              </ul>
             
            </motion.div>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;
