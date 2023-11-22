import axios from "axios";
const apiKey = 'AIzaSyAG61NrUZkmMW8AS9F7B8mCdT9KQhgG95s'; // API key
const handleSearchAddress = async (searchAddress) => {
  if (!searchAddress) {
    return;
  }
  const apiUrl = `https://cors-anywhere.herokuapp.com/https://maps.googleapis.com/maps/api/place/textsearch/json?query=${encodeURIComponent(searchAddress)}&region=vn&key=${apiKey}`;
  try {
    const response = await axios.get(apiUrl);
    if (response.data.status === "OK" && response.data.results.length > 0) {
      // Process the data here
      console.log(response)
      const firstResult = response.data.results[0];
      const location = firstResult.geometry.location;
      const name = firstResult.name;
    } else {
      return response.data.status ==="DOK"
    }
    return response;
  } catch (error) {
    alert('Có lỗi xảy ra khi tìm kiếm địa chỉ. Vui lòng thử lại sau.');
  }
};
const searchAddress = async (searchQuery) => {
  try {
    const response = await axios.get('http://localhost:3002/search-address', {
      params: {
        searchAddress: searchQuery
      }
    });

    if (response.status === 200) {
      // Xử lý dữ liệu trả về ở đây
      const data = response.data;
      return data;
    } else {
      console.error('Lỗi khi gọi tuyến đường search-address:', response.data);
      return null;
    }
  } catch (error) {
    console.error('Lỗi khi gọi tuyến đường search-address:', error);
    return null;
  }
};


export {
    handleSearchAddress,
    searchAddress
}