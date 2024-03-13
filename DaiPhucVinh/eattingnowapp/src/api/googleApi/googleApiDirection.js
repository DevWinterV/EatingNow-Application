import axios from "axios";

const apiKey = 'AIzaSyAG61NrUZkmMW8AS9F7B8mCdT9KQhgG95s'; // API key

const calculateDistanceAndTime = async (origin, destination) => {
  if (!origin || !destination) {
    return;
  }
  const apiUrl = `http://localhost:3002/calculateDistanceAndTime?origin=${encodeURIComponent(origin)}&destination=${encodeURIComponent(destination)}`;

  try {
    const response = await axios.get(apiUrl);
    if (response.data.status === "OK" && response.data.routes.length > 0) {
      // Extract distance and duration from the first route
      const route = response.data.routes[0];
      const leg = route.legs[0];
      const distance = leg.distance.value;
      const duration = leg.duration.value;
      return { distance, duration };
    } else {
      // Handle error cases
      return null;
    }
  } catch (error) {
    console.error('Đã xảy ra lỗi khi tính toán thời gian và quãng đường.', error);
  }
};

const calculateDistanceAndTimeProxy = async (origin, destination) => {
  try {
    // Tạo URL với origin và destination
    const apiUrl = `http://localhost:3002/calculateDistanceAndTime?origin=${encodeURIComponent(origin)}&destination=${encodeURIComponent(destination)}`;

    // Gửi yêu cầu GET đến URL
    const response = await axios.get(apiUrl);

    // Kiểm tra nếu phản hồi hợp lệ
    if (response.status === 200) {
      // Trích xuất dữ liệu từ phản hồi JSON
      const { distance, duration } = response.data;

      // Trả về kết quả
      return { distance, duration };
    } else {
      console.error('Yêu cầu không thành công.');
    }
  } catch (error) {
    console.error('Đã xảy ra lỗi khi lấy dữ liệu.', error);
  }
};

export {
    calculateDistanceAndTime,
    calculateDistanceAndTimeProxy
}
