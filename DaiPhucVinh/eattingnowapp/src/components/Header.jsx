import React, { useState ,useEffect} from "react";
import { Logo, Avatar } from "../assets";
import { MdShoppingBasket, MdAdd, MdLogout, MdLogin, MdAccountCircle, MdSettings } from "react-icons/md";
import { motion } from "framer-motion";
import { Link } from "react-router-dom";
import { useStateValue } from "../context/StateProvider";
import { actionType } from "../context/reducer";
import { useNavigate } from 'react-router-dom';
import { GetAllNotificationCustomer, SetIsReadAllNotification } from "../api/customer/customerService";
import $ from 'jquery'; // Ensure you have jQuery installed
window.jQuery = $;
require('signalr'); // Ensure you have the SignalR library installed

const Header = () => {
      //Kết nối đến SignalR Ordernotication
  let connection = $.hubConnection('http://localhost:3002/signalr/hubs');
  let proxy = connection.createHubProxy('OrderNotificationHub');
  const [isMenu, setIsMenu] = useState(false);
  const Login = () => {
    setIsMenu(!isMenu);
  };
  const navigate = useNavigate();

  const [dataNotifi, setdataNotifi] = useState([]);
  const [countnotification, setcountnotification] = useState(0);
  const [showNotifications , setshowNotifications] =useState(false);
  const [notification, setNotification]= useState({
          icon: '🔔',
          title: 'New Notification',
          content: 'This is a sample notification.',
          timestamp: 'Just now',
  });
  const [{ cartShow, cartItems, linked, user, customer }, dispatch] =
    useStateValue();
  const ShowCart = () => {
    dispatch({
      type: actionType.SET_CART_SHOW,
      cartShow: !cartShow,
    });
  };
  const [request, setRequest] = useState({
    CustomerID: customer,
    DefautLineNoifi: true,
  });
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
    setcountnotification(0);
    setdataNotifi([]);
    dispatch({
      type: actionType.SET_CUSTOMER,
      customer: null,
    });
    localStorage.setItem("customer", JSON.stringify(null));
  };
  function handleBellClick() {
    // Toggle the display of notifications
    setshowNotifications(!showNotifications);
  };

  const handleItemClick = (link) => {
    setshowNotifications(false);
    // Điều hướng đến trang "/order/id" với id cụ thể
    navigate(link);
  };
  
  const handleClickAccount = () => {
    // Điều hướng đến trang "/order/id" với id cụ thể
    navigate(`/account`);
  };
  const   handleViewAllNotification= () => {
    // Điều hướng đến trang "/order/id" với id cụ thể
    navigate(`/order/all`);
  };


  // Xóa thông tin người dùng khi tát trình duyệt
  window.addEventListener('beforeunload', function (event) {
    // Thực hiện các thao tác trước khi trình duyệt đóng
    // Ví dụ: Gọi phương thức xóa UserConnection trên SignalR Hub
    if(customer != null){
      proxy.invoke('RemoveUserConnection', customer)
      .done(() => {
          console.log('Đã xóa UserConnection thành công trước khi trình duyệt đóng');
      })
      .fail((error) => {
          console.error('Lỗi xóa UserConnection:', error);
      });
    }
  });

  async function GetAllNotification() {
      let response = await GetAllNotificationCustomer(request);
      setdataNotifi(response.data);
      setcountnotification(response.dataCount)
  }

  function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = String(date.getFullYear());
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    return `${day}/${month}/${year} ${hours}:${minutes}`;
  }
  useEffect(() => {
    // Set up a client method to receive order notifications
    proxy.on('ReceiveOrderNotification', (orderMessage) => {
      GetAllNotification();
    });
    // Set up a client method to receive user-specific order notifications
    proxy.on('ReceiveOrderNotificationOfUser', (orderMessage) => {
      GetAllNotification();
      
    });
  
    // Attempt connection and handle connection and error events
    connection.start()
      .done(() => {
        console.log('Kết nối thành công SignalR');
        // Đăng ký người dùng khi kết nối thành công
        if (customer !== null) {
          proxy.invoke('SetCustomerId', customer)
            .done(() => {
              console.log('Đăng ký người dùng thành công');
            })
            .fail((error) => {
              console.error('Lỗi đăng ký người dùng:', error);
            });            
        }
      })
      .fail((error) => {
        console.error('Could not connect:', error);
      });
  
    // Log the connection status
    connection.stateChanged((change) => {
      console.log('Connection state changed:', change.newState);
    });
  }, []);
  useEffect(() =>{
    GetAllNotification();
  },[request]);
  
  
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

          {/* THÔNG BÁO*/}
          <div className="relative">
            <span
              className="bell-icon"
              onClick={handleBellClick}
              style={{ position: 'relative', display: 'inline-block' , fontSize: '24px' , cursor: 'pointer'}}
            >
              🔔
              {countnotification >0 ?(
                  <span
                  className="notification-count"
                  style={{
                    fontWeight:'bold',
                    position: 'absolute',
                    top: '-6px', // Adjust the vertical position as needed
                    right: '-3px', // Adjust the horizontal position as needed
                    fontSize: '16px', // Adjust the font size as needed
                    color: 'red', // Adjust the color as needed
                  }}
                  >
                    {countnotification}
                  </span>
              ):(
                null
              )}
            </span>
            {showNotifications && (
              <motion.div
                initial={{ opacity: 0, scale: 0.6 }}
                animate={{ opacity: 1, scale: 1 }}
                exit={{ opacity: 0, scale: 0.6 }}
                className="w-70  bg-gray-50 shadow-xl rounded-lg flex flex-col absolute top-12 right-0"
                >
                {/* <p className="px-4 py-2 flex items-center gap-3 cursor-pointer hover:bg-slate-100 transition-all duration-100 ease-in-out text-textColor text-base justify-between">
                  Thông tin <MdAdd />
                </p> */}
                <div>
              {showNotifications && (
              <ul>
                {
                  countnotification > 0 ?(
                    <h3 className="notification-header">{countnotification} thông báo mới</h3>

                  ):(
                    <h3 className="notification-header">Thông báo</h3>

                  )
                }   
                  <h3 
                    onClick={async () => {
                      try {
                        const response = await SetIsReadAllNotification({
                          CustomerID: customer
                        });
                        // Xử lý response ở đây
                        if (response.success) {
                          GetAllNotification()
                        } else {
                          console.log("Xóa không thành công!")
                        }
                      } catch (error) {
                        console.log(error);
                      }
                    }}
                    className="text-deleteAll"
                    >Đánh dấu đọc tất cả</h3>    
                <ul class="notification-ul overflow-auto">
                {dataNotifi &&  (
                    dataNotifi.map((item, index) => (
                      <li key={index} className="notification-item-horizontal"
                      onClick={() => handleItemClick(item.Action_Link)} // Gọi hàm khi item được click
                      style={{ backgroundColor: item.IsRead ? 'lightgreen' : 'initial' }}
                      >
                        <motion.div
                        whileTap={{ scale: 0.9 }}
                        className="notification-list"
                        > 
                          <span className="notification-icon">🔔</span>
                          <div className="notification-content">
                            <h4>{item.Message}</h4>
                            <p>{item.SenderName}</p>
                          </div>
                          <span className="notification-timestamp">
                            {formatDate(item.NotificationDate)}
                          </span>
                        </motion.div>
                      </li>
                    ))
                    )}
                 {
                  dataNotifi === null  &&(
                    <li className="notification-item-horizontal">
                      <motion.div
                        whileTap={{ scale: 0.9 }}
                        className="notification-list"
                        > 
                          <span className="notification-icon">🔔</span>
                          <div className="notification-content">
                            <h4>Chưa có thông báo</h4>
                            <p>HỆ THỐNG</p>
                          </div>
                          <span className="notification-timestamp">
                          </span>
                        </motion.div>
                    </li>
                  )
                }
                  <li className="notification-item-horizontal">
                          <div class="notification-link"
                          onClick={handleViewAllNotification}>
                            Xem tất cả
                          </div>
                  </li>
          
                </ul>
              </ul>
                )}
                </div>

              </motion.div>
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
                    
                      <p
                      onClick={handleClickAccount}
                        className="m-2 p-2 rounded-md shadow-md flex items-center justify-center bg-gray-200 gap-3 cursor-pointer hover:bg-slate-300 transition-all duration-100 ease-in-out text-textColor text-base"
                      >
                        Tài khoản <MdAccountCircle />
                      </p>
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
  
        {/** Thông báo mới */}
        
          <div className="relative">
            <span
              className="bell-icon"
              onClick={handleBellClick}
              style={{ position: 'relative', display: 'inline-block' , fontSize: '24px' }}
            >
              🔔
              {countnotification >0 ?(
                  <span
                  className="notification-count"
                  style={{
                    fontWeight:'bold',
                    position: 'absolute',
                    top: '-6px', // Adjust the vertical position as needed
                    right: '-3px', // Adjust the horizontal position as needed
                    fontSize: '16px', // Adjust the font size as needed
                    color: 'red', // Adjust the color as needed
                  }}
                  >
                    {countnotification}
                  </span>
              ):(
                null
              )}
            </span>
            {showNotifications && (
              <motion.div
                initial={{ opacity: 0, scale: 0.6 }}
                animate={{ opacity: 1, scale: 1 }}
                exit={{ opacity: 0, scale: 0.6 }}
                className="w-70  bg-gray-50 shadow-xl rounded-lg flex flex-col absolute top-12 right-0"
                >
                {/* <p className="px-4 py-2 flex items-center gap-3 cursor-pointer hover:bg-slate-100 transition-all duration-100 ease-in-out text-textColor text-base justify-between">
                  Thông tin <MdAdd />
                </p> */}
                <div>
                {showNotifications && (
              <ul>
              {
                countnotification > 0 ?(
                  <h3 className="notification-header">{countnotification} thông báo mới</h3>

                ):(
                  <h3 className="notification-header">Thông báo</h3>

                )
              }   <h3 
              onClick={async () => {
                try {
                  const response = await SetIsReadAllNotification({
                    CustomerID: customer
                  });
                  // Xử lý response ở đây
                  if (response.success) {
                    GetAllNotification()
                  } else {
                    console.log("Xóa không thành công!")
                  }
                } catch (error) {
                  console.log(error);
                }
              }}
              className="text-deleteAll"
              >Đánh dấu đọc tất cả</h3>    
          <ul class="notification-ul overflow-auto">
          {dataNotifi &&  (
              dataNotifi.map((item, index) => (
                <li key={index} className="notification-item-horizontal"
                onClick={() => handleItemClick(item.Action_Link)} // Gọi hàm khi item được click
                style={{ backgroundColor: item.IsRead ? 'lightgreen' : 'initial' }}
                >
                  <motion.div
                  whileTap={{ scale: 0.9 }}
                  className="notification-list"
                  > 
                    <span className="notification-icon">🔔</span>
                    <div className="notification-content">
                      <h4>{item.Message}</h4>
                      <p>{item.SenderName}</p>
                    </div>
                    <span className="notification-timestamp">
                      {formatDate(item.NotificationDate)}
                    </span>
                  </motion.div>
                </li>
              ))
              )}
               {
                dataNotifi === null  &&(
                  <li className="notification-item-horizontal">
                    <motion.div
                      whileTap={{ scale: 0.9 }}
                      className="notification-list"
                      > 
                        <span className="notification-icon">🔔</span>
                        <div className="notification-content">
                          <h4>Chưa có thông báo</h4>
                          <p>HỆ THỐNG</p>
                        </div>
                        <span className="notification-timestamp">
                        </span>
                      </motion.div>
                  </li>
                )
              }
                  <li className="notification-item-horizontal">
                    <a href="/account" rel="noopener noreferrer" class="notification-link">
                      Xem tất cả
                    </a>
                  </li>
         
          </ul>
      </ul>
                )}
                </div>

              </motion.div>
            )}
          </div>

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
                      <p
                      onClick={handleClickAccount}
                      className="m-2 p-2 rounded-md shadow-md flex items-center justify-center bg-gray-200 gap-3 cursor-pointer hover:bg-slate-300 transition-all duration-100 ease-in-out text-textColor text-base">
                        Tài khoản <MdAccountCircle />
                      </p>
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
