class FoodRatingRequest {
  int foodRatingId;
  String customerId;
  int foodId;
  double rating;
  String comment;
  DateTime addedAt;
  String orderHeaderId;

  FoodRatingRequest({
    required this.foodRatingId,
    required this.customerId,
    required this.foodId,
    required this.rating,
    required this.comment,
    required this.addedAt,
    required this.orderHeaderId,
  });

  factory FoodRatingRequest.fromJson(Map<String, dynamic> json) {
    return FoodRatingRequest(
      foodRatingId: json['FoodRatingId'],
      customerId: json['CustomerId'],
      foodId: json['FoodId'],
      rating: json['Rating'],
      comment: json['Comment'],
      addedAt: DateTime.parse(json['AddedAt']),
      orderHeaderId: json['OrderHeaderId'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'FoodRatingId': foodRatingId,
      'CustomerId': customerId,
      'FoodId': foodId,
      'Rating': rating,
      'Comment': comment,
      'AddedAt': addedAt.toIso8601String(),
      'OrderHeaderId': orderHeaderId,
    };
  }
}
