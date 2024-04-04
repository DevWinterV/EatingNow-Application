import 'dart:async';

import 'package:fam/storage/UserAccountstorage.dart';
import 'package:fam/util/Colors.dart';
import 'package:fam/util/dimensions.dart';
import 'package:flutter/material.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:intl_phone_field/intl_phone_field.dart';
import '../../data/Api/CustomerService.dart';
import '../../data/Api/firebase_api.dart';
import '../../storage/UserAccountstorage.dart';
import '../../util/app_constants.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});
  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  CustomerService  customerService = CustomerService(apiUrl: AppConstants.CheckCustomer);
  TextEditingController _otpController = TextEditingController();
  FirebaseAuth _auth = FirebaseAuth.instance;
  String _verificationId = "";
  final _streamPhonenumber = StreamController<String>.broadcast();
  final _streamOTP = StreamController<String>.broadcast();
  bool _isCodeSent = false;
  FocusNode focusNode = FocusNode();
  late UserAccountStorage prefs ;

  @override
  void initState() {
    super.initState();
    initialization();
    prefs = UserAccountStorage();
  }

  void initialization() async {
    FlutterNativeSplash.remove();
  }

  void SaveDataUser(PhoneAuthCredential credential, String _phoneNumber) async {
    final userAuth = await _auth.signInWithCredential(credential);
    final result = await customerService.fecthUserData({
      "CustomerId": userAuth.user?.uid ?? "",
      "Phone": _phoneNumber
    });
    if(result.data!.length > 0){
      UserAccount userAccount = new UserAccount(userId: result.data?[0].customerId ?? "", name: result.data?[0].completeName ?? "", phone: result.data?[0].phone ?? "");
      UserAccountStorage userAccountStorage = new UserAccountStorage();
      userAccountStorage.saveUserAccount(userAccount);
      await FirebaseApi().initNotifications();
      navigateToHome();
    }
    else{
      UserAccount userAccount = new UserAccount(userId: userAuth.user?.uid ?? "", name:  "", phone: _phoneNumber ?? "");
      await FirebaseApi().initNotifications();
      navigateToNewUser();
    }
  }

  Future<void> _signInWithPhoneNumber(String phoneNumber) async {
    try {
      await _auth.verifyPhoneNumber(
        phoneNumber: phoneNumber,
        verificationCompleted: (PhoneAuthCredential credential) async {
             SaveDataUser(credential, phoneNumber);
          },
        verificationFailed: (FirebaseAuthException e) {

        },
        codeSent: (String verificationId, int? resendToken) {
          setState(() {
            _verificationId = verificationId;
            _isCodeSent = true;
          });
        },
        codeAutoRetrievalTimeout: (String verificationId) {},
      );
    } catch (e) {

    }
  }

  void navigateToHome() {
    if (_auth.currentUser?.uid != null) {
      Navigator.pop(context);
    }
  }

  void navigateToNewUser() {
    if (_auth.currentUser?.uid != null) {
      Navigator.pushReplacementNamed(context, "/");
    }
  }

  Future<void> _login(String phoneNumber, String OTP) async {
    try {
      if (_isCodeSent) {
          PhoneAuthCredential credential = PhoneAuthProvider.credential(
            verificationId: _verificationId,
            smsCode: OTP,
          );
          SaveDataUser(credential, phoneNumber);
      }
    } catch (e) {

    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
      ),
      backgroundColor: Colors.white,
      body:
      StreamBuilder<String>(
        stream: _streamPhonenumber.stream,
        builder: (BuildContext context, AsyncSnapshot<String> snapshot) {
          String _phoneNumber = snapshot.data ?? "";
          return Padding(
            padding: EdgeInsets.all(16.0),
            child: _isCodeSent == false
                ? Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text("Chào mừng bạn đến với",style: TextStyle(color: AppColors.mainColor, fontSize: Dimensions.font26, fontWeight: FontWeight.bold),),
                Text(AppConstants.APP_NAME,style: TextStyle(color: AppColors.mainColor, fontSize: Dimensions.font26, fontWeight: FontWeight.bold),),
                SizedBox(height: 16.0),
                IntlPhoneField(
                  focusNode: focusNode,
                  decoration: InputDecoration(
                    hintText: "Nhập số điện thoại",
                    labelText: 'Số điện thoại',
                    border: OutlineInputBorder(
                      borderSide: BorderSide(),
                    ),
                  ),
                  languageCode: "vi",
                  initialCountryCode: "VN",
                  onChanged: (phone) {
                    _streamPhonenumber.sink.add(phone.completeNumber);
                  },
                  onCountryChanged: (country) {

                  },
                ),
                SizedBox(height: 18.0),
                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: AppColors.mainColor,
                  ),
                  onPressed: _phoneNumber.isNotEmpty && _phoneNumber.length >= 10 ?  () {
                    _signInWithPhoneNumber(_phoneNumber);
                  } : null,
                  child: Text('Nhận mã OTP', style: TextStyle(color: Colors.white),),
                ),
              ],
            )
                :
            StreamBuilder<String>(stream: _streamOTP.stream, builder: (builder,snapshotOtp){
                final Otp = snapshotOtp.data ?? "";
                return Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    TextField(
                      decoration: InputDecoration(
                        hintText: "Nhập mã OTP 6 chữ số",
                        labelText: 'Nhập mã OTP',
                        border: OutlineInputBorder(
                          borderSide: BorderSide(),
                        ),
                      ),
                      onChanged: (otp){
                        _streamOTP.sink.add(otp);
                      },
                    ),
                    SizedBox(height: 18.0),
                    ElevatedButton(
                      style: ElevatedButton.styleFrom(
                        backgroundColor: AppColors.mainColor,
                      ),
                      onPressed: Otp.length == 6 ? () {
                        _login(_phoneNumber, Otp);
                      } : null,
                      child: Text('Đăng nhập', style: TextStyle(color: Colors.white),),
                    ),
                  ],
                );
            }),
          );
        }
      )
    );
  }
}
