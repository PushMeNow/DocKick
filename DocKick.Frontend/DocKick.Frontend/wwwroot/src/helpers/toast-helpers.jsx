import { toast } from "react-toastify";

export const toastSuccess = (message, config = {}) => toast(message, { type: "success", ...config });
export const toastError = (message, config = {}) => toast(message, { type: "error", ...config });