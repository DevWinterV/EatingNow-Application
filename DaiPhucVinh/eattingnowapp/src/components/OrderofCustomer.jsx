import React, { useState, useEffect } from "react";
import { TakeOrderByCustomer } from "../api/customer/customerService";
import { useStateValue } from "../context/StateProvider";
import Loader from "./Loader";
import Modal from 'react-modal';
import { TbCircleX} from "react-icons/tb";
import { GetListOrderLineDetails } from "../api/store/storeService";
import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table";
export default function OrderofCustomer({id, onDelete}) {
  const [Id, setId] = useState('');
  console.log(id);
  useEffect(()=>{
    setId(id);
  },[id]
  )
  let subtitle;
  //Mặc định là tất cả
  const [activeTab, setActiveTab] = useState("all");
  const [dataOrderdetail, setdataOrderdetail] = useState([]);
  const [modalIsOpen, setIsOpen] = React.useState(false);
  const [orderHeaderId, setOrderHeaderId] = React.useState(Id);
  console.log(orderHeaderId);
  const [Orderheader, setOrderheader] = useState({
    Id: "",
    Date: "",
    TotalMoney: 0,
    Ship: 0,
    OrderMoney: 0,
    StoreName: "",
    Status: false
  });
  const customStyles = {
    content: {
      top: '50%',
      left: '50%',
      right: 'auto',
      bottom: 'auto',
      marginRight: '-50%',
      width: '80%', // Để bề rộng bằng với chiều rộng của thiết bị
      transform: 'translate(-50%, -50%)',
      maxHeight: '80vh',
      overflow: 'auto',
    },
  }; 
  const [address, setAddress] = useState({
    RecipientName: "",
    RecipientPhone: "",
    FormatAddress: "",
    NameAddress: "",
  })
  const [isLoading, setIsLoading] = React.useState(false);
  const [countWaiting, setcountWaiting] = useState(0);
  const [{  customer }, dispatch] = useStateValue();
  const [isloading, setIsloading] = useState(false);
  const handleTabClick = (tab) => {
    setActiveTab(tab);
    if(tab ==="all"){
        setRequest({
            ...request,
            OrderType: 0,
            Status: "",
        });
    }
    else if(tab ==="waiting-confirmation"){
        setRequest({
            ...request,
            OrderType: 1,
            Status: false,
        });
    }
    else if(tab ==="confirmed"){
        setRequest({
            ...request,
            OrderType: 1,            
            Status: true,
        });
    }
  };
  const [request, setRequest] =useState({
    CustomerId: customer,
    OrderType: "",
    Status: "",
  });
  const [data, setData] =useState([]);

  async function getOrder() {
    setIsloading(true);
    let response = await TakeOrderByCustomer(request);
    if (response.success) {
      setData(response.data);
      setcountWaiting(response.data.length);
    }
    setIsloading(false);
  }
  

  useEffect(() =>{
    getOrder();
  },[request])
  useEffect(()=>{
    if (Id != null && Id !== 'all') {
      var i = data.find(item => item.OrderHeaderId === Id);
      console.log(i);
      if (i) {
        setOrderHeaderId(i.OrderHeaderId);  
        setOrderheader({
          Id: i.OrderHeaderId,
          Date: formatDate(i.CreationDate),
          TotalMoney: formatMoney(i.IntoMoney),
          Ship: formatMoney(i.TransportFee),
          OrderMoney: formatMoney(i.TotalAmt),
          StoreName: i.StoreName,
          Status: i.Status
        });
        setAddress({
          RecipientName: i.RecipientName,
          RecipientPhone: i.RecipientPhone,
          FormatAddress: i.FormatAddress,
          NameAddress: i.NameAddress
        });
        setIsOpen(!false);
      }
    }
  },[data])
  function afterOpenModal() {
    // references are now sync'd and can be accessed.
    subtitle.style.color = '#f00';
  }
  function closeModal() {
    setdataOrderdetail(null);
    setIsOpen(false);
    onDelete();
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
  function formatMoney(amount) {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
      minimumFractionDigits: 0,
    }).format(amount);
  }
  async function GetOrderLineDetails() {
    setIsLoading(true);
    if (orderHeaderId != "") {
      let response = await GetListOrderLineDetails(orderHeaderId);
      setdataOrderdetail(response.data);
      setIsLoading(false);
    }
  }
  useEffect(() => {
    getOrder();
    GetOrderLineDetails();
  }, [orderHeaderId]);


  
  return (
    <div>
      <Modal
                isOpen={modalIsOpen}
                onAfterOpen={afterOpenModal}
                onRequestClose={closeModal}
                style={customStyles}
                contentLabel="Form"
                shouldCloseOnOverlayClick={false} // Đặt giá trị này thành false để ngăn Modal đóng khi nhấn ở ngoài
              >
                  <button
                    type="submit"
                    className="px-2 py-1 bg-red-500 text-white rounded-md hover:bg-red-600 mt-3"
                    onClick={closeModal}
                    style={{
                      position: 'absolute',
                      top: '10px', // Điều chỉnh khoảng cách từ trên xuống
                      right: '10px', // Điều chỉnh khoảng cách từ phải sang
                    }}
                  >
                  <TbCircleX className="text-2xl" />
                  </button>
                 <div className="row" style={{ display: "flex", alignItems: "center" }}>
                  <h2 className="text-2xl font-semibold">
                    Chi tiết đơn hàng của bạn
                  </h2>
                </div>
                <div className="container mt-2 mb-2">
                  <div className="row ">
                    <div className="col-md-6">
                            <div className="order-details"><div className="order-status">
                            {
                                !Orderheader.Status ? (
                                  <h1 className="order-info">Đang chờ xác nhận</h1>
                                ) : (
                                  <h1 className="order-info ">Đã xác nhận</h1>

                                )
                              }
                            </div>
                            <h1 className="order-info">Mã đơn hàng: {Orderheader.Id}</h1>
                            <h1 className="order-info">Đặt lúc: {Orderheader.Date}</h1>
                            <h1 className="order-info">{Orderheader.StoreName}</h1>
                            <div className="address-container">
                              <h1 className="address-title">Địa chỉ nhận hàng:</h1>
                              <div className="recipient-info">
                                <h6 className="recipient-detail">
                                  <span className="recipient-label">Tên người nhận:</span>
                                  <span className="recipient-value">{address.RecipientName} | {address.RecipientPhone}</span>
                                </h6>
                                <h6 className="recipient-detail">
                                  <span className="recipient-label">Địa chỉ:</span>
                                  <span className="recipient-value">{address.NameAddress} | {address.FormatAddress}</span>
                                </h6>
                              </div>
                            </div>
                      </div>
                       
                    </div>
                  </div>
                </div>
                    <div className="w-full table-auto">
                        <div>
                          <Table className="w-full table-auto">
                            <Thead className="bg-orange-50">
                              <Tr>
                                <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                                  STT
                                </Th>
                                <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                                  Hình ảnh
                                </Th>
                                <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                                  Tên món
                                </Th>
                                {/* <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                                  Loại món
                                </Th> */}
                                <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                                  Số lượng
                                </Th>
                                <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                                  Giá bán
                                </Th>
                                <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                                  Mô tả
                                </Th>
                                <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                                  Tổng tiền
                                </Th>
                              </Tr>
                            </Thead>
                            <Tbody className="bg-orange-50">
                              {isLoading ? (
                                <Td colspan="4" className="text-center">
                                  <Loader />
                                </Td>
                              ) : dataOrderdetail && dataOrderdetail.length > 0 ? (
                                dataOrderdetail.map((item, index) => (
                                  <Tr
                                    key={item.OrderLineId}
                                    className="bg-orange-50"
                                  >
                                    <Td className="p-3 text-sm text-orange-900 whitespace-nowrap text-center">
                                      <p className="font-bold text-orange-900 hover:underline">
                                        {index + 1}
                                      </p>
                                    </Td>
                                    <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center flex justify-center">
                                      <img
                                        src={item?.UploadImage}
                                        className="w-20 h-20 max-w-[60px] rounded-full object-contain"
                                        alt=""
                                      />
                                    </Td>
                                    <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                      {item.FoodName}
                                    </Td>
                                    {/* <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                      {item.CategoryId}
                                    </Td> */}
                                    <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                      {item.qty}
                                    </Td>
                                    <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                      {formatMoney(item.Price)}
                                    </Td>
                                    <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                      {item.Description ? (
                                        <p>{item.Description}</p>
                                      ):(<p>Không có</p>)}
                                    </Td>
                                    <Td className="capitalize p-3 text-sm font-bold text-orange-900 whitespace-nowrap text-center">
                                      {formatMoney(item.TotalPrice)}
                                    </Td>
                                    </Tr>
                                ))
                              ) : (
                                ""
                              )}
                             
                            </Tbody>
                          </Table>
                          <Table className="w-full table-auto">
                          <Thead className="bg-orange-50">
                            <Tr>
                              <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              </Th>
                              <Th className="p-3 text-orange-900 text-sm font-bold tracking-wide text-center">
                              </Th>
                            </Tr>
                          </Thead>
                          <Tbody className="bg-orange-50">
                            <>
                              {/* Sản phẩm 1 */}
                              <Tr className="bg-orange-50">
                                <Td className="p-3 text-sm font-bold text-orange-900 text-center">
                                  Tổng tiền món ăn
                                </Td>
                                <Td className="p-3 text-sm font-bold text-orange-900 text-center">
                                  {Orderheader.OrderMoney}
                                </Td>
                              </Tr>

                              {/* Sản phẩm 2 */}
                              <Tr className="bg-orange-50">
                              <Td className="p-3 text-sm font-bold text-orange-900 text-center">
                                  Phí vận chuyển
                                </Td>
                                <Td className="p-3 text-sm font-bold text-orange-900 text-center">
                                  {Orderheader.Ship}
                                </Td>
                              </Tr>

                              {/* Tổng tiền sản phẩm */}
                              <Tr className="bg-orange-50">
                                <Td className="p-3 text-sm font-bold text-orange-900 text-center">
                                  Tổng thanh toán
                                </Td>
                                <Td className="p-3 text-sm font-bold text-orange-900 text-center">
                                  {Orderheader.TotalMoney}
                                </Td>
                              </Tr>
                            </>
                          </Tbody>
                        </Table>
                         {/*footer*/}
                          <div className="flex items-center justify-end p-6 border-t border-solid border-slate-200 rounded-b">
                          <button
                          className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 mt-2 ml-2"
                          type="button"
                            onClick={() => {
                              closeModal();
                            }}
                          >
                            Đóng
                          </button>
                          </div>  
                          </div>        
                    </div>

      </Modal>
     <h1 className="text-xl mb-2 text-red-700 font-bold">     
         Đơn hàng của tôi
     </h1>    
     <nav className="nav">
     <ul>
      <li className={activeTab === "all" ? 'active-tab' : ''}>
        <button onClick={() => handleTabClick("all")}>
          Tất cả
        </button>
      </li>
      <li className={activeTab === "waiting-confirmation" ? 'active-tab' : ''}>
        <button onClick={() => handleTabClick("waiting-confirmation")}>
          Đang chờ xác nhận
        </button>
      </li>
      <li style={{ marginTop: '-4px' }}>
        {activeTab === "waiting-confirmation" && (
          <h5 className="text-0xl mb-1 text-red-600 ">
            {countWaiting}
          </h5>
        )}
      </li>
      <li className={activeTab === "confirmed" ? 'active-tab' : ''}>
        <button onClick={() => handleTabClick("confirmed")}>
          Đã xác nhận
        </button>
      </li>
      <li className={activeTab === "delivering" ? 'active-tab' : ''}>
        <button onClick={() => handleTabClick("delivering")}>
          Đang giao hàng
        </button>
      </li>
      <li className={activeTab === "complete" ? 'active-tab' : ''}>
        <button onClick={() => handleTabClick("complete")}>
          Hoàn thành
        </button>
      </li>
    </ul>
    </nav>

    {isloading ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
            <div>
            {data.length > 0 ? (
              activeTab === "waiting-confirmation" ? (
                <All />
              ) : activeTab === "confirmed" ? (
                <All />
              ) : activeTab === "delivering" ? (
                <Delivering />
              ) : activeTab === "all" ? (
                <All />
              ) : activeTab === "complete" ? (
                <Complete />
              ) : null
            ) : (
              <>
                <h1 className="text-center">Không có đơn hàng</h1>
              </>
            )}
          </div>          
        )
        }

    </div>
  );

function Delivering() {
  return <h2>Đang giao hàng</h2>;
}
function All() {
    return (
        <div>
        <ul className="bg-pink p-1 m-0">
        { data &&
              data.map((i, index) => (
                // Check if address is not null before rendering
                i !== null && (
                  <li key={index} className="mb-4 border p-2 rounded-lg border-gray">
                    <h6 className="order-info text-base text-gray-700 font-normal">
                      Mã đơn hàng: {i.OrderHeaderId}
                    </h6>
                    <h6 className="text-base text-gray-700 font-normal">
                      Đặt lúc: {formatDate(i.CreationDate)}
                    </h6>
                    <h6 className="text-base text-gray-700 font-normal">
                      Tổng tiền: {formatMoney(i.IntoMoney)}
                    </h6>
                        <div className="row">
                        <button
                        onClick={() => {
                            setOrderHeaderId(i.OrderHeaderId);  
                            setOrderheader({
                              Id: i.OrderHeaderId,
                              Date: formatDate(i.CreationDate),
                              TotalMoney: formatMoney(i.IntoMoney),
                              Ship: formatMoney(i.TransportFee),
                              OrderMoney: formatMoney(i.TotalAmt),
                              StoreName: i.StoreName,
                              Status: i.Status
                            });
                            setAddress({
                              RecipientName: i.RecipientName,
                              RecipientPhone: i.RecipientPhone,
                              FormatAddress: i.FormatAddress,
                              NameAddress: i.NameAddress
                            });
                            setIsOpen(true);
                            }}
                            className="btn btn-primary btn-sm"
                            style={{ border: "1px solid grey", backgroundColor: "white" }}
                            >
                            <p className="text-sm text-red-500 p-1">Xem chi tiết
                            </p>                   
                        </button>
                        {
                            !i.Status ?(
                                <button
                                onClick={() => {
                                
                                    }}
                                    className="btn btn-primary btn-sm ml-2"
                                    style={{ border: "1px solid grey", backgroundColor: "white" }}
                                    >
                                    <p className="text-sm text-red-500 p-1 ">Hủy đơn hàng
                                    </p>                   
                                </button>
                            ):(
                                null
                            )
                        }
                        </div>
                      
                        
                  </li>
                )
              ))
            }
            </ul>
        </div>
    )
}
function Complete() {
    return <h2>Hoàn thành</h2>;
}
}