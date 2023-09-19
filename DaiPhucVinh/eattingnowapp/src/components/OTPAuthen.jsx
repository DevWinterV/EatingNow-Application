import { BsFillShieldLockFill, BsTelephoneFill } from "react-icons/bs";
import { CgSpinner } from "react-icons/cg";

import OtpInput from "otp-input-react";
import { useState } from "react";
import PhoneInput from "react-phone-input-2";
import "react-phone-input-2/lib/style.css";
import { auth } from "../firebase.config";
import { RecaptchaVerifier, signInWithPhoneNumber } from "firebase/auth";
import { useStateValue } from "../context/StateProvider";
import { actionType } from "../context/reducer";
import { useNavigate } from "react-router-dom";
import { Pane, Portal, Text, toaster } from "evergreen-ui";
import { motion } from "framer-motion";
import { MdLogin } from "react-icons/md";

const OTPAuthen = () => {
  const [{ customer, linked }, dispatch] = useStateValue();
  const navigate = useNavigate();
  const [otp, setOtp] = useState("");
  const [ph, setPh] = useState("");
  const [loading, setLoading] = useState(false);
  const [showOTP, setShowOTP] = useState(false);
  const [user, setUser] = useState(null);

  function onCaptchVerify() {
    if (!window.recaptchaVerifier) {
      window.recaptchaVerifier = new RecaptchaVerifier(
        "recaptcha-container",
        {
          size: "invisible",
          callback: (response) => {
            onSignup();
          },
          "expired-callback": () => {},
        },
        auth
      );
    }
  }

  function onSignup() {
    if (ph.trim() !== "") {
      setLoading(true);
      onCaptchVerify();

      const appVerifier = window.recaptchaVerifier;

      const formatPh = "+" + ph;

      signInWithPhoneNumber(auth, formatPh, appVerifier)
        .then((confirmationResult) => {
          window.confirmationResult = confirmationResult;
          setLoading(false);
          setShowOTP(true);
        })
        .catch((error) => {
          console.log(error);
          setLoading(false);
        });
    } else {
      toaster.warning("Cảnh báo !", {
        description: "Số điện thoại không được bỏ trống !",
        id: "forbidden-action",
      });
    }
  }

  function onOTPVerify() {
    setLoading(true);
    window.confirmationResult
      .confirm(otp)
      .then(async (res) => {
        console.log(res);
        setUser(res.user);
        dispatch({
          type: actionType.SET_CUSTOMER,
          customer: res.user.uid,
        });
        localStorage.setItem("customer", JSON.stringify(res.user.uid));
        
        dispatch({
          type: actionType.SET_LINKED,
          linked: !linked,
        });
        navigate("/*");
        setLoading(false);
      })
      .catch((err) => {
        console.log(err);
        setLoading(false);
      });
  }

  async function handChangeLogin() {
    navigate("/*");
  }

  async function handChangeHome() {
    dispatch({
      type: actionType.SET_LINKED,
      linked: !linked,
    });
    navigate("/*");
  }

  return (
    <section className="bg-orange-100 flex items-center justify-center h-screen">
      <div>
        <div id="recaptcha-container"></div>
        {user ? (
          <h2 className="text-center text-white font-medium text-2xl">
            👍Login Success
          </h2>
        ) : (
          <div className="w-80 flex flex-col gap-4 rounded-lg p-4">
            <h1 className="font-bold text-center leading-normal text-stone-600 text-3xl mb-6">
              CHÀO MỪNG ĐẾN <br /> EATINGNOW
            </h1>
            {showOTP ? (
              <>
                <div className="bg-white text-orange-500 w-fit mx-auto p-4 rounded-full">
                  <BsFillShieldLockFill size={30} />
                </div>
                <label
                  htmlFor="otp"
                  className="font-bold text-xl text-stone-500 text-center"
                >
                  Nhập mã OTP
                </label>
                <OtpInput
                  value={otp}
                  onChange={setOtp}
                  OTPLength={6}
                  otpType="number"
                  disabled={false}
                  autoFocus
                  className="opt-container "
                ></OtpInput>
                <button
                  onClick={onOTPVerify}
                  className="bg-orange-600 w-full flex gap-1 items-center justify-center py-2.5 text-white rounded"
                >
                  {loading && (
                    <CgSpinner size={20} className="mt-1 animate-spin" />
                  )}
                  <span>Xác thực OTP</span>
                </button>
              </>
            ) : (
              <>
                <div className="bg-white text-orange-500 w-fit mx-auto p-4 rounded-full">
                  <BsTelephoneFill size={30} />
                </div>
                <label
                  htmlFor=""
                  className="font-bold text-xl text-stone-600 text-center"
                >
                  Xác minh số điện thoại
                </label>
                <PhoneInput country={"vn"} value={ph} onChange={setPh} />
                <button
                  onClick={onSignup}
                  className="bg-orange-600 w-full flex gap-1 items-center justify-center py-2.5 text-white rounded"
                >
                  {loading && (
                    <CgSpinner size={20} className="mt-1 animate-spin" />
                  )}
                  <span>Gửi mã SMS</span>
                </button>
              </>
            )}
          </div>
        )}
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
              <span className="text-2xl">EATINGNOW.</span>
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
            onClick={handChangeLogin}
          >
            <motion.button
              whileTap={{ scale: 0.75 }}
              style={{ width: "250px" }}
              className="mt-1 rounded-xl text-white py-1 duration-300 flex justify-center items-center"
            >
              <MdLogin className="mr-2" />
              <span>Đăng nhập bằng tài khoản</span>
            </motion.button>
          </motion.button>
        </Pane>
      </Portal>
    </section>
  );
};

export default OTPAuthen;
