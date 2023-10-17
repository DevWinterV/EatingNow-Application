import React from "react";
import { Routes, Route } from "react-router-dom";
import HomePage from "./pages/dashboard/HomePage";
import { LoginPage } from "./pages/auth";
import { DetailCustomerPage, ListCustomerPage } from "./pages/customer";
import { MapsPage, MapsDeliverPage} from "./pages/Maps";
import { OrderPage, DeliveryPage, DetailDeliveryPage, DetailOrderPage} from "./pages/order";
import { ListCuisine, CreateCuisine } from "./pages/Cuisine";
import { ListStorePage, DetailStorePage } from "./pages/Store";
import { CreateProvince, ListProvince } from "./pages/Province";
import { CreateDistrict, ListDistrict } from "./pages/District";
import { CreateWard, ListWard } from "./pages/Ward";
import { CreateAccountType, ListAccountType } from "./pages/AccountType";
const Routers = ({ auth }) => {
  return (
    <Routes>
      {!auth ? (
        <>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
        </>
      ) : (
        <>
          <Route path="/" element={<HomePage />} />
          <Route path="/order" element={<OrderPage />} />
          <Route path="/order/detail/:id" element={<DetailOrderPage />} />
          <Route path="/delivery" element={<DeliveryPage />} />
          <Route path="/delivery/create" element={<DetailDeliveryPage />} />
          <Route path="/delivery/edit/:id" element={<DetailDeliveryPage />} />
          <Route path="/maps" element={<MapsPage />} />
          <Route path="/mapsdlv" element={<MapsDeliverPage/>} />
          <Route path="/store" element={<ListStorePage />} />
          <Route path="/store/create" element={<DetailStorePage />} />
          <Route path="/store/detail/:id" element={<DetailStorePage />} />
          <Route path="/customer" element={<ListCustomerPage />} />
          <Route path="/customer/detail/:id" element={<DetailCustomerPage />} />
          <Route path="/cuisine" element={<ListCuisine />} />
          <Route path="/cuisine/create" element={<CreateCuisine />} />
          <Route path="/cuisine/edit/:id" element={<CreateCuisine />} />
          <Route path="/province" element={<ListProvince />} />
          <Route path="/province/create" element={<CreateProvince />} />
          <Route path="/province/edit/:id" element={<CreateProvince />} />
          <Route path="/district" element={<ListDistrict />} />
          <Route path="/district/create" element={<CreateDistrict />} />
          <Route path="/district/edit/:id" element={<CreateDistrict />} />
          <Route path="/ward" element={<ListWard />} />
          <Route path="/ward/create" element={<CreateWard />} />
          <Route path="/ward/edit/:id" element={<CreateWard />} />
          <Route path="/accounttype" element={<ListAccountType />} />
          <Route path="/accounttype/create" element={<CreateAccountType />} />
          <Route path="/accounttype/edit/:id" element={<CreateAccountType />} />
        </>
      )}
    </Routes>
  );
};

export default Routers;
