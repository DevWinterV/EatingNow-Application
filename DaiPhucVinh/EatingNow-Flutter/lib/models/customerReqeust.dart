class EN_CustomerRequest {
  final String customerId;
  final String completeName;
  final String phone;
  final String email;
  final bool status;

  EN_CustomerRequest({
    required this.customerId,
    required this.completeName,
    required this.phone,
    required this.email,
    required this.status,
  });

  Map<String, dynamic> toJson() {
    return {
      'CustomerId': customerId,
      'CompleteName': completeName,
      'Phone': phone,
      'Email': email,
      'Status': status,
    };
  }
}