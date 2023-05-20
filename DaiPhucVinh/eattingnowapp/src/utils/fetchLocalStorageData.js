export const fetchUser = () => {
  const userInfo =
    localStorage.getItem("user") !== "undefined"
      ? JSON.parse(localStorage.getItem("user"))
      : localStorage.clear();

  return userInfo;
};

export const fetchCart = () => {
  const cartInfo =
    localStorage.getItem("cartItems") !== "undefined"
      ? JSON.parse(localStorage.getItem("cartItems"))
      : localStorage.clear();

  return cartInfo ? cartInfo : [];
};

export const fetchCustomer = () => {
  const customerInfo =
    localStorage.getItem("customer") !== "undefined"
      ? JSON.parse(localStorage.getItem("customer"))
      : localStorage.clear();

  return customerInfo ? customerInfo : null;
};

export const fetchCartStore = () => {
  const cartStoreInfo =
    localStorage.getItem("cartStore") !== "undefined"
      ? JSON.parse(localStorage.getItem("cartStore"))
      : localStorage.clear();

  return cartStoreInfo ? cartStoreInfo : 0;
};
