import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";
import toast, { Toaster } from "react-hot-toast";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import UserService from "../../Services/User/UserService";
import { ImSpinner9 } from "react-icons/im";

export const DeleteAccountForm = () => {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  async function onFinish(values) {
    try {
      setLoading(true);
      const response = await UserService.confirmAccountDeletion(values.token);
      if (response.success) {
        toast.success("Account deleted successfully", {
          duration: 4500,
          position: "top-right",
          icon: "😭",
          className: "bg-green-500 text-white",
        });
      } else {
        toast.error(response.error || "Account deletion failed", {
          duration: 2500,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
      }
    } catch (error) {
      toast.error("An error occurred during login. Please try again later.", {
        duration: 2000,
        position: "top-right",
        icon: "🤌🏻",
        className: "bg-red-500 text-white",
      });
    } finally {
      setLoading(false);
      setTimeout(() => {
        navigate("/");
      }, 2500);
    }
  }
  return (
    <>
      <Toaster />
      <div className="flex flex-col items-center justify-center h-screen dark">
        <div className="w-full max-w-md bg-gray-800 rounded-lg shadow-md p-6">
          <div className="flex flex-row justify-start">
            <h2 className="text-2xl pl-[25%] font-bold text-gray-200 mb-4 w-full">
              Delete Account Permanently
            </h2>
          </div>
          <Formik
            initialValues={{
              token: "",
            }}
            validationSchema={Yup.object({
              token: Yup.string().required("Token is required"),
            })}
            onSubmit={onFinish}
          >
            <Form className="flex flex-col">
              <div>
                <Field
                  type="text"
                  name="token"
                  autoComplete="off"
                  placeholder="Token"
                  className="bg-gray-700 text-gray-200 border-0 w-[100%] rounded-md p-2 mb-4 focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
                />
                <ErrorMessage
                  name="token"
                  component="div"
                  className="text-red-500"
                />
              </div>
              <button
                disabled={loading}
                className="bg-gradient-to-r from-[#E0B1CB] to-[#240046] text-white font-bold py-2 px-4 rounded-md mt-4 hover:bg-indigo-600 hover:to-[#7B2CBF] transition ease-in-out duration-150"
                type="submit"
              >
                {loading ? (
                  <ImSpinner9
                    className="text-white animate-spin mx-auto"
                    size={25}
                  />
                ) : (
                  "Delete"
                )}
              </button>
            </Form>
          </Formik>
        </div>
      </div>
    </>
  );
};
