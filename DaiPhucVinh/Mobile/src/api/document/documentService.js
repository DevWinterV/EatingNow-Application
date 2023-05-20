import { Proxy } from "../Proxy";
const API = {
  GetCurrentDocument: "/documents/GetCurrentDocument",
  UpdateOrCreateDocument: "/documents/UpdateOrCreateDocument",
  UpdateDocumentWithAttach: "/documents/UpdateDocumentWithAttach",
  DeleteDocumentWithAttach: "/documents/DeleteDocumentWithAttach",
};

export const UpdateOrCreateDocument = async (request) =>
  await Proxy("post", API.UpdateOrCreateDocument, request);
export const GetCurrentDocument = async () => await Proxy("get", API.GetCurrentDocument, null);
export const CreateDocumentWithAttach = async (request) =>
  await Proxy("post", API.CreateDocumentWithAttach, request);
export const UpdateDocumentWithAttach = async (request) =>
  await Proxy("post", API.UpdateDocumentWithAttach, request);
export const DeleteDocumentWithAttach = async (request) =>
  await Proxy("post", API.DeleteDocumentWithAttach, request);
