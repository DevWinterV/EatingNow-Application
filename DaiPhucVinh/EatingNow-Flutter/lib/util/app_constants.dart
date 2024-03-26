class AppConstants{
  static const String APP_NAME="XpressEat.";
  static const String PageNotFound="PageNotFound.";
  static const double APP_VERSION = 1.0;
  static const String BASE_URL="https://6b38-2402-800-63fe-aeeb-54bb-6fda-9cb5-f9e5.ngrok-free.app";
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
  static const String SearchFoodListByUser= BASE_URL+"/api/foodlist/SearchFoodListByUser";
  // FoodRating
  static const String TakeFoodsRatingByOrderHeaderId= BASE_URL+"/api/foodrating/TakeFoodsRatingByOrderHeaderId";
  static const String CreateFoodRating= BASE_URL+"/api/foodrating/CreateFoodRating";
  static const String UpdateFoodRating= BASE_URL+"/api/foodrating/UpdateFoodRating";
  static const String DeleteFoodRating= BASE_URL+"/api/foodrating/DeleteFoodRating";

}