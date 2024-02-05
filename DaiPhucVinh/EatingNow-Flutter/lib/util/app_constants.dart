class AppConstants{
  static const String APP_NAME="XpressEat.";
  static const int APP_VERSION = 1;
  static const String BASE_URL="https://cd31-2402-800-63a4-d8c1-3de4-4de9-cd50-4eda.ngrok-free.app";
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