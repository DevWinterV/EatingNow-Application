import { Proxy } from "../Proxy";
const API = {
  CreateBooking: "/booking/CreateBooking",
  GetLastBookingByCustomer: "/booking/GetLastBookingByCustomer",
  TakeAllBooking: "/booking/TakeAllBooking",
};

export const CreateBooking = async (request) => await Proxy("post", API.CreateBooking, request);

export const GetLastBookingByCustomer = async (request) =>
  await Proxy("post", API.GetLastBookingByCustomer, request);

export const TakeAllBooking = async (request) => await Proxy("post", API.TakeAllBooking, request);
