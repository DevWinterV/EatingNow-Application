import React, { useState } from 'react';
import Account from './Account'; // Import component Tài khoản
import Setting from './Setting'; // Import component Cài đặt
import BankAccountForm from './BankAccountForm';
import { useStateValue } from "../context/StateProvider";
import { Button } from 'evergreen-ui';
import { Link, useNavigate } from "react-router-dom";
import { actionType } from '../context/reducer';

function ProfileForm() {
  const [selectedTab, setSelectedTab] = useState('account'); // Mặc định hiển thị trang Tài khoản
  const [{linked, customer }, dispatch] =
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
      /*
      else
      {
          return(
            <form onSubmit={handlePasswordChange} className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
            <h1 className="text-2xl mb-4">Đổi mật khẩu</h1>
            <div className="mb-4">
              <label htmlFor="currentPassword" className="block text-gray-700 text-sm font-bold mb-2">
                Mật khẩu hiện tại:
              </label>
              <input
                type="password"
                id="currentPassword"
                name="currentPassword"
                value={passwordData.currentPassword}
                onChange={handlePasswordChange}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              />
            </div>
            <div className="mb-4">
              <label htmlFor="newPassword" className="block text-gray-700 text-sm font-bold mb-2">
                Mật khẩu mới:
              </label>
              <input
                type="password"
                id="newPassword"
                name="newPassword"
                value={passwordData.newPassword}
                onChange={handlePasswordChange}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              />
            </div>
            <div className="mb-6">
              <label htmlFor="confirmPassword" className="block text-gray-700 text-sm font-bold mb-2">
                Xác nhận mật khẩu mới:
              </label>
              <input
                type="password"
                id="confirmPassword"
                name="confirmPassword"
                value={passwordData.confirmPassword}
                onChange={handlePasswordChange}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              />
            </div>
            <div className="text-center">
              <button
                type="submit"
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
              >
                Đổi mật khẩu
              </button>
            </div>
          </form>
  
          )
      }
    */
   }
  };

  const [passwordData, setPasswordData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });

  const handlePasswordChange = (e) => {
    const { name, value } = e.target;
    setPasswordData({
      ...passwordData,
      [name]: value,
    });
  };

  return (
    <div className="bg-orange-50 min-h-screen p-5 overflow-y-auto scrollbar py-2 px-2">
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
                  className={`cursor-pointer py-2 px-4 mb-2 rounded-lg ${selectedTab === 'setting' ? 'bg-blue-600 text-white' : 'text-blue-600'}`}
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
