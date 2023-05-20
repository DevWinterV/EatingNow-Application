import { DeleteAccountPage } from "../Auth";
import {
  CustomerAccountPage,
  CustomerChangePasswordPage,
  CustomerProfilePage,
  CustomerBookingPage,
  CustomerBookingHistoryPage,
  CustomerBookingSuccessPage,
  CustomerTrackingServicePage,
} from "../Customer";
import HomePostDetailPage from "../HomePage/HomePostDetailPage";
import LocationPage from "../LocationPage";
import { DocumentWithAttachPage, PartnerAccountPage } from "../Partner";
const SiteMap = [
  // {
  //   name: "Medicines",
  //   component: MedicinesPage,
  // },
  {
    name: "CustomerAccount",
    component: CustomerAccountPage,
  },
  {
    name: "CustomerProfile",
    component: CustomerProfilePage,
  },
  {
    name: "CustomerChangePassword",
    component: CustomerChangePasswordPage,
  },
  {
    name: "CustomerBooking",
    component: CustomerBookingPage,
  },
  {
    name: "CustomerBookingHistory",
    component: CustomerBookingHistoryPage,
  },
  {
    name: "CustomerBookingSuccess",
    component: CustomerBookingSuccessPage,
  },
  {
    name: "CustomerTrackingService",
    component: CustomerTrackingServicePage,
  },
  {
    name: "PartnerAccount",
    component: PartnerAccountPage,
  },
  {
    name: "Location",
    component: LocationPage,
  },
  {
    name: "DocumentWithAttach",
    component: DocumentWithAttachPage,
  },
  {
    name: "HomePostDetail",
    component: HomePostDetailPage,
  },
  {
    name: "DeleteAccount",
    component: DeleteAccountPage,
  },
];
export { SiteMap };
