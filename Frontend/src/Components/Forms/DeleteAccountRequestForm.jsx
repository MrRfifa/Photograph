import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";
import toast from "react-hot-toast";
import { useState } from "react";
import UserService from "../../Services/User/UserService";
import { ImSpinner9 } from "react-icons/im";

const DeleteAccountRequestForm = () => {
  const [loading, setLoading] = useState(false);

  const initialValues = {
    password: "",
  };

  const validationSchema = Yup.object({
    password: Yup.string().required("Password is required"),
  });

  async function onFinish(values) {
    try {
      setLoading(true);
      const response = await UserService.accountDeletionRequest(
        values.password
      );

      if (response.success) {
        toast.success(response.message, {
          duration: 4500,
          position: "top-right",
          icon: "🔥",
          className: "bg-green-500 text-white",
        });
        localStorage.clear();
        window.location.reload("/");
      } else {
        toast.error(response.message || "Account deletion request failed", {
          duration: 2500,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
      }
    } catch (error) {
      toast.error("An error occurred. Please try again later.", {
        duration: 2000,
        position: "top-right",
        icon: "🤌🏻",
        className: "bg-red-500 text-white",
      });
    } finally {
      setLoading(false);
    }
  }

  return (
    <div>
      <h2 className="text-2xl pl-[25%] mx-auto font-bold text-white mb-4">
        Delete Account Request
      </h2>
      <div
        className="p-4 mb-4 text-sm text-red-800 rounded-lg bg-red-50 dark:bg-gray-800 dark:text-red-400"
        role="alert"
      >
        <span className="font-medium">Danger alert! </span>
        After submitting, a confirmation mail is sent to you containing a token.
      </div>
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={onFinish}
      >
        <Form className="px-7 grid place-items-center">
          <div className="grid gap-6" id="form">
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="password">
                Password
              </label>
              <Field
                type="password"
                id="password"
                name="password"
                className="p-3 w-full text-black placeholder-black rounded-2xl focus:border-solid focus:border-2"
              />
              <ErrorMessage
                name="password"
                component="div"
                className="text-red-500"
              />
            </div>
            <button
              className="outline-none rounded-xl shadow-2xl w-full p-3 bg-blue-400 hover:border-blue-500 hover.border-solid hover.border-2 hover.text-blue-500 font-bold"
              type="submit"
              disabled={loading}
            >
              {loading ? (
                <ImSpinner9
                  className="text-white animate-spin mx-auto"
                  size={25}
                />
              ) : (
                "Send request"
              )}
            </button>
          </div>
        </Form>
      </Formik>
    </div>
  );
};

export default DeleteAccountRequestForm;
