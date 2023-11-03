import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";
import PropTypes from "prop-types";
import { useState } from "react";
import UserService from "../../Services/User/UserService";
import toast from "react-hot-toast";

const ChangeEmailForm = ({ userId }) => {
  const [loading, setLoading] = useState(false);
  async function onFinish(values) {
    try {
      setLoading(true);
      const response = await UserService.changeEmailAddress(
        userId,
        values.email,
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
        window.location.reload();
      } else {
        toast.error("Email change is failed", {
          duration: 2500,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
        setLoading(false);
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
    }
  }
  return (
    <div>
      <h2 className="text-2xl pl-[25%] font-bold text-white mb-4">
        Change email
      </h2>
      <Formik
        initialValues={{
          email: "",
          password: "",
        }}
        validationSchema={Yup.object({
          email: Yup.string()
            .email("Invalid email address")
            .required("Email is required"),
          password: Yup.string()
            .min(8, "Password must be at least 8 characters")
            .required("Password is required")
            .matches(
              /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
              "Password must contain at least one uppercase letter, one lowercase letter, and one number"
            ),
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
                className="p-3 w-full text-black placeholder-black rounded-2xl focus:border-solid focus:border-2 "
              />
              <ErrorMessage
                name="email"
                component="div"
                className="text-red-500"
              />
            </div>
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="password">
                Current password
              </label>
              <Field
                type="password"
                id="password"
                name="password"
                className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2 "
                required
              />
              <ErrorMessage
                name="password"
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

export default ChangeEmailForm;

ChangeEmailForm.propTypes = {
  userId: PropTypes.string.isRequired,
};
