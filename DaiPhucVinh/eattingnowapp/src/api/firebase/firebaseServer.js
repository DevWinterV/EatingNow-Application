import { realtime_database} from "../../firebase.config";
import { ref, push, set } from 'firebase/database';

const writeUserData = async (userData)  =>{
    const db = realtime_database;
    const usersRef = ref(db, 'driverInfor');
    // Sử dụng push() để tạo một UID tự động
    const newUserRef = push(usersRef);
    // Lấy UID đã được tạo
    const userId = newUserRef.key;
    // Thêm dữ liệu vào UID đã tạo
    set(newUserRef, userData)
      .then(() => {
        console.log('Data written successfully.');
      })
      .catch((error) => {
        console.error('Error writing data:', error);
      });
  }

export {
    writeUserData,
};