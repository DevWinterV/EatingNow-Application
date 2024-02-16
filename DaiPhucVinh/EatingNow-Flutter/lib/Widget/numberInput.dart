import 'package:flutter/material.dart';
import 'package:flutter/services.dart'; // Import để sử dụng TextInputFormatter

// Định nghĩa một đối tượng định dạng để chỉ chấp nhận số
class NumberTextInputFormatter extends TextInputFormatter {
  @override
  TextEditingValue formatEditUpdate(
      TextEditingValue oldValue, TextEditingValue newValue) {
    // Bộ lọc để chỉ chấp nhận số
    return newValue.copyWith(
        text: newValue.text.replaceAll(RegExp(r'[^0-9]'), '')); // Chỉ giữ lại số
  }
}

