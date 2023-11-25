import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";
import { useState } from "react";
import toast from "react-hot-toast";
import UserService from "../../Services/User/UserService";

const ChangeEmailForm = () => {
  const [loading, setLoading] = useState(false);

  async function onFinish(values) {
    try {
      setLoading(true);
      const response = await UserService.changeEmailAddress(
        values.email,
        values.currentPassword
      );

      if (response.success) {
        toast.success(response.message, {
          duration: 4500,
          position: "top-right",
          icon: "🔥",
          className: "bg-green-500 text-white",
        });
      } else {
        toast.error("Email change failed: " + response.message, {
          duration: 2500,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
      }
    } catch (error) {
      console.error("An error occurred during email change:", error);
      toast.error(
        "An error occurred during email change. Please try again later.",
        {
          duration: 2000,
          position: "top-right",
          icon: "🤌🏻",
          className: "bg-red-500 text-white",
        }
      );
    } finally {
      setLoading(false);
    }
  }

  return (
    <div>
      <h2 className="text-2xl font-bold text-white mb-4">Change email</h2>
      <Formik
        initialValues={{
          email: "",
          currentPassword: "",
        }}
        validationSchema={Yup.object({
          email: Yup.string()
            .email("Invalid email address")
            .required("Email is required"),
          currentPassword: Yup.string().required("Password is required"),
        })}
        onSubmit={onFinish}
      >
        <Form className="px-7 grid place-items-center">
          <div className="grid gap-6" id="form">
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="email">
                New email
              </label>
              <Field
                type="email"
                id="email"
                name="email"
                className="p-3 w-full text-black placeholder-black rounded-2xl focus:border-solid focus:border-2"
              />
              <ErrorMessage
                name="email"
                component="div"
                className="text-red-500"
              />
            </div>
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="currentPassword">
                Current password
              </label>
              <Field
                type="password"
                id="currentPassword"
                name="currentPassword"
                className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2"
                required
              />
              <ErrorMessage
                name="currentPassword"
                component="div"
                className="text-red-500"
              />
            </div>
            <button
              className="outline-none shadow-2xl w-full p-3 bg-blue-400 hover:border-blue-500 hover:border-solid hover:border-2 hover:text-blue-500 font-bold"
              type="submit"
            >
              {loading ? "Updating..." : "Update"}
            </button>
          </div>
        </Form>
      </Formik>
    </div>
  );
};

ChangeEmailForm.propTypes = {};

export default ChangeEmailForm;
