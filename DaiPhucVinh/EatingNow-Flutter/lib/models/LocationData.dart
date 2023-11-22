class LocationData {
  final String name;
  final double latitude;
  final double longitude;
  final String address;

  LocationData({
    required this.name,
    required this.latitude,
    required this.longitude,
    required this.address,
  });

  factory LocationData.fromJson(Map<String, dynamic> json) {
    return LocationData(
      name: json['name'] as String,
      latitude: json['latitude'] as double,
      longitude: json['longitude'] as double,
      address: json['address'] as String,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'name': String,
      'latitude': latitude,
      'longitude': longitude,
      'address': address,
    };
  }
}
