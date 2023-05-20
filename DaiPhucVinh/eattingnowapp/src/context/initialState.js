import {
  fetchCustomer,
  fetchCart,
  fetchUser,
  fetchCartStore,
} from "../utils/fetchLocalStorageData";

const userInfo = fetchUser();
const cartInfo = fetchCart();
const customerInfo = fetchCustomer();
const cartStoreInfo = fetchCartStore();

export const initialState = {
  user: userInfo,
  foodItems: null,
  location: null,
  cartShow: false,
  cartItems: cartInfo,
  auth: false,
  linked: true,
  customer: customerInfo,
  cartStore: cartStoreInfo,
};
