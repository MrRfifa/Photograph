import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";
import PropTypes from "prop-types";
import { useState } from "react";
import UserService from "../../Services/User/UserService";
import toast from "react-hot-toast";

const ChangePasswordForm = ({ userId }) => {
  const [loading, setLoading] = useState(false);

  const initialValues = {
    oldPassword: "",
    newPassword: "",
    confirmNewPassword: "",
  };

  const validationSchema = Yup.object({
    oldPassword: Yup.string().required("Old password is required"),
    newPassword: Yup.string()
      .min(8, "Password must be at least 8 characters")
      .required("New password is required")
      .matches(
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
        "Password must contain at least one uppercase letter, one lowercase letter, and one number"
      ),
    confirmNewPassword: Yup.string()
      .oneOf([Yup.ref("newPassword"), null], "Passwords must match")
      .required("Must retype the new password"),
  });

  async function onFinish(values) {
    try {
      setLoading(true);
      const response = await UserService.changePassword(
        userId,
        values.oldPassword,
        values.newPassword,
        values.confirmNewPassword
      );

      if (response.success) {
        toast.success(response.message, {
          duration: 4500,
          position: "top-right",
          icon: "🔥",
          className: "bg-green-500 text-white",
        });
        window.location.reload("/settings");
      } else {
        toast.error(response.message || "Password change failed", {
          duration: 2500,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
      }
    } catch (error) {
      toast.error(
        "An error occurred during password change. Please try again later.",
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
      <h2 className="text-2xl pl-[25%] font-bold text-white mb-4">
        Change password
      </h2>
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={onFinish}
      >
        <Form className="px-7 grid place-items-center">
          <div className="grid gap-6" id="form">
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="oldPassword">
                Old Password
              </label>
              <Field
                type="password"
                id="oldPassword"
                name="oldPassword"
                className="p-3 w-full text-black placeholder-black rounded-2xl focus:border-solid focus:border-2"
              />
              <ErrorMessage
                name="oldPassword"
                component="div"
                className="text-red-500"
              />
            </div>
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="newPassword">
                New password
              </label>
              <Field
                type="password"
                id="newPassword"
                name="newPassword"
                className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2"
                required
              />
              <ErrorMessage
                name="newPassword"
                component="div"
                className="text-red-500"
              />
            </div>
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="confirmNewPassword">
                Retype password
              </label>
              <Field
                type="password"
                id="confirmNewPassword"
                name="confirmNewPassword"
                className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2"
                required
              />
              <ErrorMessage
                name="confirmNewPassword"
                component="div"
                className="text-red-500"
              />
            </div>
            <button
              className="outline-none shadow-2xl w-full p-3 bg-blue-400 hover:border-blue-500 hover.border-solid hover.border-2 hover.text-blue-500 font-bold"
              type="submit"
              disabled={loading}
            >
              {loading ? "Updating..." : "Update"}
            </button>
          </div>
        </Form>
      </Formik>
    </div>
  );
};

export default ChangePasswordForm;

ChangePasswordForm.propTypes = {
    userId: PropTypes.string.isRequired,
};
