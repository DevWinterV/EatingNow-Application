class AppConstants{
  static const String APP_NAME="XpressEat.";
  static const double APP_VERSION = 1.0;
  static const String BASE_URL="https://7f98-2402-800-63a4-facd-9121-addc-332f-4e21.ngrok-free.app";
  static const String TakeAllStore = BASE_URL+"/api/store/TakeAllStore";
  static const String TakeAllFoodListByStoreId= BASE_URL+"/api/store/TakeAllFoodListByStoreId";
  static const String TakeFoodListByStoreId= BASE_URL+"/api/store/TakeFoodListByStoreId";
  static const String TakeCategoryByStoreId= BASE_URL+"/api/store/TakeCategoryByStoreId";
  static const String TakeAllCuisine = BASE_URL+"/api/cuisine/TakeAllCuisine";
  static const String TakeRecommendedFoodList = BASE_URL+"/api/foodlist/TakeRecommendedFoodList";
  static const String TakeStoreByCuisineId = BASE_URL+"/api/store/TakeStoreByCuisineId";
  static const String CreateOreder = BASE_URL+"/api/customer/CreateOrderCustomer";
  static const String CheckCustomer= BASE_URL+"/api/customer/CheckCustomer";
  static const String UpdateToken= BASE_URL+"/api/customer/UpdateToken";
  static const String UpdateInfoCustomer= BASE_URL+"/api/customer/UpdateInfoCustomer";
  static const String TakeOrderByCustomer= BASE_URL+"/api/customer/TakeOrderByCustomer";
  static const String PaymentConfirm= BASE_URL+"/api/customer/PaymentConfirm";
  static const String GetListOrderLineDetails= BASE_URL+"/api/store/GetListOrderLineDetails";
}