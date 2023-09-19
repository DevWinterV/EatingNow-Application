export const actionType = {
  SET_USER: "SET_USER",
  SET_FOOD_ITEMS: "SET_FOOD_ITEMS",
  SET_CART_SHOW: "SET_CART_SHOW",
  SET_CARTITEMS: "SET_CARTITEMS",
  SET_LOCATION: "SET_LOCATION",
  SET_AUTH: "SET_AUTH",
  SET_LINKED: "SET_LINKED",
  SET_CUSTOMER: "SET_CUSTOMER",
  SET_CARTSTORE: "SET_CARTSTORE",
  SET_TOKEN: "SET_TOKEN",
  SET_EMAIL: "SET_EMAIL"
};

const reducer = (state, action) => {
   console.log(action);
  switch (action.type) {
    case actionType.SET_USER:
      return {
        ...state,
        user: action.user,
      };

    case actionType.SET_FOOD_ITEMS:
      return {
        ...state,
        foodItems: action.foodItems,
      };

    case actionType.SET_CART_SHOW:
      return {
        ...state,
        cartShow: action.cartShow,
      };

    case actionType.SET_CARTITEMS:
      return {
        ...state,
        cartItems: action.cartItems,
      };
    case actionType.SET_LOCATION:
      return {
        ...state,
        location: action.location,
      };
    case actionType.SET_AUTH:
      return {
        ...state,
        auth: action.auth,
      };
    case actionType.SET_LINKED:
      return {
        ...state,
        linked: action.linked,
      };
    case actionType.SET_CUSTOMER:
      return {
        ...state,
        customer: action.customer,
      };
    case actionType.SET_CARTSTORE:
      return {
        ...state,
        cartStore: action.cartStore,
      };
      case actionType.SET_TOKEN:
        return {
          ...state,
          token: action.token,
        };
        case actionType.SET_EMAIL:
        return {
          ...state,
          email: action.email,
        };
    default:
      return state;
  }
};

export default reducer;
