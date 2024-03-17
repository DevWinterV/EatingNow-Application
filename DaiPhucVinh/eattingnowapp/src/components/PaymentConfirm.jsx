import React, { useEffect } from 'react';
import { useLocation } from 'react-router-dom';

const PaymentConfirm = () => {
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const vnp_Amount = searchParams.get('vnp_Amount');
  const vnp_BankCode = searchParams.get('vnp_BankCode');
  const vnp_BankTranNo = searchParams.get('vnp_BankTranNo');
  const vnp_CardType = searchParams.get('vnp_CardType');
  const vnp_OrderInfo = searchParams.get('vnp_OrderInfo');
  const vnp_PayDate = searchParams.get('vnp_PayDate');
  const vnp_ResponseCode = searchParams.get('vnp_ResponseCode');
  const vnp_TmnCode = searchParams.get('vnp_TmnCode');
  const vnp_TransactionNo = searchParams.get('vnp_TransactionNo');
  const vnp_TransactionStatus = searchParams.get('vnp_TransactionStatus');
  const vnp_TxnRef = searchParams.get('vnp_TxnRef');
  const vnp_SecureHash = searchParams.get('vnp_SecureHash');

  useEffect(() => {
    // Xử lý các thông tin từ URL ở đây
    console.log('vnp_Amount:', vnp_Amount);
    console.log('vnp_BankCode:', vnp_BankCode);
    // ... và tiếp tục với các thông tin khác
  }, [
    vnp_Amount,
    vnp_BankCode,
    vnp_BankTranNo,
    vnp_CardType,
    vnp_OrderInfo,
    vnp_PayDate,
    vnp_ResponseCode,
    vnp_TmnCode,
    vnp_TransactionNo,
    vnp_TransactionStatus,
    vnp_TxnRef,
    vnp_SecureHash
  ]);

  return (
    <div>
      {/* Nội dung của component */}
      <h1>Payment Confirmation</h1>
      <p>Amount: {vnp_Amount}</p>
      <p>Bank Code: {vnp_BankCode}</p>
      {/* ... và các thông tin khác */}
    </div>
  );
};

export default PaymentConfirm;
