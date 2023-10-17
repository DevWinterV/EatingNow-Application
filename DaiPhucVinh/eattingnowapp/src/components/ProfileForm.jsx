import React, { useState, useEffect  } from 'react';
import Account from './Account'; // Import component Tài khoản
import Setting from './Setting'; // Import component Cài đặt
import BankAccountForm from './BankAccountForm';
import { useStateValue } from "../context/StateProvider";
import { Button } from 'evergreen-ui';
import { Link, useNavigate } from "react-router-dom";
import { actionType } from '../context/reducer';
import CustomerAddressContainer from "./CustomerAddressContainer"
import OrderofCustomer from './OrderofCustomer';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import CartContainer from './CartContainer';
// Example in a component using CSS modules
function ProfileForm() {
  const [selectedTab, setSelectedTab] = useState('account'); // Mặc định hiển thị trang Tài khoản
  const [{linked, customer , cartShow}, dispatch] =
    useStateValue();  const navigate = useNavigate();

  const handleLogin = () => {
    dispatch({
      type: actionType.SET_LINKED,
      linked: !linked,
    });
    <Link to={"/otpauthen"}></Link>;
  };

  const handleLogout = () => {
    dispatch({
      type: actionType.SET_CUSTOMER,
      customer: null,
    });
    localStorage.setItem("customer", JSON.stringify(null));

  };
  const renderContent = () => {
    if(customer){
      if (selectedTab === 'account') {
        return <Account />;
      } else if (selectedTab === 'setting') {
        return <Setting />;
      }else if (selectedTab === 'banking') {
        return <BankAccountForm />;
      }
      else if (selectedTab === 'address') {
        return <CustomerAddressContainer/>
         }
         else if(selectedTab === 'order'){
          return <OrderofCustomer/>
         }
        }
  }
  return (
    <div className="bg-orange-50 min-h-screen p-5 overflow-y-auto scrollbar py-2 px-2">
      <ToastContainer />
      {cartShow && <CartContainer />}
      {/* Menu */}
      {
        customer ?(
          <div className="flex">
          <div className="w-1/4 bg-gray-200 p-5">
            <ul>
              <li
                className={`cursor-pointer py-2 px-4 mb-2 rounded-lg ${selectedTab === 'account' ? 'bg-blue-600 text-white' : 'text-blue-600'}`}
                onClick={() => setSelectedTab('account')}
              >
                Thông tin
              </li>
              <li
                className={`cursor-pointer py-2 px-4 mb-2 rounded-lg ${selectedTab === 'order' ? 'bg-blue-600 text-white' : 'text-blue-600'}`}
                onClick={() => setSelectedTab('order')}
              >
                Đơn hàng
              </li>
              <li
                className={`cursor-pointer py-2 px-4 mb-2 rounded-lg ${selectedTab === 'address' ? 'bg-blue-600 text-white' : 'text-blue-600'}`}
                onClick={() => setSelectedTab('address')}
              >
                Địa chỉ
              </li>
              <li
                className={`cursor-pointer py-2 px-4 mb-2 rounded-lg ${selectedTab === 'banking' ? 'bg-blue-600 text-white' : 'text-blue-600'}`}
                onClick={() => setSelectedTab('banking')}
              >
                Tài khoản ngân hàng
              </li>
              {
                /*
              <li
              className={`cursor-pointer py-2 px-4 mb-2 rounded-lg ${selectedTab === 'changepassword' ? 'bg-blue-600 text-white' : 'text-blue-600'}`}
              onClick={() => setSelectedTab('changepassword')}
              >
                Đổi mật khẩu
              </li>
              */
              }
           
              <li
                className={`cursor-pointer py-2 px-4 mb-2 rounded-lg ${selectedTab === 'setting' ? 'bg-blue-600 text-white' : 'text-blue-600'}`}
                onClick={() => setSelectedTab('setting')}
              >
                Cài đặt
              </li>
              {
                customer ?(
                  <li
                  className={`cursor-pointer py-2 px-4 mb-2 rounded-lg ${selectedTab === 'login' ? 'bg-blue-600 text-white' : 'text-blue-600'}`}
                  onClick={() => handleLogout()}
                >
                  Đăng xuất
                </li>
                ):
                 null
              }
            </ul>
          </div>
          {/* Hiển thị nội dung tương ứng với menu được chọn */}
          <div className="w-3/4 bg-white p-5">
            {renderContent()}
          </div>
        </div>
        ):(
          <div className="bg-white min-h-screen p-5 overflow-y-auto scrollbar py-2 px-2">
        <div className="title">
        <div className="flex justify-center pb-4 items-center ">
                            <Button
                            className='bg-blue'
                            onClick={handleLogin}
                            >
                              Vui lòng đăng nhập
                            </Button>
                          </div>
                        </div>      
          </div>
        )
      }
   
    </div>
  );
  
}

export default ProfileForm;
