import 'package:flutter/material.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:intl_phone_field/intl_phone_field.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: LoginPage(),
    );
  }
}

class LoginPage extends StatefulWidget {
  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  TextEditingController _phoneNumberController = TextEditingController();
  TextEditingController _otpController = TextEditingController();
  FirebaseAuth _auth = FirebaseAuth.instance;
  String _verificationId = "";
  String _phoneNumber = "";
  bool _isCodeSent = false;
  FocusNode focusNode = FocusNode();

  Future<void> _signInWithPhoneNumber(String phoneNumber) async {
    try {
      await _auth.verifyPhoneNumber(
        phoneNumber: phoneNumber,
        verificationCompleted: (PhoneAuthCredential credential) async {
          await _auth.signInWithCredential(credential);
          print("User signed in automatically: ${_auth.currentUser?.uid}");
        },
        verificationFailed: (FirebaseAuthException e) {
          print('Verification failed: ${e.message}');
          // Handle verification failure
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
      print('Error sending OTP: $e');
      // Handle error
    }
  }

  Future<void> _login() async {
    try {
      // Check if the code is sent before attempting to log in
      if (_isCodeSent) {
        // Create a PhoneAuthCredential with the code
        PhoneAuthCredential credential = PhoneAuthProvider.credential(
          verificationId: _verificationId,
          smsCode: _otpController.text,
        );

        // Sign the user in (or link) with the credential
        await _auth.signInWithCredential(credential);
        if(_auth.currentUser?.uid != null){
          Navigator.pushReplacement(
              context,
              Navigator.pushNamed(context, "/") as Route<Object?>
          );
        }
        print("User signed in: ${_auth.currentUser?.uid}");

      } else {
        // Handle the case where the code hasn't been sent yet
        print('Please send OTP first');
      }
    } catch (e) {
      print('Login failed: $e');
      // Handle login failure
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Login Page'),
      ),
      body: Padding(
        padding: EdgeInsets.all(16.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            IntlPhoneField(
              focusNode: focusNode,
              decoration: InputDecoration(
                labelText: 'Số điện thoại',
                border: OutlineInputBorder(
                  borderSide: BorderSide(),
                ),
              ),
              languageCode: "vi",
              initialCountryCode: "VN", // Mặc định là Việt Nam
              onChanged: (phone) {
                setState(() {
                  _phoneNumber = phone.completeNumber;
                });
                print(phone.completeNumber);
              },
              onCountryChanged: (country) {
                print('Country changed to: ' + country.name);
              },
            ),
            SizedBox(height: 16.0),
            if (_isCodeSent)
              Column(
                children: [

                  TextField(
                    textAlign: TextAlign.center,
                    controller: _otpController,
                    obscureText: true,
                    keyboardType: TextInputType.number,
                    decoration: InputDecoration(labelText: 'OTP'),
                  ),
                  SizedBox(height: 16.0),
                  ElevatedButton(
                    onPressed: _login,
                    child: Text('Đăng nhập'),
                  ),
                ],
              ),
            if (!_isCodeSent)
              ElevatedButton(

                onPressed: () {
                  _signInWithPhoneNumber(_phoneNumber);
                },
                child: Text('Gửi mã OTP'),
              ),
          ],
        ),
      ),
    );
  }
}
