import moment from "moment";

export function isBetween(timeAt, startTime, endTime) {
  var start = moment(startTime, "HH:mm");
  var end = moment(endTime, "HH:mm");
  var time = moment(timeAt, "HH:mm");
  return start.diff(time) <= 0 && time.diff(end) <= 0;
}
export function DateTimeToString(dateString, timeString) {
  var dateObj = new Date(dateString);
  var day = "";
  var month = "";
  var year = "";
  var formattedDate = "";
  if (!dateString || !timeString) {
    if (!isNaN(dateObj)) {
      day = dateObj.getDate().toString().padStart(2, "0");
      month = (dateObj.getMonth() + 1).toString().padStart(2, "0");
      year = dateObj.getFullYear().toString();
      formattedDate = `${day}/${month}/${year}`;
      return `Ngày ${formattedDate}`;
    }
    return null;
  }
}
