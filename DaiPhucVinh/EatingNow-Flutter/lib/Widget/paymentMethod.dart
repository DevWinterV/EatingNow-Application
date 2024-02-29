import 'package:flutter/material.dart';

class PaymentMethodWidget extends StatefulWidget {
  @override
  _PaymentMethodWidgetState createState() => _PaymentMethodWidgetState();
}

class _PaymentMethodWidgetState extends State<PaymentMethodWidget> {
  bool isExpanded = true;

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        GestureDetector(
          onTap: () {
            setState(() {
              isExpanded = !isExpanded;
            });
          },
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                'Phương thức thanh toán:',
                style: TextStyle(fontWeight: FontWeight.bold),
              ),
              Icon(isExpanded ? Icons.keyboard_arrow_up : Icons.keyboard_arrow_down),
            ],
          ),
        ),
        AnimatedContainer(
          duration: Duration(milliseconds: 300),
          height: isExpanded ? 120 : 0,
          child: SingleChildScrollView(
            child: Column(
              children: [
                // Add your payment method options here
                ListTile(
                  leading: Image.asset(
                    "assets/image/cod.png",
                    height: 50,
                    width: 50,),
                  title: Text('Thanh toán khi nhận hàng'),
                  onTap: () {
                    // Handle selection
                  },
                ),
                ListTile(
                  leading: Image.asset(
                    "assets/image/VNPay_logo.png",
                    height: 50,
                    width: 50,),
                  title: Text('Cổng VNPay'),
                  onTap: () {
                    // Handle selection
                  },
                ),
                // Add more payment methods as needed
              ],
            ),
          ),
        ),
      ],
    );
  }
}
