class AppConstants{
  static const String APP_NAME="XpressEat.";
  static const int APP_VERSION = 1;
  static const String BASE_URL="https://4772-14-168-12-26.ngrok-free.app";
  static const String TakeAllStore = BASE_URL+"/api/store/TakeAllStore";
  static const String TakeAllFoodListByStoreId= BASE_URL+"/api/store/TakeAllFoodListByStoreId";
  static const String TakeFoodListByStoreId= BASE_URL+"/api/store/TakeFoodListByStoreId";
  static const String TakeCategoryByStoreId= BASE_URL+"/api/store/TakeCategoryByStoreId";
  static const String TakeAllCuisine = BASE_URL+"/api/cuisine/TakeAllCuisine";
  static const String TakeRecommendedFoodList = BASE_URL+"/api/foodlist/TakeRecommendedFoodList";
  static const String TakeStoreByCuisineId = BASE_URL+"/api/store/TakeStoreByCuisineId";
  static const String CreateOreder = BASE_URL+"/api/customer/CreateOrderCustomer";
  static const String CheckCustomer= BASE_URL+"/api/customer/CheckCustomer";
}