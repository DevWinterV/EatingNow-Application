import React, { useState } from 'react';

function BankAccountForm() {
  const [formData, setFormData] = useState({
    accountNumber: '',
    accountHolderName: '',
    balance: 0,
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Xử lý gửi thông tin tài khoản ngân hàng lên máy chủ (đã loại bỏ để minh họa)
    console.log('Dữ liệu đã gửi:', formData);
  };

  return (
    <div className="bg-white p-4">
                <div className="title">
                  <div className="flex justify-between pb-4 items-center">
                    <h1 className="text-xl mb-2 text-red-700 font-bold">
                      Tài khoản ngân hàng
                    </h1>
                  </div>

                </div>     
        <div className="text-center">
          <button
            type="submit"
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
          >
           Thêm tài khoản ngân hàng 
          </button>
        </div>
    </div>
  );
}

export default BankAccountForm;
