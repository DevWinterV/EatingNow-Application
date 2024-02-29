import 'package:shared_preferences/shared_preferences.dart';

class PaymentMethodStorage {
  Future<bool> savePaymentMethodStorage(String paymentmethod) async {
    try{
      SharedPreferences prefs = await SharedPreferences.getInstance();
      // Chuyển đối tượng UserAccount thành chuỗi JSON trước khi lưu
      await prefs.setString('paymentMethod', paymentmethod);
      return true;
    }
    catch(e){
      print(e);
      return false;
    }
  }

  Future<String> getSavedPaymentMethod() async{
    SharedPreferences prefs = await SharedPreferences.getInstance();
    final String paymentmethod = prefs.getString('paymentMethod') ?? '';
    return paymentmethod;
  }
}

