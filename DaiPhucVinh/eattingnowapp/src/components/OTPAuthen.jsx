import { BsFillShieldLockFill, BsTelephoneFill } from "react-icons/bs";
import { CgSpinner } from "react-icons/cg";
import OtpInput from "otp-input-react";
import PhoneInput from "react-phone-input-2";
import "react-phone-input-2/lib/style.css";
import { UpdateToken , CheckCustomerEmail} from "../api/customer/customerService";
import { RecaptchaVerifier, signInWithPhoneNumber } from "firebase/auth";
import { useStateValue } from "../context/StateProvider";
import { actionType } from "../context/reducer";
import { useNavigate } from "react-router-dom";
import { Pane, Portal, Text, toaster } from "evergreen-ui";
import { motion } from "framer-motion";
import  { useEffect, useRef, useState } from "react";
import { TbBrandFacebook, TbBrandGoogle, TbFaceId, TbLogin } from "react-icons/tb";
import { provider} from "../firebase.config";
import { getAuth,signInWithPopup, GoogleAuthProvider } from "firebase/auth";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const OTPAuthen = () => {
  const [{ customer, linked, token }, dispatch] = useStateValue();
  const navigate = useNavigate();
  const [otp, setOtp] = useState("");
  const [ph, setPh] = useState("");
  const [loading, setLoading] = useState(false);
  const [showOTP, setShowOTP] = useState(false);
  const [user, setUser] = useState(null);

  let faceioInstance = null
  const handleError = (errCode) => {
    // Log all possible error codes during user interaction..
    // Refer to: https://faceio.net/integration-guide#error-codes
    // for a detailed overview when these errors are triggered.
    // const fioErrCode={PERMISSION_REFUSED:1,NO_FACES_DETECTED:2,UNRECOGNIZED_FACE:3,MANY_FACES:4,PAD_ATTACK:5,FACE_MISMATCH:6,NETWORK_IO:7,WRONG_PIN_CODE:8,PROCESSING_ERR:9,UNAUTHORIZED:10,TERMS_NOT_ACCEPTED:11,UI_NOT_READY:12,SESSION_EXPIRED:13,TIMEOUT:14,TOO_MANY_REQUESTS:15,EMPTY_ORIGIN:16,FORBIDDDEN_ORIGIN:17,FORBIDDDEN_COUNTRY:18,UNIQUE_PIN_REQUIRED:19,SESSION_IN_PROGRESS:20},fioState={UI_READY:1,PERM_WAIT:2,PERM_REFUSED:3,PERM_GRANTED:4,REPLY_WAIT:5,PERM_PIN_WAIT:6,AUTH_FAILURE:7,AUTH_SUCCESS:8}
    switch (errCode) {
      case fioErrCode.PERMISSION_REFUSED:
        console.log("Access to the Camera stream was denied by the end user")
        break
      case fioErrCode.NO_FACES_DETECTED:
        console.log("No faces were detected during the enroll or authentication process")
        break
      case fioErrCode.UNRECOGNIZED_FACE:
        console.log("Unrecognized face on this application's Facial Index")
        break
      case fioErrCode.MANY_FACES:
        console.log("Two or more faces were detected during the scan process")
        break
      case fioErrCode.PAD_ATTACK:
        console.log("Presentation (Spoof) Attack (PAD) detected during the scan process")
        break
      case fioErrCode.FACE_MISMATCH:
        console.log("Calculated Facial Vectors of the user being enrolled do not matches")
        break
      case fioErrCode.WRONG_PIN_CODE:
        console.log("Wrong PIN code supplied by the user being authenticated")
        break
      case fioErrCode.PROCESSING_ERR:
        console.log("Server side error")
        break
      case fioErrCode.UNAUTHORIZED:
        console.log("Your application is not allowed to perform the requested operation (eg. Invalid ID, Blocked, Paused, etc.). Refer to the FACEIO Console for additional information")
        break
      case fioErrCode.TERMS_NOT_ACCEPTED:
        console.log("Terms & Conditions set out by FACEIO/host application rejected by the end user")
        break
      case fioErrCode.UI_NOT_READY:
        console.log("The FACEIO Widget code could not be (or is being) injected onto the client DOM")
        break
      case fioErrCode.SESSION_EXPIRED:
        console.log("Client session expired. The first promise was already fulfilled but the host application failed to act accordingly")
        break
      case fioErrCode.TIMEOUT:
        console.log("Ongoing operation timed out (eg, Camera access permission, ToS accept delay, Face not yet detected, Server Reply, etc.)")
        break
      case fioErrCode.TOO_MANY_REQUESTS:
        console.log("Widget instantiation requests exceeded for freemium applications. Does not apply for upgraded applications")
        break
      case fioErrCode.EMPTY_ORIGIN:
        console.log("Origin or Referer HTTP request header is empty or missing")
        break
      case fioErrCode.FORBIDDDEN_ORIGIN:
        console.log("Domain origin is forbidden from instantiating fio.js")
        break
      case fioErrCode.FORBIDDDEN_COUNTRY:
        console.log("Country ISO-3166-1 Code is forbidden from instantiating fio.js")
        break
      case fioErrCode.SESSION_IN_PROGRESS:
        console.log("Another authentication or enrollment session is in progress")
        break
      case fioErrCode.NETWORK_IO:
      default:
        console.log("Error while establishing network connection with the target FACEIO processing node")
        break
    }
  }

  useEffect(() => {
    const script = document.createElement('script')
    script.src = '//cdn.faceio.net/fio.js'
    script.async = true
    script.onload = () => loaded()
    document.body.appendChild(script)
    return () => {
      document.body.removeChild(script)
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  const loaded = () => {
    if (faceIO && !faceioInstance) {
      faceioInstance = new faceIO('fioa8957')// Public ID
    }
  }

  
  //Function SignIn using FACE IO
  const faceSignIn = async () => {
    try {
      console.log(faceioInstance)
      const userData = await faceioInstance.authenticate({
        locale: "auto",
      })
      if(userData != null){
        localStorage.setItem("customer", JSON.stringify(userData.payload.userId));
        window.location.href = "/*";  
        toast.success('Đăng nhập thành công', { autoClose: 3000 });

      }
      else
      {
        toast.warning('Chưa đăng ký gương mặt !', { autoClose: 3000 });
      }
      dispatch({
        type: actionType.SET_LINKED,
        linked: !linked,
      });
    } catch (errorCode) {
      handleError(errorCode)
    }
  }

  function onCaptchVerify() {
    if (!window.recaptchaVerifier) {
      window.recaptchaVerifier = new RecaptchaVerifier(
        "recaptcha-container",
        {
          size: "invisible",
          callback: (response) => {
            console.log(response);
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
          console.log(confirmationResult);
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
        toast.success('Đăng nhập thành công', { autoClose: 3000 });
        localStorage.setItem("phone", JSON.stringify(ph));
        // UPdate token again when login succes
        let reponse = await UpdateToken({
          CustomerId: res.user.uid,
          TokenWeb: token,
        })
        if(reponse.success){
          console.log("Update Successfull");
        }

        dispatch({
          type: actionType.SET_LINKED,
          linked: !linked,
        });
        navigate("/*");
        setLoading(false);
      })
      .catch((err) => {
        setLoading(false);
        toast.warning('Vui lòng nhập đúng mã OTP', { autoClose: 3000 });
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


   // Sign in with google
   const auth = getAuth();
   function signInGoogle() {
     signInWithPopup(auth, provider)
     .then(async (result) => {
       // This gives you a Google Access Token. You can use it to access the Google API.
       const credential = GoogleAuthProvider.credentialFromResult(result);
       const token = credential.accessToken;
       // The signed-in user info.
       const user = result.user;
       console.log(user.email)
       dispatch({
         type: actionType.SET_EMAIL,
         email: user.email,
       });
       localStorage.setItem("email", JSON.stringify(user.email));
          // Check email customer arealy in DB
          let reponse = await CheckCustomerEmail({
           Email: user.email,
         })
         if(reponse.success){
          console.log(result)
          window.location.href = "/*";
           localStorage.setItem("customer", JSON.stringify(reponse.data[0].CustomerId));
         }
         else
         {
          window.location.href = "/*";
         }

     }).catch((error) => {
       // Handle Errors here.
       const errorCode = error.code;
       const errorMessage = error.message;
       // The email of the user's account used.
       const email = error.customData.email;
       // The AuthCredential type that was used.
       const credential = GoogleAuthProvider.credentialFromError(error);
       // ...
     });
   }

  return (
    <section className="bg-gray-100 flex items-center justify-center h-screen">
      <div>
      <ToastContainer />
        <div id="recaptcha-container"></div>
        {user != null ? (
          <h2 className="text-center text-white font-medium text-2xl">
            👍Đăng nhập thành công ...
          </h2>
        ) : (
          <div className="w-120 flex flex-col gap-4 rounded-lg p-4">
            <h1 className="font-bold text-center leading-normal text-stone-600 text-3xl mb-6">
              {/* CHÀO MỪNG ĐẾN <br /> EATINGNOW */}
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
                  Nhập số điện thoại
                </label>
                <PhoneInput country={"vn"} value={ph} onChange={setPh} />
                <button
                  onClick={onSignup}
                  className="bg-orange-600 w-full flex gap-1 items-center justify-center py-2.5 text-white rounded"
                >
                  {loading && (
                    <CgSpinner size={20} className="mt-1 animate-spin" />
                  )}
                  <span>Gửi mã OTP</span>
                </button>
            <div className="text-center flex justify-center items-center space-x-4">
                <p>Hoặc:</p>
           </div>
           <div className="text-center flex justify-center items-center space-x-4">
            {/* Đăng nhập với FB
                  <button
                  className="p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-300 rounded-lg bg-opacity-50"
                  onClick={() => {}}
                >
                  <TbBrandFacebook className="text-2xl" /> Facebook
                </button>
                  */  }
            
              {
                      /* Đăng nhập bằng google
                <button
                className="p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-300 rounded-lg bg-opacity-50"
                onClick={() => {
                  signInGoogle()
                }}
              >
                <TbBrandGoogle className="text-2xl" /> Google
              </button>*/
              }
       
              <button
                className="p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-600 rounded-lg bg-opacity-50"
                onClick={() => {
                  handChangeLogin()
                }}
              >
                <TbLogin className="text-2xl" /> Đăng nhập bằng tài khoản
              </button>
              <button
                className="p-1.5 text-xs font-medium uppercase tracking-wider text-blue-800 bg-orange-600 rounded-lg bg-opacity-50"
                onClick={() => {
                  faceSignIn()
                }}
              >
                <TbFaceId className="text-2xl" /> Khuôn mặt
              </button>
            </div>




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
  
        </Pane>
      </Portal>
    </section>
  );
};

export default OTPAuthen;
