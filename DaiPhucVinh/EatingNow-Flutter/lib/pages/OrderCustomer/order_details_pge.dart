import 'package:flutter/material.dart';
import 'package:flutter/cupertino.dart';

class OrderDetailsPage extends StatefulWidget {
  const OrderDetailsPage({super.key});

  @override
  State<OrderDetailsPage> createState() => _OrderDetailsPageState();
}

class _OrderDetailsPageState extends State<OrderDetailsPage> {
  late String orderId;
  @override
  Widget build(BuildContext context) {
    final arguments = ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
    orderId = arguments['data'] as String; // Nhận dữ liệu orderId
    return const Placeholder();
  }
}
